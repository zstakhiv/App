using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserProfileRepository UserProfile { get; }
        INationalityRepository Nationality { get; }
        IEventRepository Event { get;  }
        IGallaryRepository Gallary { get; }
        IParticipantStatusRepository ParticipantStatus { get;}
        IParticipantRepository Participant { get; }
        IEventCategoryRepository EventCategory { get; }
        IEventGallaryRepository EventGallary { get; }
        IEventAdminRepository EventAdmin { get; }
        ISubEventCategoryRepository SubEventCategory { get; }
        IEventStatusRepository EventStatus { get; }



        IEducationRepository Education { get; }
        IDegreeRepository Degree { get; }
        IReligionRepository Religion { get; }
        ISexRepository Sex { get; }
        IWorkRepository Work { get; }
        IConfirmedUserRepository ConfirmedUser { get; }
        IConfirmatorRepository Confirmator { get; }
        void Save();
    }
}
