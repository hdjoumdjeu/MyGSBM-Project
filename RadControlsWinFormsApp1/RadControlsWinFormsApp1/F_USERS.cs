using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using ALCHANYSCHOOL.Properties;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using Telerik.WinControls.UI;
using System.Security.Cryptography;
namespace ALCHANYSCHOOL
{
    public partial class F_UTILISATEURS : Telerik.WinControls.UI.RadForm
    {
		Connection MyObjConnection;
        public F_UTILISATEURS()
        {
            InitializeComponent();
			MyObjConnection = new Connection (Settings.Default["ServerName"].ToString (), Settings.Default["DataBase"].ToString (), Settings.Default["UserName"].ToString (), Settings.Default["KEY"].ToString ());

        }

		private void F_USERS_Load (object sender, EventArgs e)
			{
				ChargementData_Professeurs ();
                ChargementData_UserGroupe ();
                ChargementData_Groupe ();
			}
		void ChargementData_Professeurs ()
			{
			try
				{
				int cpt = 0;
				if (MyObjConnection.Con.State == ConnectionState.Closed)
					MyObjConnection.Con.Open ();
				SqlDataReader dataReader;
				dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_Professeurs ").ExecuteReader ();
				if (DGVProfesseurs.Rows.Count > 0)
					{
					DGVProfesseurs.Rows.Clear ();
					}
				while (dataReader.Read ())
					{
						DGVProfesseurs.Rows.AddNew ();
						cpt = DGVProfesseurs.RowCount - 1;
                        DGVProfesseurs.Rows[cpt].Cells["Matricule"].Value = dataReader["MatriculeP"].ToString ();
						DGVProfesseurs.Rows[cpt].Cells["Nom"].Value = dataReader["NomP"].ToString ();				
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{

                MessageBox.Show (ex.Message, "erreur ChargementData_Professeurs", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}
		String matricule="";
		private void MasterTemplate_CellClick (object sender, GridViewCellEventArgs e)
			{
				
                try {
                         if (e.RowIndex>=0) {
                                     matricule = DGVProfesseurs.Rows[e.RowIndex].Cells["Matricule"].Value.ToString ();
                             }

                    } catch (Exception ex) {

                    MessageBox.Show (ex.Message, "erreur ....", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
			}
		void ChargementData_Groupe()
			{
			try
				{
				int index = 0;
				SqlDataReader dataReader;
				if (ComboBoxGroupe.MultiColumnComboBoxElement.Rows.Count>0)
						ComboBoxGroupe.MultiColumnComboBoxElement.Rows.Clear ();
				dataReader = MyObjConnection.GetCommand ("SELECT * FROM P_GROUPE ").ExecuteReader ();
				while (dataReader.Read ())
					{
						ComboBoxGroupe .MultiColumnComboBoxElement.Rows.AddNew ();
						index = ComboBoxGroupe.MultiColumnComboBoxElement.Rows.Count - 1;
						ComboBoxGroupe.MultiColumnComboBoxElement.Rows[index].Cells["Code"].Value = dataReader["Code"].ToString ();
						ComboBoxGroupe.MultiColumnComboBoxElement.Rows[index].Cells["Intitule"].Value = dataReader["Intitule"].ToString ();					
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur Chargement des Groupe", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}
		void CreationUser (String Matricule)
			{
			try
				{
				String Query = "";
                MD5 md5 = MD5.Create ();
                if (txtlogin.Text != "" && txtpassword.Text != "" && ComboBoxGroupe.Text != "" && matricule !="")
					{
                    Query = "INSERT INTO F_USERS(PersonneId,UserName,Password,IdGroupe,EstActif) VALUES('" + Matricule + "','" + txtlogin.Text.Replace ("'", "''").Replace (@"\", @"\\") + "','" +
						 PasswordMD5 (txtpassword.Text) +"','" + ComboBoxGroupe.Text + "','" + chboxActif.Checked  + "')";
						MyObjConnection.Execute (Query);	
                        MessageBox.Show ("Enregistrement effectuée avec Succès!", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ChargementData_UserGroupe ();
                        CleanText ();
                    } else {
                    if ( !(matricule !="")) {
                          MessageBox.Show("Veillez selection l'utilisateur SVP!", "Controle de selection de l'utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
				}
			catch (Exception ex)
				{
					MessageBox.Show (ex.Message, "erreur creation de l'utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
        void CleanText () {
                 txtlogin.Text = ""; 
                 txtpassword.Text = "";
                 matricule  ="";
                 lblSave.Text="&Enregistrer";
            }
       void CreationGroupe ()
			{
			try
				{
				    String Query = "";
				    if (txtcode.Text != "" && txtintitule.Text != "")
					    {
					         Query = "INSERT INTO P_GROUPE(Code,Intitule) VALUES('" + txtcode.Text.Replace ("'", "''").Replace (@"\", @"\\") + "','" +
					         txtintitule.Text.Replace ("'", "''").Replace (@"\", @"\\") + "')";
					         MyObjConnection.Execute (Query);
					    }
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur creation du groupe", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
           public static string PasswordMD5(string s)
            {
                  MD5 md5 = MD5.Create ();
                StringBuilder builder = new StringBuilder();                           
            
                foreach (byte b in md5.ComputeHash(Encoding.UTF8.GetBytes(s)))
                    builder.Append(b.ToString("x2").ToLower());
            
                return builder.ToString();        
            }
		private void lblSaveGrp_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
			{
            if (txtcode.Text != "" && txtintitule.Text != "") 
                {
                    CreationGroupe ();
                    ChargementData_Groupe ();
                } else {
                    MessageBox.Show ("Veillez a bien remplir les champs avant la validation SVP", "Controle sur les champs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }				
			}

		private void lblSave_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
        if (lblSave.Text.Equals ("&Modifier"))
            {
            String Query="";
            if (txtpassword.Text!="") {
                Query = "UPDATE F_USERS SET UserName='" + txtlogin.Text.Replace ("'", "''").Replace (@"\", @"\\") + "',EstActif='" + chboxActif.Checked  + "',Password='" + PasswordMD5 (txtpassword.Text) + "',IdGroupe='" + ComboBoxGroupe.Text  + "' WHERE ID=" + Identifiant;

                } else {
                Query = "UPDATE F_USERS SET UserName='" + txtlogin.Text.Replace ("'", "''").Replace (@"\", @"\\") + "',EstActif='" + chboxActif.Checked  + "',IdGroupe='" + ComboBoxGroupe.Text  + "' WHERE ID=" + Identifiant;
                }
                 MyObjConnection.Execute (Query);
                 MessageBox.Show ("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
              CreationUser (matricule);
            }			
                ChargementData_UserGroupe ();
		}
        void ChargementData_UserGroupe() {
            try {
                int cpt = 0;
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open ();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_USERS FU INNER JOIN P_GROUPE PG ON FU.IdGroupe=PG.Code INNER JOIN F_Professeurs FP ON FP.MatriculeP=FU.PersonneId").ExecuteReader ();
                if (DGV_USERS.Rows.Count > 0) {
                        DGV_USERS.Rows.Clear ();
                    }
                while (dataReader.Read ()) {
                          DGV_USERS.Rows.AddNew ();
                    cpt = DGV_USERS.RowCount - 1;
                    DGV_USERS.Rows[cpt].Cells["ID"].Value = dataReader["ID"].ToString ();
                    DGV_USERS.Rows[cpt].Cells["Login"].Value = dataReader["NomP"].ToString ();
                    DGV_USERS.Rows[cpt].Cells["Groupe"].Value = dataReader["Intitule"].ToString ();
                    DGV_USERS.Rows[cpt].Cells["EstActif"].Value = dataReader["EstActif"].ToString ();
                    DGV_USERS.Rows[cpt].Cells["IdGroupe"].Value = dataReader["IdGroupe"].ToString ();
                    DGV_USERS.Rows[cpt].Cells["UserName"].Value = dataReader["UserName"].ToString ();
                    }
                dataReader.Close ();
                } catch (Exception ex) {

                MessageBox.Show (ex.Message, "erreur ChargementData_UserGroupe", MessageBoxButtons.OK);
                }

            }
        String Identifiant="";
        private void MasterTemplate_CellClick_1 (object sender, GridViewCellEventArgs e)
            {  
   
                 try {
                         if (e.RowIndex>=0) {
                                    Identifiant=  DGV_USERS.Rows[e.RowIndex].Cells["ID"].Value.ToString ();
                                    txtlogin.Text = DGV_USERS.Rows[e.RowIndex].Cells["UserName"].Value.ToString ();
                                    ComboBoxGroupe.Text= DGV_USERS.Rows[e.RowIndex].Cells["IdGroupe"].Value.ToString ();
                                    chboxActif.Checked=(bool)DGV_USERS.Rows[e.RowIndex].Cells["EstActif"].Value;
                                    lblSave.Text="&Modifier";
                             }

                     } catch (Exception ex) {

                     MessageBox.Show (ex.Message, "erreur ....", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
            }

        private void lblAnnuler_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e) {
                     CleanText ();
            }

        private void linkLabel1_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e) {
                    txtintitule.Text="";
                    txtcode.Text="";
            }
    }
}
