namespace EPlast.DataAccess.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private EPlastDBContext _dbContext;
        private IUserRepository _user;
        private INationalityRepository _nationality;
        private IOrgranRepository _oragn;
        private IDecesionTargetRepository _decesionTarget;
        private IDecesionStatusRepository _decesionStatus;
        private IDocumentTemplateRepository _documentTemplate;
        private IDecesionRepository _decesion;

        public IDecesionRepository Decesion
        {
            get
            {
                if (_decesion == null)
                {
                    _decesion = new DecesionRepository(_dbContext);
                }
                return _decesion;
            }
        }

        public IDocumentTemplateRepository DocumentTemplate
        {
            get
            {
                if (_documentTemplate == null)
                {
                    _documentTemplate = new DocumentTemplateRepository(_dbContext);
                }
                return _documentTemplate;
            }
        }

        public IDecesionStatusRepository DecesionStatus
        {
            get
            {
                if (_decesionStatus == null)
                {
                    _decesionStatus = new DecesionStatusRepository(_dbContext);
                }
                return _decesionStatus;
            }
        }

        public IDecesionTargetRepository DecesionTarget
        {
            get
            {
                if (_decesionTarget == null)
                {
                    _decesionTarget = new DecesionTargetRepository(_dbContext);
                }
                return _decesionTarget;
            }
        }

        public IOrgranRepository Orgran
        {
            get
            {
                if (_oragn == null)
                {
                    _oragn = new OrganRepository(_dbContext);
                }
                return _oragn;
            }
        }

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