﻿using EPlast.BussinessLayer;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPlast.Wrapper;
using Microsoft.AspNetCore.Http;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IAnnualReportVMInitializer _annualReportVMCreator;
        private readonly IDecisionVMIitializer _decisionVMCreator;
        private readonly IPDFService _PDFService;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IViewAnnualReportsVMInitializer _viewAnnualReportsVMInitializer;
        private readonly IDirectoryManager _directoryManager;
        private readonly IFileManager _fileManager;
        private readonly IFileStreamManager _fileStreamManager;
        private readonly ICityAccessManager _cityAccessManager;

        private const string DecesionsDocumentFolder = @"\documents\";

        public DocumentationController(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IAnnualReportVMInitializer annualReportVMCreator,
            IDecisionVMIitializer decisionVMCreator, IPDFService PDFService, IHostingEnvironment appEnvironment, IViewAnnualReportsVMInitializer viewAnnualReportsVMInitializer,
            ICityAccessManager cityAccessManager, IDirectoryManager directoryManager, IFileManager fileManager, IFileStreamManager fileStreamManager)
        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
            _userManager = userManager;
            _PDFService = PDFService;
            _decisionVMCreator = decisionVMCreator;
            _appEnvironment = appEnvironment;
            _viewAnnualReportsVMInitializer = viewAnnualReportsVMInitializer;
            _directoryManager = directoryManager;
            _fileManager = fileManager;
            _fileStreamManager = fileStreamManager;
            _cityAccessManager = cityAccessManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public DecesionViewModel CreateDecesion()
        {
            try
            {
                DecesionViewModel decesionViewModel = new DecesionViewModel
                {
                    Decesion = new Decesion(),
                    OrganizationListItems = (from item in _repoWrapper.Organization.FindAll()
                                             select new SelectListItem
                                             {
                                                 Text = item.OrganizationName,
                                                 Value = item.ID.ToString()
                                             }),
                    DecesionTargets = _repoWrapper.DecesionTarget.FindAll().ToList(),
                    DecesionStatusTypeListItems = _decisionVMCreator.GetDecesionStatusTypes()
                };

                return decesionViewModel;
            }
            catch
            {
                RedirectToAction("HandleError", "Error");
                return null;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> SaveDecesionAsync(DecesionViewModel decesionViewModel)
        {
            try
            {
                ModelState.Remove("Decesion.DecesionStatusType");
                if (!ModelState.IsValid && decesionViewModel.Decesion.DecesionTarget.ID != 0 || decesionViewModel == null)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return Json(new { success = false });
                }

                if (decesionViewModel.File != null && decesionViewModel.File.Length > 10485760)
                {
                    ModelState.AddModelError("", "файл за великий (більше 10 Мб)");
                    return Json(new { success = false });
                }

                decesionViewModel.Decesion.HaveFile = decesionViewModel.File != null ? true : false;

                _repoWrapper.Decesion.Attach(decesionViewModel.Decesion);
                _repoWrapper.Decesion.Create(decesionViewModel.Decesion);
                _repoWrapper.Save();

                if (decesionViewModel.Decesion.HaveFile)
                {
                    try
                    {
                        string path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decesionViewModel.Decesion.ID;
                        _directoryManager.CreateDirectory(path);

                        if (!_directoryManager.Exists(path))
                        {
                            throw new ArgumentException($"directory '{path}' is not exist");
                        }

                        if (decesionViewModel.File != null)
                        {
                            path = Path.Combine(path, decesionViewModel.File.FileName);

                            using (var stream = _fileStreamManager.GenerateFileStreamManager(path, FileMode.Create))
                            {
                                await _fileStreamManager.CopyToAsync(decesionViewModel.File, stream.GetStream());
                                if (!_fileManager.Exists(path))
                                {
                                    throw new ArgumentException($"File was not created it '{path}' directory");
                                }
                            }
                        }
                    }
                    catch
                    {
                        return Json(new { success = false });
                    }
                }
                return Json(new
                {
                    success = true,
                    Text = "Рішення додано, обновіть сторінку.",
                    id = decesionViewModel.Decesion.ID,
                    decesionOrganization = _repoWrapper.Organization.FindByCondition(x => x.ID == decesionViewModel.Decesion.Organization.ID).Select(x => x.OrganizationName)
                });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ReadDecesion()
        {
            try
            {
                var decisions = new List<DecesionViewModel>(
                    _repoWrapper.Decesion
                    .Include(x => x.DecesionTarget, x => x.Organization)
                    .Take(200)
                    .Select(decesion => new DecesionViewModel
                    {
                        Decesion = decesion
                    })
                    .ToList());
                foreach (var decesion in decisions)
                {
                    string path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decesion.Decesion.ID;
                    if (!decesion.Decesion.HaveFile || !_directoryManager.Exists(path))
                    {
                        continue;
                    }
                    var files = _directoryManager.GetFiles(path);

                    if (files.Length == 0)
                    {
                        throw new ArgumentException($"File count in '{path}' is 0");
                    }

                    decesion.Filename = Path.GetFileName(files.First());
                }
                return View(Tuple.Create(CreateDecesion(), decisions));
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Download(string id, string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(id))
                    return Content("filename or id not present");

                var path = Path.Combine(_appEnvironment.WebRootPath + DecesionsDocumentFolder, id);

                if (!_directoryManager.Exists(path) || _directoryManager.GetFiles(path).Length == 0)
                {
                    throw new ArgumentException($"directory '{path}' is not exist");
                }
                path = Path.Combine(path, filename);
                var memory = new MemoryStream();
                using (var stream = _fileStreamManager.GenerateFileStreamManager(path, FileMode.Open))
                {
                    await _fileStreamManager.CopyToAsync(stream.GetStream(), memory);

                    if (memory.Length == 0)
                    {
                        throw new ArgumentException("memory length is 0");
                    }
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        private static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> CreatePDFAsync(int objId)
        {
            try
            {
                if (objId <= 0)
                {
                    throw new ArgumentException("Cannot crated pdf id is not valid");
                }

                byte[] arr = await _PDFService.DecesionCreatePDFAsync(_repoWrapper.Decesion.Include(x => x.DecesionTarget,
                        x => x.Organization)
                    .FirstOrDefault(x => x.ID == objId));
                return File(arr, "application/pdf");
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Голова Станиці")]
        [HttpGet]
        public IActionResult CreateAnnualReport()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var city = _cityAccessManager.GetCities(userId).First();
                var cityMembers = _repoWrapper.User
                    .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == city.ID && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var annualReportViewModel = new AnnualReportViewModel
                {
                    CityName = city.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = _annualReportVMCreator.GetAnnualReport(userId, city.ID, cityMembers)
                };
                return View(annualReportViewModel);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public IActionResult CreateAnnualReportAsAdmin(int cityId)
        {
            var userId = _userManager.GetUserId(User);
            if (!_cityAccessManager.HasAccess(userId, cityId))
                return RedirectToAction("HandleError", "Error", new { code = 403 });
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(c => c.ID == cityId)
                    .First();
                var cityMembers = _repoWrapper.User
                    .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == cityId && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var annualReportViewModel = new AnnualReportViewModel
                {
                    CityName = city.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = _annualReportVMCreator.GetAnnualReport(userId, city.ID, cityMembers)
                };
                return View("CreateAnnualReport", annualReportViewModel);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        [HttpPost]
        public IActionResult CreateAnnualReport(int cityId, AnnualReport annualReport)
        {
            var userId = _userManager.GetUserId(User);
            if (!_cityAccessManager.HasAccess(userId, cityId))
                return RedirectToAction("HandleError", "Error", new { code = 403 });
            try
            {
                annualReport.UserId = _userManager.GetUserId(User);
                annualReport.CityId = cityId;
                annualReport.Status = AnnualReportStatus.Unconfirmed;
                annualReport.Date = DateTime.Today;
                var city = _repoWrapper.City
                        .FindByCondition(c => c.ID == cityId)
                        .First();
                if (ModelState.IsValid)
                {
                    var annualReportCheck = _repoWrapper.AnnualReports
                        .FindByCondition(ar => ar.CityId == cityId && ar.Date.Year == DateTime.Today.Year && ar.Status != AnnualReportStatus.Canceled)
                        .FirstOrDefault();
                    if (annualReportCheck == null)
                    {
                        _repoWrapper.AnnualReports.Create(annualReport);
                        _repoWrapper.Save();
                        ViewData["Message"] = $"Звіт станиці {city.Name} за {annualReport.Date.Year} рік створено!";
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = $"Звіт станиці {city.Name} за {annualReport.Date.Year} рік вже існує!";
                    }
                    return View();
                }
                else
                {
                    var cityMembers = _repoWrapper.User
                        .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == cityId && cm.EndDate == null))
                        .Include(u => u.UserPlastDegrees);
                    var annualReportViewModel = new AnnualReportViewModel
                    {
                        CityName = city.Name,
                        CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                        CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                        AnnualReport = annualReport
                    };
                    return View(annualReportViewModel);
                }
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public IActionResult ViewAnnualReports()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var cities = _cityAccessManager.GetCities(userId);
                var annualReports = _repoWrapper.AnnualReports
                    .FindAll()
                    .Include(ar => ar.City)
                        .ThenInclude(c => c.Region)
                    .Include(ar => ar.User)
                    .ToList();
                annualReports.RemoveAll(ar => !cities.Any(c => c.ID == ar.CityId));
                var viewAnnualReportsViewModel = new ViewAnnualReportsViewModel
                {
                    AnnualReports = annualReports,
                    Cities = _viewAnnualReportsVMInitializer.GetCities(cities)
                };
                return View(viewAnnualReportsViewModel);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public IActionResult GetAnnualReport(int id)
        {
            try
            {
                var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id)
                    .Include(ar => ar.City)
                    .Include(ar => ar.MembersStatistic)
                    .Include(ar => ar.CityManagement)
                        .ThenInclude(cm => cm.User)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
                return PartialView("_GetAnnualReport", annualReport);
            }
            catch
            {
                return NotFound("Не вдалося завантажити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> ConfirmAnnualReport(int id)
        {
            try
            {
                bool whetherTheRoleShouldBeDeleted = false;
                bool whetherTheRoleShouldBeAdded = false;
                AnnualReport annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Unconfirmed)
                    .Include(ar => ar.City)
                    .Include(ar => ar.CityManagement)
                        .ThenInclude(cm => cm.User)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }

                // update annualReport status
                annualReport.Status = AnnualReportStatus.Confirmed;
                _repoWrapper.AnnualReports.Update(annualReport);

                // update oldCityAdmin EndDate
                CityAdministration cityAdminOld = _repoWrapper.CityAdministration
                        .FindByCondition(ca => ca.CityId == annualReport.CityId && ca.EndDate == null)
                        .Include(ca => ca.User)
                        .FirstOrDefault();
                if (cityAdminOld != null && annualReport.CityManagement.User != null && cityAdminOld.UserId != annualReport.CityManagement.UserId)
                {
                    cityAdminOld.EndDate = DateTime.Today;
                    _repoWrapper.CityAdministration.Update(cityAdminOld);
                    whetherTheRoleShouldBeDeleted = true;
                }

                // create newCityAdmin
                if (annualReport.CityManagement.User != null && (cityAdminOld == null || (cityAdminOld != null && cityAdminOld.EndDate != null)))
                {
                    AdminType adminType = _repoWrapper.AdminType
                        .FindByCondition(at => at.AdminTypeName == "Голова станиці")
                        .First();
                    CityAdministration cityAdminNew = new CityAdministration
                    {
                        UserId = annualReport.CityManagement.UserId,
                        CityId = annualReport.CityId,
                        AdminTypeId = adminType.ID,
                        StartDate = DateTime.Today
                    };
                    _repoWrapper.CityAdministration.Create(cityAdminNew);
                    whetherTheRoleShouldBeAdded = true;
                }

                // update oldCityLegalStatus EndDate
                CityLegalStatus cityLegalStatusOld = _repoWrapper.CityLegalStatuses
                    .FindByCondition(cls => cls.CityId == annualReport.CityId && cls.DateFinish == null)
                    .FirstOrDefault();
                if (cityLegalStatusOld != null)
                {
                    cityLegalStatusOld.DateFinish = DateTime.Today;
                    _repoWrapper.CityLegalStatuses.Update(cityLegalStatusOld);
                }

                // create newCityLegalStatus
                CityLegalStatus cityLegalStatusNew = new CityLegalStatus
                {
                    CityId = annualReport.CityId,
                    LegalStatusType = annualReport.CityManagement.CityLegalStatus,
                    DateStart = DateTime.Today
                };
                _repoWrapper.CityLegalStatuses.Create(cityLegalStatusNew);
                _repoWrapper.Save();
                if (whetherTheRoleShouldBeDeleted)
                {
                    await _userManager.RemoveFromRoleAsync(cityAdminOld.User, "Голова Станиці");
                }
                if (whetherTheRoleShouldBeAdded)
                {
                    await _userManager.AddToRoleAsync(annualReport.CityManagement.User, "Голова Станиці");
                }
                return Ok($"Звіт станиці {annualReport.City.Name} за {annualReport.Date.Year} рік підтверджено!");
            }
            catch
            {
                return NotFound("Не вдалося підтвердити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public IActionResult CancelAnnualReport(int id)
        {
            try
            {
                var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Unconfirmed)
                    .Include(ar => ar.City)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
                annualReport.Status = AnnualReportStatus.Canceled;
                _repoWrapper.AnnualReports.Update(annualReport);
                _repoWrapper.Save();
                return Ok($"Звіт станиці {annualReport.City.Name} за {annualReport.Date.Year} рік скасовано!");
            }
            catch
            {
                return NotFound("Не вдалося скасувати річний звіт!");
            }
        }
    }
}