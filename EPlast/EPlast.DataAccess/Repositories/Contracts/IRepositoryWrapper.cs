namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        INationalityRepository Nationality { get; }
        IEducationRepository Education { get;  }
        IDegreeRepository Degree { get; }

        IReligionRepository Religion { get; }

        ISexRepository Sex { get; }

        IWorkRepository Work { get; }

        IUserComissionRepository UserComission { get; }
        void Save();
    }
}
