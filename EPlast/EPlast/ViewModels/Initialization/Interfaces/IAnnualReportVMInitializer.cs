using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.ViewModels.Initialization.Interfaces
{
    public interface IAnnualReportVMInitializer
    {
        IEnumerable<SelectListItem> GetCityMembers(IEnumerable<User> cityMembers);
        IEnumerable<SelectListItem> GetCityLegalStatusTypes(IEnumerable<CityLegalStatusType> cityLegalStatusTypes);
        AnnualReport GetAnnualReport(IEnumerable<User> cityMember, IEnumerable<UserPlastDegreeType> userPlastDegreeTypes);
    }
}