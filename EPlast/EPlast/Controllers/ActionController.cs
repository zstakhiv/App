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
    public class ActionController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;


        public ActionController(UserManager<User> userManager, IRepositoryWrapper repoWrapper)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
        }

        public IActionResult GetAction()
        {
            try
            {
                List<EventCategoryViewModel> _evc = _repoWrapper.EventCategory.FindAll()
                .Select(eventCategory => new EventCategoryViewModel() { EventCategory = eventCategory })
                .ToList();
                return View(_evc);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        public IActionResult GetSubAction(int? ID)
        {
            if (ID == null)
            {
                return Content("Не вибрано жодної акції!");
            }

            string eventCategory = _repoWrapper.EventCategory.FindByCondition(e => e.ID == ID)
                .FirstOrDefault().EventCategoryName;
            List<SubEventCategoryViewModel> _subCat = _repoWrapper.SubEventCategory
                .FindByCondition(x => x.EventCategoryID == ID)
                .Select(subcat => new SubEventCategoryViewModel() { SubEventCategory = subcat })
                .ToList();

            if (_subCat.Count == 0)
            {
                return Content($"Акція '{eventCategory}' не містить жодних підкатегорій .");
            }
            return View(_subCat);
        }

        public IActionResult Events(int? ID)
        {
            try
            {
                List<EventViewModel> _event = _repoWrapper.Event
                .FindByCondition(e => e.EventCategoryID == ID)
                .Include(e => e.EventAdmins)
                .Include(e =>e.Participants)
                .Select(ev => new EventViewModel() { Event = ev })
                .ToList();
                return View(_event);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        
         }

        public IActionResult EventInfo()
        {
            return View();
        }
    }
}