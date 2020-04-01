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
    }
}
