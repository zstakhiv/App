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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EPlast.XUnitTest
{
    public class EventUserControllerTests
    {
        private EventUserController controller;
        private ViewResult result;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        [TestInitialize]
        public void SetupContext()
        {
            controller = new EventUserController(_userManager,_repoWrapper);
            result = controller.EventUser() as ViewResult;
        }

        [TestMethod]
        public void EventUserViewResultNotNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EventUserViewEqualEventUserCshtml()
        {
            Assert.AreEqual("EventUser", result.ViewName);
        }
    }
}
