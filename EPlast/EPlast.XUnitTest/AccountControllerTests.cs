using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AccountControllerTests
    {
        [Fact]
        public void EditTest()
        {
            // Arrange
            var expected = new User
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfile
                {
                    Nationality = new Nationality { Name = "Українець" },
                    Religion = new Religion { Name = "Християнство" },
                    Education = new Education() { PlaceOfStudy = "ЛНУ", Speciality = "КН", Degree = new Degree { Name = "Бакалавр" } },
                    Work = new Work { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    Gender = new Gender { Name = "Чоловік" }
                }
            };
            var repository = new Mock<IRepositoryWrapper>();
            
            repository.Setup(r => r.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>{new User
            {
                FirstName = "Vova",
                LastName = "Vermii",
                UserProfile = new UserProfile
                {
                    Nationality = new Nationality { Name = "Українець" },
                    Religion = new Religion { Name = "Християнство" },
                    Education = new Education() { PlaceOfStudy = "ЛНУ", Speciality = "КН", Degree = new Degree { Name = "Бакалавр" } },
                    Work = new Work { PlaceOfwork = "SoftServe", Position = "ProjectManager" },
                    Gender = new Gender { Name = "Чоловік" }
                }
            } }.AsQueryable() );

            var userStoreMock = new Mock<IUserStore<User>>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var usManager = new Mock<UserManager<User>>(userStoreMock.Object,
                null, null, null, null, null, null, null, null);
            var signInmanager = new Mock<SignInManager<User>>(usManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
            var log = new Mock<ILogger<AccountController>>();
            var emConfrm = new Mock<IEmailConfirmation>();
            var hostEnv = new Mock<IHostingEnvironment>();

            usManager.Setup(x => x.CreateSecurityTokenAsync(expected));
            
            var controller = new AccountController(usManager.Object, signInmanager.Object, repository.Object, log.Object, emConfrm.Object, hostEnv.Object);
            var mockFile = new Mock<IFormFile>();
            // Act
            var user = new UserViewModel { User = expected };
            var result=controller.Edit(user,mockFile.Object);
            // Assert
            repository.Verify(r => r.User.Update(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public void DeletePositionTrueRemoveRoleTrueTest()
        {
            // Arrange
            var cityAdministrations = new List<CityAdministration>
            {
                new CityAdministration
                {
                    ID = 1,
                    User = new User(),
                    AdminType = new AdminType(),
                },
            };
            var repoMock = new Mock<IRepositoryWrapper>();
            repoMock.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityAdministrations.AsQueryable());
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, null, repoMock.Object, null, null, null);

            // Act
            var result = controller.DeletePosition(cityAdministrations[0].ID);

            // Assert
            Assert.True(result.Result);
            userManagerMock.Verify(u => u.RemoveFromRoleAsync(cityAdministrations[0].User, cityAdministrations[0].AdminType.AdminTypeName));
        }

        [Fact]
        public void DeletePositionTrueRemoveRoleFalseTest()
        {
            // Arrange
            var cityAdministrations = new List<CityAdministration>
            {
                new CityAdministration
                {
                    ID = 1,
                    User = new User(),
                    AdminType = new AdminType(),
                    EndDate = DateTime.Now,
                },
            };
            var repoMock = new Mock<IRepositoryWrapper>();
            repoMock.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityAdministrations.AsQueryable());
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, null, repoMock.Object, null, null, null);

            // Act
            var result = controller.DeletePosition(cityAdministrations[0].ID);

            // Assert
            Assert.True(result.Result);
            userManagerMock.Verify(u => u.RemoveFromRoleAsync(cityAdministrations[0].User, cityAdministrations[0].AdminType.AdminTypeName), Times.Never);
        }

        [Fact]
        public void DeletePositionFalseTest()
        {
            // Arrange
            var repoMock = new Mock<IRepositoryWrapper>();
            repoMock.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(new List<CityAdministration>().AsQueryable());
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, null, repoMock.Object, null, null, null);

            // Act
            var result = controller.DeletePosition(0);

            // Assert
            Assert.False(result.Result);
        }

        [Fact]
        public void EndPositionTrueTest()
        {
            // Arrange
            var cityAdministrations = new List<CityAdministration>
            {
                new CityAdministration
                {
                    ID = 1,
                    User = new User(),
                    AdminType = new AdminType(),
                    StartDate = DateTime.Now
                },
            };
            var repoMock = new Mock<IRepositoryWrapper>();
            repoMock.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(cityAdministrations.AsQueryable());
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, null, repoMock.Object, null, null, null);

            // Act
            var result = controller.EndPosition(cityAdministrations[0].ID);

            // Assert
            Assert.True(result.Result);
            Assert.NotNull(cityAdministrations[0].EndDate);
        }

        [Fact]
        public void EndPositionFalseTest()
        {
            // Arrange
            var repoMock = new Mock<IRepositoryWrapper>();
            repoMock.Setup(r => r.CityAdministration.FindByCondition(It.IsAny<Expression<Func<CityAdministration, bool>>>()))
                .Returns(new List<CityAdministration>().AsQueryable());
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var controller = new AccountController(userManagerMock.Object, null, repoMock.Object, null, null, null);

            // Act
            var result = controller.EndPosition(0);

            // Assert
            Assert.False(result.Result);
        }
    }
}
