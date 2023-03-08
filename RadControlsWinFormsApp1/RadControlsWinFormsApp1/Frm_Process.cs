using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Data.SqlClient;
using ALCHANYSCHOOL.Properties;
using System.IO;

namespace ALCHANYSCHOOL
{
    public partial class Frm_Process : Telerik.WinControls.UI.RadForm
    {
		Connection MyObjConnection;
        public Frm_Process()
        {
            InitializeComponent();
			MyObjConnection = new Connection (Settings.Default["ServerName"].ToString (), Settings.Default["DataBase"].ToString (), Settings.Default["UserName"].ToString (), Settings.Default["KEY"].ToString ());
        }
		void ChargementData_Classes ()
			{
			try
				{

				radDListClasse.Items.Clear ();
				SqlDataReader dataReader;
				dataReader = MyObjConnection.GetCommand ("SELECT DISTINCT Intitule  FROM F_Inscriptions Fi INNER JOIN F_Classes  FC ON FI.ClasseId =FC.Code  AND FI.Statut=1").ExecuteReader ();
				while (dataReader.Read ())
					{
					radDListClasse.Items.Add (dataReader["Intitule"].ToString ());
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{

				MessageBox.Show (ex.Message, "erreur Chargement des classes", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}

		private void Frm_Process_Load (object sender, EventArgs e)
			{
				ChargementData_Classes ();
				ChargementData_Vehicule ();
				ChargementData_Supplement ();
			}

		private void radDListClasse_TextChanged (object sender, EventArgs e)
			{
				ChargementData_Edutiants (getIdClasse (radDListClasse.SelectedItem.Text));
			}
		public String getIdClasse (String intituleClasse)
			{

			try
				{
				SqlDataReader dataReader;
				dataReader = MyObjConnection.GetReader ("SELECT * FROM F_Classes WHERE Intitule='" + intituleClasse + "'");
				while (dataReader.Read ())
					{
					return dataReader["Code"].ToString ();
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				return null;
				}

			return "";
			}
		void ChargementData_Edutiants (String ClasseId)
			{
			try
				{
				int cpt = 0;
				Boolean IsTrue = false;
				SqlDataReader dataReader;

				dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_Inscriptions Fi INNER JOIN F_Etudiants FE ON FI.EtudiantId=FE.Matricule INNER JOIN F_Classes  FC ON FI.ClasseId =FC.Code  AND FI.Statut=1 AND FI.ClasseId='" + ClasseId + "' AND FI.EXERCICE='" + Settings.Default["Exercice"] + "'").ExecuteReader ();
				if (DGVEtudiants.Rows.Count > 0)
					{
					DGVEtudiants.Rows.Clear ();
					}
				while (dataReader.Read ())
					{
					if (dataReader["infosLibre"].ToString ().Equals ("Abandonner"))
						{
							continue;
						}
					IsTrue = true;
					DGVEtudiants.Rows.AddNew ();
					cpt = DGVEtudiants.RowCount - 1;
					DGVEtudiants.Rows[cpt].Cells["Matricule"].Value = dataReader["Matricule"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["NomE"].Value = dataReader["NomE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["Prenome"].Value = dataReader["PrenomE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["DOBE"].Value = dataReader["DOBE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["CNI"].Value = dataReader["CNI"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["TelephoneE"].Value = dataReader["TelephoneE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["TelephoneMobileE"].Value = dataReader["TelephoneMobileE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["EmailE"].Value = dataReader["EmailE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["AdresseE"].Value = dataReader["AdresseE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["VilleE"].Value = dataReader["VilleE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["NiveauScolaireE"].Value = dataReader["NiveauScolaireE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["RemarqueE"].Value = dataReader["RemarqueE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["ProfilPro"].Value = dataReader["ProfilPro"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["GenderE"].Value = dataReader["GenderE"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["infosLibre"].Value = dataReader["infosLibre"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["Discompte"].Value = dataReader["Dsicompte"].ToString () + "%";
					//InscriptionID
					DGVEtudiants.Rows[cpt].Cells["InscriptionID"].Value = dataReader["IdInsp"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["ClasseID"].Value = dataReader["ClasseId"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["Statut"].Value = dataReader["Statut"].ToString ();

					DGVEtudiants.Rows[cpt].Cells["Section"].Value = dataReader["Section"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["TypeFormation"].Value = dataReader["TypeFormation"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["IntituleClasse"].Value = dataReader["Intitule"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["Capacite"].Value = dataReader["Capacite"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["Groupe"].Value = dataReader["Groupe"].ToString ();
					DGVEtudiants.Rows[cpt].Cells["Choix"].Value = false;
					Initializer ();
					if (!dataReader["infosLibre"].ToString ().Equals (""))
						{
						DGVEtudiants.Rows[cpt].Cells["NomE"].Style.ForeColor = System.Drawing.Color.Brown;
						DGVEtudiants.Rows[cpt].Cells["NomE"].Style.Font = new System.Drawing.Font ("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
						DGVEtudiants.Rows[cpt].Cells["NomE"].Style.BackColor = System.Drawing.Color.Yellow;

						DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.BackColor = System.Drawing.Color.Yellow;
						DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.ForeColor = System.Drawing.Color.Brown;
						DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.Font = new System.Drawing.Font ("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
						DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.BackColor = System.Drawing.Color.Gainsboro;
						btnSave.Enabled = false;
						}
					else
						{
						//traitement en attente ici 
						btnSave.Enabled = true;
						}
					}
				if (IsTrue == false)
					{
						Initializer ();
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur ChargementData_Etudiants", MessageBoxButtons.OK);
				}
			}
		void Initializer () 
			{
					//reminitialiser les champsa ici
			
			}

		void ChargementData_Supplement ()
			{
			try
				{
				int cpt = 0;
				SqlDataReader dataReader;
				dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_SUPPLEMENTS ").ExecuteReader ();
				if (DGV_SUPPLEMENT.Rows.Count > 0)
					DGV_SUPPLEMENT.Rows.Clear ();
				while (dataReader.Read ())
					{
					DGV_SUPPLEMENT.Rows.AddNew ();
					cpt = DGV_SUPPLEMENT.RowCount - 1;
					DGV_SUPPLEMENT.Rows[cpt].Cells["ID"].Value = dataReader["Id"].ToString ();
					DGV_SUPPLEMENT.Rows[cpt].Cells["LibelleSup"].Value = dataReader["Libelle"].ToString ();
					DGV_SUPPLEMENT.Rows[cpt].Cells["MontantSup"].Value = double.Parse (dataReader["Montant"].ToString ());
					DGV_SUPPLEMENT.Rows[cpt].Cells["Selection"].Value = false;
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur Chargement des Supplements", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		void ChargementData_Vehicule ()
			{
			try
				{
				int index = 0;
				SqlDataReader dataReader;
				dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_VEHICULE ").ExecuteReader ();
				while (dataReader.Read ())
					{
							CombVehicule.MultiColumnComboBoxElement.Rows.AddNew ();
							index = CombVehicule.MultiColumnComboBoxElement.Rows.Count - 1;
							CombVehicule.MultiColumnComboBoxElement.Rows[index].Cells["ID"].Value = dataReader["ID"].ToString ();
							CombVehicule.MultiColumnComboBoxElement.Rows[index].Cells["Libelle"].Value = dataReader["Libelle"].ToString ();
							CombVehicule.MultiColumnComboBoxElement.Rows[index].Cells["Model"].Value = dataReader["Model"].ToString ();
							CombVehicule.MultiColumnComboBoxElement.Rows[index].Cells["killometrage"].Value = int.Parse (dataReader["killometrage"].ToString ());
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur Chargement des Transports", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}

		private void btnSave_Click (object sender, EventArgs e)
			{
			String Choix;
				for (int i = 0; i < DGVEtudiants.RowCount; i++)
				{
				Choix = DGVEtudiants.Rows[i].Cells["Choix"].Value.ToString ();
				if (Choix=="True")
					{
					if (txtlieu.Text != "" && txtmontant.Text != "" && CombNature.Text != "" && CombVehicule.Text != "" && numDuree.Value != 0)
						{
							CreationUpdateLigneTransport (DGVEtudiants.Rows[i].Cells["Matricule"].Value.ToString ());
						}
						CreationUpdateLigneSupplement (DGVEtudiants.Rows[i].Cells["Matricule"].Value.ToString ());					
					}				
				}
				if (IsCreatedVLine || IsCreatedSupLine )
					{
						MessageBox.Show ("Enregistrement effectuée avec Succès!", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
						txtlieu.Text = "" ;
						txtmontant.Text = "" ;
						CombNature.Text = "";
						CombVehicule.Text = "" ;
						numDuree.Value = 1;
						chboxAutoRenew.Checked = false;
					}
			}
		Boolean IsCreatedVLine = false;
		void CreationUpdateLigneTransport (String Matricule) 
			{

			try
				{
				String Query = "";
				if (txtlieu.Text != "" && txtmontant.Text != "" && CombNature.Text != "" && CombVehicule.Text != "" && numDuree.Value != 0 && Matricule != "")
					{
					Query = "INSERT INTO F_LineVehicule(Lieu,Nature,Montant,Duree,Enroll,Idvehicule,EtudiantId,Exercice) VALUES('" + txtlieu.Text.Replace ("'", "''").Replace (@"\", @"\\") + "','" +
						CombNature.Text.Replace ("'", "''").Replace (@"\", @"\\") + "'," + txtmontant.Text + "," + numDuree.Value + ",'" + chboxAutoRenew.Checked + "'," + CombVehicule.Text + ",'" + Matricule + "','" + Settings.Default["Exercice"] + "')";
					MyObjConnection.Execute (Query);
					IsCreatedVLine = true;
					}
				}
			catch (Exception ex)
				{
				IsCreatedVLine = false ;
					MessageBox.Show (ex.Message, "erreur creation Section-1 transport ", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}		
			}
		Boolean IsCreatedSupLine= false;
		void CreationUpdateLigneSupplement (String Matricule)
			{

			try
				{
				String Query = "";
				String  Selection = "";
				for (int i = 0; i < DGV_SUPPLEMENT.RowCount; i++)
					{
					Selection=DGV_SUPPLEMENT.Rows[i].Cells["Selection"].Value.ToString();
					if (Selection.Equals ("True"))
						{
						Query = "INSERT INTO F_LineSupplements(IdSup,EtudiantId,Exercice) VALUES(" + DGV_SUPPLEMENT.Rows[i].Cells["Id"].Value + ",'" + Matricule + "','" + Settings.Default["Exercice"] + "')";
						MyObjConnection.Execute (Query);
						IsCreatedSupLine = true;
						}
					}
				}
			catch (Exception ex)
				{
				IsCreatedSupLine = false;
				MessageBox.Show (ex.Message, "erreur creation Section-2 Supplement ", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

		private void splitPanel10_Click (object sender, EventArgs e)
			{

			}
		DirectoryInfo Dossier = new DirectoryInfo ("PhotoEtudiants");
		private void DGVEtudiants_CellClick (object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
			{
				
                try {
                     if (e.RowIndex>=0) {
                                 String path = Dossier.FullName.ToString ();
                                 ChargementData_Supplement (DGVEtudiants.Rows[e.RowIndex].Cells["Matricule"].Value.ToString ());
                                 ChargementData_Transport (DGVEtudiants.Rows[e.RowIndex].Cells["Matricule"].Value.ToString ());
                                 PictureEleve.ImageLocation = path + @"\" + DGVEtudiants.Rows[e.RowIndex].Cells["ProfilPro"].Value.ToString ();
                         }
                    } catch (Exception ex) {               
                    				MessageBox.Show (ex.Message, "erreur creation Section-2 Supplement ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
			}
		void ChargementData_Supplement (String Matricule)
			{
			try
				{
				int cpt = 0;
				SqlDataReader dataReader;
				dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_SUPPLEMENTS FS INNER JOIN F_LineSupplements FLS ON FS.Id=FLS.IdSup AND FLS.EtudiantId='" + Matricule  + "'").ExecuteReader ();
				if (DGVSUPPREMENT.Rows.Count > 0)
					DGVSUPPREMENT.Rows.Clear ();
				while (dataReader.Read ())
					{
						DGVSUPPREMENT.Rows.AddNew ();
						cpt = DGVSUPPREMENT.RowCount - 1;
						DGVSUPPREMENT.Rows[cpt].Cells["ID"].Value = dataReader["Id"].ToString ();
						DGVSUPPREMENT.Rows[cpt].Cells["LibelleSup"].Value = dataReader["Libelle"].ToString ();
						DGVSUPPREMENT.Rows[cpt].Cells["MontantSup"].Value = double.Parse (dataReader["Montant"].ToString ());					
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur Chargement des Supplements", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

		void ChargementData_Transport (String Matricule)
			{
				try
					{
					Boolean IsRecorded = false;
						SqlDataReader dataReader;
						dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_Etudiants FE INNER JOIN F_LineVehicule FLV ON FE.Matricule =FLV.EtudiantId  INNER JOIN F_VEHICULE FV ON FV.Id =FLV.Idvehicule AND  FLV.EtudiantId='"+ Matricule  +"'").ExecuteReader ();
						while (dataReader.Read ())
							{
								lblvehicule.Text = dataReader["Libelle"].ToString () + " " + dataReader["Model"].ToString ();
								lblLieu.Text = dataReader["Lieu"].ToString ();
								lblnature.Text = dataReader["Nature"].ToString ();
								lblmois.Text = dataReader["Duree"].ToString ()+ " mois";
								lblmontant.Text =double.Parse( dataReader["Montant"].ToString ()).ToString();
								checkRew.Checked =Convert.ToBoolean( dataReader["Enroll"].ToString ());
								IsRecorded = true;
							}
						if (IsRecorded == false)
							{
							lblvehicule.Text = "...";
							lblLieu.Text = "...";
							lblnature.Text = "...";
							lblmois.Text = "...";
							lblmontant.Text = "...";
							checkRew.Checked = false;
							}
						dataReader.Close ();
					}
				catch (Exception ex)
					{
					MessageBox.Show (ex.Message, "erreur afftectattion element selectionnez", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
			}

		private void btnFermer_Click (object sender, EventArgs e)
			{
				this.Close ();
			}
    }
}
