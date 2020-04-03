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
    public class CityControllerTests
    {
        [Fact]
        public void IndexViewModelNotNull()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var _repoWrapper = new Mock<IRepositoryWrapper>();
            var _env = new Mock<IHostingEnvironment>();
            var usManager = new Mock<UserManager<User>>(userStoreMock.Object,
                null, null, null, null, null, null, null, null);

            var cityList = new List<City>()
            {
                new City()
                {
                    Name="Харків",
                    PhoneNumber="+380956345756",
                    Email="test@eplast.com",
                    CityURL="",
                    Description="",
                    Street="Шевченка",
                    HouseNumber="2a",
                    OfficeNumber="72",
                    PostIndex="72000",
                    Region= new Region{RegionName="Харківський"},
                    Logo = null
                }
            };

            _repoWrapper.Setup(x => x.City.FindAll()).Returns(cityList.AsQueryable());
            var citycontroller = new CityController(_repoWrapper.Object,usManager.Object,_env.Object);
            var indexResult = citycontroller.Index() as ViewResult;

            Assert.NotNull(indexResult);
            Assert.NotNull(indexResult.Model);
            var viewModel = indexResult.Model as List<CityViewModel>;
            Assert.NotNull(viewModel);

            Assert.Single(viewModel);
            Assert.NotNull(viewModel[0].City);
            Assert.Equal(1, viewModel[0].City.ID);
            Assert.Equal("Харків", viewModel[0].City.Name);

        }

    }
}
