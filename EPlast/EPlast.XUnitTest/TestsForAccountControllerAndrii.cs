using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace EPlast.XUnitTest
{
    public class TestsForAccountControllerAndrii
    {
        public (Mock<SignInManager<User>>, Mock<UserManager<User>>, Mock<IEmailConfirmation>, AccountController) CreateAccountController()
        {
            var testUser = new User
            {
                UserName = "andriishainoha@gmail.com"
            };

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

            var mockUserManager = new Mock<UserManager<User>>(userPasswordStore.Object,
                options.Object, new PasswordHasher<User>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<User>>>().Object);

            mockUserManager
                .Setup(s => s.FindByNameAsync(testUser.UserName.ToUpperInvariant()))
                .Returns(Task.FromResult(testUser)).Verifiable();

            mockUserManager
                .Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockUserManager
                .Setup(s => s.FindByIdAsync(testUser.Id))
                .Returns(Task.FromResult(testUser)).Verifiable();

            var applicationUser = GetTestUserWithEmailConfirmed();

            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null, null);

            Mock<IRepositoryWrapper> mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            Mock<IEmailConfirmation> mockEmailConfirmation = new Mock<IEmailConfirmation>();
            Mock<IHostingEnvironment> mockHosting = new Mock<IHostingEnvironment>();

            //SignInManager does not mocked
            AccountController accountController = new AccountController(mockUserManager.Object, null,
                mockRepositoryWrapper.Object, mockLogger.Object, mockEmailConfirmation.Object, mockHosting.Object, null);

            return (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController);
        }

        //ChangePassword
        [Fact]
        public async Task TestChangePasswordGetReturnsChangePasswordView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed().EmailConfirmed);

            var result = await accountController.ChangePassword();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordGetReturnsChangePasswordNotAllowedView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserWithNotEmailConfirmed());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestUserWithNotEmailConfirmed().EmailConfirmed);

            var result = await accountController.ChangePassword();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePasswordNotAllowed", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnsLoginRedirect()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(GetTestUserWithNullFields()));

            var result = await accountController.ChangePassword(GetTestChangeViewModel()) as RedirectToActionResult;
            Assert.Equal("Login", result.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnsChangePasswordView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(GetTestUserWithAllFields()));

            mockUserManager
                .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            var result = await accountController.ChangePassword(GetTestChangeViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnChangePasswordConfirmationView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(GetTestUserWithAllFields()));

            mockUserManager
                .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockSignInManager
                .Setup(s => s.RefreshSignInAsync(It.IsAny<User>()))
                .Verifiable();
            //треба налаштувати signInManager

            /*var result = await accountController.ChangePassword(GetTestChangeViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);*/
        }

        //ResetPassword
        [Fact]
        public void TestResetPasswordGetReturnsErrorView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var result = accountController.ResetPassword();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void TestResetPasswordGetReturnsResetPasswordView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var result = accountController.ResetPassword(GetTestCodeForResetPassword());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResetPasswordView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await accountController.ResetPassword(GetTestResetViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResetPasswordConfirmation()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockUserManager
                .Setup(s => s.IsLockedOutAsync(It.IsAny<User>()))
                .Returns(Task.FromResult(false));

            var result = await accountController.ResetPassword(GetTestResetViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResultFailedResetPasswordView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            var result = await accountController.ResetPassword(GetTestResetViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ForgotPassword
        [Fact]
        public void TestForgotPasswordGetReturnsForgotPasswordView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var result = accountController.ForgotPassword();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]   //переробити бо ModelState завжди каже шо тру
        public async Task TestForgotPasswordPostModelIsNotValid()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var result = await accountController.ForgotPassword(GetBadTestForgotViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostReturnsForgotPasswordView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestUserWithAllFields().EmailConfirmed);

            var result = await accountController.ForgotPassword(GetTestForgotViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostReturnsForgotPasswordConfirmationView()
        {
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed().EmailConfirmed);

            mockUserManager
                .Setup(i => i.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCodeForResetPassword());

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            accountController.Url = mockUrlHelper.Object;
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = await accountController.ForgotPassword(GetTestForgotViewModel());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        private string GetTestCodeForResetPassword()
        {
            return new string("500");
        }

        private User GetTestUserWithNullFields()
        {
            return null;
        }

        private ForgotPasswordViewModel GetTestForgotViewModel()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "andriishainoha@gmail.com"
            };
            return forgotPasswordViewModel;
        }

        private ForgotPasswordViewModel GetBadTestForgotViewModel()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "andr"
            };
            return forgotPasswordViewModel;
        }

        private User GetTestUserWithAllFields()
        {
            return new User()
            {
                UserName = "andriishainoha@gmail.com",
                FirstName = "Andrii",
                LastName = "Shainoha",
                EmailConfirmed = true
            };
        }

        private ResetPasswordViewModel GetTestResetViewModel()
        {
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                ConfirmPassword = "andrii123",
                Code = "500"
            };
            return resetPasswordViewModel;
        }

        private ChangePasswordViewModel GetTestChangeViewModel()
        {
            var changePasswordViewModel = new ChangePasswordViewModel
            {
                CurrentPassword = "password123",
                NewPassword = "newpassword123",
                ConfirmPassword = "newpassword123"
            };
            return changePasswordViewModel;
        }

        private User GetTestUserWithEmailConfirmed()
        {
            return new User()
            {
                EmailConfirmed = true
            };
        }

        private User GetTestUserWithNotEmailConfirmed()
        {
            return new User()
            {
                EmailConfirmed = false
            };
        }
    }
}