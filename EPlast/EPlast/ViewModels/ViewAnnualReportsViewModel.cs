using System.Collections.Generic;
using EPlast.DataAccess.Entities;

namespace EPlast.ViewModels
{
    public class ViewAnnualReportsViewModel
    {
        public IEnumerable<AnnualReport> AnnualReports { get; set; }
        public IEnumerable<City> Cities { get; set; }
    }
}