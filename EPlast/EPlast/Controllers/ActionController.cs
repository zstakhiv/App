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
            #region Old version
            List<EventCategoryViewModel> evc = new List<EventCategoryViewModel>()
            {
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=1,EventCategoryName="Інструкторський Вишкіл (УСП)" } },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=2,EventCategoryName="Конгрес"} },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=3,EventCategoryName="КПЗ"} },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=4,EventCategoryName="Крайовий Вишкіл Виховників УПЮ (КВВ УПЮ)"} },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=5,EventCategoryName="Крайовий Вишкіл Дійсного Членства (КВДЧ)"} },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=6,EventCategoryName="Крайовий Вишкіл Зв'язкових"}},
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=7,EventCategoryName="Крайовий Вишкіл Провідників Вишколів"} },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=8,EventCategoryName="Крайовий Дошкіл Виховників УПЮ (КВВ УПЮ)"} },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=9,EventCategoryName="Крайовий табір"} },
                new EventCategoryViewModel(){EventCategory=new EventCategory{ID=10,EventCategoryName="Курінний табір"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =11,EventCategoryName="ЛШ вишкіл булавних" } },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =12,EventCategoryName="ЛШ вишкіл бунчужних"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =13,EventCategoryName="Окружний табір"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =14,EventCategoryName="Рада Орлиної Спеціалізації (провідників вишколів)"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory {ID = 15,EventCategoryName="Рада Орлиної Спеціалізації (провідників таборів)"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory {ID = 16,EventCategoryName="Рада Орлиного Вогню (булавних)"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =17,EventCategoryName="Рада Орлиного Вогню (впорядників)"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =18,EventCategoryName="Рада Орлиного Вогню (гніздових)"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =19,EventCategoryName="Станичний табір"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =20,EventCategoryName="ШБ вишкіл булавних"} },
                new EventCategoryViewModel(){ EventCategory = new EventCategory { ID =21,EventCategoryName="ШБ вишкіл бунчужних"} },
            };
            #endregion
            List<EventCategoryViewModel> _evc = _repoWrapper.EventCategory.FindAll()
            .Select(eventCategory => new EventCategoryViewModel() { EventCategory=eventCategory})
            .ToList();
             return View(_evc);
        }

        public IActionResult GetSubAction(int? ID)
        {
            if(ID==null)
            {
                return Content("Не вибрано жодної акції!");
            }
            List<SubEventCategoryViewModel> subCat = new List<SubEventCategoryViewModel>()
            {
                new SubEventCategoryViewModel(){SubEventCategory=new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія А" },},
                new SubEventCategoryViewModel(){SubEventCategory=new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія Б" },},
                new SubEventCategoryViewModel(){SubEventCategory=new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія В" },},
                new SubEventCategoryViewModel(){SubEventCategory=new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія Г" },},
            };
               return View(subCat);
        }

        }
}
