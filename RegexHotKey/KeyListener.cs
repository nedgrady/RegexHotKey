using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utilities.Logging;
using System.Resources;

namespace RegexHotKey
{
    public delegate void KeysCallback<T>(T keys)
        where T : IEnumerable<char>;

    public class KeyListener
    {
#region static_block
        public static IReadOnlyList<IKeysSubscriber> Subscribers => _keysSubscribers;

        private static readonly List<IKeysSubscriber> _keysSubscribers
            = new List<IKeysSubscriber>();

        public async static Task Initialize()
        {
            await Logger.Instance.LogAsync(LogLevel.Information, $"Started Registering Subscribers");
            IEnumerable<Assembly> assems = AppDomain.CurrentDomain.GetAttributedAssemblies<KeyDownAttribute>();

            if (assems?.Count() < 1)
                return;

            //List<(MethodInfo, RegexHandlerAttribute)> methods = new List<(MethodInfo, RegexHandlerAttribute)>();
            foreach (Assembly ass in assems)
            {
                await RegisterAssembly(ass);
            }
            Hooker.Instance.OnKeyDown += KeyDown;
        }

        public static async Task<int> RegisterAssembly(Assembly ass)
        {
            int count = 0;
            foreach (Type t in ass.GetTypes())
            {
                foreach ((MethodInfo, RegexHandlerAttribute) subscriber in t.GetStaticAttributedMethods<RegexHandlerAttribute>())
                {
                    RegexHandlerAttribute ra = subscriber.Item2;
                    MethodInfo mi = subscriber.Item1;
                    ParameterInfo[] pis = mi.GetParameters();
                    if (pis.Length != 1 || pis[0].GetType().IsAssignableFrom(typeof(IEnumerable<char>)))
                    {
#if DEBUG
                        Console.WriteLine($"Invalid Type Signature");
#endif
                        await Logger.Instance.LogAsync(LogLevel.Error, Errors.METHOD_SIGNATURE_INVALID, t.Name, mi.Name, $"({mi.GetParameterString()})", ass.GetName().Name);
                        continue;
                    }

                    switch (ra.CallbackType)
                    {
                        case CallbackType.CharArray:
                            if (pis[0].ParameterType != typeof(char[]))
                            {
                                await Logger.Instance.LogAsync(LogLevel.Error, Errors.SUBSCRIBER_PARAMETER_TYPE_MISMATCH, t.Name, mi.Name, $"({mi.GetParameterString()})", ass.GetName().Name, ra.CallbackType.ToString());
                                break;
                            }
                            _keysSubscribers.Add(new Subscriber<char[]>(new RegexProcessor(ra.Regex, ra.ClearTime, ra.ClearOnMatch, ra.ClearChars), mi));
                            break;
                        case CallbackType.CharEnumerable:
                            if (pis[0].ParameterType != typeof(IEnumerable<char>))
                            {
                                await Logger.Instance.LogAsync(LogLevel.Error, Errors.SUBSCRIBER_PARAMETER_TYPE_MISMATCH, t.Name, mi.Name, $"({mi.GetParameterString()})", ass.GetName().Name, ra.CallbackType.ToString());
                                break;
                            }
                            _keysSubscribers.Add(new Subscriber<IEnumerable<char>>(new RegexProcessor(ra.Regex, ra.ClearTime, ra.ClearOnMatch, ra.ClearChars), mi));
                            break;
                        case CallbackType.String:
                            if (pis[0].ParameterType != typeof(string))
                            {
                                await Logger.Instance.LogAsync(LogLevel.Error, Errors.SUBSCRIBER_PARAMETER_TYPE_MISMATCH, t.Name, mi.Name, $"({mi.GetParameterString()})", ass.GetName().Name, ra.CallbackType.ToString());
                                break;
                            }
                            _keysSubscribers.Add(new Subscriber<string>(new RegexProcessor(ra.Regex, ra.ClearTime, ra.ClearOnMatch, ra.ClearChars), mi));
                            break;
                        default:
                            await Logger.Instance.LogAsync(LogLevel.Error, Errors.CALLBACK_TYPE_INVALID, t.Name, mi.Name, $"({mi.GetParameterString()})",ass.GetName().Name, ra.CallbackType.ToString());
#if DEBUG
                            throw new Exception("");
#pragma warning disable CS0162 // Unreachable code detected
#endif
                            break;
#if DEBUG
#pragma warning restore CS0162 // Unreachable code detected
#endif
                    }
                    count++;
                }
            }
            return count;
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
