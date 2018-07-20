﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GlobalKeyListener
{
    //TODO -- need an individual delegate for each regex instace.. derp

    public delegate void KeysCallback<T>(T keys)
        where T : IEnumerable<char>;

    public class KeyListener
    {

        static readonly List<(RegexProcessor, Type)> _subscriberMap
            = new List<(RegexProcessor, Type)>();

        //static Dictionary<Type, Delegate> _delegateMap = new Dictionary<Type, Delegate>();
        private static KeysCallback<char[]> charArrCallback;
        private static KeysCallback<IEnumerable<char>> enumerableCharCallback;
        private static KeysCallback<string> stringCallback;


        static KeyListener()
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
                        //var func = (KeysCallback)Delegate.CreateDelegate(typeof(CharCallback), subscriber.Item1);
                        //Delegate callback = null;
                        Type type;
                        switch (ra.CallbackType)
                        {
                            //TODO - this is really quite horrible...
                            case CallbackType.CharArray:
                                type = typeof(char[]);
                                KeysCallback<char[]> charArrCb = (KeysCallback<char[]>)
                                    Delegate.CreateDelegate(typeof(KeysCallback<char[]>), subscriber.Item1);

                                charArrCallback += charArrCb;
                                break;
                            case CallbackType.CharEnumerable:
                                type = typeof(IEnumerable<char>);
                                KeysCallback<IEnumerable<char>> charEnumCb = (KeysCallback<IEnumerable<char>>)
                                    Delegate.CreateDelegate(typeof(KeysCallback<IEnumerable<char>>), subscriber.Item1);

                                enumerableCharCallback += charEnumCb;
                                break;
                            case CallbackType.String:
                                type = typeof(string);
                                KeysCallback<string> stringCb = (KeysCallback<string>)
                                    Delegate.CreateDelegate(typeof(KeysCallback<string>), subscriber.Item1);

                                stringCallback += stringCb;
                                break;
                            default:
                                throw new Exception("");
                        }

                        _subscriberMap.Add((new RegexProcessor(ra.Regex, ra.ClearTime, ra.ClearOnMatch, ra.ClearChars), type));
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

        //public static bool Register<T>(KeysCallback<T> callback, Regex r, TimeSpan clearTime, bool clearOnmatch, IEnumerable<char> clearChars)
        //    where T:IEnumerable<char>
        //{
        //    if(callback is KeysCallback<char[]>)
        //    {
        //        charArrCallback += ((KeysCallback<char[]>)callback);
        //    }
        //}

        private static void AddDel()
        {

        }


        private static void KeyDown(char c)
        {
            foreach (var subscriber in _subscriberMap)
            {
                RegexProcessor rp = subscriber.Item1;
                Type type = subscriber.Item2;

                if (rp.Add(c, out string itemOut, rp.ClearOnMatch))
                {
                    //don't forget string implements IEnumerable<char> =]
                    if (type == typeof(string) || type == typeof(IEnumerable<char>))
                    {
                        stringCallback(itemOut);
                    }
                    else if(type == typeof(char[]))
                    {
                        charArrCallback(itemOut.ToArray());
                    }
                }
            }
        }
    }
}
