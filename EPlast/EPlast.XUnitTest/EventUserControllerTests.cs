using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using EPlast.ViewModels.Events;
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
    public class EventUserControllerTests
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IUserStore<User>> _store;
        private Mock<UserManager<User>> _usermanager;
        private Mock<Models.ViewModelInitializations.Interfaces.ICreateEventVMInitializer> _iCreateEventVM;

        public EventUserControllerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _store = new Mock<IUserStore<User>>();
            _usermanager = new Mock<UserManager<User>>(_store.Object, null, null, null, null, null, null, null, null);
            _iCreateEventVM = new Mock<Models.ViewModelInitializations.Interfaces.ICreateEventVMInitializer>();
        }
    }
}
