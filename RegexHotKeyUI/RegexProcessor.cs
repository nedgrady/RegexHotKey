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
        private readonly Regex _rgx;

        private readonly object matchLock = new object();
        private readonly IEnumerable<char> _clearChars;

        private readonly TimeSpan _clearTime;
        // we can use this bit of global state because StreamProcessor locks between the execution 
        // of Test and Transform, caching the match here means we only have to search the regex once.
        private Match _matchCache;
        private DateTime _lastTime = DateTime.MaxValue;

        public RegexProcessor(Regex rgx, TimeSpan clearTime, bool clearStreamOnMatch, IEnumerable<char> clearChars)
            : base(clearStreamOnMatch, clearChars)
        {
            _rgx = rgx ?? throw new ArgumentNullException("rgx");

            _clearTime = clearTime;

            _clearChars = clearChars ?? Enumerable.Empty<char>();
        }

        protected override bool Test(IEnumerable<char> stream)
        {
            bool timeout = (DateTime.Now - _lastTime) > _clearTime;
            string strStream;
            char last;
            if(timeout)
            {
                last = stream.Last();
                strStream = last.ToString();
                Clear();
                _stream.Add(last);
            }
            else
            {
                strStream = stream.GetString();
            }

            _matchCache = Regex.Match(strStream);
            _lastTime = DateTime.Now;
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
