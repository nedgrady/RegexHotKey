using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
namespace RegexHotKey
{
    static class ReflectionExtensions
    {
        public static IEnumerable<Assembly> GetAttributedAssemblies<T>(this AppDomain appDomain)
            where T : Attribute
        {
            return appDomain.GetAssemblies().Where((ass) => (ass.GetCustomAttribute<T>() != null));
        }



        public static IEnumerable<(MethodInfo, T)> GetAttributedMethods<T>(this Type type)
            where T : Attribute
        {

            IEnumerable<MethodInfo> methods = type.GetType().GetMethods();

            List<(MethodInfo, T)> list = new List<(MethodInfo, T)>();

            foreach (MethodInfo method in methods)
            {
                list.AddRange(method.GetCustomAttributes<T>()
                    .Select((T attribute) => (method, attribute)));
            }

            return list;
        }

        public static IEnumerable<(MethodInfo, T)> GetStaticAttributedMethods<T>(this Type type)
            where T : Attribute
        {

            IEnumerable<MethodInfo> methods =
                type.GetMethods().Where(m => m.IsStatic);

            List<(MethodInfo, T)> list = new List<(MethodInfo, T)>();

            foreach (MethodInfo method in methods)
            {
                list.AddRange(method.GetCustomAttributes<T>()
                    .Select((T attribute) => (method, attribute)));
            }

            return list;
        }

        public static string GetParameterString(this MethodInfo methodInfo) =>
            methodInfo.GetParameters()
                .Select(pi => $"{pi.ParameterType.Name} {pi.Name}")
                .Aggregate((s1, s2) => s1 + ", " + s2);
    }
}
