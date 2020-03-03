using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class City
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "City name cannot exceed 50 characters")]
        public string Name { get; set; }
        [MaxLength(16, ErrorMessage = "City phone number cannot exceed 16 characters")]
        public string PhoneNumber { get; set; }
        [MaxLength(50, ErrorMessage = "City email cannot exceed 50 characters")]
        public string Email { get; set; }
        [MaxLength(256, ErrorMessage = "City url cannot exceed 256 characters")]
        public string CityURL { get; set; }
        [MaxLength(1024, ErrorMessage = "City description cannot exceed 1024 characters")]
        public string Description { get; set; }
        [Required, MaxLength(60, ErrorMessage = "City street cannot exceed 60 characters")]
        public string Street { get; set; }
        [Required, MaxLength(10, ErrorMessage = "City house number cannot exceed 10 characters")]
        public string HouseNumber { get; set; }
        [MaxLength(10, ErrorMessage = "City office number cannot exceed 10 characters")]
        public string OfficeNumber { get; set; }
        [MaxLength(7, ErrorMessage = "City post index cannot exceed 7 characters")]
        public string PostIndex { get; set; }
        public Region Region { get; set; }
        public ICollection<CityDocuments> CityDocuments { get; set; }
        public ICollection<CityMembers> CityMembers { get; set; }
        public ICollection<UnconfirmedCityMember> UnconfirmedCityMember { get; set; }
        public ICollection<CityAdministration> CityAdministration { get; set; }
        public ICollection<AnnualReport> AnnualReports { get; set; }
    }
}
