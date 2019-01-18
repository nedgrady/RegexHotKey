using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegexHotKey;

namespace RegexHotKeyUI
{
    class SubscriberView
    {
        public RegexProcessor RegexProcessor => _rp;
        private RegexProcessor _rp;

        public IKeysSubscriber KeysSubscriber => _keysSubscriber;
        private IKeysSubscriber _keysSubscriber;

        private string _code;
        public string Code => _code;

        private SubscriberView()
        {

        }
    }
}
