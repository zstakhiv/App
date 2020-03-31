using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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

        [Authorize]
        public IActionResult Events(int ID)
        {
            try
            {
                int actionID = _repoWrapper.EventType.FindByCondition(et => et.EventTypeName == "Акція").First().ID;
                int approvedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Учасник").First().ID;
                int undeterminedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Розглядається").First().ID;
                int rejectedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено").First().ID;
                int approvedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Затверджений(-на)").First().ID;
                int finishedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Завершений(-на)").First().ID;
                int notApprovedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Не затверджені").First().ID;

                List<GeneralEventViewModel> newEvents = _repoWrapper.Event
                 .FindByCondition(e => e.EventCategoryID == ID && e.EventTypeID == actionID)
                 .Include(e => e.EventAdmins)
                 .Include(e => e.Participants)
                 .Select(ev => new GeneralEventViewModel
                 {
                   Event=ev,
                   IsUserEventAdmin = ev.EventAdmins.Any(e => e.UserID == _userManager.GetUserId(User)),
                   IsUserParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User)),
                   IsUserApprovedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == approvedStatus),
                   IsUserUndeterminedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == undeterminedStatus),
                   IsUserRejectedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == rejectedStatus),
                   IsEventApproved = ev.EventStatusID == approvedEvent,
                   IsEventNotApproved = ev.EventStatusID == notApprovedEvent,
                   IsEventFinished = ev.EventStatusID == finishedEvent
                 }).ToList();

                //List<Event> events = _repoWrapper.Event
                //.FindByCondition(e => e.EventCategoryID == ID && e.EventTypeID == actionID)
                //.Include(e => e.EventAdmins)
                //.Include(e => e.Participants)
                //.ToList();
                //EventsViewModel _event = new EventsViewModel() { Events = events, user = _userManager };
                return View(newEvents);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }

        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

        [Authorize]
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
                       .Include(e => e.EventType)
                       .Include(e => e.EventCategory)
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
                      .Include(e => e.EventType)
                      .Include(e => e.EventCategory)
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

    }
}