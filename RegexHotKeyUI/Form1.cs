using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using RegexHotKey;

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
            foreach(IKeysSubscriber subscriber in KeyListener.Subscribers)
            {
                subscribersListBox.Items.Add(subscriber);
                Console.WriteLine(subscriber.GetHashCode());
            }
        }

        private IntPtr OnKeyPress(int code, UIntPtr wParam, IntPtr lparam)
        {
            Console.WriteLine($"{code} {wParam} {lparam} ");
            return new IntPtr();
        }

        private void subscribersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            IKeysSubscriber item = (IKeysSubscriber)((ListBox)sender).SelectedItem;
            if (item == null)
                return;

            RegexProcessor rp = item.GetRegexProcessor();
            regexTextBox.Text = rp.Regex.ToString();
            clearOnMatchRadioButton.Checked = rp.ClearOnMatch;

            clearCharactersTextBox.Clear();

            foreach(char c in rp.ClearItems)
            {
                clearCharactersTextBox.Text += MaybeEscape(c) + Environment.NewLine;
            }
            
        }

        private static IReadOnlyDictionary<char, string> EscapeChars =
            new Dictionary<char, string>()
            {
                { '\n',  "{enter}"},
                { '\t',  "{tab}"},
                { ' ',  "{space}"},
                { '\r',  "{enter}"}
            };

        private string MaybeEscape(char c)
        {
            if (EscapeChars.TryGetValue(c, out string s))
                return s;

            return c.ToString();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}

