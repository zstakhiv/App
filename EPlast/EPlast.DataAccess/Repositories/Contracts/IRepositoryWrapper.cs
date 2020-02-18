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

        void Save();
    }
}