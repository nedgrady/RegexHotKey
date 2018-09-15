using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexHotKey
{
    public static class StringExtensions
    {
        public static string ReplaceMany(this string @string, params string[] replacements)
        {
            if (@string== null)
                throw new ArgumentNullException("@string");

            if(replacements == null)
                throw new ArgumentNullException("replacements");

            for (int i = 0; i < replacements.Length; i++)
            {
                @string = @string.Replace($"{{{i}}}", replacements[i] ?? "");
            }

            return @string;
        }
    }
}
