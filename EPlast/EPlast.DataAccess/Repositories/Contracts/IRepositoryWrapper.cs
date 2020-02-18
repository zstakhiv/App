namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        INationalityRepository Nationality { get; }

        IReligionRepository Religion { get; }

        ISexRepository Sex { get; }

        IWorkRepository Work { get; }
        void Save();
    }
}
