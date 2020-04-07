using EPlast.BussinessLayer;
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

        private const string DecesionsDocumentFolder = @"\documents\";

        public DocumentationController(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IAnnualReportVMInitializer annualReportVMCreator,
            IDecisionVMIitializer decisionVMCreator, IPDFService PDFService, IHostingEnvironment appEnvironment, IViewAnnualReportsVMInitializer viewAnnualReportsVMInitializer)

        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
            _userManager = userManager;
            _PDFService = PDFService;
            _decisionVMCreator = decisionVMCreator;
            _appEnvironment = appEnvironment;
            _viewAnnualReportsVMInitializer = viewAnnualReportsVMInitializer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public DecesionViewModel _CreateDecesion()
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveDecesionAsync(DecesionViewModel decesionViewModel)
        {
            try
            {
                if (!ModelState.IsValid && decesionViewModel.Decesion.DecesionTarget.ID != 0)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return View("CreateDecesion");
                }
                else if (decesionViewModel.File != null && decesionViewModel.File.Length > 10485760)
                {
                    ModelState.AddModelError("", "файл за великий (більше 10 Мб)");
                    return View("CreateDecesion");
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
                        Directory.CreateDirectory(path);

                        if (!Directory.Exists(path))
                        {
                            throw new ArgumentException($"directory '{path}' is not exist");
                        }

                        path = Path.Combine(path, decesionViewModel.File.FileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await decesionViewModel.File.CopyToAsync(fileStream);
                            if (!System.IO.File.Exists(path))
                            {
                                throw new ArgumentException($"File was not created it '{path}' directory");
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("HandleError", "Error");
                    }
                }

                return RedirectToAction("CreateDecesion");
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
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
                    if (!decesion.Decesion.HaveFile || !Directory.Exists(path))
                    {
                        continue;
                    }
                    var files = Directory.GetFiles(path);

                    if (files.Length == 0)
                    {
                        throw new ArgumentException($"File count in '{path}' is 0");
                    }

                    decesion.Filename = Path.GetFileName(files.First());
                }
                return View(Tuple.Create(_CreateDecesion(), decisions));
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

                if (!Directory.Exists(path) || Directory.GetFiles(path).Length == 0)
                {
                    throw new ArgumentException($"directory '{path}' is not exist");
                }
                path = Path.Combine(path, filename);
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
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
                var user = _repoWrapper.User
                .FindByCondition(u => u.Id == _userManager.GetUserId(User))
                .First();
                var adminType = _repoWrapper.AdminType
                    .FindByCondition(at => at.AdminTypeName == "Голова станиці")
                    .First();
                var city = _repoWrapper.City
                    .FindByCondition(c => c.CityAdministration
                    .Any(ca => ca.UserId == user.Id && ca.AdminTypeId == adminType.ID && ca.StartDate != null && ca.EndDate == null))
                    .First();
                var cityMembers = _repoWrapper.User
                    .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == city.ID && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var annualReportViewModel = new AnnualReportViewModel
                {
                    CityName = city.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = _annualReportVMCreator.GetAnnualReport(user.Id, city.ID, cityMembers)
                };
                return View(annualReportViewModel);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateAnnualReportAsAdmin(int cityId)
        {
            try
            {
                var user = _repoWrapper.User
                    .FindByCondition(u => u.Id == _userManager.GetUserId(User))
                    .First();
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
                    AnnualReport = _annualReportVMCreator.GetAnnualReport(user.Id, city.ID, cityMembers)
                };
                return View("CreateAnnualReport", annualReportViewModel);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin, Голова Станиці")]
        [HttpPost]
        public IActionResult CreateAnnualReport(int cityId, AnnualReport annualReport)
        {
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
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ViewAnnualReports()
        {
            try
            {
                var annualReports = _repoWrapper.AnnualReports
                    .FindAll()
                    .Include(ar => ar.City)
                        .ThenInclude(c => c.Region)
                    .Include(ar => ar.User)
                    .ToList();
                var cities = _repoWrapper.City.FindAll();
                var viewAnnualReportsViewModel = new ViewAnnualReportsViewModel
                {
                    AnnualReports = annualReports,
                    Cities = _viewAnnualReportsVMInitializer.GetCities(cities)
                };
                return View(viewAnnualReportsViewModel);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetAnnualReport(int id)
        {
            var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id)
                    .Include(ar => ar.City)
                    .Include(ar => ar.MembersStatistic)
                    .Include(ar => ar.CityManagement)
                        .ThenInclude(cm => cm.User)
                    .First();
            return PartialView("_GetAnnualReport", annualReport);
        }

        [Authorize(Roles = "Admin")]
        public async Task<string> ConfirmAnnualReport(int id)
        {
            bool whetherTheRoleShouldBeDeleted = false;
            bool whetherTheRoleShouldBeAdded = false;

            // update annualReport status
            AnnualReport annualReport = _repoWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Unconfirmed)
                .Include(ar => ar.City)
                .Include(ar => ar.CityManagement)
                    .ThenInclude(cm => cm.User)
                .First();
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
            return $"Звіт станиці {annualReport.City.Name} за {annualReport.Date.Year} рік підтверджено!";
        }

        [Authorize(Roles = "Admin")]
        public string CancelAnnualReport(int id)
        {
            var annualReport = _repoWrapper.AnnualReports
                .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Unconfirmed)
                .Include(ar => ar.City)
                .First();
            annualReport.Status = AnnualReportStatus.Canceled;
            _repoWrapper.AnnualReports.Update(annualReport);
            _repoWrapper.Save();
            return $"Звіт станиці {annualReport.City.Name} за {annualReport.Date.Year} рік скасовано!";
        }
    }
}