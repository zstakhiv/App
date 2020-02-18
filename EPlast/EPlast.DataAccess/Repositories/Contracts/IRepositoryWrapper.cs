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
        void Save();
    }
}
