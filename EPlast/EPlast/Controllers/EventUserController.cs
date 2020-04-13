﻿using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPlast.Models.ViewModelInitializations.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class EventUserController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public EventUserController(IRepositoryWrapper repoWrapper, UserManager<User> userManager)

        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
        }

        public IActionResult EventUser()
        {
            EventUserViewModel model = new EventUserViewModel();
            var user = _repoWrapper.User.
            FindByCondition(q => q.Id == _userManager.GetUserId(User)).First();
            model.User = user;
            model.EventAdmins = _repoWrapper.EventAdmin.FindByCondition(i => i.UserID == _userManager.GetUserId(User)).
                            Include(i => i.Event).Include(i => i.User).ToList();
            model.Participants = _repoWrapper.Participant.FindByCondition(i => i.UserId == _userManager.GetUserId(User)).
                Include(i => i.Event).ToList();
            model.CreatedEventCount = 0;
            model.CreatedEvents = new List<Event>();
            foreach (var eventAdmin in model.EventAdmins)
            {
                if (eventAdmin.UserID == _userManager.GetUserId(User))
                {
                    model.CreatedEvents.Add(eventAdmin.Event);
                    model.CreatedEventCount += 1;
                }
            }
            model.PlanedEventCount = 0;
            model.PlanedEvents = new List<Event>();
            model.VisitedEventsCount = 0;
            model.VisitedEvents = new List<Event>();
            foreach (var participant in model.Participants)
            {
                if (participant.UserId == _userManager.GetUserId(User) &&
                    participant.Event.EventDateStart <= DateTime.Now)
                {
                    model.PlanedEvents.Add(participant.Event);
                    model.PlanedEventCount += 1;
                }
                else if (participant.UserId == _userManager.GetUserId(User) &&
                    participant.Event.EventDateStart <= DateTime.Now)
                {
                    model.VisitedEventsCount = 0;
                    model.VisitedEvents = new List<Event>();
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EventCreate()
        {
            var model = new EventCreateViewModel()
            {
                Users = _repoWrapper.User.FindAll(),
                EventTypes = _repoWrapper.EventType.FindAll(),
                EventCategories = _repoWrapper.EventCategory.FindAll()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult EventCreate(EventCreateViewModel createVM)
        {
            try
            {
                EventStatus status = _repoWrapper.EventStatus.
                    FindByCondition(i => i.EventStatusName == "Не затверджені").
                    FirstOrDefault();
                createVM.Event.EventStatusID = status.ID;
                EventAdmin eventAdmin = new EventAdmin()
                {
                    Event = createVM.Event,
                    UserID = createVM.EventAdmin.UserID
                };
                if (ModelState.IsValid)
                {
                    _repoWrapper.EventAdmin.Create(eventAdmin);
                    _repoWrapper.Event.Create(createVM.Event);
                    _repoWrapper.Save();
                    EventUserViewModel eventUser = new EventUserViewModel();
                    return View("EventUser", eventUser);
                }
                else
                {
                    var eventCategories = _repoWrapper.EventCategory.FindAll();
                    var model = new EventCreateViewModel()
                    {
                        Users = _repoWrapper.User.FindAll(),
                        EventTypes = _repoWrapper.EventType.FindAll(),
                        EventCategories = _repoWrapper.EventCategory.FindAll()
                    };
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
    }
}