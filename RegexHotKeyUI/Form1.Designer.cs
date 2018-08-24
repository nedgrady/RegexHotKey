namespace RegexHotKeyUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.subscribersListBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.newButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clearCharactersTextBox = new System.Windows.Forms.TextBox();
            this.clearOnMatchRadioButton = new System.Windows.Forms.RadioButton();
            this.timeoutNumericInput = new System.Windows.Forms.NumericUpDown();
            this.regexTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericInput)).BeginInit();
            this.SuspendLayout();
            // 
            // subscribersListBox
            // 
            this.subscribersListBox.FormattingEnabled = true;
            this.subscribersListBox.Location = new System.Drawing.Point(12, 12);
            this.subscribersListBox.Name = "subscribersListBox";
            this.subscribersListBox.Size = new System.Drawing.Size(248, 407);
            this.subscribersListBox.TabIndex = 0;
            this.subscribersListBox.SelectedIndexChanged += new System.EventHandler(this.subscribersListBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.newButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.codeTextBox);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.clearCharactersTextBox);
            this.groupBox1.Controls.Add(this.clearOnMatchRadioButton);
            this.groupBox1.Controls.Add(this.timeoutNumericInput);
            this.groupBox1.Controls.Add(this.regexTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(278, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 406);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Regex Hotkey Properties";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // newButton
            // 
            this.newButton.Location = new System.Drawing.Point(338, 62);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(72, 25);
            this.newButton.TabIndex = 11;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Code:";
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(61, 194);
            this.codeTextBox.Multiline = true;
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(387, 206);
            this.codeTextBox.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(338, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 25);
            this.button1.TabIndex = 8;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // clearCharactersTextBox
            // 
            this.clearCharactersTextBox.Location = new System.Drawing.Point(114, 109);
            this.clearCharactersTextBox.Multiline = true;
            this.clearCharactersTextBox.Name = "clearCharactersTextBox";
            this.clearCharactersTextBox.Size = new System.Drawing.Size(100, 69);
            this.clearCharactersTextBox.TabIndex = 7;
            // 
            // clearOnMatchRadioButton
            // 
            this.clearOnMatchRadioButton.AutoSize = true;
            this.clearOnMatchRadioButton.Location = new System.Drawing.Point(115, 90);
            this.clearOnMatchRadioButton.Name = "clearOnMatchRadioButton";
            this.clearOnMatchRadioButton.Size = new System.Drawing.Size(14, 13);
            this.clearOnMatchRadioButton.TabIndex = 6;
            this.clearOnMatchRadioButton.TabStop = true;
            this.clearOnMatchRadioButton.UseVisualStyleBackColor = true;
            // 
            // timeoutNumericInput
            // 
            this.timeoutNumericInput.Location = new System.Drawing.Point(115, 62);
            this.timeoutNumericInput.Name = "timeoutNumericInput";
            this.timeoutNumericInput.Size = new System.Drawing.Size(120, 20);
            this.timeoutNumericInput.TabIndex = 5;
            // 
            // regexTextBox
            // 
            this.regexTextBox.Location = new System.Drawing.Point(114, 30);
            this.regexTextBox.Name = "regexTextBox";
            this.regexTextBox.Size = new System.Drawing.Size(192, 20);
            this.regexTextBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Clear Characters:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Timeout:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Clear On Match:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Regex:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.subscribersListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox subscribersListBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox clearCharactersTextBox;
        private System.Windows.Forms.RadioButton clearOnMatchRadioButton;
        private System.Windows.Forms.NumericUpDown timeoutNumericInput;
        private System.Windows.Forms.TextBox regexTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button newButton;
    }
}

