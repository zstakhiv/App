using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class TestForAccountController
    {
        public (Mock<SignInManager<User>>, Mock<UserManager<User>>, Mock<IEmailConfirmation>, AccountController) CreateAccountController()
        {
            Mock<IUserStore<User>> mockUserStore = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(mockUserStore.Object,
                null, null, null, null, null, null, null, null);

            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null,null);

            Mock<IRepositoryWrapper> mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            Mock<IEmailConfirmation> mockEmailConfirmation = new Mock<IEmailConfirmation>();
            Mock<IHostingEnvironment> mockHosting = new Mock<IHostingEnvironment>();
            AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object,
                mockRepositoryWrapper.Object, mockLogger.Object, mockEmailConfirmation.Object, mockHosting.Object);
            return (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController);
        }

        /*[Fact]
        public void TestCheckingIfUserIsNotNull()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var registerViewModel = new RegisterViewModel
            {
                Email = "andriishainoha@gmail.com",
                Name = "Andrii",
                SurName = "Shainoha",
                Password = "testpassword123",
                ConfirmPassword = "testpassword123"
            };
            var result = accountController.Register(registerViewModel);
            Assert.Equal(result, registerViewModel.Email);
        }*/

        /*[Fact]
        public async Task TestRegisterMethodViewNameEqualRegisterPost()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            
            var registerViewModel = new RegisterViewModel
            {
                Email = "andriishainoha@gmail.com",
                Name = "Andrii",
                SurName = "Shainoha",
                Password = "testpassword123",
                ConfirmPassword = "testpassword123"
            };
            accountController.ModelState.AddModelError("", "Required");
            var result = await accountController.Register(registerViewModel);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void TestRegisterMethodViewNameEqualRegisterGet()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var result = accountController.Register();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }*/

        //return View("AcceptingEmail");

        [Fact]
        public async Task TestAcceptingEmailInRegisterMethod()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var registerViewModel = new RegisterViewModel
            {
                Email = "andriishainoha@gmail.com",
                Name = "Andrii",
                SurName = "Shainoha",
                Password = "testpassword123",
                ConfirmPassword = "testpassword123"
            };

            var result = await accountController.Register(registerViewModel);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AcceptingEmail", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }
    }
}
