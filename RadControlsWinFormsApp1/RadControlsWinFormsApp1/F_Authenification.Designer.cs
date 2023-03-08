namespace ALCHANYSCHOOL
{
    partial class F_Authenification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (F_Authenification));
            this.telerikMetroBlueTheme1 = new Telerik.WinControls.Themes.TelerikMetroBlueTheme ();
            this.radButton1 = new Telerik.WinControls.UI.RadButton ();
            this.radButton2 = new Telerik.WinControls.UI.RadButton ();
            this.ChkMySessionUID = new Telerik.WinControls.UI.RadCheckBox ();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager ();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel ();
            this.txtlogin = new Telerik.WinControls.UI.RadTextBox ();
            this.txtpawd = new Telerik.WinControls.UI.RadTextBox ();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel ();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel ();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel ();
            this.DropDownListY = new Telerik.WinControls.UI.RadDropDownList ();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme ();
            this.ComboCheckYearArchive = new Telerik.WinControls.UI.RadDropDownList ();
            this.pictureBox10 = new System.Windows.Forms.PictureBox ();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMySessionUID)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.txtlogin)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.txtpawd)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.DropDownListY)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.ComboCheckYearArchive)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit ();
            this.SuspendLayout ();
            // 
            // radButton1
            // 
            this.radButton1.BackColor = System.Drawing.Color.Green;
            this.radButton1.ForeColor = System.Drawing.Color.White;
            this.radButton1.Location = new System.Drawing.Point (428, 235);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size (110, 33);
            this.radButton1.TabIndex = 4;
            this.radButton1.Text = "&Connexion";
            this.radButton1.ThemeName = "Windows8";
            this.radButton1.Click += new System.EventHandler (this.radButton1_Click);
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point (568, 235);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size (110, 33);
            this.radButton2.TabIndex = 5;
            this.radButton2.Text = "&Quitter";
            this.radButton2.ThemeName = "Windows8";
            this.radButton2.Click += new System.EventHandler (this.radButton2_Click);
            // 
            // ChkMySessionUID
            // 
            this.ChkMySessionUID.BackColor = System.Drawing.Color.Transparent;
            this.ChkMySessionUID.Font = new System.Drawing.Font ("Segoe UI Semibold", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkMySessionUID.ForeColor = System.Drawing.Color.Black;
            this.ChkMySessionUID.Location = new System.Drawing.Point (533, 192);
            this.ChkMySessionUID.Name = "ChkMySessionUID";
            this.ChkMySessionUID.Size = new System.Drawing.Size (134, 22);
            this.ChkMySessionUID.TabIndex = 6;
            this.ChkMySessionUID.Text = "garde ma session";
            this.ChkMySessionUID.ThemeName = "Windows8";
            this.ChkMySessionUID.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // radLabel1
            // 
            this.radLabel1.BackColor = System.Drawing.Color.Transparent;
            this.radLabel1.Font = new System.Drawing.Font ("Segoe UI", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.radLabel1.Location = new System.Drawing.Point (324, 32);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size (424, 37);
            this.radLabel1.TabIndex = 7;
            this.radLabel1.Text = "Tapez votre login et votre mot de passe";
            // 
            // txtlogin
            // 
            this.txtlogin.Location = new System.Drawing.Point (514, 127);
            this.txtlogin.Name = "txtlogin";
            this.txtlogin.Size = new System.Drawing.Size (206, 24);
            this.txtlogin.TabIndex = 9;
            this.txtlogin.ThemeName = "Windows8";
            // 
            // txtpawd
            // 
            this.txtpawd.Location = new System.Drawing.Point (514, 162);
            this.txtpawd.Name = "txtpawd";
            this.txtpawd.PasswordChar = '*';
            this.txtpawd.Size = new System.Drawing.Size (206, 24);
            this.txtpawd.TabIndex = 10;
            this.txtpawd.Text = "@Junior67";
            this.txtpawd.ThemeName = "Windows8";
            this.txtpawd.KeyDown += new System.Windows.Forms.KeyEventHandler (this.txtpawd_KeyDown);
            // 
            // radLabel2
            // 
            this.radLabel2.BackColor = System.Drawing.Color.Transparent;
            this.radLabel2.Font = new System.Drawing.Font ("Segoe UI", 13.25F);
            this.radLabel2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.radLabel2.Location = new System.Drawing.Point (429, 122);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size (77, 34);
            this.radLabel2.TabIndex = 11;
            this.radLabel2.Text = "Login :";
            // 
            // radLabel3
            // 
            this.radLabel3.BackColor = System.Drawing.Color.Transparent;
            this.radLabel3.Font = new System.Drawing.Font ("Segoe UI", 13.25F);
            this.radLabel3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.radLabel3.Location = new System.Drawing.Point (350, 156);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size (156, 34);
            this.radLabel3.TabIndex = 12;
            this.radLabel3.Text = "Mot de passe :";
            // 
            // radLabel4
            // 
            this.radLabel4.BackColor = System.Drawing.Color.Transparent;
            this.radLabel4.Font = new System.Drawing.Font ("Segoe UI", 13.25F);
            this.radLabel4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.radLabel4.Location = new System.Drawing.Point (339, 88);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size (167, 34);
            this.radLabel4.TabIndex = 13;
            this.radLabel4.Text = "Année scolaire :";
            // 
            // DropDownListY
            // 
            this.DropDownListY.Font = new System.Drawing.Font ("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropDownListY.Location = new System.Drawing.Point (514, 88);
            this.DropDownListY.Name = "DropDownListY";
            this.DropDownListY.Size = new System.Drawing.Size (204, 28);
            this.DropDownListY.TabIndex = 16;
            this.DropDownListY.ThemeName = "Windows8";
            this.DropDownListY.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler (this.DropDownListY_SelectedIndexChanged);
            // 
            // ComboCheckYearArchive
            // 
            this.ComboCheckYearArchive.Font = new System.Drawing.Font ("Segoe UI", 10.2F);
            this.ComboCheckYearArchive.Location = new System.Drawing.Point (514, 88);
            this.ComboCheckYearArchive.Name = "ComboCheckYearArchive";
            this.ComboCheckYearArchive.Size = new System.Drawing.Size (204, 28);
            this.ComboCheckYearArchive.TabIndex = 17;
            this.ComboCheckYearArchive.ThemeName = "Windows8";
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox10.Image = global::ALCHANYSCHOOL.Properties.Resources.login;
            this.pictureBox10.Location = new System.Drawing.Point (720, 72);
            this.pictureBox10.Margin = new System.Windows.Forms.Padding (4);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size (142, 163);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox10.TabIndex = 18;
            this.pictureBox10.TabStop = false;
            // 
            // F_Authenification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.BackgroundImage = global::ALCHANYSCHOOL.Properties.Resources._360_F;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size (867, 299);
            this.Controls.Add (this.DropDownListY);
            this.Controls.Add (this.radLabel4);
            this.Controls.Add (this.radLabel2);
            this.Controls.Add (this.txtpawd);
            this.Controls.Add (this.txtlogin);
            this.Controls.Add (this.radLabel1);
            this.Controls.Add (this.ChkMySessionUID);
            this.Controls.Add (this.radButton2);
            this.Controls.Add (this.radButton1);
            this.Controls.Add (this.radLabel3);
            this.Controls.Add (this.ComboCheckYearArchive);
            this.Controls.Add (this.pictureBox10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject ("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F_Authenification";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Group Scolaire Madawe";
            this.ThemeName = "Windows8";
            this.Load += new System.EventHandler (this.F_Authenification_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler (this.F_Authenification_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMySessionUID)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.txtlogin)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.txtpawd)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.DropDownListY)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.ComboCheckYearArchive)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit ();
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private Telerik.WinControls.Themes.TelerikMetroBlueTheme telerikMetroBlueTheme1;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadCheckBox ChkMySessionUID;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtlogin;
        private Telerik.WinControls.UI.RadTextBox txtpawd;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadDropDownList DropDownListY;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
        private Telerik.WinControls.UI.RadDropDownList ComboCheckYearArchive;
        private System.Windows.Forms.PictureBox pictureBox10;
    }
}
