using System;
using System.Linq;
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
    }
}