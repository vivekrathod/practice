namespace SybaseADOClient
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
            this.ResultGrid = new System.Windows.Forms.DataGridView();
            this.Execute = new System.Windows.Forms.Button();
            this.SQLText = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.Username = new System.Windows.Forms.TextBox();
            this.Database = new System.Windows.Forms.TextBox();
            this.Port = new System.Windows.Forms.TextBox();
            this.Server = new System.Windows.Forms.TextBox();
            this.CharSet = new System.Windows.Forms.RichTextBox();
            this.Language = new System.Windows.Forms.RichTextBox();
            this.CodePageType = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // ResultGrid
            // 
            this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultGrid.Location = new System.Drawing.Point(72, 312);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.Size = new System.Drawing.Size(907, 403);
            this.ResultGrid.TabIndex = 16;
            // 
            // Execute
            // 
            this.Execute.Location = new System.Drawing.Point(72, 260);
            this.Execute.Name = "Execute";
            this.Execute.Size = new System.Drawing.Size(75, 23);
            this.Execute.TabIndex = 15;
            this.Execute.Text = "Execute!";
            this.Execute.UseVisualStyleBackColor = true;
            this.Execute.Click += new System.EventHandler(this.Execute_Click_1);
            // 
            // SQLText
            // 
            this.SQLText.Location = new System.Drawing.Point(72, 206);
            this.SQLText.Name = "SQLText";
            this.SQLText.Size = new System.Drawing.Size(907, 20);
            this.SQLText.TabIndex = 14;
            this.SQLText.Text = "select * from syslogins";
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(254, 140);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(99, 20);
            this.Password.TabIndex = 13;
            this.Password.UseSystemPasswordChar = true;
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(72, 140);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(99, 20);
            this.Username.TabIndex = 12;
            this.Username.Text = "sa";
            // 
            // Database
            // 
            this.Database.Location = new System.Drawing.Point(72, 98);
            this.Database.Name = "Database";
            this.Database.Size = new System.Drawing.Size(99, 20);
            this.Database.TabIndex = 11;
            this.Database.Text = "master";
            // 
            // Port
            // 
            this.Port.Location = new System.Drawing.Point(72, 59);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(99, 20);
            this.Port.TabIndex = 10;
            this.Port.Text = "5000";
            // 
            // Server
            // 
            this.Server.Location = new System.Drawing.Point(72, 24);
            this.Server.Name = "Server";
            this.Server.Size = new System.Drawing.Size(193, 20);
            this.Server.TabIndex = 9;
            this.Server.Text = "yang";
            // 
            // CharSet
            // 
            this.CharSet.Location = new System.Drawing.Point(580, 134);
            this.CharSet.Name = "CharSet";
            this.CharSet.Size = new System.Drawing.Size(122, 26);
            this.CharSet.TabIndex = 17;
            this.CharSet.Text = "utf8";
            // 
            // Language
            // 
            this.Language.Location = new System.Drawing.Point(580, 77);
            this.Language.Name = "Language";
            this.Language.Size = new System.Drawing.Size(122, 26);
            this.Language.TabIndex = 18;
            this.Language.Text = "us_english";
            // 
            // CodePageType
            // 
            this.CodePageType.Location = new System.Drawing.Point(857, 140);
            this.CodePageType.Name = "CodePageType";
            this.CodePageType.Size = new System.Drawing.Size(122, 26);
            this.CodePageType.TabIndex = 19;
            this.CodePageType.Text = "ANSI";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Database";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "User";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "SQL Stmt";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(529, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "CharSet";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(770, 146);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "CodePageType";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(519, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Language";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 739);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CodePageType);
            this.Controls.Add(this.Language);
            this.Controls.Add(this.CharSet);
            this.Controls.Add(this.ResultGrid);
            this.Controls.Add(this.Execute);
            this.Controls.Add(this.SQLText);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Username);
            this.Controls.Add(this.Database);
            this.Controls.Add(this.Port);
            this.Controls.Add(this.Server);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ResultGrid;
        private System.Windows.Forms.Button Execute;
        private System.Windows.Forms.TextBox SQLText;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.TextBox Database;
        private System.Windows.Forms.TextBox Port;
        private System.Windows.Forms.TextBox Server;
        private System.Windows.Forms.RichTextBox CharSet;
        private System.Windows.Forms.RichTextBox Language;
        private System.Windows.Forms.RichTextBox CodePageType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}

