using EPlast.DataAccess.Repositories;
//using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private EPlastDBContext _dbContext;
        private IUserRepository _user;
        private INationalityRepository _nationality;
        private IEventRepository _event;
        private IParticipantStatusRepository _participantStatuses;
        private IGallaryRepository _gallary;
        private IEventGallaryRepository _eventGallary;
        private IParticipantRepository _participant;
        private IEventCategoryRepository _eventCategory;
        private ISubEventCategoryRepository _subEventCategory;
        private IEventStatusRepository _eventStatus;





        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_dbContext);
                }

                return _user;
            }
        }

        public INationalityRepository Nationality
        {
            get
            {
                if (_nationality == null)
                {
                    _nationality = new NationalityRepository(_dbContext);
                }

                return _nationality;
            }
        }

        public IEventRepository Event
        {
            get
            {
                if (_event == null)
                {
                    _event = new EventRepository(_dbContext);
                }

                return _event;
            }
        }
        public IGallaryRepository Gallary
        {
            get
            {
                if (_gallary == null)
                {
                    _gallary = new GallaryRepository(_dbContext);
                }
                return _gallary;
            }
        }
        public IEventGallaryRepository EventGallary
        {
            get
            {
                if (_eventGallary == null)
                {
                    _eventGallary = new EventGallaryRepository(_dbContext);
                }
                return _eventGallary;
            }
        }
        public IParticipantStatusRepository ParticipantStatus
        {
            get
            {
                if (_participantStatuses == null)
                {
                    _participantStatuses = new ParticipantStatusRepository(_dbContext);
                }

                return _participantStatuses;
            }
        }

        public IParticipantRepository Participant
        {
            get
            {
                if (_participant == null)
                {
                    _participant = new ParticipantRepository(_dbContext);
                }

                return _participant;
            }
        }

        public IEventCategoryRepository EventCategory
        {
            get
            {
                if (_eventCategory == null)
                {
                    _eventCategory = new EventCategoryRepository(_dbContext);
                }

                return _eventCategory;
            }
        }

        public ISubEventCategoryRepository SubEventCategory
        {
            get
            {
                if (_subEventCategory == null)
                {
                    _subEventCategory = new SubEventCategoryRepository(_dbContext);
                }

                return _subEventCategory;
            }
        }

        public IEventStatusRepository EventStatus
        {
            get
            {
                if (_eventStatus == null)
                {
                    _eventStatus = new EventStatusRepository(_dbContext);
                }

                return _eventStatus;
            }
        }

        // public IEventRepository Events => throw new System.NotImplementedException();

        public RepositoryWrapper(EPlastDBContext ePlastDBContext)
        {
            _dbContext = ePlastDBContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
