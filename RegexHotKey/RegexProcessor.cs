using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using DontThink.Utilities.Logging;

namespace RegexHotKey
{
    public class RegexProcessor
        : StreamProcessor<char, IEnumerable<Match>>
    {
        public static readonly char[] DEFAULT_WHITESPACE =  { ' ', '\n', '\r', '\t' };

        public Regex Regex => _rgx;
        private readonly Regex _rgx;

        private readonly object matchLock = new object();

        private readonly TimeSpan _clearTime;
        // we can use this bit of global state because StreamProcessor locks between the execution 
        // of Test and Transform, caching the match here means we only have to search the regex once.
        private MatchCollection _matchCache;
        private DateTime _lastTime = DateTime.MaxValue;

        public RegexProcessor(Regex rgx, TimeSpan? clearTime = null, bool clearStreamOnMatch = true, IEnumerable<char> clearChars = null)
            : base(clearStreamOnMatch, clearChars ?? Enumerable.Empty<char>())
        {
            //TODO - handle null regex here
            _rgx = rgx ?? throw new ArgumentNullException("rgx");

            //TODO - this is a bit of a hack
            _clearTime = clearTime ?? new TimeSpan(long.MaxValue);
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

            _matchCache = Regex.Matches(strStream);
            _lastTime = DateTime.Now;
            return _matchCache.Count > 0;
        }

        protected override IEnumerable<Match> Transform(IEnumerable<char> stream)
        {
            if (_matchCache.Count < 1)
            {
                //TODO - fix this
                Logger.Instance.LogAsync(LogLevel.Warning, Errors.TRANSFORMING_UNMATHCED_STREAM, (string)stream, _rgx.ToString());
#if DEBUG
                throw new Exception("We're trying to traansform on an unmatched regex... this should never happen.....");
#else
                yield break;
#endif
            }
            foreach (Match m in _matchCache)
                yield return m;
            /*foreach (Match match in _matchCache)
            {
                foreach(Group g in match.Groups)
                {
                    yield return g.Value;
                }
            }*/
        }
    }
}
