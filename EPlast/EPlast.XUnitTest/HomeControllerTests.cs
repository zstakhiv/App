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
    public class HomeControllerTests
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IEmailConfirmation> _emailConfirmation;

        public HomeControllerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _emailConfirmation = new Mock<IEmailConfirmation>();
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            var controller = new HomeController(_emailConfirmation.Object,_repoWrapper.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void AboutPLASTViewResultNotNull()
        {
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);

            var result = controller.AboutPLAST();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void ContactsViewResultNotNull()
        {
            
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);

            var result = controller.Contacts();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void FAQViewResultNotNull()
        {
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);

            var result = controller.FAQ();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void FeedBackSendedResultNotNull()
        {
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);

            var result = controller.FeedBackSended();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void SearchResultNotNull()
        {
            _repoWrapper.Setup(p => p.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(
                new List<User> { 
                    new User
                    {
                        FirstName="Денис",
                        LastName = "Іванків"
                    },
                    new User
                    {
                        FirstName="Іван",
                        LastName = "Іванків"
                    },
                    new User
                    {
                        FirstName="Петро",
                        LastName = "Іванків"
                    },
                    new User
                    {
                        FirstName="Олег",
                        LastName = "Іванків"
                    },
                    new User
                    {
                        FirstName="Андрій",
                        LastName = "Іванків"
                    },
                    new User
                    {
                        FirstName="Микола",
                        LastName = "Іванків"
                    }
                }.AsQueryable());

            var homecontroller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);
            var searchResultNotNull = homecontroller.Search("Іванків") as ViewResult;
            var searchResultNull = homecontroller.Search(null) as ViewResult;

            Assert.NotNull(searchResultNotNull);
            Assert.NotNull(searchResultNotNull.Model);
        }

    }
}
