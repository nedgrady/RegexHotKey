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
        private Regex _regex;

        public CallbackType CallbackType => _cbType;
        private CallbackType _cbType;

        public bool ClearOnMatch => _clearOnMatch;
        private bool _clearOnMatch;

        public IEnumerable<char> ClearChars => _clearChars;
        private char[] _clearChars;

        public RegexHandlerAttribute(
            string strRegex,
            CallbackType type,
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

        }
    }
}
