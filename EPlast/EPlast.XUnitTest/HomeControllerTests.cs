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
            Assert.NotNull(result);
        }
    }
}
