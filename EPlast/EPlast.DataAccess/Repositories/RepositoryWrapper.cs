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
        private IUnconfirmedCityMemberRepository _unconfirmedCityMember;
        private IAdminTypeRepository _admintype;
        private IClubRepository _club;
        private IClubMembersRepository _clubMembers;
        private IClubAdministrationRepository _clubAdministration;
        private IRegionRepository _region;
        private IRegionAdministrationRepository _regionAdministration;

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

        public IUnconfirmedCityMemberRepository UnconfirmedCityMember
        {
            get
            {
                if (_unconfirmedCityMember == null)
                {
                    _unconfirmedCityMember = new UnconfirmedCityMemberRepository(_dbContext);
                }

                return _unconfirmedCityMember;
            }
        }

        public IAdminTypeRepository AdminType
        {
            get
            {
                if (_admintype == null)
                {
                    _admintype = new AdminTypeRepository(_dbContext);
                }

                return _admintype;
            }
        }

        public IClubRepository Club
        {
            get
            {
                if (_club == null)
                {
                    _club = new ClubRepository(_dbContext);
                }

                return _club;
            }
        }

        public IClubMembersRepository ClubMembers
        {
            get
            {
                if (_clubMembers==null)
                {
                    _clubMembers = new ClubMembersRepository(_dbContext);
                }

                return _clubMembers;
            }
        }

        public IClubAdministrationRepository GetClubAdministration
        {
            get
            {
                if(_clubAdministration==null)
                {
                    _clubAdministration = new ClubAdministrationRepository(_dbContext);
                }

                return _clubAdministration;
            }
        }

        public IRegionRepository Region
        {
            get
            {
                if (_region == null)
                {
                    _region = new RegionRepository(_dbContext);
                }

                return _region;
            }
        }

        public IRegionAdministrationRepository RegionAdministration
        {
            get
            {
                if (_regionAdministration == null)
                {
                    _regionAdministration = new RegionAdministrationRepository(_dbContext);
                }

                return _regionAdministration;
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
