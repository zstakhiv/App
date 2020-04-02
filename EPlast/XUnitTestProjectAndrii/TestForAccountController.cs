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
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

           
            var userStore = new Mock<IUserStore<User>>();


            userStore.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
           .Returns(Task.FromResult(IdentityResult.Success));


            var mockUserManager = new Mock<UserManager<User>>(userStore.Object,
                options.Object, new PasswordHasher<User>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<User>>>().Object);

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
            var result = await accountController.Register(GetTestUserForRegistration());  //тут вилітає
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
            Assert.False(accountController.TryValidateModel(GetTestUserForRegistration()));
        }

        private RegisterViewModel GetTestUserForRegistration()
        {
            var registerViewModel = new RegisterViewModel
            {
                Email = "andriishainoha@gmail.com",
              
                Password = "testpassword123",
                Name = "Andrii",
                SurName = "Shainoha",
                ConfirmPassword = "testpassword123"
            };
            return registerViewModel;
        }

    }
}
