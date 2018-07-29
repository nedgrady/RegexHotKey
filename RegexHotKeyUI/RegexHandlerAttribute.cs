using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace GlobalKeyListener
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RegexHandlerAttribute
        : Attribute
    {
        //maybe make this an IEnumerable<char> so its contents can't be changed?
        private readonly char[] DEFAULT_WHITESPACE = new char[]{ ' ', '\n', '\r', '\t' };

        public Regex Regex => _regex;
        private readonly Regex _regex;

        public CallbackType CallbackType => _cbType;
        private readonly CallbackType _cbType;

        public TimeSpan ClearTime => _clearTime;
        private readonly TimeSpan _clearTime;

        public bool ClearOnMatch => _clearOnMatch;
        private readonly bool _clearOnMatch;

        public IEnumerable<char> ClearChars => _clearChars;
        private readonly char[] _clearChars;


        public RegexHandlerAttribute(
            string strRegex,
            CallbackType type,
            int clearTimeMs = -1,
            char[] clearChars = null,
            bool clearInputOnMatch = true
            )
        {
            if (strRegex == null)
                throw new ArgumentNullException("strRegex");

            _clearChars = clearChars ?? DEFAULT_WHITESPACE;
            _cbType = type;
            _regex = new Regex(strRegex);
            _clearOnMatch = clearInputOnMatch;

            clearTimeMs = clearTimeMs < 0 ? int.MaxValue : clearTimeMs;
            _clearTime = new TimeSpan(0, 0, 0, 0, clearTimeMs);
        }
    }
}
