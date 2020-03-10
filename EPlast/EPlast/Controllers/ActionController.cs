using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
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
            List<EventCategoryViewModel> evc = new List<EventCategoryViewModel>()
            {
                new EventCategoryViewModel(){ID=1,EventCategoryName="Інструкторський Вишкіл (УСП)"},
                new EventCategoryViewModel(){ID=2,EventCategoryName="Конгрес"},
                new EventCategoryViewModel(){ID=3,EventCategoryName="КПЗ"},
                new EventCategoryViewModel(){ID=4,EventCategoryName="Крайовий Вишкіл Виховників УПЮ (КВВ УПЮ)"},
                new EventCategoryViewModel(){ID=5,EventCategoryName="Крайовий Вишкіл Дійсного Членства (КВДЧ)"},
                new EventCategoryViewModel(){ID=6,EventCategoryName="Крайовий Вишкіл Зв'язкових"},
                new EventCategoryViewModel(){ID=7,EventCategoryName="Крайовий Вишкіл Провідників Вишколів"},
                new EventCategoryViewModel(){ID=8,EventCategoryName="Крайовий Дошкіл Виховників УПЮ (КВВ УПЮ)"},
                new EventCategoryViewModel(){ID=9,EventCategoryName="Крайовий табір"},
                new EventCategoryViewModel(){ID=10,EventCategoryName="Курінний табір"},
                new EventCategoryViewModel(){ID=11,EventCategoryName="ЛШ вишкіл булавних"},
                new EventCategoryViewModel(){ID=12,EventCategoryName="ЛШ вишкіл бунчужних"},
                new EventCategoryViewModel(){ID=13,EventCategoryName="Окружний табір"},
                new EventCategoryViewModel(){ID=14,EventCategoryName="Рада Орлиної Спеціалізації (провідників вишколів)"},
                new EventCategoryViewModel(){ID=15,EventCategoryName="Рада Орлиної Спеціалізації (провідників таборів)"},
                new EventCategoryViewModel(){ID=16,EventCategoryName="Рада Орлиного Вогню (булавних)"},
                new EventCategoryViewModel(){ID=17,EventCategoryName="Рада Орлиного Вогню (впорядників)"},
                new EventCategoryViewModel(){ID=18,EventCategoryName="Рада Орлиного Вогню (гніздових)"},
                new EventCategoryViewModel(){ID=19,EventCategoryName="Станичний табір"},
                new EventCategoryViewModel(){ID=20,EventCategoryName="ШБ вишкіл булавних"},
                new EventCategoryViewModel(){ID=21,EventCategoryName="ШБ вишкіл бунчужних"},
            };
            return View(evc);
        }

        public IActionResult GetSubAction(int? ID)
        {
            if(ID==null)
            {
                return Content("Не вибрано жодної акції!");
            }
            List<SubEventCategoryViewModel> subCat = new List<SubEventCategoryViewModel>()
            {
                new SubEventCategoryViewModel(){ID=1,SubEventCategoryName="Підкатегорія А",},
                new SubEventCategoryViewModel(){ID=1,SubEventCategoryName="Підкатегорія Б",},
                new SubEventCategoryViewModel(){ID=1,SubEventCategoryName="Підкатегорія В",},
                new SubEventCategoryViewModel(){ID=1,SubEventCategoryName="Підкатегорія Г",}
            };
               return View(subCat);
        }

        }
}
