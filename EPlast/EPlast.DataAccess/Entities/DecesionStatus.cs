using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public enum DecesionStatus
    {
        [Description("У розгляді")]
        InReview,

        [Description("Підтверджено")]
        Confirmed,

        [Description("Скасовано")]
        Canceled
    }
}