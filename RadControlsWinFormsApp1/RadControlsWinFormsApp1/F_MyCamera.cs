using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
//using DarrenLee.Media;
using System.IO;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Aztec;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
namespace ALCHANYSCHOOL
{
    public partial class F_MyCamera : Telerik.WinControls.UI.RadForm
    {
        public string MyLocation;
        public string s;
        
		FilterInfoCollection filterInfoCollection;
		VideoCaptureDevice videoCaptureDevice;


        public F_MyCamera(string matricule, String Location)
        {
            InitializeComponent();
            GetInfos();
            MyLocation = Location;
            s = "";
            txtmatricule.Text = matricule;
        }
        private void GetInfos()
        {
                  rDListDevice.Items.Clear();          
                  rDListDevice.SelectedIndex = 0;         
        }
        private void F_MyCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (videoCaptureDevice.IsRunning == true) 
				{
					videoCaptureDevice.Stop ();
				}						
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo Dossier;
                if (MyLocation.Equals("F_ETUDIANTS"))
                {
                    Dossier = new DirectoryInfo("PhotoEtudiants");
                }
                else
                {
                     Dossier = new DirectoryInfo("PhotoProfesseurs");  
                }
                           
                if (!Dossier.Exists)
                {
                    Dossier.Create();
                    MessageBox.Show("Le dossier de sauvegade des photos des professerus vient d'etre cree...", "dossier de photos  ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
					if (MessageBox.Show ("Voulez-vous reelement prendre cette photo?", "Prise de Photo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
								string path = Dossier.FullName.ToString ();
								pictImage.Image.Save (path + @"\" + txtmatricule.Text + ".jpg", ImageFormat.Jpeg);
								s = path + @"\" + txtmatricule.Text + ".jpg";
								if (videoCaptureDevice.IsRunning == true)
									{
											videoCaptureDevice.Stop ();
											this.Close ();
									}		
						}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur  ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void F_MyCamera_Load(object sender, EventArgs e)
        {
			filterInfoCollection = new FilterInfoCollection (FilterCategory.VideoInputDevice);
			foreach (FilterInfo Device in filterInfoCollection)
				{
					rDListDevice.Items.Add (Device.Name);				
				}				
			rDListDevice.SelectedIndex = 0;
			videoCaptureDevice = new VideoCaptureDevice ();
			radButton1_Click (sender, e);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {        
			videoCaptureDevice = new VideoCaptureDevice (filterInfoCollection[rDListDevice.SelectedIndex].MonikerString);
			videoCaptureDevice.NewFrame += FinalFrame_NewFrame;
			videoCaptureDevice.Start ();
        }
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs  eventArtgs) {
            try
            {
                 pictImage.Image = (Bitmap)eventArtgs.Frame.Clone();
            }
            catch (Exception ex)
            {
				MessageBox.Show (ex.Message, "erreur  ", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }        
        }
	}
}
