namespace RuneReformed1._0._15
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ChampBox = new System.Windows.Forms.ComboBox();
            this.SetRunes = new System.Windows.Forms.Button();
            this.RuneBox = new System.Windows.Forms.ComboBox();
            this.Championlbl = new System.Windows.Forms.Label();
            this.RunePagelbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ChampBox
            // 
            this.ChampBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChampBox.Enabled = false;
            this.ChampBox.FormattingEnabled = true;
            this.ChampBox.Location = new System.Drawing.Point(72, 12);
            this.ChampBox.Name = "ChampBox";
            this.ChampBox.Size = new System.Drawing.Size(178, 21);
            this.ChampBox.TabIndex = 0;
            this.ChampBox.SelectedIndexChanged += new System.EventHandler(this.ChampBox_SelectedIndexChanged);
            // 
            // SetRunes
            // 
            this.SetRunes.Location = new System.Drawing.Point(175, 70);
            this.SetRunes.Name = "SetRunes";
            this.SetRunes.Size = new System.Drawing.Size(75, 23);
            this.SetRunes.TabIndex = 1;
            this.SetRunes.Text = "Set Runes";
            this.SetRunes.UseVisualStyleBackColor = true;
            this.SetRunes.Click += new System.EventHandler(this.button1_Click);
            // 
            // RuneBox
            // 
            this.RuneBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RuneBox.Enabled = false;
            this.RuneBox.FormattingEnabled = true;
            this.RuneBox.Location = new System.Drawing.Point(72, 39);
            this.RuneBox.Name = "RuneBox";
            this.RuneBox.Size = new System.Drawing.Size(178, 21);
            this.RuneBox.TabIndex = 2;
            // 
            // Championlbl
            // 
            this.Championlbl.AutoSize = true;
            this.Championlbl.Location = new System.Drawing.Point(5, 15);
            this.Championlbl.Name = "Championlbl";
            this.Championlbl.Size = new System.Drawing.Size(54, 13);
            this.Championlbl.TabIndex = 3;
            this.Championlbl.Text = "Champion";
            // 
            // RunePagelbl
            // 
            this.RunePagelbl.AutoSize = true;
            this.RunePagelbl.Location = new System.Drawing.Point(5, 42);
            this.RunePagelbl.Name = "RunePagelbl";
            this.RunePagelbl.Size = new System.Drawing.Size(61, 13);
            this.RunePagelbl.TabIndex = 4;
            this.RunePagelbl.Text = "Rune Page";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 105);
            this.Controls.Add(this.RunePagelbl);
            this.Controls.Add(this.Championlbl);
            this.Controls.Add(this.RuneBox);
            this.Controls.Add(this.SetRunes);
            this.Controls.Add(this.ChampBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "RunesReformed";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ChampBox;
        private System.Windows.Forms.Button SetRunes;
        private System.Windows.Forms.ComboBox RuneBox;
        private System.Windows.Forms.Label Championlbl;
        private System.Windows.Forms.Label RunePagelbl;
    }
}

