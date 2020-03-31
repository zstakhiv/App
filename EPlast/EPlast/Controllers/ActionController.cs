using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.Controllers
{
    public class ActionController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public ActionController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public IActionResult GetAction()
        {
            List<EventCategoryViewModel> _evc = _repoWrapper.EventCategory.FindAll()
            .Select(eventCategory => new EventCategoryViewModel() { EventCategory = eventCategory })
            .ToList();
            return View(_evc);
        }

        public IActionResult Events(int? ID)
        {
            return View();
        }

        public IActionResult EventInfo()
        {
            return View();
        }
    }
}