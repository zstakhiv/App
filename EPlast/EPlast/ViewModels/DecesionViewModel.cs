using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class DecesionViewModel
    {
        public Decesion Decesion { get; set; }
        public IEnumerable<SelectListItem> SelectListItems { get; set; }
        public IEnumerable<DecesionTarget> DecesionTargets { get; set; }
    }
}