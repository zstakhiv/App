using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EPlast.DataAccess.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Ім'я")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,20})*$",
            ErrorMessage = "Ім'я має містити тільки літери")]
        [Required, MaxLength(50, ErrorMessage = "Ім'я не може перевищувати 50 символів")]
        public string FirstName { get; set; }

        [Display(Name = "Прізвище")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,20})*$",
            ErrorMessage = "Прізвище має містити тільки літери")]
        [Required, MaxLength(50, ErrorMessage = "Прізвище не може перевищувати 50 символів")]
        public string LastName { get; set; }

        [Display(Name = "По-батькові")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,20}((\s+|-)[a-zA-Zа-яА-ЯІіЄєЇїҐґ'.`]{1,20})*$",
            ErrorMessage = "По-батькові має містити тільки літери")]
        [MaxLength(50, ErrorMessage = "По-батькові не може перевищувати 50 символів")]
        public string FatherName { get; set; }
        public DateTime RegistredOn { get; set; }
        public string ImagePath { get; set; }
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
        public ICollection<AnnualReport> AnnualReports { get; set; }
        public ICollection<UserPlastDegree> UserPlastDegrees { get; set; }
    }
}