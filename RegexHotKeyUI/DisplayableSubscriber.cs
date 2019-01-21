using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RegexHotKey;

namespace RegexHotKeyUI
{
    class DisplayableSubscriber
        : Subscriber
    {
        private readonly IKeysSubscriber _keysSubscriber;
        private readonly RegexProcessor _regexProcessor;

        public Regex Regex => _regexProcessor.Regex;

        public IEnumerable<char> ClearChars => _regexProcessor.ClearItems;

        public TimeSpan ClearTime => _regexProcessor.ClearTime;

        private readonly string _methodText;

        public DisplayableSubscriber()
        {
        }

        public string MethodText => _methodText;

        #region EqualityOverrides
        public override bool Equals(object obj)
        {
            DisplayableSubscriber other = obj as DisplayableSubscriber;

            if (other == null)
                return false;
            return other._keysSubscriber.Equals(this);
        }

        public override int GetHashCode()
        {
            return -1703252652 + EqualityComparer<IKeysSubscriber>.Default.GetHashCode(_keysSubscriber);
        }
        #endregion
    }
}
