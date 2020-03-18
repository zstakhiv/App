﻿using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.ViewModels.Initialization.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.Controllers
{
    public class ReportController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;
        private readonly IAnnualReportVMInitializer _annualReportVMCreator;
        private readonly UserManager<User> _userManager;

        public ReportController(DataAccess.Repositories.IRepositoryWrapper repoWrapper, UserManager<User> userManager, IAnnualReportVMInitializer annualReportVMCreator)
        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
            _userManager = userManager;
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
                return View(new AnnualReportViewModel
                {
                    CityName = city.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = _annualReportVMCreator.GetAnnualReport(cityMembers)
                });
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
                    return View(new AnnualReportViewModel
                    {
                        CityName = city.Name,
                        CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                        CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                        AnnualReport = annualReport
                    });
                }
            }
            catch (Exception e)
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