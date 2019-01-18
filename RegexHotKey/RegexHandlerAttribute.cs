using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace RegexHotKey
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RegexHandlerAttribute
        : Attribute
    {

        public Regex Regex => _regex;
        private readonly Regex _regex;

        public TimeSpan ClearTime => _clearTime;
        private readonly TimeSpan _clearTime;

        public bool ClearOnMatch => _clearOnMatch;
        private readonly bool _clearOnMatch;

        public IEnumerable<char> ClearChars => _clearChars;
        private readonly char[] _clearChars;


        public RegexHandlerAttribute(
            string strRegex,
            int clearTimeMs = -1,
            char[] clearChars = null,
            bool clearInputOnMatch = true
            )
        {
            if (strRegex == null)
                throw new ArgumentNullException("strRegex");

            _clearChars = clearChars ?? RegexProcessor.DEFAULT_WHITESPACE;
            _regex = new Regex(strRegex);
            _clearOnMatch = clearInputOnMatch;

            clearTimeMs = clearTimeMs < 0 ? int.MaxValue : clearTimeMs;
            _clearTime = new TimeSpan(0, 0, 0, 0, clearTimeMs);
        }
    }
}
