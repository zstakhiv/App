namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        INationalityRepository Nationality { get; }
        IEducationRepository Education { get;  }
        IDegreeRepository Degree { get; }
        void Save();
    }
}
