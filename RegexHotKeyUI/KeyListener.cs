using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using PostSharp.Aspects;
namespace GlobalKeyListener
{
    public delegate void KeysCallback<T>(T keys)
        where T : IEnumerable<char>;

    public class KeyListener
    {
        #region static_block
        private static readonly List<(RegexProcessor, Type)> _subscriberMap
            = new List<(RegexProcessor, Type)>();

        private static readonly List<IKeysSubscriber> _keysSubscribers
            = new List<IKeysSubscriber>();

        [ModuleInitializerAttribute(0)]
        static void ModuleInitialize()
        {
            IEnumerable<Assembly> assems = AppDomain.CurrentDomain.GetAttributedAssemblies<KeyDownAttribute>();

            if (assems?.Count() < 1)
                return;

            List<(MethodInfo, RegexHandlerAttribute)> methods = new List<(MethodInfo, RegexHandlerAttribute)>();
            foreach (Assembly ass in assems)
            {
                foreach (Type t in ass.GetTypes())
                {
                    foreach ((MethodInfo, RegexHandlerAttribute) subscriber in t.GetStaticAttributedMethods<RegexHandlerAttribute>())
                    {
                        RegexHandlerAttribute ra = subscriber.Item2;
                        MethodInfo mi = subscriber.Item1;

                        if(mi.GetParameters().Length < 1)
                        {
                            //TODO - think of something to do in this case...
                            Console.WriteLine("Invalid Type Signature");
                            continue;
                        }
                        
                        Type type;
                        switch (ra.CallbackType)
                        {
                            case CallbackType.CharArray:
                                _keysSubscribers.Add(new Subscriber<char[]>(new RegexProcessor(ra.Regex, ra.ClearTime, ra.ClearOnMatch, ra.ClearChars), mi));
                                break;
                            case CallbackType.CharEnumerable:
                                _keysSubscribers.Add(new Subscriber<IEnumerable<char>>(new RegexProcessor(ra.Regex, ra.ClearTime, ra.ClearOnMatch, ra.ClearChars), mi));
                                break;
                            case CallbackType.String:
                                _keysSubscribers.Add(new Subscriber<string>(new RegexProcessor(ra.Regex, ra.ClearTime, ra.ClearOnMatch, ra.ClearChars), mi));
                                break;
                            default:
                                //TODO - handle this
                                throw new Exception("");
                        }
                    }
                }
            }

            Hooker.Instance.OnKeyDown += KeyDown;

            //List the assemblies in the current application domain.
            Console.WriteLine("List of assemblies loaded in current appdomain:");
            foreach (Assembly assem in assems)
            {
                Console.WriteLine(assem.ToString());
            }

        }

        private static void KeyDown(char c)
        {
            foreach (var subscriber in _keysSubscribers)
            {
                subscriber.Notify(c);
            }
        }
        #endregion

        #region instance_block
        
        #endregion
    }
}
