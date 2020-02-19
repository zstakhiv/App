using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserProfileRepository UserProfile { get; }
        INationalityRepository Nationality { get; }
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
