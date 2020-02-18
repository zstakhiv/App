using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private EPlastDBContext _dbContext;
        private IUserRepository _user;
        private INationalityRepository _nationality;
        private ICityAdministrationRepository _cityAdministration;
        private ICityDocumentsRepository _cityDocuments;
        private ICityDocumentTypeRepository _cityDocumentType;
        private ICityMembersRepository _cityMembers;
        private ICityRepository _city;

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

        public ICityAdministrationRepository CityAdministration
        {
            get
            {
                if (_cityAdministration == null)
                {
                    _cityAdministration = new CityAdministrationRepository(_dbContext);
                }

                return _cityAdministration;
            }
        }

        public ICityDocumentsRepository CityDocuments
        {
            get
            {
                if (_cityDocuments == null)
                {
                    _cityDocuments = new CityDocumentsRepository(_dbContext);
                }

                return _cityDocuments;
            }
        }

        public ICityDocumentTypeRepository CityDocumentType
        {
            get
            {
                if (_cityDocumentType == null)
                {
                    _cityDocumentType = new CityDocumentTypeRepository(_dbContext);
                }

                return _cityDocumentType;
            }
        }

        public ICityMembersRepository CityMembers
        {
            get
            {
                if (_cityMembers == null)
                {
                    _cityMembers = new CityMembersRepository(_dbContext);
                }

                return _cityMembers;
            }
        }

        public ICityRepository City
        {
            get
            {
                if (_city == null)
                {
                    _city = new CityRepository(_dbContext);
                }

                return _city;
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
