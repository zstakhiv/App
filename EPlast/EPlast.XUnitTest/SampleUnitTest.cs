using System.Collections.Generic;
using System.Linq;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EPlast.XUnitTest
{
    public class SampleUnitTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        public SampleUnitTest()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
        }
        [Fact]
        public void GetActionEmptyTest()
        {
            var eventCategoryList = new List<EventCategory>();
            _repoWrapper.Setup(x => x.EventCategory.FindAll()).Returns(eventCategoryList.AsQueryable());
            var actionsController = new ActionController(_repoWrapper.Object);
            var actionResult = actionsController.GetAction() as ViewResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Model);

            var viewModel = actionResult.Model as List<EventCategoryViewModel>;
            Assert.NotNull(viewModel);
            Assert.Empty(viewModel);
        }
        [Fact]
        public void GetActionTest()
        {
            var eventCategoryList = new List<EventCategory>()
            {
                new EventCategory()
                {
                    EventCategoryName = "Some name",
                    ID = 1,
                    Events = new List<Event>()
                }
            };
            _repoWrapper.Setup(x => x.EventCategory.FindAll()).Returns(eventCategoryList.AsQueryable());
            var actionsController = new ActionController(_repoWrapper.Object);
            var actionResult = actionsController.GetAction() as ViewResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Model);

            var viewModel = actionResult.Model as List<EventCategoryViewModel>;
            Assert.NotNull(viewModel);
             
            Assert.Single(viewModel);
            Assert.NotNull(viewModel[0].EventCategory);
            Assert.Equal(1, viewModel[0].EventCategory.ID);
            Assert.Equal("Some name", viewModel[0].EventCategory.EventCategoryName);
            Assert.NotNull(viewModel[0].EventCategory.Events);
            Assert.Empty(viewModel[0].EventCategory.Events);

        }
    }
}
