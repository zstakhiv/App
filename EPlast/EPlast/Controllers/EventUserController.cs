using EPlast.DataAccess.Entities;
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
        public IActionResult EventCreate(int id)
        {
            try
            {
                var events = _repoWrapper.Event.
                    FindByCondition(p=>p.ID==id).
                Include(i => i.EventCategory).
                Include(g => g.EventAdmins).
                Include(g => g.EventStatus).
                Include(g => g.EventAdministrations).
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
                    EventTypes = _repoWrapper.EventType.FindAll(),
                    EventCategory = _repoWrapper.EventCategory.FindAll(),
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
                var events = _repoWrapper.Event.
                Include(i => i.EventCategory).
                Include(i=>i.EventType).
                Include(g => g.EventAdmins).
                Include(g => g.EventStatus).
                Include(g => g.EventAdministrations).
                FirstOrDefault();

                if (model.Event.EventCategory.ID == 0)
                {
                    string EventCategoryName = model.Event.EventCategory.EventCategoryName;
                    if (string.IsNullOrEmpty(EventCategoryName))
                    {
                        model.Event.EventCategory = null;
                    }
                    else
                    {
                        model.Event.EventCategory = new EventCategory()
                        { EventCategoryName = EventCategoryName };
                    }
                }
                _repoWrapper.Event.Update(model.Event);
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