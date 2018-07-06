using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RegexHotKeyUI
{
    static class Program
    {
        public delegate void CharCallback(char c);
        [DllImport("H:\\Code\\RegexHotKey\\Debug\\HooksUnmanaged.dll", 
            CharSet = CharSet.Auto, 
            SetLastError = true, 
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool RegisterRawHandler(IntPtr charCallback);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        static void KeyDown(char c)
        {
            Console.WriteLine(c);
        }
    }
}
