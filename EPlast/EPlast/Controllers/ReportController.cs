using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.ViewModels.Initialization.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.Controllers
{
    public class ReportController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;
        private readonly IAnnualReportVMInitializer _annualReportVMCreator;

        public ReportController(DataAccess.Repositories.IRepositoryWrapper repoWrapper, IAnnualReportVMInitializer annualReportVMCreator)
        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateRaport()
        {
            return View(new DecesionViewModel());
        }

        [HttpPost]
        public IActionResult SaveReport(DecesionViewModel decesionViewModel)
        {
            decesionViewModel.Decesion.DecesionStatus = new DecesionStatus { ID = 1, DecesionStatusName = "У розгляді" };
            _repoWrapper.Decesion.Attach(decesionViewModel.Decesion);
            _repoWrapper.Decesion.Create(decesionViewModel.Decesion);
            _repoWrapper.Save();
            return RedirectToAction("CreateRaport");
        }

        public IActionResult ReadRaport()
        {
            List<DecesionViewModel> decesions = new List<DecesionViewModel>(
                _repoWrapper.Decesion
                .Include(x => x.DecesionStatus, x => x.DecesionTarget, x => x.Organization)
                .Take(200)
                .Select(decesion => new DecesionViewModel { Decesion = decesion })
                .ToList());

            return View(decesions);
        }

        [HttpGet]
        public async Task<ActionResult> CreatePDFAsync(int objId)
        {
            BussinessLayer.PDFService PDFService = new BussinessLayer.PDFService();
            byte[] arr = await PDFService.DecesionCreatePDFAsync(_repoWrapper.Decesion.Include(x => x.DecesionStatus,
                                                                                       x => x.DecesionTarget,
                                                                                       x => x.Organization).Where(x => x.ID == objId).FirstOrDefault());
            return File(arr, "application/pdf");
        }

        [HttpGet]
        public IActionResult CreateAnnualReport(string userId, int cityId)
        {
            try
            {
                var user = _repoWrapper.User
                .FindByCondition(u => u.Id == userId)
                .First();
                var city = _repoWrapper.City
                    .FindByCondition(c => c.ID == cityId)
                    .First();
                var cityMembers = _repoWrapper.User
                    .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == cityId && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var cityLegalStatusTypes = _repoWrapper.CityLegalStatusTypes
                    .FindAll();
                return View(new AnnualReportViewModel
                {
                    CityName = city.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(cityLegalStatusTypes),
                    AnnualReport = _annualReportVMCreator.GetAnnualReport(cityMembers)
                });
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [HttpPost]
        public IActionResult CreateAnnualReport(string userId, int cityId, CityAdministration cityAdministration, CityLegalStatus cityLegalStatus, AnnualReport annualReport)
        {
            try
            {
                var user = _repoWrapper.User
                    .FindByCondition(u => u.Id == userId)
                    .FirstOrDefault();
                var city = _repoWrapper.City
                    .FindByCondition(c => c.ID == cityId)
                    .FirstOrDefault();
                cityAdministration.CityId = cityId;
                cityAdministration.AdminTypeId = 1;
                cityLegalStatus.CityId = cityId;
                annualReport.UserId = userId;
                annualReport.CityId = cityId;
                annualReport.AnnualReportStatusId = 1;
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
                        .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == cityId && cm.EndDate == null))
                        .Include(u => u.UserPlastDegrees);
                    var cityLegalStatusTypes = _repoWrapper.CityLegalStatusTypes
                        .FindAll();
                    return View(new AnnualReportViewModel
                    {
                        CityName = city.Name,
                        CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                        CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(cityLegalStatusTypes),
                        AnnualReport = annualReport
                    });
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
                return View(new ViewAnnualReportsViewModel
                {
                    Regions = regions,
                    Years = years,
                    UnconfirmedAnnualReports = _repoWrapper.AnnualReports
                        .FindByCondition(ar => ar.AnnualReportStatusId == 1)
                        .Include(ar => ar.City)
                        .Include(ar => ar.User)
                        .Include(ar => ar.MembersStatistic),
                    ConfirmedAnnualReports = _repoWrapper.AnnualReports
                        .FindByCondition(ar => ar.AnnualReportStatusId == 2)
                        .Include(ar => ar.City)
                        .Include(ar => ar.User)
                        .Include(ar => ar.MembersStatistic),
                    CanceledAnnualReports = _repoWrapper.AnnualReports
                        .FindByCondition(ar => ar.AnnualReportStatusId == 3)
                        .Include(ar => ar.City)
                        .Include(ar => ar.User)
                        .Include(ar => ar.MembersStatistic),
                });
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }
    }
}