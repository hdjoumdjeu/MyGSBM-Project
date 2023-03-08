namespace ALCHANYSCHOOL
{
    partial class F_MyCamera
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
		this.components = new System.ComponentModel.Container ();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (F_MyCamera));
		this.pictImage = new System.Windows.Forms.PictureBox ();
		this.lblnom = new Telerik.WinControls.UI.RadLabel ();
		this.txtmatricule = new Telerik.WinControls.UI.RadTextBox ();
		this.rDListDevice = new Telerik.WinControls.UI.RadDropDownList ();
		this.lblville = new Telerik.WinControls.UI.RadLabel ();
		this.btnSave = new Telerik.WinControls.UI.RadButton ();
		this.radButton1 = new Telerik.WinControls.UI.RadButton ();
		this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList ();
		this.radLabel2 = new Telerik.WinControls.UI.RadLabel ();
		this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox ();
		this.timer1 = new System.Windows.Forms.Timer (this.components);
		this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme ();
		((System.ComponentModel.ISupportInitialize)(this.pictImage)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.lblnom)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.txtmatricule)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.rDListDevice)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.lblville)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit ();
		((System.ComponentModel.ISupportInitialize)(this)).BeginInit ();
		this.SuspendLayout ();
		// 
		// pictImage
		// 
		this.pictImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.pictImage.Image = global::ALCHANYSCHOOL.Properties.Resources.student_icon_512;
		this.pictImage.Location = new System.Drawing.Point (20, 156);
		this.pictImage.Name = "pictImage";
		this.pictImage.Size = new System.Drawing.Size (406, 261);
		this.pictImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictImage.TabIndex = 19;
		this.pictImage.TabStop = false;
		// 
		// lblnom
		// 
		this.lblnom.BackColor = System.Drawing.Color.Transparent;
		this.lblnom.Font = new System.Drawing.Font ("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblnom.Location = new System.Drawing.Point (125, 95);
		this.lblnom.Name = "lblnom";
		this.lblnom.Size = new System.Drawing.Size (94, 28);
		this.lblnom.TabIndex = 21;
		this.lblnom.Text = "Matricule :";
		// 
		// txtmatricule
		// 
		this.txtmatricule.Location = new System.Drawing.Point (226, 95);
		this.txtmatricule.Name = "txtmatricule";
		this.txtmatricule.ReadOnly = true;
		this.txtmatricule.Size = new System.Drawing.Size (195, 24);
		this.txtmatricule.TabIndex = 20;
		this.txtmatricule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.txtmatricule.ThemeName = "Windows8";
		// 
		// rDListDevice
		// 
		this.rDListDevice.Location = new System.Drawing.Point (226, 126);
		this.rDListDevice.Name = "rDListDevice";
		this.rDListDevice.Size = new System.Drawing.Size (195, 24);
		this.rDListDevice.TabIndex = 26;
		this.rDListDevice.ThemeName = "Windows8";
		// 
		// lblville
		// 
		this.lblville.BackColor = System.Drawing.Color.Transparent;
		this.lblville.Font = new System.Drawing.Font ("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblville.Location = new System.Drawing.Point (84, 126);
		this.lblville.Name = "lblville";
		this.lblville.Size = new System.Drawing.Size (137, 28);
		this.lblville.TabIndex = 23;
		this.lblville.Text = "Camera device :";
		// 
		// btnSave
		// 
		this.btnSave.Location = new System.Drawing.Point (226, 425);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size (200, 39);
		this.btnSave.TabIndex = 27;
		this.btnSave.Text = "Capturer && Modifier";
		this.btnSave.ThemeName = "Windows8";
		this.btnSave.Click += new System.EventHandler (this.btnSave_Click);
		// 
		// radButton1
		// 
		this.radButton1.Location = new System.Drawing.Point (1, 425);
		this.radButton1.Name = "radButton1";
		this.radButton1.Size = new System.Drawing.Size (110, 39);
		this.radButton1.TabIndex = 28;
		this.radButton1.Text = "start";
		this.radButton1.ThemeName = "Windows8";
		this.radButton1.Visible = false;
		this.radButton1.Click += new System.EventHandler (this.radButton1_Click);
		// 
		// radDropDownList1
		// 
		this.radDropDownList1.Location = new System.Drawing.Point (218, 184);
		this.radDropDownList1.Name = "radDropDownList1";
		this.radDropDownList1.Size = new System.Drawing.Size (195, 24);
		this.radDropDownList1.TabIndex = 31;
		this.radDropDownList1.ThemeName = "Office2013Light";
		// 
		// radLabel2
		// 
		this.radLabel2.Location = new System.Drawing.Point (112, 184);
		this.radLabel2.Name = "radLabel2";
		this.radLabel2.Size = new System.Drawing.Size (106, 22);
		this.radLabel2.TabIndex = 30;
		this.radLabel2.Text = "Camera device :";
		// 
		// radTextBox1
		// 
		this.radTextBox1.AutoSize = false;
		this.radTextBox1.Location = new System.Drawing.Point (218, 155);
		this.radTextBox1.Multiline = true;
		this.radTextBox1.Name = "radTextBox1";
		this.radTextBox1.ReadOnly = true;
		this.radTextBox1.Size = new System.Drawing.Size (195, 66);
		this.radTextBox1.TabIndex = 32;
		this.radTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.radTextBox1.ThemeName = "Office2013Light";
		// 
		// F_MyCamera
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 16F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackgroundImage = global::ALCHANYSCHOOL.Properties.Resources.welcome_back_school_background_3589_817;
		this.ClientSize = new System.Drawing.Size (434, 476);
		this.Controls.Add (this.radButton1);
		this.Controls.Add (this.btnSave);
		this.Controls.Add (this.rDListDevice);
		this.Controls.Add (this.lblville);
		this.Controls.Add (this.lblnom);
		this.Controls.Add (this.txtmatricule);
		this.Controls.Add (this.pictImage);
		this.Controls.Add (this.radTextBox1);
		this.Controls.Add (this.radDropDownList1);
		this.Controls.Add (this.radLabel2);
		this.Icon = ((System.Drawing.Icon)(resources.GetObject ("$this.Icon")));
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.Name = "F_MyCamera";
		// 
		// 
		// 
		this.RootElement.ApplyShapeToControl = true;
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "                       My Camera";
		this.ThemeName = "Windows8";
		this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.F_MyCamera_FormClosing);
		this.Load += new System.EventHandler (this.F_MyCamera_Load);
		((System.ComponentModel.ISupportInitialize)(this.pictImage)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.lblnom)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.txtmatricule)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.rDListDevice)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.lblville)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit ();
		((System.ComponentModel.ISupportInitialize)(this)).EndInit ();
		this.ResumeLayout (false);
		this.PerformLayout ();

        }

        #endregion

		private System.Windows.Forms.PictureBox pictImage;
		private Telerik.WinControls.UI.RadLabel lblnom;
		private Telerik.WinControls.UI.RadTextBox txtmatricule;
		private Telerik.WinControls.UI.RadDropDownList rDListDevice;
		private Telerik.WinControls.UI.RadLabel lblville;
		private Telerik.WinControls.UI.RadButton btnSave;
		private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadTextBox radTextBox1;
        private System.Windows.Forms.Timer timer1;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
    }
}
