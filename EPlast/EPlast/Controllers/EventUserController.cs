using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            model.Participants = _repoWrapper.Participant.FindByCondition(i => i.UserId == _userManager.GetUserId(User)).
                Include(i => i.Event).ToList();
            model.Events = new List<Event>();
            foreach (var item in model.Participants)
            {
                model.Events.Add(item.Event);
            }
            return View(model);
        }
    }
}