using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ExtensionMethods
{
    public static class NumericExtensions
    {
        public static int Ceil(this double val)
        {
            return Convert.ToInt32(Math.Ceiling(val));
        }

        public static T ToEnum<T>(this int enumVal)
        {
            if (Enum.IsDefined(typeof(T), enumVal))
                return (T)Enum.ToObject(typeof(T), enumVal);

            return default(T);
        }

        public static T ToEnum<T>(this short enumVal)
        {
            if (Enum.IsDefined(typeof(T), enumVal))
                return (T)Enum.ToObject(typeof(T), enumVal);

            return default(T);
        }

        //public static decimal GetDefaultValue(this decimal? value)
        //{
        //    return value.HasValue ? Convert.ToDecimal(value) : 0;
        //}

        //public static T GetDefaultValue<T>(this T? t) where T : struct
        //{
        //    if (t.HasValue)
        //    {
        //        return t.Value;
        //    }
        //    else
        //    {
        //       // return (T) Convert.ChangeType(0, t.GetType());
        //        return default(T);
        //    }
        //}
    }
}
