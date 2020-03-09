﻿using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private EPlastDBContext _dbContext;
        private IUserRepository _user;
        private IUserProfileRepository _userprofile;
        private INationalityRepository _nationality;
        private IOrganizationRepository _organization;
        private IDecesionTargetRepository _decesionTarget;
        private IDecesionStatusRepository _decesionStatus;
        private IDocumentTemplateRepository _documentTemplate;
        private IDecesionRepository _decesion;
        private IEventRepository _event;
        private IParticipantStatusRepository _participantStatuses;
        private IGallaryRepository _gallary;
        private IEventGallaryRepository _eventGallary;
        private IParticipantRepository _participant;
        private IEventCategoryRepository _eventCategory;
        private IEventAdminRepository _eventAdmin;
        private ISubEventCategoryRepository _subEventCategory;
        private IEventStatusRepository _eventStatus;
        private IReligionRepository _religion;
        private IGenderRepository _gender;
        private IWorkRepository _work;
        private IEducationRepository _education;
        private IDegreeRepository _degree;
        private IConfirmedUserRepository _confirmedUser;
        private IApproverRepository _approver;
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
        private IAnnualReportsRepository _annualReports;
        private IAnnualReportStatusesRepository _annualReportStatuses;
        private IMembersStatisticsRepository _membersStatistics;
        private ICityLegalStatusesRepository _cityLegalStatuses;
        private ICityLegalStatusTypesRepository _cityLegalStatusTypes;

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

        public IOrganizationRepository Organization
        {
            get
            {
                if (_organization == null)
                {
                    _organization = new OrganizationRepository(_dbContext);
                }
                return _organization;
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

        public IUserProfileRepository UserProfile
        {
            get
            {
                if (_userprofile == null)
                {
                    _userprofile = new UserProfileRepository(_dbContext);
                }
                return _userprofile;
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

        public IEventAdminRepository EventAdmin
        {
            get
            {
                if (_eventAdmin == null)
                {
                    _eventAdmin = new EventAdminRepository(_dbContext);
                }

                return _eventAdmin;
            }
        }

        public IReligionRepository Religion
        {
            get
            {
                if (_religion == null)
                {
                    _religion = new ReligionRepository(_dbContext);
                }

                return _religion;
            }
        }

        public IGenderRepository Gender
        {
            get
            {
                if (_gender == null)
                {
                    _gender = new GenderRepository(_dbContext);
                }

                return _gender;
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

                return _work;
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

        public IConfirmedUserRepository ConfirmedUser
        {
            get
            {
                if (_confirmedUser == null)
                {
                    _confirmedUser = new ConfirmedUserRepository(_dbContext);
                }

                return _confirmedUser;
            }
        }

        public IApproverRepository Approver
        {
            get
            {
                if (_approver == null)
                {
                    _approver = new ApproverRepository(_dbContext);
                }

                return _approver;
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
                if (_clubMembers == null)
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
                if (_clubAdministration == null)
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

        public IAnnualReportsRepository AnnualReports
        {
            get
            {
                if (_annualReports == null)
                {
                    _annualReports = new AnnualReportsRepository(_dbContext);
                }
                return _annualReports;
            }
        }

        public IAnnualReportStatusesRepository AnnualReportStatuses
        {
            get
            {
                if (_annualReportStatuses == null)
                {
                    _annualReportStatuses = new AnnualReportStatusesRepository(_dbContext);
                }
                return _annualReportStatuses;
            }
        }

        public IMembersStatisticsRepository MembersStatistics
        {
            get
            {
                if (_membersStatistics == null)
                {
                    _membersStatistics = new MembersStatisticsRepository(_dbContext);
                }
                return _membersStatistics;
            }
        }

        public ICityLegalStatusesRepository CityLegalStatuses
        {
            get
            {
                if (_cityLegalStatuses == null)
                {
                    _cityLegalStatuses = new CityLegalStatusesRepository(_dbContext);
                }
                return _cityLegalStatuses;
            }
        }

        public ICityLegalStatusTypesRepository CityLegalStatusTypes
        {
            get
            {
                if (_cityLegalStatusTypes == null)
                {
                    _cityLegalStatusTypes = new CityLegalStatusTypeRepository(_dbContext);
                }
                return _cityLegalStatusTypes;
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