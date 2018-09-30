using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RegexHotKey
{
    public static class TextExtensions
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

        public static IEnumerable<XElement> GetElementsByName(this XElement xElement, string name)
        {
            return xElement.Elements().Where(xEl => xEl.Name == name);
        }

        public static XElement GetFirstElementByName(this XElement xElement, string name)
        {
            return xElement.Elements().Where(xEl => xEl.Name == name).First();
        }

        public static string ToCallbackTypeName(this string s)
        {
            switch (s.ToLower())
            {
                case "string": return "string";
                case "chararray": return "char[]";
                case "charenumerable": return "System.Linq.IEnumerable<char>";
                default: return null;
            }
        }
    }
}

/*String,
  CharArray,
  CharEnumerable*/
