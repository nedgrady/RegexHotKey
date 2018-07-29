using System;
using System.Collections.Generic;
using System.Text;

namespace RegexHotKey
{
    public interface IKeysSubscriber
    {
        void Notify(char keys);
    }
}
