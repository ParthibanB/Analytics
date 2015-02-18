﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Common.ExtensionMethods
{
    public static class TypeExtensions
    {
        public static bool IsString(this Type type)
        {
            return type == typeof(string);
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive && !IsString(type) && type != typeof(IntPtr);
        }

        public static bool IsSimple(this Type type)
        {
            return type.IsPrimitive || IsString(type) || type.IsEnum;
        }

        public static bool IsConcrete(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        public static bool IsNotConcrete(this Type type)
        {
            return !type.IsConcrete();
        }

        /// <summary>
        /// Returns true if the type is a DateTime or nullable DateTime
        /// </summary>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public static bool IsDateTime(this Type typeToCheck)
        {
            return typeToCheck == typeof(DateTime) || typeToCheck == typeof(DateTime?);
        }

        public static bool IsBoolean(this Type typeToCheck)
        {
            return typeToCheck == typeof(bool) || typeToCheck == typeof(bool?);
        }

        /// <summary>
        /// Displays type names using CSharp syntax style. Supports funky generic types.
        /// </summary>
        /// <param name="type">Type to be pretty printed</param>
        /// <returns></returns>
        public static string PrettyPrint(this Type type)
        {
            return type.PrettyPrint(t => t.Name);
        }

        /// <summary>
        /// Displays type names using CSharp syntax style. Supports funky generic types.
        /// </summary>
        /// <param name="type">Type to be pretty printed</param>
        /// <param name="selector">Function determining the name of the type to be displayed. Useful if you want a fully qualified name.</param>
        /// <returns></returns>
        public static string PrettyPrint(this Type type, Func<Type, string> selector)
        {
            string typeName = selector(type) ?? string.Empty;
            if (!type.IsGenericType)
            {
                return typeName;
            }

            Func<Type, string> genericParamSelector = type.IsGenericTypeDefinition ? t => t.Name : selector;
            string genericTypeList = String.Join(",", type.GetGenericArguments().Select(genericParamSelector).ToArray());
            int tickLocation = typeName.IndexOf('`');
            if (tickLocation >= 0)
            {
                typeName = typeName.Substring(0, tickLocation);
            }
            return string.Format("{0}<{1}>", typeName, genericTypeList);
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the type is:
        /// int, long, decimal, short, float, or double
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Bool indicating whether the type is numeric</returns>
        public static bool IsNumeric(this Type type)
        {
            return type.IsFloatingPoint() || type.IsIntegerBased();
        }


        /// <summary>
        /// Returns a boolean value indicating whether or not the type is:
        /// int, long or short
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Bool indicating whether the type is integer based</returns>
        public static bool IsIntegerBased(this Type type)
        {
            return _integerTypes.Contains(type);
        }

        private static readonly IList<Type> _integerTypes = new List<Type>
                                    {
                                        typeof (byte),
                                        typeof (short),
                                        typeof (int),
                                        typeof (long),
                                        typeof (sbyte),
                                        typeof (ushort),
                                        typeof (uint),
                                        typeof (ulong),
                                        typeof (byte?),
                                        typeof (short?),
                                        typeof (int?),
                                        typeof (long?),
                                        typeof (sbyte?),
                                        typeof (ushort?),
                                        typeof (uint?),
                                        typeof (ulong?)
                                    };

        /// <summary>
        /// Returns a boolean value indicating whether or not the type is:
        /// decimal, float or double
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Bool indicating whether the type is floating point</returns>
        public static bool IsFloatingPoint(this Type type)
        {
            return type == typeof(decimal) || type == typeof(float) || type == typeof(double);
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static object GetDefault(Type type)
        {
            return ﻿type.IsValueType ? Activator.CreateInstance(type) : null;﻿﻿﻿﻿﻿
        }

        public static string GetPropertyName<TSource, TProperty>(this TSource source,
    Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo.IsNotNull()? propInfo.Name: string.Empty;
        }
    }
}
