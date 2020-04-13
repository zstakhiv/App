using EPlast.BussinessLayer;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace EPlast.XUnitTest
{
    public class EventUserControllerTests
    {
        [Fact]
        public void EventCreateTest()
        {
            // Arrange
            var expected = new Event
            {
                EventName = "Event1",
                EventType = new EventType { EventTypeName = "EventType1" },
                EventCategory = new EventCategory { EventCategoryName = "EventCategory1" },
                EventStatus = new EventStatus { EventStatusName = "EventStatus1" },
                EventDateStart = DateTime.Now,
                EventDateEnd = DateTime.Now,
                Eventlocation = "Location1",
                FormOfHolding = "FormOfHolding1",
                ForWhom = "ForWhom1",
                NumberOfPartisipants = 50,
                Description = "Description1",
                Questions = "Questions",
            };
            var repository = new Mock<IRepositoryWrapper>();
            repository.Setup(r => r.Event.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(new List<Event>{new Event
            {
                EventName = "Event1",
                EventType = new EventType { EventTypeName = "EventType1" },
                EventCategory = new EventCategory { EventCategoryName = "EventCategory1" },
                EventStatus = new EventStatus { EventStatusName = "EventStatus1" },
                EventDateStart = DateTime.Now,
                EventDateEnd = DateTime.Now,
                Eventlocation = "Location1",
                FormOfHolding = "FormOfHolding1",
                ForWhom = "ForWhom1",
                NumberOfPartisipants = 50,
                Description = "Description1",
                Questions = "Questions",
            } }.AsQueryable());

            var userStoreMock = new Mock<IUserStore<User>>();
            var usManager = new Mock<UserManager<User>>(userStoreMock.Object,
               null, null, null, null, null, null, null, null);
            var controller = new EventUserController(repository.Object, usManager.Object);
            // Act
            var events = new EventCreateViewModel { Event = expected };
            var result = controller.EventCreate(events);
            // Assert
            repository.Verify(r => r.Event.Update(It.IsAny<Event>()), Times.Once());
        }
    }
}