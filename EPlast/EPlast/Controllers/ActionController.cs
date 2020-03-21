using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Mvc;


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
            .Select(eventCategory => new EventCategoryViewModel() { EventCategory=eventCategory})
            .ToList();
             return View(_evc);
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

            if (_subCat.Count==0)
            {
                return Content($"Акція '{eventCategory}' не містить жодних підкатегорій .");
            }
            return View(_subCat);
        }

        public IActionResult Events()
        {
            return View();
        }

    }

    

}
