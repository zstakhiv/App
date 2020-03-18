﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using EPlast.DataAccess.Entities;
using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.ViewModels.Initialization.Interfaces;

namespace EPlast.ViewModels.Initialization
{
    public class AnnualReportVMInitializer : IAnnualReportVMInitializer
    {
        public IEnumerable<SelectListItem> GetCityMembers(IEnumerable<User> cityMembers)
        {
            var users = new List<SelectListItem>();
            foreach (var cityMember in cityMembers)
            {
                users.Add(new SelectListItem
                    {
                        Value = cityMember.Id,
                        Text = $"{cityMember.FirstName} {cityMember.LastName}"
                    });
            }
            return users;
        }

        public IEnumerable<SelectListItem> GetCityLegalStatusTypes()
        {
            var cityLegalStatusTypesSLI = new List<SelectListItem>();
            foreach (Enum cityLegalStausType in Enum.GetValues(typeof(CityLegalStatusType)))
            {
                cityLegalStatusTypesSLI.Add(new SelectListItem
                {
                    Value = cityLegalStausType.ToString(),
                    Text = cityLegalStausType.GetDescription()
                });
            }
            return cityLegalStatusTypesSLI;
        }

        public AnnualReport GetAnnualReport(IEnumerable<User> cityMembers)
        {
            return new AnnualReport
            {
                MembersStatistic = new MembersStatistic
                {
                    NumberOfSeniorPlastynSupporters = cityMembers
                        .ToList()
                        .Where(cm => cm.UserPlastDegrees.Any(upd => upd.DateFinish == null 
                            && upd.UserPlastDegreeType == UserPlastDegreeType.SeniorPlastynSupporter))
                        .Count(),
                    NumberOfSeniorPlastynMembers = cityMembers
                        .ToList()
                        .Where(cm => cm.UserPlastDegrees.Any(upd => upd.DateFinish == null
                            && upd.UserPlastDegreeType == UserPlastDegreeType.SeniorPlastynMember))
                        .Count(),
                    NumberOfSeigneurSupporters = cityMembers
                        .ToList()
                        .Where(cm => cm.UserPlastDegrees.Any(upd => upd.DateFinish == null
                            && upd.UserPlastDegreeType == UserPlastDegreeType.SeigneurSupporter))
                        .Count(),
                    NumberOfSeigneurMembers = cityMembers
                        .ToList()
                        .Where(cm => cm.UserPlastDegrees.Any(upd => upd.DateFinish == null
                            && upd.UserPlastDegreeType == UserPlastDegreeType.SeigneurMember))
                        .Count(),
                }
            };
        }
    }
}