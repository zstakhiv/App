using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class ReportController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IAnnualReportVMInitializer _annualReportVMCreator;
        private readonly UserManager<User> _userManager;
        private readonly IPDFService _PDFService;

        public ReportController(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IAnnualReportVMInitializer annualReportVMCreator,
            IPDFService PDFService)

        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
            _userManager = userManager;
            _PDFService = PDFService;
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
            try
            {
                decesionViewModel.Decesion.DecesionStatus = new DecesionStatus { ID = 1, DecesionStatusName = "У розгляді" };
                _repoWrapper.Decesion.Attach(decesionViewModel.Decesion);
                _repoWrapper.Decesion.Create(decesionViewModel.Decesion);
                _repoWrapper.Save();
                return RedirectToAction("CreateRaport");
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        public IActionResult ReadRaport()
        {
            try
            {
                List<DecesionViewModel> decesions = new List<DecesionViewModel>(
                    _repoWrapper.Decesion
                    .Include(x => x.DecesionStatus, x => x.DecesionTarget, x => x.Organization)
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

        public async Task<IActionResult> UsersTable()
        {
            try
            {
                var users = _repoWrapper.User
                    .Include(x => x.UserProfile, x => x.UserPlastDegrees)
                    .ToList();

                var cities = _repoWrapper.City
                    .Include(x => x.Region)
                    .ToList();
                var clubMembers = _repoWrapper.ClubMembers.Include(x => x.Club)
                                                          .ToList();
                var cityMembers = _repoWrapper.CityMembers.Include(x => x.City)
                                                          .ToList();
                List<UserTableViewModel> userTableViewModels = new List<UserTableViewModel>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var cityName = cityMembers.Where(x => x.User.Id.Equals(user.Id) && x.EndDate == null)
                                              .Select(x => x.City.Name)
                                              .LastOrDefault() ?? string.Empty;

                    #region Delete when all users will have UserPlastDegrees automatically

                    if (user.UserPlastDegrees.Count == 0 || cityName.Equals(string.Empty))
                        continue;

                    #endregion Delete when all users will have UserPlastDegrees automatically

                    userTableViewModels.Add(new UserTableViewModel
                    {
                        User = user,
                        ClubName = clubMembers.Where(x => x.UserId.Equals(user.Id) && x.IsApproved == true)
                                              .Select(x => x.Club.ClubName).LastOrDefault() ?? string.Empty,
                        CityName = cityName,
                        RegionName = cities.Where(x => x.Name.Equals(cityName))
                                           .FirstOrDefault()
                                           .Region
                                           .RegionName ?? string.Empty,
                        UserPlastDegreeName = user.UserPlastDegrees.Where(x => x.UserId == user.Id && x.DateFinish == null)
                                                                   .FirstOrDefault()
                                                                   .UserPlastDegreeType
                                                                   .GetDescription() ?? string.Empty,
                        UserRoles = string.Join(", ", roles)
                    });
                }

                return View(userTableViewModels);
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> CreatePDFAsync(int objId)
        {
            byte[] arr = await _PDFService.DecesionCreatePDFAsync(_repoWrapper.Decesion.Include(x => x.DecesionStatus,
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