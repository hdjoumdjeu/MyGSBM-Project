namespace ALCHANYSCHOOL
{
    partial class FrmPlashScreem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (FrmPlashScreem));
            this.timer1 = new System.Windows.Forms.Timer (this.components);
            this.timer2 = new System.Windows.Forms.Timer (this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox ();
            this.pictureBox1 = new System.Windows.Forms.PictureBox ();
            this.lblCh = new System.Windows.Forms.Label ();
            this.label2 = new System.Windows.Forms.Label ();
            this.label3 = new System.Windows.Forms.Label ();
            this.pictureBox3 = new System.Windows.Forms.PictureBox ();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit ();
            this.SuspendLayout ();
            // 
            // timer1
            // 
            this.timer1.Interval = 600;
            this.timer1.Tick += new System.EventHandler (this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler (this.timer2_Tick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Turquoise;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Image = global::ALCHANYSCHOOL.Properties.Resources.SD;
            this.pictureBox2.Location = new System.Drawing.Point (0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding (4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size (160, 324);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::ALCHANYSCHOOL.Properties.Resources.Cameroon_240_animated_flag_gifs1;
            this.pictureBox1.Location = new System.Drawing.Point (177, 13);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding (4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size (171, 107);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // lblCh
            // 
            this.lblCh.AutoSize = true;
            this.lblCh.BackColor = System.Drawing.Color.Transparent;
            this.lblCh.Font = new System.Drawing.Font ("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCh.ForeColor = System.Drawing.Color.Maroon;
            this.lblCh.Location = new System.Drawing.Point (189, 146);
            this.lblCh.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
            this.lblCh.Name = "lblCh";
            this.lblCh.Size = new System.Drawing.Size (271, 20);
            this.lblCh.TabIndex = 11;
            this.lblCh.Text = "Chargement des éléments graphiques...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb (((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point (207, 196);
            this.label2.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size (366, 68);
            this.label2.TabIndex = 12;
            this.label2.Text = "Ce logiciel est protégé par la commision de validation \r\nde logiciel au Cameroun " +
    "toute  tentative de reproduction \r\n partielle ou complète sera transmise devant " +
    "les \r\njuridictions compétentes.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font ("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label3.Location = new System.Drawing.Point (269, 297);
            this.label3.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size (213, 19);
            this.label3.TabIndex = 13;
            this.label3.Text = "Copyright : Tout droit reservé.";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = global::ALCHANYSCHOOL.Properties.Resources.Drapeau_Etats_Unis_240_gif;
            this.pictureBox3.Location = new System.Drawing.Point (402, 13);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding (4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size (171, 107);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // FrmPlashScreem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size (619, 324);
            this.Controls.Add (this.pictureBox3);
            this.Controls.Add (this.label3);
            this.Controls.Add (this.label2);
            this.Controls.Add (this.lblCh);
            this.Controls.Add (this.pictureBox2);
            this.Controls.Add (this.pictureBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject ("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPlashScreem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmPlashScreem";
            this.Load += new System.EventHandler (this.FrmPlashScreem_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit ();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit ();
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblCh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}