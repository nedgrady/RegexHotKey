using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalKeyListener;
using System.Text.RegularExpressions;

namespace RegexHotKeyUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Hooker h = new Hooker();
            //h.RegisterKey(OnKeyPress, Handle);
        }

        private IntPtr OnKeyPress(int code, UIntPtr wParam, IntPtr lparam)
        {
            Console.WriteLine($"{code} {wParam} {lparam} ");
            return new IntPtr();
        }
    }
}

