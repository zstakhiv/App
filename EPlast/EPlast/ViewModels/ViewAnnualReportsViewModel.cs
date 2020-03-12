using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPlast.DataAccess.Entities;

namespace EPlast.ViewModels
{
    public class ViewAnnualReportsViewModel
    {
        public IEnumerable<SelectListItem> Regions { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
        public IEnumerable<AnnualReport> UnconfirmedAnnualReports { get; set; }
        public IEnumerable<AnnualReport> ConfirmedAnnualReports { get; set; }
        public IEnumerable<AnnualReport> CanceledAnnualReports { get; set; }
    }
}