using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RegexHotKey
{
    public delegate void KeysCallback<T>(T keys)
        where T : IEnumerable<char>;

    public class KeyListener
    {
        #region static_block
        private static readonly List<IKeysSubscriber> _keysSubscribers
            = new List<IKeysSubscriber>();

        public static void Initialize()
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
        }

        private static void KeyDown(char c)
        {
            foreach (var subscriber in _keysSubscribers)
            {
                subscriber.Notify(c);
            }
        }

        public static IKeysSubscriber Register<T>(KeysCallback<T> callback, RegexProcessor rgxp)
            where T : IEnumerable<char>
        {
            IKeysSubscriber subscriber = new Subscriber<T>(rgxp, callback);
            _keysSubscribers.Add(subscriber);
            return subscriber;
        }

        public static bool UnRegister<T>(IKeysSubscriber subscriber)
            where T : IEnumerable<char>
        {
            return _keysSubscribers.Remove(subscriber);
        }
        #endregion

        #region instance_block

        #endregion
    }
}
