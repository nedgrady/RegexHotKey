using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RegexHotKey
{
    public sealed class Subscriber
                    : IKeysSubscriber
    {

        private static KeysCallback _callback;

        private readonly RegexProcessor _regexProcessor;
        public RegexProcessor RegexProcessor => _regexProcessor;
        RegexProcessor IKeysSubscriber.GetRegexProcessor() => RegexProcessor;

        public Assembly Assembly => _ass;
        private readonly Assembly _ass;

        public Subscriber(RegexProcessor rgxp, MethodInfo methodInfo, Assembly ass)
            : this(rgxp, ToKeysCallback(methodInfo), ass)
        { }

        public Subscriber(RegexProcessor rgxp, KeysCallback callback, Assembly ass)
        {
            _regexProcessor = rgxp ?? throw new ArgumentNullException("RegexProcessor rgxp");
            _callback = callback ?? throw new ArgumentNullException($"KeysCallback callback");
            _ass = ass ?? throw new ArgumentNullException("Assembly ass");
        }

        public override string ToString()
        {
            return $"{_regexProcessor.Regex} {GetHashCode()}";
        }

        public void Notify(char key)
        {
            if(_regexProcessor.Add(key, out IEnumerable<Match> matchesOut))
            {
                foreach(Match m in matchesOut)
                    _callback(m);
            }
        }

        private static KeysCallback ToKeysCallback(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length < 1
                || parameters[0].ParameterType != typeof(Match))
            {
                //TODO -- handle dis
                throw new Exception();
            }
            else
            {
                return (KeysCallback)Delegate.CreateDelegate(typeof(KeysCallback), methodInfo);
            }
        }

    }
}
