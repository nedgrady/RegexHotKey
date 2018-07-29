using System;
using System.Collections.Generic;
using System.Text;

namespace RegexHotKey
{
    public static class CollectionExtensions
    {

        public static string GetString(this IEnumerable<char> stream)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in stream)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        //public static void CreateOrAddDelegate<T>(this IDictionary<Type, T> dict, T del)
        //    //where T : System.Delegate
        //{
        //    Type type = typeof(T);
        //
        //    if (dict.ContainsKey(type))
        //    {
        //        var d = (T)dict[type];
        //        (Delegate)d += (Delegate)del;
        //    }
        //    else
        //    {
        //        dict[type] += del.Method;
        //    }
        //
        //
        //}
    }
}
