﻿using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.Controllers
{
    public class EventUserController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private UserManager<User> _userManager;

        public EventUserController(UserManager<User> userManager, IRepositoryWrapper repoWrapper)
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
            model.Participants = _repoWrapper.Participant.FindByCondition
                (i => i.UserId == _userManager.GetUserId(User)).
                Include(i => i.Event).ToList();
            model.Events = new List<Event>();
            foreach (var item in model.Participants)
            {
                model.Events.Add(item.Event);
            }
            
            return View(model);
        }
        
        [HttpGet]
        public IActionResult EventCreate()
        {
            try
            {
                var events = _repoWrapper.Event.
                Include(i => i.EventCategory).
                Include(g => g.EventAdmins).
                Include(g => g.EventAdministrations).
                Include(g => g.EventStatus).
                FirstOrDefault();
                ViewBag.categories = (from item in _repoWrapper.EventCategory.FindAll()
                                   select new SelectListItem
                                   {
                                       Text = item.EventCategoryName,
                                       Value = item.ID.ToString()
                                   });
                var model = new EventCreateViewModel()
                {
                    Event = events,
                    EventCategory = _repoWrapper.EventCategory.FindAll(),
                    SubEventCategories = _repoWrapper.SubEventCategory.FindAll(),
                };

                return View(model);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult EventCreate(EventCreateViewModel model)
        {
            try
            {
                if (model.Event.EventCategory.SubEventCategories.ID == 0)
                {
                    string subEventCategoryName = model.Event.EventCategory.SubEventCategories.SubEventCategoryName;
                    if (string.IsNullOrEmpty(subEventCategoryName))
                    {
                        model.Event.EventCategory.SubEventCategories = null;
                    }
                    else
                    {
                        model.Event.EventCategory.SubEventCategories = new SubEventCategory() 
                        { SubEventCategoryName = subEventCategoryName };
                    }
                }
                _repoWrapper.Event.Update(model.Event);
                _repoWrapper.EventAdmin.Update(model.EventAdmin);
                _repoWrapper.EventAdministration.Update(model.EventAdministration);
                _repoWrapper.Save();
                return RedirectToAction("EventUser");
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}