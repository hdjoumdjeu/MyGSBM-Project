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
    public partial class F_Configuration : Telerik.WinControls.UI.RadForm
    {
        private string DirectoryIniFilePath = Application.StartupPath + "\\ConnectAPI.ini";
        Connection MyObjConnection;

        public F_Configuration()
        {
            InitializeComponent();
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            var MyIni = new IniFilePro(DirectoryIniFilePath);
            
           
            if (DropDownListDB.Text != "" & txtservername.Text != "" & txtpwd.Text != "" & txtLogin.Text != "")
            {
                /**************************** Parametrage du server SQL*************************/
                MyIni.Write("ServerName", txtservername.Text);
                MyIni.Write("UserName", txtLogin.Text);
                MyIni.Write("KEY", EncryptDecrypt.EncryptPlainTextToCipherText(txtpwd.Text));
                MyIni.Write("DataBase", DropDownListDB.Text);
                /******************************************************************************/

                MyIni.Write("ENTETE-PAGE", txtentete.Text ,"LINK");
                MyIni.Write("PIED-PAGE", txtpied.Text, "LINK");
                MyIni.Write("LOGO", txtLogo.Text, "LINK");
                if (CheckStatusImpression.Checked == true)
                {
                    MyIni.Write("StatusImpression", "YES", "LINK");
                }
                else
                {
                    MyIni.Write("StatusImpression", "NO", "LINK");
                }
                MessageBox.Show("Configuration effectue avec Success !", "Config fichier ini ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veillez a remplir tout les champs SVP ! ");
            }   
        }
        private void btnfermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Ini File |*.ini";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                txturlFile.Text = FileName;
            }
        }

        private void F_Configuration_Load(object sender, EventArgs e)
        {

            if (File.Exists(DirectoryIniFilePath)) {
                var MyIni = new IniFilePro(DirectoryIniFilePath);
                txtservername.Text = MyIni.Read("ServerName");
                txtLogin.Text = MyIni.Read("UserName");                              
                DropDownListDB.Text = MyIni.Read("DataBase").ToUpper();
                txtentete.Text = MyIni.Read("ENTETE-PAGE", "LINK");
                txtpied.Text = MyIni.Read("PIED-PAGE", "LINK");
                txtLogo.Text = MyIni.Read("LOGO", "LINK");
                String StatusNotify = MyIni.Read("StatusImpression", "LINK");

                if (StatusNotify.Equals("YES"))
                    CheckStatusImpression.Checked = true;
                else
                    CheckStatusImpression.Checked = false;
                txtpwd.Text = EncryptDecrypt.DecryptCipherTextToPlainText(MyIni.Read("KEY"));
                MyObjConnection = new Connection(txtservername.Text, DropDownListDB.Text, txtLogin.Text, txtpwd.Text);
                lblresultat.Text = "Reponse du Serveur DB : " + MyObjConnection.Con.State.ToString();
            }            
        }

        private void btnCheckConnexion_Click(object sender, EventArgs e)
        {
            try
            {     
                MyObjConnection = new Connection(txtservername.Text, "", txtLogin.Text, txtpwd.Text);
                lblresultat.Text = "Reponse du serveur est : " + MyObjConnection.Con.State.ToString();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT* FROM sys.databases").ExecuteReader();
                DropDownListDB.Items.Clear();
                while (dataReader.Read())
                {
                    DropDownListDB.Items.Add(dataReader.GetValue(0).ToString());
                }
                dataReader.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Can not open connection ! "); 
            }
           
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "School Logo |*.JPG";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                txtLogo.Text = FileName;
            }
        }

        private void radGroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Logo Entete de Page |*.JPG";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                txtentete.Text = FileName;
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Logo Pied de Page |*.JPG";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                txtpied.Text = FileName;
            }
        }
    }
}
