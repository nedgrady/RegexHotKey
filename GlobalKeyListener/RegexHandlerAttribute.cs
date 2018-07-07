using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GlobalKeyListener
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RegexHandlerAttribute
        : Attribute
    {
        public Regex Regex => _regex;
        private Regex _regex;

        public RegexHandlerAttribute(string strRegex)
        {
            if (strRegex == null)
                throw new ArgumentNullException("strRegex");

            _regex = new Regex(strRegex);

        }
    }
}
