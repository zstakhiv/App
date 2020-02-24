using Microsoft.AspNetCore.Mvc;

namespace EPlast.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateRaport()
        {
            return View();
        }

        public IActionResult ReadRaport()
        {
            return View();
        }

        [System.Obsolete]
        public void CreatePDF()
        {
            Models.PDFCreator creator = new Models.PDFCreator();
            creator.CreateDoucment();
            Response.Redirect(Url.Content("~/Report.pdf"));
        }
    }
}