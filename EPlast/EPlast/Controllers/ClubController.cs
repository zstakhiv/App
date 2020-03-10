using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;

        public ClubController(DataAccess.Repositories.IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        public IActionResult Index()
        {
            List<ClubViewModel> clubs = new List<ClubViewModel>(
                _repoWrapper.Club
                .FindAll()
                .Select(club => new ClubViewModel { Club = club })
                .ToList());

            return View(clubs);
        }
        public IActionResult Club(int index)
        {
            return View(index);
        }
    }
}