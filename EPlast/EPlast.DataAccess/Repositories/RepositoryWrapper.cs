﻿using EPlast.DataAccess.Repositories;

namespace EPlast.DataAccess.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private EPlastDBContext _dbContext;
        private IUserRepository _user;
        private INationalityRepository _nationality;
        private IEventRepository _event;
        private IParticipantStatusRepository _participantStatuses;


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
