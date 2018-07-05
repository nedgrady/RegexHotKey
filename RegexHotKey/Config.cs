using System;
using System.Collections.Generic;
using System.Text;

namespace RegexHotKey
{
    public static class Config
    {
        public static char[] Keys => _keys; 

        private static readonly char[] _keys = { 'a', 'b', 'c' };
    }
}
