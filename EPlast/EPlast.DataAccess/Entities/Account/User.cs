using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EPlast.DataAccess.Entities
{
    public class User : IdentityUser
    {
        [Required, MaxLength(50, ErrorMessage = "FirstName cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required, MaxLength(50, ErrorMessage = "LastName cannot exceed 50 characters")]
        public string LastName { get; set; }
        [Required, MaxLength(50, ErrorMessage = "FatherName cannot exceed 50 characters")]
        public string FatherName { get; set; }
        public DateTime RegistredOn { get; set; }
        public UserProfile UserProfile { get; set; }
        public ICollection<ConfirmedUser> ConfirmedUsers { get; set; }
        public ICollection<Approver> Approvers { get; set; }
        public ICollection<EventAdmin> Events { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<CityMembers> CityMembers { get; set; }
        public ICollection<CityAdministration> CityAdministrations { get; set; }
        public ICollection<UnconfirmedCityMember> UnconfirmedCityMembers { get; set; }
        public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<RegionAdministration> RegionAdministrations { get; set; }
    }
}