﻿using System;
using System.ComponentModel;

namespace EPlast.BussinessLayer.ExtensionMethods
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T val)
            where T : Enum
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}