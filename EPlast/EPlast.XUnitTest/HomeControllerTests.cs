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
        
        [Fact]
        public void IndexViewResultNotNull()
        {
            var _repoWrapper = new Mock<IRepositoryWrapper>();
            var _emailConfirmation = new Mock<IEmailConfirmation>();
            // Arrange
            var controller = new HomeController(_emailConfirmation.Object,_repoWrapper.Object);
            // Act
            var result = controller.Index();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void AboutPLASTViewResultNotNull()
        {
            var _repoWrapper = new Mock<IRepositoryWrapper>();
            var _emailConfirmation = new Mock<IEmailConfirmation>();
            // Arrange
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);
            // Act
            var result = controller.AboutPLAST();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void ContactsViewResultNotNull()
        {
            var _repoWrapper = new Mock<IRepositoryWrapper>();
            var _emailConfirmation = new Mock<IEmailConfirmation>();
            // Arrange
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);
            // Act
            var result = controller.Contacts();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void FAQViewResultNotNull()
        {
            var _repoWrapper = new Mock<IRepositoryWrapper>();
            var _emailConfirmation = new Mock<IEmailConfirmation>();
            // Arrange
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);
            // Act
            var result = controller.FAQ();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void FeedBackSendedResultNotNull()
        {
            var _repoWrapper = new Mock<IRepositoryWrapper>();
            var _emailConfirmation = new Mock<IEmailConfirmation>();
            // Arrange
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);
            // Act
            var result = controller.FeedBackSended();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void SearchResultNotNull()
        {
            var _repoWrapper = new Mock<IRepositoryWrapper>();
            var _emailConfirmation = new Mock<IEmailConfirmation>();
            // Arrange
            var controller = new HomeController(_emailConfirmation.Object, _repoWrapper.Object);
            // Act
            //var result = controller.Search();
            // Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            //Assert.NotNull(viewResult);
        }


    }
}
