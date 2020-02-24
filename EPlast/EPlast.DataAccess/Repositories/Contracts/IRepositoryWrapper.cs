namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        INationalityRepository Nationality { get; }
        IOrgranRepository Orgran { get; }
        IDecesionTargetRepository DecesionTarget { get; }
        IDecesionStatusRepository DecesionStatus { get; }
        IDocumentTemplateRepository DocumentTemplate { get; }
        IDecesionRepository Decesion { get; }

        IEventRepository Event { get;  }
        IGallaryRepository Gallary { get; }
        IParticipantStatusRepository ParticipantStatus { get;}
        IParticipantRepository Participant { get; }
        IEventCategoryRepository EventCategory { get; }
        IEventGallaryRepository EventGallary { get; }
        IEventAdminRepository EventAdmin { get; }
        ISubEventCategoryRepository SubEventCategory { get; }
        IEventStatusRepository EventStatus { get; }

        void Save();
    }
}