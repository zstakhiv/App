﻿using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserProfileRepository UserProfile { get; }
        INationalityRepository Nationality { get; }
        IOrganizationRepository Organization { get; }
        IDecesionTargetRepository DecesionTarget { get; }
        IDocumentTemplateRepository DocumentTemplate { get; }
        IDecesionRepository Decesion { get; }

        IEventRepository Event { get; }
        IGallaryRepository Gallary { get; }
        IParticipantStatusRepository ParticipantStatus { get; }
        IParticipantRepository Participant { get; }
        IEventCategoryRepository EventCategory { get; }
        IEventGallaryRepository EventGallary { get; }
        IEventAdminRepository EventAdmin { get; }
        IEventTypeRepository EventType { get; }
        IEventStatusRepository EventStatus { get; }

        IEducationRepository Education { get; }
        IDegreeRepository Degree { get; }
        IReligionRepository Religion { get; }
        IGenderRepository Gender { get; }
        IWorkRepository Work { get; }
        IConfirmedUserRepository ConfirmedUser { get; }
        IApproverRepository Approver { get; }
        ICityAdministrationRepository CityAdministration { get; }
        ICityDocumentsRepository CityDocuments { get; }
        ICityDocumentTypeRepository CityDocumentType { get; }
        ICityMembersRepository CityMembers { get; }
        ICityRepository City { get; }
        IUnconfirmedCityMemberRepository UnconfirmedCityMember { get; }
        IAdminTypeRepository AdminType { get; }
        IClubRepository Club { get; }
        IClubMembersRepository ClubMembers { get; }
        IClubAdministrationRepository GetClubAdministration { get; }
        IRegionRepository Region { get; }
        IRegionAdministrationRepository RegionAdministration { get; }
        IAnnualReportsRepository AnnualReports { get; }
        IMembersStatisticsRepository MembersStatistics { get; }
        ICityLegalStatusesRepository CityLegalStatuses { get; }
        IUserPlastDegreesRepository UserPlastDegrees { get; }
        ICityManagementsRepository CityManagements { get; }
        IEventAdministrationRepository EventAdministration { get; }

        void Save();
    }
}