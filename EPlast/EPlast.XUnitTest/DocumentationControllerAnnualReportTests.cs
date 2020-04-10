using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPlast.ViewModels;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.Models.ViewModelInitializations.Interfaces;
using Moq;
using Xunit;
using Newtonsoft.Json;

namespace EPlast.XUnitTest
{
    public class DocumentationControllerAnnualReportTests
    {
        private readonly Mock<IRepositoryWrapper> repositoryWrapper;
        private readonly Mock<IUserStore<User>> userStore;
        private readonly Mock<UserManager<User>> userManager;
        private readonly IAnnualReportVMInitializer annualReportVMInitializer;
        private readonly IViewAnnualReportsVMInitializer viewAnnualReportsVMInitializer;
        private readonly Mock<ICityAccessManager> cityAccessManager;

        public DocumentationControllerAnnualReportTests()
        {
            repositoryWrapper = new Mock<IRepositoryWrapper>();
            userStore = new Mock<IUserStore<User>>();
            userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            annualReportVMInitializer = new AnnualReportVMInitializer();
            viewAnnualReportsVMInitializer = new ViewAnnualReportsVMInitializer();
            cityAccessManager = new Mock<ICityAccessManager>();
        }

        [Fact]
        public void CreateAnnualReportHttpGetCorrect()
        {
            // Arrange
            var user = new User
            {
                Id = "0",
            };
            var cities = new List<City>
            {
                new City { ID = 1, Name = "Золочів" }
            };
            var users = new List<User>
            {
                new User { Id = "1", FirstName = "Роман", LastName = "Романенко", UserPlastDegrees = new List<UserPlastDegree> 
                    { new UserPlastDegree { UserPlastDegreeType = UserPlastDegreeType.SeniorPlastynSupporter } } },
                new User { Id = "2", FirstName = "Петро", LastName = "Петренко", UserPlastDegrees = new List<UserPlastDegree>
                    { new UserPlastDegree { UserPlastDegreeType = UserPlastDegreeType.SeniorPlastynSupporter } } },
                new User { Id = "3", FirstName = "Степан", LastName = "Степаненко", UserPlastDegrees = new List<UserPlastDegree>
                    { new UserPlastDegree { UserPlastDegreeType = UserPlastDegreeType.SeigneurMember } } }
            };
            var expectedViewModel = new AnnualReportViewModel
            {
                CityName = cities[0].Name,
                CityMembers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "" },
                    new SelectListItem { Value = "1", Text = "Роман Романенко" },
                    new SelectListItem { Value = "2", Text = "Петро Петренко" },
                    new SelectListItem { Value = "3", Text = "Степан Степаненко" },
                },
                CityLegalStatusTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "RegisteredLegalEntity", Text = "Зареєстрована юридична особа" },
                    new SelectListItem { Value = "LegalizedByMessage", Text = "Легалізована шляхом повідомлення" },
                    new SelectListItem { Value = "NotLegalizedInByLocalAuthorities", Text = "Нелегалізована у місцевих органах влади" },
                    new SelectListItem { Value = "InTheProcessOfLegalization", Text = "В процесі легалізації/реєстрації" },
                    new SelectListItem { Value = "RegisteredSeparatedSubdivision", Text = "Зареєстрований відокремлений підрозділ" }
                },
                AnnualReport = new AnnualReport
                {
                    UserId = user.Id,
                    CityId = cities[0].ID,
                    MembersStatistic = new MembersStatistic
                    {
                        NumberOfSeniorPlastynSupporters = 2,
                        NumberOfSeigneurMembers = 1
                    }
                }
            };
            userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(user.Id);
            cityAccessManager.Setup(cam => cam.GetCities(It.IsAny<string>())).Returns(cities);
            repositoryWrapper.Setup(rw => rw.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(users.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null,
                null, null, cityAccessManager.Object);

            // Act
            var result = controller.CreateAnnualReport();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = Assert.IsAssignableFrom<AnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel),
                JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public void CreateAnnualReportHttpGetCorrectCityMembersEmpty()
        {
            // Arrange
            var user = new User
            {
                Id = "0",
            };
            var cities = new List<City>
            {
                new City { ID = 1, Name = "Золочів" }
            };
            var expectedViewModel = new AnnualReportViewModel
            {
                CityName = cities[0].Name,
                CityMembers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "" }
                },
                CityLegalStatusTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "RegisteredLegalEntity", Text = "Зареєстрована юридична особа" },
                    new SelectListItem { Value = "LegalizedByMessage", Text = "Легалізована шляхом повідомлення" },
                    new SelectListItem { Value = "NotLegalizedInByLocalAuthorities", Text = "Нелегалізована у місцевих органах влади" },
                    new SelectListItem { Value = "InTheProcessOfLegalization", Text = "В процесі легалізації/реєстрації" },
                    new SelectListItem { Value = "RegisteredSeparatedSubdivision", Text = "Зареєстрований відокремлений підрозділ" }
                },
                AnnualReport = new AnnualReport
                {
                    UserId = user.Id,
                    CityId = cities[0].ID,
                    MembersStatistic = new MembersStatistic()
                }
            };
            userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(user.Id);
            cityAccessManager.Setup(cam => cam.GetCities(It.IsAny<string>())).Returns(cities);
            repositoryWrapper.Setup(rw => rw.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Enumerable.Empty<User>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null,
                null, null, cityAccessManager.Object);

            // Act
            var result = controller.CreateAnnualReport();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = Assert.IsAssignableFrom<AnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel),
                JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public void CreateAnnualReportHttpGetIncorrectCityEmpty()
        {
            // Arrange
            cityAccessManager.Setup(cam => cam.GetCities(It.IsAny<string>()))
                .Returns(Enumerable.Empty<City>());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null,
                null, null, cityAccessManager.Object);

            // Act
            var result = (RedirectToActionResult)controller.CreateAnnualReport();

            // Assert
            Assert.Equal("HandleError", result.ActionName);
            Assert.Equal("Error", result.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
            cityAccessManager.Verify(cam => cam.GetCities(It.IsAny<string>()));
            repositoryWrapper.Verify(rw => rw.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()), Times.Never);
        }

        [Fact]
        public void CreateAnnualReportAsAdminCorrect()
        {
            // Arrange
            var cities = new List<City>
            {
                new City { ID = 1, Name = "Золочів" }
            };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()))
                .Returns(cities.AsQueryable());
            repositoryWrapper.Setup(rw => rw.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Enumerable.Empty<User>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null,
                null, null, cityAccessManager.Object);

            // Act
            var result = controller.CreateAnnualReportAsAdmin(cities[0].ID);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<AnnualReportViewModel>(viewResult.Model);
            Assert.NotNull(viewModel);
        }

        [Fact]
        public void CreateAnnualReportAsAdminIncorrectCityEmpty()
        {
            // Arrange
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()))
                .Returns(Enumerable.Empty<City>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null,
                null, null, cityAccessManager.Object);

            // Act
            var result = (RedirectToActionResult)controller.CreateAnnualReportAsAdmin(0);

            // Assert
            Assert.Equal("HandleError", result.ActionName);
            Assert.Equal("Error", result.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
            cityAccessManager.Verify(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()));
            repositoryWrapper.Verify(rw => rw.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()), Times.Never);
        }

        [Fact]
        public void CreateAnnualReportAsAdminIncorrectHasNoAccess()
        {
            // Arrange
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);
            repositoryWrapper.Setup(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()))
                .Returns(Enumerable.Empty<City>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null,
                null, null, cityAccessManager.Object);

            // Act
            var result = (RedirectToActionResult)controller.CreateAnnualReportAsAdmin(0);

            // Assert
            Assert.Equal("HandleError", result.ActionName);
            Assert.Equal("Error", result.ControllerName);
            Assert.Equal(403, result.RouteValues["code"]);
            repositoryWrapper.Verify(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()), Times.Never);
        }

        [Fact]
        public void CreateAnnualReportHttpPostCorrectIsValid()
        {
            // Arrange
            var cities = new List<City>
            {
                new City { ID = 1, Name = "Золочів" }
            };
            var annualReport = new AnnualReport()
            {
                CityManagement = new CityManagement(),
                MembersStatistic = new MembersStatistic()
            };
            userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(string.Empty);
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()))
                .Returns(cities.AsQueryable());
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(Enumerable.Empty<AnnualReport>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = controller.CreateAnnualReport(cities[0].ID, annualReport);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal($"Звіт станиці {cities[0].Name} за {annualReport.Date.Year} рік створено!", viewResult.ViewData["Message"]);
        }

        [Fact]
        public void CreateAnnualReportHttpPostIsValidNotCreate()
        {
            // Arrange
            var cities = new List<City>
            {
                new City { ID = 1, Name = "Золочів" }
            };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport()
                {
                    CityManagement = new CityManagement(),
                    MembersStatistic = new MembersStatistic()
                }
            };
            userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(string.Empty);
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()))
                .Returns(cities.AsQueryable());
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);
            controller.ModelState.Clear();

            // Act
            var result = controller.CreateAnnualReport(cities[0].ID, annualReports[0]);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal($"Звіт станиці {cities[0].Name} за {annualReports[0].Date.Year} рік вже існує!", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public void CreateAnnualReportHttpPostCorrectIsInvalid()
        {
            // Arrange
            var cities = new List<City>
            {
                new City { ID = 1, Name = "Золочів" }
            };
            var annualReport = new AnnualReport()
            {
                CityManagement = new CityManagement(),
                MembersStatistic = new MembersStatistic(),
                NumberOfAdministrators = -1
            };
            userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(string.Empty);
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()))
                .Returns(cities.AsQueryable());
            repositoryWrapper.Setup(rw => rw.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Enumerable.Empty<User>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null, null, null, cityAccessManager.Object);
            controller.ModelState.AddModelError("test", "test");

            // Act
            var result = controller.CreateAnnualReport(cities[0].ID, annualReport);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<AnnualReportViewModel>(viewResult.Model);
            Assert.NotNull(viewModel);
            repositoryWrapper.Verify(rw => rw.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()));
        }

        [Fact]
        public void CreateAnnualReportHttpPostIncorrectHasNoAccess()
        {
            // Arrange
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null,
                null, null, cityAccessManager.Object);

            // Act
            var result = (RedirectToActionResult)controller.CreateAnnualReport(0, null);

            // Assert
            Assert.Equal("HandleError", result.ActionName);
            Assert.Equal("Error", result.ControllerName);
            Assert.Equal(403, result.RouteValues["code"]);
            repositoryWrapper.Verify(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()), Times.Never);
        }

        [Fact]
        public void CreateAnnualReportHttpPostIncorrectCitiesEmpty()
        {
            // Arrange
            userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(string.Empty);
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.City.FindByCondition(It.IsAny<Expression<Func<City, bool>>>()))
                .Returns(Enumerable.Empty<City>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, annualReportVMInitializer, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = (RedirectToActionResult)controller.CreateAnnualReport(0, null);

            // Assert
            Assert.Equal("HandleError", result.ActionName);
            Assert.Equal("Error", result.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
        }

        [Fact]
        public void ViewAnnualReportsCorrect()
        {
            // Arrange
            var cities = new List<City>
            {
                new City { ID = 1, Name = "Золочів" },
                new City { ID = 2, Name = "Перемишляни" }
            };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport { ID = 1, CityId = 1 },
                new AnnualReport { ID = 1, CityId = 2 },
                new AnnualReport { ID = 1, CityId = 3 },
                new AnnualReport { ID = 1, CityId = 4 },
            };
            var expectedViewModel = new ViewAnnualReportsViewModel
            {
                AnnualReports = new List<AnnualReport> { annualReports[0], annualReports[1] },
                Cities = viewAnnualReportsVMInitializer.GetCities(cities)
            };
            cityAccessManager.Setup(cam => cam.GetCities(It.IsAny<string>()))
                .Returns(cities);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindAll())
                .Returns(annualReports.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, viewAnnualReportsVMInitializer, cityAccessManager.Object);

            // Act
            var result = controller.ViewAnnualReports();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = Assert.IsAssignableFrom<ViewAnnualReportsViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel),
                JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public void ViewAnnualReportsCorrectCitiesEmpty()
        {
            // Arrange
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport { ID = 1, CityId = 1 },
                new AnnualReport { ID = 1, CityId = 2 },
                new AnnualReport { ID = 1, CityId = 3 },
                new AnnualReport { ID = 1, CityId = 4 },
            };
            var expectedViewModel = new ViewAnnualReportsViewModel
            {
                AnnualReports = Enumerable.Empty<AnnualReport>(),
                Cities = viewAnnualReportsVMInitializer.GetCities(Enumerable.Empty<City>())
            };
            cityAccessManager.Setup(cam => cam.GetCities(It.IsAny<string>()))
                .Returns(Enumerable.Empty<City>());
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindAll())
                .Returns(annualReports.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, viewAnnualReportsVMInitializer, cityAccessManager.Object);

            // Act
            var result = controller.ViewAnnualReports();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = Assert.IsAssignableFrom<ViewAnnualReportsViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel),
                JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public void ViewAnnualReportsCorrectCitiesNull()
        {
            // Arrange
            cityAccessManager.Setup(cam => cam.GetCities(It.IsAny<string>()))
                .Returns((IQueryable<City>)null);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindAll())
                .Returns(Enumerable.Empty<AnnualReport>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, viewAnnualReportsVMInitializer, cityAccessManager.Object);

            // Act
            var result = (RedirectToActionResult)controller.ViewAnnualReports();

            // Assert
            Assert.Equal("HandleError", result.ActionName);
            Assert.Equal("Error", result.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
        }

        [Fact]
        public void GetAnnualReportCorrect()
        {
            // Arrange
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport { ID = 1, CityId = 1 },
                new AnnualReport { ID = 1, CityId = 2 }
            };
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = controller.GetAnnualReport(1);

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_GetAnnualReport", viewResult.ViewName);
            var actualViewModel = Assert.IsAssignableFrom<AnnualReport>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(annualReports[0]),
                JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public void GetAnnualReportIncorrectHasNoAccess()
        {
            // Arrange
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport { ID = 1, CityId = 1 },
                new AnnualReport { ID = 1, CityId = 2 }
            };
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = (RedirectToActionResult)controller.GetAnnualReport(1);

            // Assert
            Assert.Equal("HandleError", result.ActionName);
            Assert.Equal("Error", result.ControllerName);
            Assert.Equal(403, result.RouteValues["code"]);
        }

        [Fact]
        public void GetAnnualReportIncorrectAnnualReportsEmpty()
        {
            // Arrange
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(Enumerable.Empty<AnnualReport>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = controller.GetAnnualReport(1);

            // Assert
            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFoundRequest.Value);
            Assert.Equal("Не вдалося завантажити річний звіт!", message);
        }

        [Fact]
        public async Task ConfirmAnnualReportCorrectWithoutAdmins()
        {
            // Arrange
            var city = new City { Name = "Золочів" };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport() { City = city, Date = DateTime.Now, CityManagement = new CityManagement() },
            };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(Enumerable.Empty<CityAdministration>().AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityLegalStatuses.FindByCondition(It.IsAny<Expression<Func<CityLegalStatus, bool>>>()))
                .Returns(Enumerable.Empty<CityLegalStatus>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var okRequest = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okRequest.Value);
            Assert.Equal($"Звіт станиці {annualReports[0].City.Name} за {annualReports[0].Date.Year} рік підтверджено!", message);
            Assert.Equal(AnnualReportStatus.Confirmed, annualReports[0].Status);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Update(It.IsAny<CityAdministration>()), Times.Never);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Create(It.IsAny<CityAdministration>()), Times.Never);
            userManager.Verify(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            userManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAnnualReportCorrectAdminOldNewAdminEmpty()
        {
            // Arrange
            var city = new City { Name = "Золочів" };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport() { City = city, Date = DateTime.Now, CityManagement = new CityManagement() },
            };
            var cityOldAdmins = new List<CityAdministration>
            {
                new CityAdministration()
            };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityOldAdmins.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityLegalStatuses.FindByCondition(It.IsAny<Expression<Func<CityLegalStatus, bool>>>()))
                .Returns(Enumerable.Empty<CityLegalStatus>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var okRequest = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okRequest.Value);
            Assert.Equal($"Звіт станиці {annualReports[0].City.Name} за {annualReports[0].Date.Year} рік підтверджено!", message);
            Assert.Equal(AnnualReportStatus.Confirmed, annualReports[0].Status);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Update(It.IsAny<CityAdministration>()), Times.Never);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Create(It.IsAny<CityAdministration>()), Times.Never);
            userManager.Verify(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            userManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAnnualReportCorrectAdminOldEqualNewAdmin()
        {
            // Arrange
            var user = new User();
            var city = new City { Name = "Золочів" };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport() { City = city, Date = DateTime.Now, CityManagement = new CityManagement {  User = user } },
            };
            var cityOldAdmins = new List<CityAdministration>
            {
                new CityAdministration { User = user }
            };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityOldAdmins.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityLegalStatuses.FindByCondition(It.IsAny<Expression<Func<CityLegalStatus, bool>>>()))
                .Returns(Enumerable.Empty<CityLegalStatus>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var okRequest = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okRequest.Value);
            Assert.Equal($"Звіт станиці {annualReports[0].City.Name} за {annualReports[0].Date.Year} рік підтверджено!", message);
            Assert.Equal(AnnualReportStatus.Confirmed, annualReports[0].Status);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Update(It.IsAny<CityAdministration>()), Times.Never);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Create(It.IsAny<CityAdministration>()), Times.Never);
            userManager.Verify(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            userManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAnnualReportCorrectChangeAdmin()
        {
            // Arrange
            var city = new City { Name = "Золочів" };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport() { City = city, Date = DateTime.Now, CityManagement = new CityManagement {  UserId = "1", User = new User() } },
            };
            var cityOldAdmins = new List<CityAdministration>
            {
                new CityAdministration { UserId = "2" }
            };
            var adminTypes = new List<AdminType> { new AdminType() };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityOldAdmins.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityLegalStatuses.FindByCondition(It.IsAny<Expression<Func<CityLegalStatus, bool>>>()))
                .Returns(Enumerable.Empty<CityLegalStatus>().AsQueryable());
            repositoryWrapper.Setup(rw => rw.AdminType.FindByCondition(It.IsAny<Expression<Func<AdminType, bool>>>()))
                .Returns(adminTypes.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var okRequest = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okRequest.Value);
            Assert.Equal($"Звіт станиці {annualReports[0].City.Name} за {annualReports[0].Date.Year} рік підтверджено!", message);
            Assert.Equal(AnnualReportStatus.Confirmed, annualReports[0].Status);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Update(It.IsAny<CityAdministration>()));
            repositoryWrapper.Verify(rw => rw.CityAdministration.Create(It.IsAny<CityAdministration>()));
            userManager.Verify(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task ConfirmAnnualReportCorrectNewAdminOldAdminEmpty()
        {
            // Arrange
            var city = new City { Name = "Золочів" };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport() { City = city, Date = DateTime.Now, CityManagement = new CityManagement {  UserId = "1", User = new User() } },
            };
            var adminTypes = new List<AdminType> { new AdminType() };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(Enumerable.Empty<CityAdministration>().AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityLegalStatuses.FindByCondition(It.IsAny<Expression<Func<CityLegalStatus, bool>>>()))
                .Returns(Enumerable.Empty<CityLegalStatus>().AsQueryable());
            repositoryWrapper.Setup(rw => rw.AdminType.FindByCondition(It.IsAny<Expression<Func<AdminType, bool>>>()))
                .Returns(adminTypes.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var okRequest = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okRequest.Value);
            Assert.Equal($"Звіт станиці {annualReports[0].City.Name} за {annualReports[0].Date.Year} рік підтверджено!", message);
            Assert.Equal(AnnualReportStatus.Confirmed, annualReports[0].Status);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Update(It.IsAny<CityAdministration>()), Times.Never);
            repositoryWrapper.Verify(rw => rw.CityAdministration.Create(It.IsAny<CityAdministration>()));
            userManager.Verify(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            userManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task ConfirmAnnualReportCorrectCityStatusOld()
        {
            // Arrange
            var city = new City { Name = "Золочів" };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport() { City = city, Date = DateTime.Now, CityManagement = new CityManagement() },
            };
            var cityOldLegalStatuses = new List<CityLegalStatus>
            {
                new CityLegalStatus()
            };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(Enumerable.Empty<CityAdministration>().AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityLegalStatuses.FindByCondition(It.IsAny<Expression<Func<CityLegalStatus, bool>>>()))
                .Returns(cityOldLegalStatuses.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var okRequest = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okRequest.Value);
            Assert.Equal($"Звіт станиці {annualReports[0].City.Name} за {annualReports[0].Date.Year} рік підтверджено!", message);
            Assert.Equal(AnnualReportStatus.Confirmed, annualReports[0].Status);
            repositoryWrapper.Verify(rw => rw.CityLegalStatuses.Update(It.IsAny<CityLegalStatus>()));
            repositoryWrapper.Verify(rw => rw.CityLegalStatuses.Create(It.IsAny<CityLegalStatus>()));
        }

        [Fact]
        public async Task ConfirmAnnualReportCorrectCityStatusOldEmpty()
        {
            // Arrange
            var city = new City { Name = "Золочів" };
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport() { City = city, Date = DateTime.Now, CityManagement = new CityManagement() },
            };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(Enumerable.Empty<CityAdministration>().AsQueryable());
            repositoryWrapper.Setup(rw => rw.CityLegalStatuses.FindByCondition(It.IsAny<Expression<Func<CityLegalStatus, bool>>>()))
                .Returns(Enumerable.Empty<CityLegalStatus>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var okRequest = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okRequest.Value);
            Assert.Equal($"Звіт станиці {annualReports[0].City.Name} за {annualReports[0].Date.Year} рік підтверджено!", message);
            Assert.Equal(AnnualReportStatus.Confirmed, annualReports[0].Status);
            repositoryWrapper.Verify(rw => rw.CityLegalStatuses.Update(It.IsAny<CityLegalStatus>()), Times.Never);
            repositoryWrapper.Verify(rw => rw.CityLegalStatuses.Create(It.IsAny<CityLegalStatus>()));
        }

        [Fact]
        public async Task ConfirmAnnualReportIncorrectAnnualReportNotFound()
        {
            // Arrange
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(Enumerable.Empty<AnnualReport>().AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFoundRequest.Value);
            Assert.Equal("Не вдалося підтвердити річний звіт!", message);
            userManager.Verify(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAnnualReportIncorrectHasNoAccess()
        {
            // Arrange
            var annualReports = new List<AnnualReport>
            {
                new AnnualReport()
            };
            cityAccessManager.Setup(cam => cam.HasAccess(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);
            repositoryWrapper.Setup(rw => rw.AnnualReports.FindByCondition(It.IsAny<Expression<Func<AnnualReport, bool>>>()))
                .Returns(annualReports.AsQueryable());
            var controller = new DocumentationController(repositoryWrapper.Object, userManager.Object, null, null, null, null, null, cityAccessManager.Object);

            // Act
            var result = await controller.ConfirmAnnualReport(0);

            // Assert
            var resultRequest = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", resultRequest.ActionName);
            Assert.Equal("Error", resultRequest.ControllerName);
            Assert.Equal(403, resultRequest.RouteValues["code"]);
            repositoryWrapper.Verify(rw => rw.AnnualReports.Update(It.IsAny<AnnualReport>()), Times.Never);
        }
    }
}