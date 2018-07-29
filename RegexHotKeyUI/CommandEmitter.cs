using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace RegexHotKeyUI
{
    static class CommandEmitter
    {
        public static void Execute(string code)
        {


            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
    using System;

    namespace RoslynCompileSample
    {
        public static class Writer
        {
            public static void Write(string message)
            {
                Console.WriteLine(message);
            }
        }
    }");

            string assemblyName = Guid.NewGuid().ToString();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    Type t = assembly.GetType("RoslynCompileSample.Matcher");
                    t.GetMethods(BindingFlags.Static).Where((MethodInfo mi) => mi.Name == "Match")
                        .First()
                        .Invoke(null, new object[] { });
                    
                }
            }
        }

        public static Assembly CompileSourceCodeDom(string sourceCode)
        {
            CodeDomProvider cpd = new CSharpCodeProvider();
            var cp = new CompilerParameters();
            Regex r = new Regex("using .+");
            var matches = r.Matches(sourceCode);
            foreach(Match m in matches)
            {
                Console.WriteLine($"{m.Value.Trim().Remove(0, 5).Trim(';')}.dll");
                cp.ReferencedAssemblies.Add($"{m.Value.Trim().Remove(0, 5).Trim(';')}.dll");
            }

            cp.GenerateExecutable = false;
            CompilerResults cr = cpd.CompileAssemblyFromSource(cp, sourceCode);

            Assembly a = cr.CompiledAssembly;
            Type fooType = cr.CompiledAssembly.GetType("Matcher");

            MethodInfo mi = fooType.GetMethod("Match");
            mi.Invoke(null, new object[] { "CODE DOM" });
            return cr.CompiledAssembly;
        }
    }
}
