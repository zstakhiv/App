using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EPlast.Controllers
{
    public class ActionController : Controller
    {
        // GET: /<controller>/
        private readonly IRepositoryWrapper _repoWrapper;
        public ActionController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
            
        }
        public IActionResult GetAction()
        {
            List<EventCategory> evc = new List<EventCategory>()
            {
                new EventCategory(){ID=1,EventCategoryName="Інструкторський Вишкіл (УСП)"},
                new EventCategory(){ID=2,EventCategoryName="Конгрес"},
                new EventCategory(){ID=3,EventCategoryName="КПЗ"},
                new EventCategory(){ID=4,EventCategoryName="Крайовий Вишкіл Виховників УПЮ (КВВ УПЮ)"},
                new EventCategory(){ID=5,EventCategoryName="Крайовий Вишкіл Дійсного Членства (КВДЧ)"},
                new EventCategory(){ID=6,EventCategoryName="Крайовий Вишкіл Зв'язкових"},
                new EventCategory(){ID=7,EventCategoryName="Крайовий Вишкіл Провідників Вишколів"},
                new EventCategory(){ID=8,EventCategoryName="Крайовий Дошкіл Виховників УПЮ (КВВ УПЮ)"},
                new EventCategory(){ID=9,EventCategoryName="Крайовий табір"},
                new EventCategory(){ID=10,EventCategoryName="Курінний табір"},
                new EventCategory(){ID=11,EventCategoryName="ЛШ вишкіл булавних"},
                new EventCategory(){ID=12,EventCategoryName="ЛШ вишкіл бунчужних"},
                new EventCategory(){ID=13,EventCategoryName="Окружний табір"},
                new EventCategory(){ID=14,EventCategoryName="Рада Орлиної Спеціалізації (провідників вишколів)"},
                new EventCategory(){ID=15,EventCategoryName="Рада Орлиної Спеціалізації (провідників таборів)"},
                new EventCategory(){ID=16,EventCategoryName="Рада Орлиного Вогню (булавних)"},
                new EventCategory(){ID=17,EventCategoryName="Рада Орлиного Вогню (впорядників)"},
                new EventCategory(){ID=18,EventCategoryName="Рада Орлиного Вогню (гніздових)"},
                new EventCategory(){ID=19,EventCategoryName="Станичний табір"},
                new EventCategory(){ID=20,EventCategoryName="ШБ вишкіл булавних"},
                new EventCategory(){ID=21,EventCategoryName="ШБ вишкіл бунчужних"},
            };
            return View(evc);
        }

        public IActionResult GetSubAction(int? ID)
        {
            if(ID==null)
            {
                return Content("Не вибрано жодної акції!");
            }
            List<SubEventCategory> subCat = new List<SubEventCategory>()
            {
                new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія А",},
                new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія Б",},
                new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія В",},
                new SubEventCategory(){ID=1,SubEventCategoryName="Підкатегорія Г",}
            };
            return View(subCat);
        }

        }
}
