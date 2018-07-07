using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GlobalKeyListener
{
    public class KeyListener
    {
       

        static Dictionary<(MethodInfo, RegexHandlerAttribute), CharCallback> _subscriberMap 
            = new Dictionary<(MethodInfo, RegexHandlerAttribute), CharCallback>();

        static KeyListener()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            IEnumerable<Assembly> assems = currentDomain.GetAttributedAssemblies<KeyDownAttribute>();


            if (assems?.Count() < 1)
                return;

            List<(MethodInfo, RegexHandlerAttribute)> methods = new List<(MethodInfo, RegexHandlerAttribute)>();
            foreach (Assembly ass in assems)
            {
                foreach(Type t in ass.GetTypes())
                {
                    foreach((MethodInfo, RegexHandlerAttribute) subscriber in t.GetStaticAttributedMethods<RegexHandlerAttribute>())
                    {
                        var func = (CharCallback)Delegate.CreateDelegate(typeof(CharCallback), subscriber.Item1);
                        _subscriberMap.Add(subscriber, func);

                        Hooker.Instance.OnKeyDown += KeyDown;
                    }
                }
            }


            //List the assemblies in the current application domain.
            Console.WriteLine("List of assemblies loaded in current appdomain:");
            foreach (Assembly assem in assems)
            {
                Console.WriteLine(assem.ToString());
            }
                
        }

        public void Register(object subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");

            Register(subscriber.GetType());

        }

        public void Register(Type subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");

            Assembly ass = subscriber.Assembly;

        }


        private static void KeyDown(char c)
        {
            //TODO - Process how each method wants to receive the Key Down, e.g. string, char[] char
            foreach (var subscriber in _subscriberMap)
            {
                Console.WriteLine("KeyListener.KeyDown");
                subscriber.Value?.Invoke(c);
            }
        }
    }
}
