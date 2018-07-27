using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
namespace GlobalKeyListener
{
    static class ReflectionExtensions
    {
        public static IEnumerable<Assembly> GetAttributedAssemblies<T>(this AppDomain appDomain)
            where T : Attribute
        {
            /*return appDomain.GetAssemblies()
                .Where((ass) =>
                {
                    return ass.GetCustomAttributes<T>(typeof(T)) != null;
                });*/

            return appDomain.GetAssemblies().Where((ass) => (ass.GetCustomAttribute<T>() != null));

            /*foreach (Assembly ass in appDomain.GetAssemblies())
                if (ass.GetCustomAttribute<T>() != null)
                    yield return ass;*/
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

        /*public static IEnumerable<(MethodInfo, Attribute)> GetStaticAttributedMethods(this Type type, Type attribute)
        {
            return type?
                .GetAttributedMethods(attribute.GetType())
                .Where((MethodInfo method) => method.IsStatic);
        }

        public static IEnumerable<(MethodInfo, Attribute)> GetInstanceAttributedMethods(this Type type, Type attribute)
        {
            return type?
                .GetAttributedMethods(attribute.GetType())
                .Where((MethodInfo method) => !method.IsStatic);
        }*/
    }
}
