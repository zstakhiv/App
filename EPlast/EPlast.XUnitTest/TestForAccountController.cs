using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Entities;
using Moq;
using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using Microsoft.AspNetCore.Http;
using EPlast.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.XUnitTest
{
    public class TestForAccountController
    {
        public (Mock<SignInManager<User>>, Mock<UserManager<User>>, Mock<IEmailConfirmation>, AccountController) CreateAccountController()
        {
            Mock<IUserPasswordStore<User>> userPasswordStore = new Mock<IUserPasswordStore<User>>();
            userPasswordStore.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
           .Returns(Task.FromResult(IdentityResult.Success));
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();

            idOptions.SignIn.RequireConfirmedEmail = true;
            idOptions.Password.RequireDigit = true;
            idOptions.Password.RequiredLength = 8;
            idOptions.Password.RequireUppercase = false;
            idOptions.User.RequireUniqueEmail = true;
            idOptions.Password.RequireNonAlphanumeric = false;
            idOptions.Lockout.MaxFailedAccessAttempts = 5;
            idOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<User>>();
            UserValidator<User> validator = new UserValidator<User>();
            userValidators.Add(validator);

            var passValidator = new PasswordValidator<User>();
            var pwdValidators = new List<IPasswordValidator<User>>();
            pwdValidators.Add(passValidator);

            var mockUserManager = new Mock<UserManager<User>>(userPasswordStore.Object,
                options.Object, new PasswordHasher<User>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<User>>>().Object);

            mockUserManager.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
           .Returns(Task.FromResult(IdentityResult.Success));

            mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<User>()));

            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null, null);

            Mock<IRepositoryWrapper> mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            Mock<IEmailConfirmation> mockEmailConfirmation = new Mock<IEmailConfirmation>();
            Mock<IHostingEnvironment> mockHosting = new Mock<IHostingEnvironment>();
            AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object,
                mockRepositoryWrapper.Object, mockLogger.Object, mockEmailConfirmation.Object, mockHosting.Object);
            return (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController);
        }

        [Fact]
        public async Task TestRegisterMethodViewNameEqualRegisterAndNotNull()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var result = await accountController.Register(GetTestUserForRegistration());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
            Assert.False(accountController.TryValidateModel(GetTestUserForRegistration()));
        }

        private RegisterViewModel GetTestUserForRegistration()
        {
            var registerViewModel = new RegisterViewModel
            {
                Email = "Andrii",
                Name = "Andrii",
                SurName = "Shainoha",
                Password = "testpassword123",
                ConfirmPassword = "testpassword123"
            };
            return registerViewModel;
        }
    }
}