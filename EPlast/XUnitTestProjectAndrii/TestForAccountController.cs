using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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

        [Fact]
        public void TestCheckingForRegisterMethod()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            //Assert.True(accountController != null);
        }
    }
}
