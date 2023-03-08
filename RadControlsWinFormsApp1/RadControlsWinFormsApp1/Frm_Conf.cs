using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.IO;
using System.Data.SqlClient;
namespace ALCHANYSCHOOL
{
    public partial class Frm_Conf : Telerik.WinControls.UI.RadForm
    {
	private string DirectoryIniFilePath = Application.StartupPath + "\\ConnectAPI.ini";
	Connection MyObjConnection;
        public Frm_Conf()
        {
            InitializeComponent();
        }
		private void btnCheckConnexion_Click (object sender, EventArgs e)
			{
			try
				{
				if (txtservername.Text != "" & txtpwd.Text != "" & txtLogin.Text != "") 
					{
					//DropDownListDB.Text != "" & 
					MyObjConnection = new Connection (txtservername.Text, "", txtLogin.Text, txtpwd.Text);
					lblresultat.Text = "Reponse du serveur est : " + MyObjConnection.Con.State.ToString ();
					SqlDataReader dataReader;
					dataReader = MyObjConnection.GetCommand ("SELECT* FROM sys.databases").ExecuteReader ();
					DropDownListDB.Items.Clear ();
					while (dataReader.Read ())
						{
							DropDownListDB.Items.Add (dataReader.GetValue (0).ToString ());
							btnValider.Enabled = true;
						}
					dataReader.Close ();			
					}				
				}
			catch (Exception)
				{
					MessageBox.Show ("Can not open connection ! ");
				}
			}

		private void btnValider_Click (object sender, EventArgs e)
			{
				var MyIni = new IniFilePro (DirectoryIniFilePath);
				if (DropDownListDB.Text != "" & txtservername.Text != "" & txtpwd.Text != "" & txtLogin.Text != "")
					{
							/**************************** Parametrage du server SQL*************************/
							MyIni.Write ("ServerName", txtservername.Text);
							MyIni.Write ("UserName", txtLogin.Text);
							MyIni.Write ("KEY", EncryptDecrypt.EncryptPlainTextToCipherText (txtpwd.Text));
							MyIni.Write ("DataBase", DropDownListDB.Text);
							/******************************************************************************/
							//MessageBox.Show ("Configuration effectue avec Success !", "Config fichier ini ", MessageBoxButtons.OK, MessageBoxIcon.Information);
							this.Close ();
							F_Authenification Authenification = new F_Authenification ();
							Authenification.Show ();
					}
				else
					{
						MessageBox.Show ("Veillez a remplir tout les champs SVP ! ");
					}   
			}

		private void btnfermer_Click (object sender, EventArgs e)
			{
					Application.Exit ();
			}
    }
}
