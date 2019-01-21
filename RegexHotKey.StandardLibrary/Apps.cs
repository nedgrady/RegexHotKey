using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexHotKey.StandardLibrary
{
    public static class Apps
    {
        public static void GotoURL(string url) => System.Diagnostics.Process.Start(url);
    }
}
