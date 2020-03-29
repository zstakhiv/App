using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.Controllers
{
    public class ActionController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;


        public ActionController(UserManager<User> userManager, IRepositoryWrapper repoWrapper)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
        }

        public IActionResult GetAction()
        {
            try
            {
                List<EventCategoryViewModel> _evc = _repoWrapper.EventCategory.FindAll()
                .Select(eventCategory => new EventCategoryViewModel() { EventCategory = eventCategory })
                .ToList();
                return View(_evc);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
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

            if (_subCat.Count == 0)
            {
                return Content($"Акція '{eventCategory}' не містить жодних підкатегорій .");
            }
            return View(_subCat);
        }

        public IActionResult Events(int? ID)
        {
            try
            {
                List<Event> events = _repoWrapper.Event
                .FindByCondition(e => e.EventCategoryID == ID)
                .Include(e => e.EventAdmins)
                .Include(e => e.Participants)
                .ToList();
                EventsViewModel _event = new EventsViewModel() { Events = events, user = _userManager };
                return View(_event);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }

        }

        [HttpPost]
        public IActionResult DeleteEvent(int ID)
        {
            try
            {
                _repoWrapper.Event.Delete(_repoWrapper.Event.FindByCondition(e => e.ID == ID).First());
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }

        }

        [HttpPost]
        public IActionResult SubscribeOnEvent(int ID)
        {
            try
            {
                _repoWrapper.Participant.Create(new Participant() { ParticipantStatusId = 3, EventId = ID, UserId = _userManager.GetUserId(User) });
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult UnSubscribeOnEvent(int ID)
        {
            try
            {
                Participant participantToDelete = _repoWrapper.Participant.FindByCondition(p => p.EventId == ID && p.UserId == _userManager.GetUserId(User)).First();
                _repoWrapper.Participant.Delete(participantToDelete);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult EventInfo(int ID)
        {
            try 
            {
                int approvedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Учасник").First().ID;
                int undeterminedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Розглядається").First().ID;
                int rejectedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено").First().ID;

                EventViewModel eventModal = new EventViewModel();
                if (_repoWrapper.Event.FindByCondition(e => e.ID == ID).Include(e => e.EventAdmins).First().EventAdmins.Any(e => e.UserID == _userManager.GetUserId(User)))
                {
                     eventModal = _repoWrapper.Event.FindByCondition(e => e.ID == ID)
                       .Include(e => e.Participants)
                            .ThenInclude(p => p.User)
                       .Include(e => e.Participants)
                            .ThenInclude(p => p.ParticipantStatus)
                       .Include(e => e.EventAdmins)
                       .ThenInclude(evAdm => evAdm.User)
                       .Include(e => e.EventStatus)
                       .Include(e => e.EventAdministrations)
                       .Select(e => new EventViewModel() 
                       { user = _userManager,
                         Event = e,
                         EventParticipants = e.Participants,
                         IsUserEventAdmin =true,
                         ApprovedStatus = approvedStatus,
                         UndeterminedStatus = undeterminedStatus,
                         RejectedStatus = rejectedStatus
                       })
                       .First();
                }
                else
                {
                    eventModal = _repoWrapper.Event.FindByCondition(e => e.ID == ID)
                      .Include(e => e.Participants)
                           .ThenInclude(p => p.User)
                      .Include(e => e.Participants)
                           .ThenInclude(p => p.ParticipantStatus)
                      .Include(e => e.EventAdmins)
                      .ThenInclude(evAdm => evAdm.User)
                      .Include(e => e.EventStatus)
                      .Include(e => e.EventAdministrations)
                      .Select(e => new EventViewModel()
                      {
                          user = _userManager,
                          Event = e,
                          EventParticipants = e.Participants.Where(p => p.ParticipantStatusId == approvedStatus),
                          IsUserEventAdmin = false,
                          ApprovedStatus = approvedStatus,
                          UndeterminedStatus = undeterminedStatus,
                          RejectedStatus = rejectedStatus
                      })
                      .First();
                }
                   return View(eventModal);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        public IActionResult ApproveParticipant(int ID)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == ID)
                    .Include(p => p.ParticipantStatus).First();
                ParticipantStatus participantStatus = _repoWrapper.ParticipantStatus.FindByCondition(ps => ps.ParticipantStatusName == "Учасник").First();
                participant.ParticipantStatus = participantStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult UndetermineParticipant(int ID)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == ID)
                   .Include(p => p.ParticipantStatus).First();
                ParticipantStatus participantStatus = _repoWrapper.ParticipantStatus.FindByCondition(ps => ps.ParticipantStatusName == "Розглядається").First();
                participant.ParticipantStatus = participantStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult RejectParticipant(int ID)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == ID)
                   .Include(p => p.ParticipantStatus).First();
                ParticipantStatus participantStatus = _repoWrapper.ParticipantStatus.FindByCondition(ps => ps.ParticipantStatusName == "Відмовлено").First();
                participant.ParticipantStatus = participantStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult Table()
        {
            return View();
        }
    }
}