namespace EPlast.DataAccess.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private EPlastDBContext _dbContext;
        private IUserRepository _user;
        private INationalityRepository _nationality;
        private IReligionRepository _religion;
        private ISexRepository _sex;
        private IWorkRepository _work;
        private IEducationRepository _education;
        private IDegreeRepository _degree;
        private IUserComissionRepository _userComission;

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

        public IReligionRepository Religion
        {
            get
            {
                if(_religion==null)
                {
                    _religion = new ReligionRepository(_dbContext);
                }

                return _religion;
            }
        }

        public ISexRepository Sex
        {
            get
            {
                if (_sex == null)
                {
                    _sex = new SexRepository(_dbContext);
                }

                return _sex;
            }
        }

        public IWorkRepository Work
        {
            get
            {
                if (_work == null)
                {
                    _work = new WorkRepository(_dbContext);
                }

                return _work ;
            }
        }

        public IEducationRepository Education
        {
            get
            {
                if (_education == null)
                {
                    _education = new EducationRepository(_dbContext);
                }

                return _education;
            }
        }

        public IDegreeRepository Degree
        {
            get
            {
                if (_degree == null)
                {
                    _degree = new DegreeRepository(_dbContext);
                }

                return _degree;
            }
        }

        public IUserComissionRepository UserComission
        {
            get
            {
                if (_userComission == null)
                {
                    _userComission = new UserComissionRepository(_dbContext);
                }

                return _userComission;
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
