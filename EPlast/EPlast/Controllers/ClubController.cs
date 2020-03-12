using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var club = _repoWrapper.Club
                .FindByCondition(q => q.ID == index)
                .Include(c => c.ClubAdministration)
                .ThenInclude(t => t.AdminType)
                .Include(n => n.ClubAdministration)
                .ThenInclude(t => t.ClubMembers)
                .ThenInclude(us => us.User)
                .Include(m => m.ClubMembers)
                .ThenInclude(u => u.User)
                .FirstOrDefault();
              
            return View(new ClubViewModel { Club = club});
        }
    }
}