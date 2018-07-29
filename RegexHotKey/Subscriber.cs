using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace RegexHotKey
{
    public sealed class Subscriber<T>
                    : IKeysSubscriber where T : IEnumerable<char>
    {
        private static KeysCallback<T> _callback;
        private readonly RegexProcessor _regexProcessor;

        public Subscriber(RegexProcessor rgxp, MethodInfo methodInfo)
            : this(rgxp, ToKeysCallback(methodInfo))
        { }

        public Subscriber(RegexProcessor rgxp, KeysCallback<T> callback)
        {
            _regexProcessor = rgxp ?? throw new ArgumentNullException("RegexProcessor rgxp");
            _callback = callback ?? throw new ArgumentNullException($"KeysCallback<{typeof(T)}> callback");
        }

        private static KeysCallback<T> ToKeysCallback(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length < 1
                || parameters[0].ParameterType != typeof(T))
            {
                //TODO -- handle dis
                throw new Exception();
            }
            else
            {
                return (KeysCallback<T>)Delegate.CreateDelegate(typeof(KeysCallback<T>), methodInfo);
            }
        }

        public void Notify(char key)
        {
            if(_regexProcessor.Add(key, out IEnumerable<string> itemsOut))
            {
                foreach(string item in itemsOut)
                {
                    _callback(ConvertString(item));
                }
            }
        }

        private T ConvertString(string s)
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(IEnumerable<char>))
            {
                return(T)(object)s;
            }
            else if (typeof(T) == typeof(char[]))
            {
                return (T)(object)s.ToCharArray();
            }
            throw new InvalidCastException("Can't detect the type of the KeysCallback");
        }
    }
}
