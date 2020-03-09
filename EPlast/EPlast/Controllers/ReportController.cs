using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using System.Linq;

namespace EPlast.Controllers
{
    public class ReportController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;

        public ReportController(DataAccess.Repositories.IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateRaport()
        {
            return View(new DecesionViewModel());
        }

        [HttpPost]
        public IActionResult SaveReport(DecesionViewModel decesionViewModel)
        {
            decesionViewModel.Decesion.DecesionStatus = new DecesionStatus { ID = 1, DecesionStatusName = "У розгляді" };
            _repoWrapper.Decesion.Attach(decesionViewModel.Decesion);
            _repoWrapper.Decesion.Create(decesionViewModel.Decesion);
            _repoWrapper.Save();
            return RedirectToAction("CreateRaport");
        }

        public IActionResult ReadRaport()
        {
            List<DecesionViewModel> decesions = new List<DecesionViewModel>(
                _repoWrapper.Decesion
                .Include(x => x.DecesionStatus, x => x.DecesionTarget, x => x.Organization)
                .Take(200)
                .Select(decesion => new DecesionViewModel { Decesion = decesion })
                .ToList());

            return View(decesions);
        }

        public void CreatePDF()
        {
            Models.PDFCreator creator = new Models.PDFCreator();
            creator.CreateDoucment();
            Response.Redirect(Url.Content("~/Report.pdf"));
        }
    }
}