using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScintillaNET;
using EasyScintilla;
using System.Drawing;

namespace RegexHotKeyUI
{
    class CSharpBox : Scintilla
    {
        public CSharpBox()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            StyleResetDefault();
            Styles[Style.Default].Font = "Consolas";
            Styles[Style.Default].Size = 10;
            StyleClearAll();

            // Configure the CPP (C#) lexer styles
            Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            Lexer = Lexer.Cpp;

            // Set the keywords
            SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
        }


    }
}
