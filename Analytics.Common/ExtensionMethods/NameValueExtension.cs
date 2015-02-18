using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
namespace Common.ExtensionMethods
{
    public static class NameValueExtension
    {
        public static string[] SafeGet(this NameValueCollection nameValColl, string key)
        {
            string[] val = default(string[]);
            if (nameValColl != null && key != null)
            {
                return nameValColl.GetValues(key);
            }
            return val;
        }

        public static string SafeGetFirstValue(this NameValueCollection nameValColl, string key)
        {
            var values = nameValColl.SafeGet(key);
            return values.IsCollectionValid() ? values[0].Trim() : string.Empty;
        }
    }
}
