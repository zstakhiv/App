namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        INationalityRepository Nationality { get; }
        IEventRepository Event { get;  }
        IGallaryRepository Gallary { get; }
        void Save();
    }
}
