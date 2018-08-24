using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using RegexHotKey;
using System.Reflection;

namespace RegexHotKeyUI
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //KeyListener.Initialize();
            
            //KeysCallback<string> keyListener = ((string keys) => {Console.WriteLine(keys); Console.WriteLine("Anon"); } );
            //KeyListener.Register(keyListener, new RegexProcessor(new Regex("^test$")));
            Assembly ass = CommandEmitter.CompileSourceCodeDom(@"
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

        public static class Matcher
        {
            public static void Match(string message)
            {
                Console.WriteLine(message);
            }

    }");
            KeyListener.Initialize();
            //Delegate subscriber = ass.GetTypes()
            //    .First(t => t.Name == "Matcher")
            //.GetMethod("Match")
            //.CreateDelegate(typeof(KeysCallback<string>));
            //
            //KeyListener.Register(
            //    (KeysCallback<string>)subscriber,
            //    new RegexProcessor(new Regex("\\S\\S")));
            //
            Application.Run(new Form1());
        }

        [RegexHandler("\\S\\S", CallbackType.CharArray, clearChars: default(char[]), clearInputOnMatch: false)]
        public static void KeyDown(char[] cs)
        {
            Console.WriteLine("KeyDown(char[] cs) ^\\d$");
            Console.WriteLine(cs);
        }
        //
        //[RegexHandler("^\\S\\S$", CallbackType.CharArray, clearTimeMs: 1000)]
        //public static void KeyDown2(char[] cs)
        //{
        //    Console.WriteLine("KeyDown(char[] cs) ^\\d$");
        //    Console.WriteLine(cs);
        //}
        //
        //[RegexHandler("^\\S\\S$", CallbackType.String)]
        //public static void KeyDown(string s)
        //{
        //    Console.WriteLine("KeyDown(string s) \\S\\S$");
        //    Console.WriteLine(s);
        //}
    }
}
