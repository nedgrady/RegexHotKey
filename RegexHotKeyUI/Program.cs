using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using GlobalKeyListener;

namespace RegexHotKeyUI
{
    static class Program
    {
        /*public delegate void CharCallback(char c);
        [DllImport("E:\\Code\\PDT\\RegexHotKey\\Debug\\HooksUnmanaged.dll", 
            CharSet = CharSet.Auto, 
            SetLastError = true, 
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool RegisterRawHandler(IntPtr charCallback);*/

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            KeyListener k = new KeyListener();
            Application.Run(new Form1());

        }

        [RegexHandler("^\\S\\S$", CallbackType.CharArray, clearTimeMs:1000)]
        public static void KeyDown(char[] cs)
        {
            Console.WriteLine("KeyDown(char[] cs) ^\\d$");
            Console.WriteLine(cs);
        }

        [RegexHandler("^\\S\\S$", CallbackType.CharArray, clearTimeMs: 1000)]
        public static void KeyDown2(char[] cs)
        {
            Console.WriteLine("KeyDown(char[] cs) ^\\d$");
            Console.WriteLine(cs);
        }

        //[RegexHandler("^\\S\\S$", CallbackType.String)]
        public static void KeyDown(string s)
        {
            Console.WriteLine("KeyDown(string s) \\S\\S$");
            Console.WriteLine(s);
        }
    }
}
