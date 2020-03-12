using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.ViewModels
{
    public class AnnualReportViewModel
    {
        public string CityName { get; set; }
        public CityAdministration CityAdministration { get; set; }
        public IEnumerable<SelectListItem> CityMembers { get; set; }
        public CityLegalStatus CityLegalStatus { get; set; }
        public IEnumerable<SelectListItem> CityLegalStatusTypes { get; set; }
        public AnnualReport AnnualReport { get; set; }
    }
}