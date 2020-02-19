using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        INationalityRepository Nationality { get; }
        ICityAdministrationRepository CityAdministration{ get; }
        ICityDocumentsRepository CityDocuments { get; }
        ICityDocumentTypeRepository CityDocumentType { get; }
        ICityMembersRepository CityMembers { get; }
        ICityRepository City { get; }
        IAdminTypeRepository AdminType { get;  }
        IClubRepository Club { get; }
        IClubMembersRepository ClubMembers { get; }
        IClubAdministrationRepository GetClubAdministration { get; }
        IRegionRepository Region { get; }
        IRegionAdministrationRepository RegionAdministration { get; }
        
        void Save();
    }
}
