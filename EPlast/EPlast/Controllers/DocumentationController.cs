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

        private const string _decesionsDocumentFolder = @"\documents\";

        public DocumentationController(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IAnnualReportVMInitializer annualReportVMCreator,
            IDecisionVMIitializer decisionVMCreator, IPDFService PDFService, IHostingEnvironment appEnvironment)

        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
            _userManager = userManager;
            _PDFService = PDFService;
            _decisionVMCreator = decisionVMCreator;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateDecesion()
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

            return View(decesionViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveDecesionAsync(DecesionViewModel decesionViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return View("CreateDecesion");
                }
                else if (decesionViewModel.File != null && decesionViewModel.File.Length > 10485760)
                {
                    ModelState.AddModelError("", "файл за великий (більше 10 мб)");
                    return View("CreateDecesion");
                }

                decesionViewModel.Decesion.HaveFile = decesionViewModel.File != null ? true : false;

                _repoWrapper.Decesion.Attach(decesionViewModel.Decesion);
                _repoWrapper.Decesion.Create(decesionViewModel.Decesion);
                _repoWrapper.Save();

                if (decesionViewModel.Decesion.HaveFile)
                {
                    string path = _appEnvironment.WebRootPath + _decesionsDocumentFolder + decesionViewModel.Decesion.ID;
                    Directory.CreateDirectory(path);
                    using (var fileStream = new FileStream(Path.Combine(path, decesionViewModel.File.FileName), FileMode.Create))
                    {
                        await decesionViewModel.File.CopyToAsync(fileStream);
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
                List<DecesionViewModel> decesions = new List<DecesionViewModel>(
                    _repoWrapper.Decesion
                    .Include(x => x.DecesionTarget, x => x.Organization)
                    .Take(200)
                    .Select(decesion => new DecesionViewModel { Decesion = decesion })
                    .ToList());

                return View(decesions);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> CreatePDFAsync(int objId)
        {
            try
            {
                if (objId <= 0)
                {
                    throw new ArgumentException();
                }

                byte[] arr = await _PDFService.DecesionCreatePDFAsync(_repoWrapper.Decesion.Include(x => x.DecesionTarget,
                                                                                                    x => x.Organization).Where(x => x.ID == objId)
                                                                                                                        .FirstOrDefault());
                return File(arr, "application/pdf");
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [HttpGet]
        public IActionResult CreateAnnualReport()
        {
            try
            {
                var user = _repoWrapper.User
                .FindByCondition(u => u.Id == _userManager.GetUserId(User))
                .First();
                var adminType = _repoWrapper.AdminType
                    .FindByCondition(at => at.AdminTypeName == "admin")
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
                    AnnualReport = _annualReportVMCreator.GetAnnualReport(cityMembers)
                };
                return View(annualReportViewModel);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [HttpPost]
        public IActionResult CreateAnnualReport(CityAdministration cityAdministration, CityLegalStatus cityLegalStatus, AnnualReport annualReport)
        {
            try
            {
                var user = _repoWrapper.User
                    .FindByCondition(u => u.Id == _userManager.GetUserId(User))
                    .First();
                var adminType = _repoWrapper.AdminType
                    .FindByCondition(at => at.AdminTypeName == "admin")
                    .First();
                var city = _repoWrapper.City
                    .FindByCondition(c => c.CityAdministration.Any(ca => ca.UserId == user.Id && ca.AdminTypeId == adminType.ID && ca.EndDate == null))
                    .First();
                cityAdministration.CityId = city.ID;
                cityAdministration.AdminTypeId = adminType.ID;
                cityLegalStatus.CityId = city.ID;
                annualReport.UserId = user.Id;
                annualReport.CityId = city.ID;
                annualReport.Status = AnnualReportStatus.Unconfirmed;
                annualReport.Date = DateTime.Today;
                if (ModelState.IsValid)
                {
                    _repoWrapper.CityAdministration.Create(cityAdministration);
                    _repoWrapper.CityLegalStatuses.Create(cityLegalStatus);
                    _repoWrapper.AnnualReports.Create(annualReport);
                    _repoWrapper.Save();
                    return RedirectToAction("ViewAnnualReports", "Report");
                }
                else
                {
                    var cityMembers = _repoWrapper.User
                        .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == city.ID && cm.EndDate == null))
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

        public IActionResult ViewAnnualReports()
        {
            try
            {
                var regions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "all", Text = "Всі регіони" }
                };
                foreach (var region in _repoWrapper.Region.FindAll())
                {
                    regions.Add(new SelectListItem
                    {
                        Value = region.ID.ToString(),
                        Text = region.RegionName
                    });
                }
                var years = new List<SelectListItem>
                {
                    new SelectListItem { Value = "all", Text = "Всі роки" }
                };
                for (int i = 2020; i < DateTime.Today.Year + 1; i++)
                {
                    years.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
                var annualReports = _repoWrapper.AnnualReports
                    .FindAll()
                    .Include(ar => ar.City)
                    .Include(ar => ar.User)
                    .Include(ar => ar.MembersStatistic)
                    .ToList();
                return View(new ViewAnnualReportsViewModel
                {
                    Regions = regions,
                    Years = years,
                    UnconfirmedAnnualReports = annualReports
                        .FindAll(ar => ar.Status == AnnualReportStatus.Unconfirmed),
                    ConfirmedAnnualReports = annualReports
                        .FindAll(ar => ar.Status == AnnualReportStatus.Confirmed),
                    CanceledAnnualReports = annualReports
                        .FindAll(ar => ar.Status == AnnualReportStatus.Canceled)
                });
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }
    }
}