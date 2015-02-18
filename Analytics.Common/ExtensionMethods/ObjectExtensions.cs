using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Common.ExtensionMethods
{


    public static class ObjectExtensions
    {
        public static bool IsNotNull<T>(this T obj) where T : class 
        {
            return obj != null;
        }

        public static bool IsNull<T>(this T obj) where T : class 
        {
            return obj == null;
        }

        public static TValue Sandbox<TParent, TValue>(this TParent obj, Func<TParent,TValue> propertyExpr) where TParent : class
        {
            if (obj == null)
            {
                return default(TValue);
            }
            return propertyExpr(obj);
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
