using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DontThink.Utilities.Logging;
using System.Resources;
using System.Text.RegularExpressions;

namespace RegexHotKey
{
    public delegate void KeysCallback(Match match);

    //TODO - should this be a static class
    public class KeyListener
    {
#region static_block
        public static IEnumerable<IKeysSubscriber> Subscribers => _keysSubscribers.Values.SelectMany(assS => assS);

        private static readonly Dictionary<Assembly, List<IKeysSubscriber>> _keysSubscribers
            = new Dictionary<Assembly, List<IKeysSubscriber>>();

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
            if (await Logger.Instance.ThrowOrLogNullArgument(ass, "ass"))
                return 0;

            int count = 0;

            // why on earth do we have to call UnregisterAssembly here rather than removing via assembly reference?
            // well that's because can't assume that an in-memory compiled assembly will have the same reference a second time
            // so we have to check its KeyDown GUID.
            UnregisterAssembly(ass);
            _keysSubscribers.Add(ass, new List<IKeysSubscriber>());

            foreach (Type t in ass.GetTypes())
            {
                foreach ((MethodInfo methodInfo, RegexHandlerAttribute attr) in t.GetStaticAttributedMethods<RegexHandlerAttribute>())
                {
                    RegexHandlerAttribute ra = attr;
                    MethodInfo mi = methodInfo;
                    ParameterInfo[] pis = mi.GetParameters();
                    if (pis.Length != 1 || pis[0].GetType().IsAssignableFrom(typeof(Match)))
                    {
                        await Logger.Instance.LogAsync(LogLevel.Error, Errors.METHOD_SIGNATURE_INVALID, t.Name, mi.Name, $"({mi.GetParameterString()})", ass.GetName().Name);
                        continue;
                    }

                    _keysSubscribers[ass].Add(new Subscriber(ra.ToRegexProcessor(), mi, ass));

                    count++;
                }
            }
            return count;
        }

        public static bool UnregisterAssembly(Assembly ass)
        {
            if (ass == null || !ass.TryGetKeyDownGuid(out Guid guid))
                return false;

            foreach (Assembly sub in _keysSubscribers.Keys.Where(a => a.GetKeyDownGuid() == guid))
                _keysSubscribers.Remove(sub);

            return true;
        }

        private static void KeyDown(char c)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.Notify(c);
            }
        }
        //TODO - how to implement these?
        /*public static IKeysSubscriber Register(KeysCallback callback, RegexProcessor rgxp)
        {
            _keysSubscribers.Add(new Subscriber(rgxp, callback));
            return new Subscriber(rgxp, callback);
        }

        public static async Task<bool> UnRegister(IKeysSubscriber subscriber)
        {
            await Logger.Instance.ThrowOrLogNullArgument(subscriber, "subscriber");

            return _keysSubscribers.Remove(subscriber);
        }*/

#endregion


    }
}
