using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ALCHANYSCHOOL
{

    public partial class FrmPlashScreem : Form
    {
		private string DirectoryIniFilePath = Application.StartupPath + "\\ConnectAPI.ini";
		Connection MyObjConnection;
        string[] tabsimul = new string[] { 
                                           "Chargement en cours...",
                                           "Vérification de l'existance de la base de données...",
                                           "Création des menus des différents formulaires...",
                                           "Création des boutons...",
                                           "chargement des images pour le design...",
                                           "Création des différents formulaires...",
                                           "Attribution des menus et boutons aux formulaires...",
                                           "Création de l'utilisateur par defaut...",
                                           "Préparation de l'interface de connection...",
                                           "Terminé."        
                                          };
        public FrmPlashScreem()
        {
            InitializeComponent();
        }
        int i = 0;
        double op = 100;
        private void FrmPlashScreem_Load_1(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i == tabsimul.Length)
            {
                timer1.Stop();
                timer2.Start();
            }
            else
            {
                lblCh.Text = tabsimul[i];
                i++;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (op == 0)
            {
			String ServerName = String.Empty;
			String UserName = String.Empty;
			String DataBase = String.Empty;
			String Password = String.Empty;
			timer2.Stop ();
			this.Close ();
				if (File.Exists (DirectoryIniFilePath))
					{
						var MyIni = new IniFilePro (DirectoryIniFilePath);
						ServerName = MyIni.Read ("ServerName");
						UserName = MyIni.Read ("UserName");
						DataBase = MyIni.Read ("DataBase").ToUpper ();
						Password = MyIni.Read ("KEY");
						if (!Password.Equals ("") )
						Password = EncryptDecrypt.DecryptCipherTextToPlainText (MyIni.Read ("KEY"));
						if (ServerName.Equals ("") || UserName.Equals ("") || DataBase.Equals ("") || Password.Equals (""))
							{
								Frm_Conf Frm_Conf = new Frm_Conf ();
								Frm_Conf.Show ();
							}
						else
							{
							MyObjConnection = new Connection (ServerName, DataBase, UserName, Password);
							if (MyObjConnection.Con.State == ConnectionState.Open)
								{
									F_Authenification Authenification = new F_Authenification ();
									Authenification.Show ();
								}
							else
								{
								Frm_Conf Frm_Conf = new Frm_Conf ();
								Frm_Conf.Show ();
								}
							}
					}
            }
            else
            {
                op -= 10;
                this.Opacity = op / 100;
            }
        }
    }
}
