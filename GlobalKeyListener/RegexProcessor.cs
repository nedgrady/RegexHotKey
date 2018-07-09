using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
namespace GlobalKeyListener
{
    class RegexProcessor
        : StreamProcessor<char, string>
    {
        public Regex Regex => _rgx;
        private Regex _rgx;

        private object matchLock = new object();
        private IEnumerable<char> _clearChars;
        // we can use this bit of global state because StreamProcessor locks between the execution 
        // of Test and Transform, caching the match here means we only have to search the regex once.
        private Match _matchCache;

        public RegexProcessor(Regex rgx, bool clearStreamOnMatch, IEnumerable<char> clearChars)
            : base(clearStreamOnMatch, clearChars)
        {
            _rgx = rgx ?? throw new ArgumentNullException("rgx");

            _clearChars = clearChars ?? Enumerable.Empty<char>();
        }

        protected override bool Test(IEnumerable<char> stream)
        {
            StringBuilder s = new StringBuilder();
            foreach (char c in stream)
                s.Append(c);

            //Console.WriteLine("Test " + " "+ s.ToString() +  " " + GetHashCode());
            _matchCache = Regex.Match(stream.GetString());
            return _matchCache.Success;
        }

        protected override string Transform(IEnumerable<char> stream)
        {
            if (!_matchCache.Success)
                throw new Exception("We're trying to traansform on an unmatched regex... this should never happen.....");
        
            return _matchCache.Value;
        }
    }
}
