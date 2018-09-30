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
        public static Assembly CompileSourceCodeDom(string sourceCode, string assemblyName = null)
        {
            CodeDomProvider cpd = new CSharpCodeProvider();
            var cp = new CompilerParameters()
            {
                OutputAssembly = assemblyName,
                GenerateExecutable = false,
                GenerateInMemory = true
            };
            Regex r = new Regex("using .+", RegexOptions.Compiled);
            var matches = r.Matches(sourceCode);
            foreach(Match m in matches)
            {
                cp.ReferencedAssemblies.Add($"{m.Value.Trim().Remove(0, 5).Trim(';').Trim()}.dll");
            }

            CompilerResults cr = cpd.CompileAssemblyFromSource(cp, sourceCode);

            if(cr.Errors.Count < 1)
            {
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
