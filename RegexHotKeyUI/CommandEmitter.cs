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
        #region broken
        public static void Execute(string code)
        {


            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"using System;

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
                options: new CSharpCompilationOptions(
                     OutputKind.DynamicallyLinkedLibrary
                     ));

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
        #endregion

        public static Assembly CompileSourceCodeDom(string sourceCode)
        {
            CodeDomProvider cpd = new CSharpCodeProvider();
            var cp = new CompilerParameters();
            Regex r = new Regex("using .+");
            var matches = r.Matches(sourceCode);
            foreach(Match m in matches)
            {
                cp.ReferencedAssemblies.Add($"{m.Value.Trim().Remove(0, 5).Trim(';').Trim()}.dll");
            }

            cp.GenerateExecutable = false;
            CompilerResults cr = cpd.CompileAssemblyFromSource(cp, sourceCode);

            if(cr.Errors.Count < 1)
            {
                Console.WriteLine(cr.PathToAssembly);
                //Console.WriteLine(cr.Output.);
                Type fooType = cr.CompiledAssembly
                    .GetTypes()
                    .First((Type t) => t.Name == "Matcher");

                foreach (Type t in cr.CompiledAssembly.GetTypes())
                    Console.WriteLine(t);

                return cr.CompiledAssembly;
            }
            else
            {
                foreach(CompilerError error in cr.Errors)
                {
                    Console.WriteLine(error.ErrorText);
                }
            }

            return null;
        }

    }
}
