using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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
            ViewBag.DecesionTargets = _repoWrapper.DecesionTarget.FindAll();
            ViewBag.Organization = (from item in _repoWrapper.Organization.FindAll()
                                    select new SelectListItem
                                    {
                                        Text = item.OrganizationName,
                                        Value = item.ID.ToString()
                                    });
            return View(new Decesion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveReport(Decesion decesion)
        {
            if (decesion is null)
            {
                return StatusCode(500);
            }
            decesion.DecesionStatus = _repoWrapper.DecesionStatus.FindAll().Where(x => x.ID == 1).FirstOrDefault();
            List<DecesionTarget> decesionTargets = _repoWrapper.DecesionTarget.FindAll().ToList();
            bool exist = decesionTargets.Any(x => x.TargetName.Equals(decesion.DecesionTarget.TargetName));
            if (exist)
            {
                decesion.DecesionTarget.ID = decesionTargets.Where(x => x.TargetName.Equals(decesion.DecesionTarget.TargetName))
                    .Select(x => x.ID)
                    .FirstOrDefault();
            }
            //_repoWrapper.Decesion.Attach(decesion);
            _repoWrapper.Decesion.Create(decesion);
            _repoWrapper.Save();
            return RedirectToAction("CreateRaport");
        }

        public IActionResult ReadRaport()
        {
            List<Decesion> decesions = _repoWrapper.Decesion.Include(x => x.DecesionStatus, x => x.DecesionTarget, x => x.Organization).Take(200).ToList();
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