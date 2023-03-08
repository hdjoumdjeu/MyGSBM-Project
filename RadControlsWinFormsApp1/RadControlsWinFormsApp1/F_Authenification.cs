using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ALCHANYSCHOOL.Properties;
using Telerik.WinControls;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace ALCHANYSCHOOL
{
    public partial class F_Authenification : Telerik.WinControls.UI.RadForm
    {
        private string DirectoryIniFilePath = Application.StartupPath + "\\ConnectAPI.ini";
        private String UserLogin;
        private String ChkStatut;
        Connection MyObjConnection;
        [DllImport ("Gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject (IntPtr hObject);

        [DllImport ("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );

        public F_Authenification()
        {
            InitializeComponent();
            IntPtr handle = CreateRoundRectRgn (0, 0, Width, Height, 80, 80);
            if (handle == IntPtr.Zero)
                ;// error with CreateRoundRectRgn
             Region = System.Drawing.Region.FromHrgn (handle);
             DeleteObject (handle);

            var MyIni = new IniFilePro(DirectoryIniFilePath);
                       Settings.Default["ServerName"] = MyIni.Read("ServerName");
                       Settings.Default["UserName"] = MyIni.Read("UserName");
                       Settings.Default["DataBase"] = MyIni.Read("DataBase");
                       Settings.Default["Exercice"] = "2019-2020";
                       Settings.Default["KEY"] = EncryptDecrypt.DecryptCipherTextToPlainText(MyIni.Read("KEY"));
                       Settings.Default["ENTETEPAGE"] = MyIni.Read("ENTETE-PAGE", "LINK");
                       Settings.Default["PIEDPAGE"] = MyIni.Read("PIED-PAGE", "LINK");
                       Settings.Default["LOGO"] = MyIni.Read("LOGO", "LINK");
                       Settings.Default["StatusImpression"] = MyIni.Read("StatusImpression", "LINK");
                       txtlogin.Text  = MyIni.Read ("MySessionUID");
                      if( MyIni.Read ("ChkStatut").Equals("True"))
                          ChkMySessionUID.Checked=true;
                          else
                          ChkMySessionUID.Checked=false;      
			}
        Boolean UsernameIsCorrect;
        String PictureProfil="";
        String UserConnecter="";
        private void radButton1_Click(object sender, EventArgs e)
        {
                if (ChkMySessionUID.Checked) {
                            var MyIni = new IniFilePro (DirectoryIniFilePath);
                            /**************************** Garde ma session*************************/
                                     MyIni.Write ("MySessionUID", txtlogin.Text);
                                     MyIni.Write ("ChkStatut", "True");
                            /******************************************************************************/
                    } else {
                            var MyIni = new IniFilePro (DirectoryIniFilePath);
                            /**************************** Garde ma session*************************/
                                     MyIni.Write ("MySessionUID", "");
                                     MyIni.Write ("ChkStatut", "");
                            /******************************************************************************/
                    }
           
            if (txtlogin.Text!="" && txtpawd.Text!="")
            {
                if (DropDownListY.Text !="")
                {                   
                    if (CheckLoginTrue (txtlogin.Text, txtpawd.Text)) 
                        {
                            if (!EstActif)
                              {
                                     MessageBox.Show ("Cet Utilisateur a étè bloqué", "Controle Authentification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                              } else
                                {
                                     Settings.Default["Exercice"] = DropDownListY.Text;
                                     Settings.Default["UserConnecter"] = UserConnecter;
                                     this.Close ();
                                     Frm_MenuApplication MDI = new Frm_MenuApplication ();
                                     MDI.PictureProfil=PictureProfil;
                                     MDI.Show ();
                                }                        
                        } else 
                        {
                        if (UsernameIsCorrect) {
                            MessageBox.Show ("Incorrect password !", "Controle Authentification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            } else 
                            {
                            MessageBox.Show ("Login et Mot de pass incorrect!", "Controle Authentification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                       }                   
                }
                else
                {
                    //traitement exercice non selectionner ici
                    MessageBox.Show ("Veillew choisir une année scolaire SVP ", "Controle Année Scolaire ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }         
            }
            else
            {
                MessageBox.Show("Echec de connection veillez a renseigner les champs!", "Verification d'authentification ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }          
        }
        public static string PasswordMD5 (string s) {
            MD5 md5 = MD5.Create ();
            StringBuilder builder = new StringBuilder ();
            foreach (byte b in md5.ComputeHash (Encoding.UTF8.GetBytes (s)))
                builder.Append (b.ToString ("x2").ToLower ());

            return builder.ToString ();
            }
        private void F_Authenification_Load(object sender, EventArgs e)
        {
     				MyObjConnection = new Connection (Settings.Default["ServerName"].ToString (), Settings.Default["DataBase"].ToString (), Settings.Default["UserName"].ToString (), Settings.Default["KEY"].ToString ());
				ChargementData_Exercices ();           
        }
        void ChargementData_Exercices()
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Exercices").ExecuteReader();
                DropDownListY.Items.Clear();
                ComboCheckYearArchive.Items.Clear();
                while (dataReader.Read())
                {
                    DropDownListY.Items.Add(Convert.ToDateTime(dataReader["DateDebut"].ToString()).Year + "-" + Convert.ToDateTime(dataReader["Datefin"].ToString()).Year);
                    ComboCheckYearArchive.Items.Add(dataReader["Estatus"].ToString());
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Chargement des exercices scolaire", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        Boolean EstActif;
        public Boolean  CheckLoginTrue (String UseName,String Password) {
            try {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader ("SELECT * FROM F_USERS FU INNER JOIN F_Professeurs FP ON FP.MatriculeP=FU.PersonneId AND FU.UserName='" + UseName + "'");
                while (dataReader.Read ()) 
                    {
                        if (PasswordMD5 (Password)==dataReader["Password"].ToString ())
                         {
                            PictureProfil=dataReader["ProfilPro"].ToString ();
                            UserConnecter=dataReader["NomP"].ToString ();
                            EstActif=(bool)dataReader["EstActif"];
                            return true;
                         }
                        UsernameIsCorrect=true;
                    }
                return false ;
                } catch (Exception ex) {
                return false ;
                }
            }
        private void radButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DropDownListY_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ComboCheckYearArchive.SelectedIndex = e.Position;
            Settings.Default["IsCurrentExercice"] = ComboCheckYearArchive.Text;
        }

        private void F_Authenification_Paint (object sender, PaintEventArgs e) {
        ControlPaint.DrawBorder (e.Graphics, this.ClientRectangle, Color.Green, 5, ButtonBorderStyle.Outset, Color.Red, 5, ButtonBorderStyle.Outset, Color.Yellow, 5, ButtonBorderStyle.Solid, Color.Red, 5, ButtonBorderStyle.Solid);
        //ControlPaint.DrawBorder (e.Graphics, this.ClientRectangle, Color.White, 1, ButtonBorderStyle.Outset, Color.White, 1, ButtonBorderStyle.Outset, Color.Yellow, 10, ButtonBorderStyle.Solid, Color.White, 1, ButtonBorderStyle.Solid);

            }
        private void txtpawd_KeyDown (object sender, KeyEventArgs e) {
                if (e.KeyCode==Keys.Enter) {
                       radButton1_Click( sender,  e);
                    }
            }      
    }
}
