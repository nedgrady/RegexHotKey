using System;
using System.Collections.Generic;
using System.Text;

namespace RegexHotKey
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class KeyDownAttribute
        : Attribute
    {
        public Guid KeyDownGUID => _guid;
        private Guid _guid;
        public KeyDownAttribute(string guid)
        {
            if (!Guid.TryParse(guid, out Guid g))
                throw new ArgumentOutOfRangeException("guid");

            _guid = g;

        }
    }
}
