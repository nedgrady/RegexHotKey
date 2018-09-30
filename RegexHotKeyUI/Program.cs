using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using RegexHotKey;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace RegexHotKeyUI
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            XElement xDefaults = XElement.Load("../../Defaults.xml");

            StringBuilder builder = new StringBuilder();

            foreach (XElement xAssembly in xDefaults.Nodes())
            {
                builder.Clear();
                /*string imports = (xAssembly.Elements()
                    .Where(xEl => xEl.Name == "imports")
                    .Select(xImport => $"using {xImport.Value};\n")*/

                builder.Append(
                    xAssembly.GetElementsByName("imports")
                    .First()
                    .Elements()
                    .Select(xEl => $"using {xEl.Value};\n")
                    .Aggregate((impBase, impNext) => impBase + impNext));

                string assName = xAssembly.Attributes().Where(a => a.Name == "name").First().Value;
                builder.AppendLine($"namespace {assName} {{\n class {assName}{{");

                foreach (XElement xSubscription in xAssembly.Element("subscribers").Elements("subscription"))
                {
                    XElement xRegexHandler = xSubscription.Element("regexHandler");

                    XElement xRegex = xRegexHandler.Element("regex");
                    XElement xCallbackType = xRegexHandler.Element("callbackType");
                    XElement xClearTimeMS = xRegexHandler.Element("clearTimeMS");
                    XElement xParameterName = xSubscription.Element("parameterName");
                    XElement xCode = xSubscription.Element("code");

                    builder.AppendLine(
                        $@"[RegexHandler(""{ xRegex.Value }"", CallbackType.{xCallbackType.Value}, clearChars: default(char[]), clearInputOnMatch: true, clearTimeMs: {xClearTimeMS?.Value ?? "-1"})]");

                    builder.AppendLine($@"public static void @{ xSubscription.FirstAttribute.Value } ({xCallbackType.Value.ToCallbackTypeName()} {xParameterName.Value}) {{");
                    builder.AppendLine(xCode.Value);
                    builder.AppendLine("}");

                }

                builder.Append("}}");
                Assembly ass2 = CommandEmitter.CompileSourceCodeDom(builder.ToString(), assName);
                await KeyListener.RegisterAssembly(ass2);
                builder.Clear();
            }
            Console.WriteLine(builder);
            //KeyListener.Initialize();
            //KeysCallback<string> keyListener = ((string keys) => {Console.WriteLine(keys); Console.WriteLine("Anon"); } );
            //KeyListener.Register(keyListener, new RegexProcessor(new Regex("^test$")));
            Assembly ass = CommandEmitter.CompileSourceCodeDom(@"
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using RegexHotKey;
        public static class Matcher
        {
            [RegexHandler(""\\D\\D"", CallbackType.String, clearChars: default(char[]), clearInputOnMatch: true)]
            public static void KeyDown(string s)
            {
                Console.WriteLine(""\\D\\D"");
                Console.WriteLine(s);
            }

        }");
            await KeyListener.Initialize();
            
            Application.Run(new Form1());
        }

        [RegexHandler("\\S\\S", CallbackType.CharArray, clearChars: default, clearInputOnMatch: true)]
        public static void KeyDown(char[] cs)
        {
            Console.WriteLine("KeyDown(char[] cs) ^\\d$");
            Console.WriteLine(cs);
        }

    }
}
