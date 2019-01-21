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
using DontThink.Utilities.Logging;
using RegexHotKey.StandardLibrary;
using Microsoft.CodeAnalysis.CSharp;
[assembly: KeyDown("2a1287b2-9e03-4b0c-861d-b1a48774b807")]
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
            Regex groupsReplaceRegex = new Regex(@"(#)(\d{0,4})");
            foreach (XElement xAssembly in xDefaults.Nodes())
            {
                builder.Clear();

                builder.AppendLine("using RegexHotKey.StandardLibrary;");

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
                    XElement xClearTimeMS = xRegexHandler.Element("clearTimeMS");
                    XElement xParameterName = xSubscription.Element("parameterName");
                    XElement xCode = xSubscription.Element("code");

                    builder.AppendLine(
                        $@"[RegexHandler(""{ xRegex.Value }"",  clearChars: default(char[]), clearInputOnMatch: true, clearTimeMs: {xClearTimeMS?.Value ?? "-1"})]");

                    builder.AppendLine($@"public static void @{ xSubscription.FirstAttribute.Value } (Match @match) {{");

                    builder.AppendLine(groupsReplaceRegex.Replace(xCode.Value, RegexReplace));
                    builder.AppendLine("}");
                }

                builder.Append("}}");

                
#if DEBUG
                await Logger.Instance.LogAsync(LogLevel.Verbose, builder.ToString());
#endif

                Assembly ass2 = await SubscriberCompiler.CompileSubscribingAssembly(builder.ToString(), assName);
                await KeyListener.RegisterAssembly(ass2);
                builder.Clear();
            }

            await KeyListener.Initialize();
            
            Application.Run(new MainView());
        }

        private static string RegexReplace(Match @match)
        {
            int x = @match?.Groups.Count ?? 1;


            string grpIndex = match.Groups[2].Value;
            return $@"@match.Groups[{grpIndex}].Value";
            // TODO - why can't i use conditional member access operator?????.
            return $@"(((match != null) ? match.Groups.Count : int.MaxValue) > {grpIndex} ? null : @match.Groups[{grpIndex}].Value) ?? 
                        ""Group {grpIndex} out of range""";
        }

        [RegexHandler("^(!c)(\\d{5})$", clearChars: default, clearInputOnMatch: true, clearTimeMs: 500)]
        public static void KeyDown(Match cs)
        {
            Console.WriteLine(cs);
            Apps.GotoURL("http://www.manuscript.com/cases/f/" + cs.Groups[2]);
        }

    }
}