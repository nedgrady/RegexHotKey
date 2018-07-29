using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKeyListener
{
    interface IKeysSubscriber
    {
        void Notify(char keys);
    }
}
