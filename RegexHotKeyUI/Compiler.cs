using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DontThink.Utilities.Logging;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using DontThink.Utilities.Compilation;
using RegexHotKey;
using DontThink.Utilities.Debugging;
using System.Collections;

namespace RegexHotKeyUI
{
    static class SubscriberCompiler
    {
        public static async Task<Assembly> CompileSubscribingAssembly(string code, string assName)
        {
            await Logger.Instance.ThrowOrLogNullLogEntry(code, "code");
            CompilerResults cr = CompilerHelper.CompileString(code, assName);

            if (cr.Errors.Count < 1)
            {
                return cr.CompiledAssembly;
            }
            else
            {
                await Logger.Instance.LogManyAsync(
                    LogLevel.Error,
                    cr.Errors.Cast<CompilerError>().Select(ce => ce.ToString()));
            }

            return null;
        }
    }
}
