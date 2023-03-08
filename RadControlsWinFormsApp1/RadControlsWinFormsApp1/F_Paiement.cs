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
namespace ALCHANYSCHOOL
{
    public partial class F_Paiement : Telerik.WinControls.UI.RadForm
    {
        Connection MyObjConnection;
        DirectoryInfo Dossier = new DirectoryInfo("PhotoEtudiants");
        String InscriptionID;
        String UserConnecter;
        decimal MontantRemise;
        public F_Paiement()
        {
            InitializeComponent();
            String EndExercice = Settings.Default["Exercice"].ToString().Split('-')[1].ToString();
            String StartExercice = Settings.Default["Exercice"].ToString().Split('-')[0].ToString();
            DateTime DateMax =new DateTime (int.Parse (EndExercice), 01, 31);
            DateTime DateMin =new DateTime (int.Parse (StartExercice), 01, 01);
            DatePaiement.MaxDate = DateMax;
            DatePaiement.MinDate = DateMin;
            MyObjConnection = new Connection(Settings.Default["ServerName"].ToString(), Settings.Default["DataBase"].ToString(), Settings.Default["UserName"].ToString(), Settings.Default["KEY"].ToString());

        }
        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                String Query = "";
                UserConnecter = Environment.UserName;
                if (btnSave.Text.Equals("&Modifier"))
                {
                    Query = "UPDATE F_Paiements SET Avance=Avance+" + txtmontantVerse.Text + ",UpdateUserId=SYSTEM_USER,DateUpdate=GETDATE(),CompteWindows='" + Environment.UserName + "' WHERE Id_Pai=(SELECT Id_Pai FROM F_Inscriptions Fi INNER JOIN F_Paiements FP ON FI.IdInsp=FP.Id_Inscrip AND FI.Statut=1 AND FI.IdInsp=" + InscriptionID + ")";
                    if (!ControleValidation())
                    {
                        MyObjConnection.Execute(Query);
                        MessageBox.Show("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Initializer();
                    }
                    else
                    {
                        MessageBox.Show("Echec d'operation veillez a renseigner tout les parametre de cet operation", "Controle de validite", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }    
                }
                else
                {
                    String MontantNet="0";
                    if (lblTT_Remise.Text !="...")
                    {
                        MontantNet = lblTT_Remise.Text;
                    }
                    else
                    {
                        MontantRemise = 0;
                    }
                    Query = "INSERT INTO F_Paiements(Montant_Net,Avance,RemiseTT,Date_paie,Id_mode_paie,Id_Inscrip,CreatedUserId,UpdateUserId,DateCreated,DateUpdate,CompteWindows,Exercice) VALUES(" +
                        lblpension.Text + "," + txtmontantVerse.Text + "," + MontantRemise + ",'" + DatePaiement.Value.ToString("yyyy-MM-dd") + "','" + ComboTypePaiement.Text.Split('-')[0].ToString() + "'," + InscriptionID +
                        ",'" + UserConnecter + "','" + UserConnecter + "',GETDATE(),GETDATE(),'" + Environment.UserName + "','" + Settings.Default["Exercice"] + "')";
                    if (!ControleValidation())
                    {
                        MyObjConnection.Execute(Query);
                        MessageBox.Show("Enregistrement effectuée avec Succès!", "Paiement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Initializer();
                    }
                    else
                    {
                        MessageBox.Show("Echec d'operation veillez a renseigner tout les parametre de cet operation", "Controle de validite", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }                                
                }               
                ChargementData_Paiement();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur creation de la classe ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        Boolean ControleValidation() {
            if (lblpension.Text == "..." || ComboTypePaiement.Text == "" || double.Parse(txtmontantVerse.Text) <= 0 || radDListClasse.Text =="")
            {
                return true;
            }
            return false;
        }
        private void F_Paiement_Load(object sender, EventArgs e)
        {
            ChargementData_ModePaiement();
            ChargementData_Classes();
            ChargementData_Paiement();
        }
        void ChargementData_Edutiants(String ClasseId)
        {
            try
            {
                int cpt = 0;
                Boolean IsTrue=false;
                SqlDataReader dataReader;

                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Inscriptions Fi INNER JOIN F_Etudiants FE ON FI.EtudiantId=FE.Matricule INNER JOIN F_Classes  FC ON FI.ClasseId =FC.Code  AND FI.Statut=1 AND FI.ClasseId='" + ClasseId + "' AND FI.EXERCICE='" + Settings.Default["Exercice"] + "'").ExecuteReader();
                if (DGVEtudiants.Rows.Count > 0)
                {
                    DGVEtudiants.Rows.Clear();
                }             
                while (dataReader.Read())
                {
                    IsTrue = true;
                    DGVEtudiants.Rows.AddNew();
                    cpt = DGVEtudiants.RowCount - 1;
                    DGVEtudiants.Rows[cpt].Cells["Matricule"].Value = dataReader["Matricule"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["NomE"].Value = dataReader["NomE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["Prenome"].Value = dataReader["PrenomE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["DOBE"].Value = dataReader["DOBE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["CNI"].Value = dataReader["CNI"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["TelephoneE"].Value = dataReader["TelephoneE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["TelephoneMobileE"].Value = dataReader["TelephoneMobileE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["EmailE"].Value = dataReader["EmailE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["AdresseE"].Value = dataReader["AdresseE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["VilleE"].Value = dataReader["VilleE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["NiveauScolaireE"].Value = dataReader["NiveauScolaireE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["RemarqueE"].Value = dataReader["RemarqueE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["ProfilPro"].Value = dataReader["ProfilPro"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["GenderE"].Value = dataReader["GenderE"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["infosLibre"].Value = dataReader["infosLibre"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["Discompte"].Value = dataReader["Dsicompte"].ToString()+"%";
                    //InscriptionID
                    DGVEtudiants.Rows[cpt].Cells["InscriptionID"].Value = dataReader["IdInsp"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["ClasseID"].Value = dataReader["ClasseId"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["Statut"].Value = dataReader["Statut"].ToString();

                    DGVEtudiants.Rows[cpt].Cells["Section"].Value = dataReader["Section"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["TypeFormation"].Value = dataReader["TypeFormation"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["IntituleClasse"].Value = dataReader["Intitule"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["Capacite"].Value = dataReader["Capacite"].ToString();
                    DGVEtudiants.Rows[cpt].Cells["Groupe"].Value = dataReader["Groupe"].ToString();
                    Initializer();
                    if (!dataReader["infosLibre"].ToString().Equals(""))
                    {
                        DGVEtudiants.Rows[cpt].Cells["NomE"].Style.ForeColor = System.Drawing.Color.Brown;
                        DGVEtudiants.Rows[cpt].Cells["NomE"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        DGVEtudiants.Rows[cpt].Cells["NomE"].Style.BackColor = System.Drawing.Color.Yellow;

                        DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.BackColor = System.Drawing.Color.Yellow;
                        DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.ForeColor = System.Drawing.Color.Brown;
                        DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
                     Initializer();
                }  
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur ChargementData_Etudiants", MessageBoxButtons.OK);
            }
        }
        void ChargementData_Paiement()
        {
            try
            {
                int cpt = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT Id_Pai,NomE,Id_Inscrip,Date_paie,Libelle,Montant_Net,Avance,Intitule,RemiseTT  FROM F_Inscriptions FI INNER JOIN F_Paiements FP ON FI.IdInsp=FP.Id_Inscrip  INNER JOIN F_Etudiants FE ON FE.Matricule =FI.EtudiantId INNER JOIN P_ModePaiement PM ON PM.Id_ModePaie=FP.Id_mode_paie INNER JOIN F_Classes FC ON FC.Code=FI.ClasseId AND FP.EXERCICE='" + Settings.Default["Exercice"] + "'").ExecuteReader();
                if (DGVPaiement.Rows.Count > 0)
                {
                    DGVPaiement.Rows.Clear();
                }
                while (dataReader.Read())
                {
                    DGVPaiement.Rows.AddNew();
                    cpt = DGVPaiement.RowCount - 1;
                    DGVPaiement.Rows[cpt].Cells["ID"].Value = dataReader["Id_Pai"].ToString();
                    DGVPaiement.Rows[cpt].Cells["Etudiants"].Value = dataReader["NomE"].ToString();
                    DGVPaiement.Rows[cpt].Cells["InscriptionID"].Value = dataReader["Id_Inscrip"].ToString();
                    DGVPaiement.Rows[cpt].Cells["Classes"].Value = dataReader["Intitule"].ToString();
                    DGVPaiement.Rows[cpt].Cells["DatePaie"].Value = Convert.ToDateTime(dataReader["Date_paie"].ToString()).ToShortDateString();
                    DGVPaiement.Rows[cpt].Cells["ModePaie"].Value = dataReader["Libelle"].ToString();
                    DGVPaiement.Rows[cpt].Cells["MontantPayer"].Value = double.Parse(dataReader["Montant_Net"].ToString());
                    DGVPaiement.Rows[cpt].Cells["MontantVerse"].Value = double.Parse(dataReader["Avance"].ToString());
                    DGVPaiement.Rows[cpt].Cells["RemiseAmount"].Value = double.Parse(dataReader["RemiseTT"].ToString());
                    DGVPaiement.Rows[cpt].Cells["MontantReste"].Value = double.Parse(dataReader["Montant_Net"].ToString()) - double.Parse(dataReader["Avance"].ToString()) - double.Parse(dataReader["RemiseTT"].ToString());

                    DGVPaiement.Rows[cpt].Cells["MontantPayer"].Style.ForeColor = System.Drawing.Color.Brown;
                    DGVPaiement.Rows[cpt].Cells["MontantPayer"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    DGVPaiement.Rows[cpt].Cells["MontantPayer"].Style.BackColor = System.Drawing.Color.White;

                    DGVPaiement.Rows[cpt].Cells["MontantVerse"].Style.ForeColor = System.Drawing.Color.Chocolate;
                    DGVPaiement.Rows[cpt].Cells["MontantVerse"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    DGVPaiement.Rows[cpt].Cells["MontantVerse"].Style.BackColor = System.Drawing.Color.White;

                    DGVPaiement.Rows[cpt].Cells["MontantReste"].Style.ForeColor = System.Drawing.Color.Blue;
                    DGVPaiement.Rows[cpt].Cells["MontantReste"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    DGVPaiement.Rows[cpt].Cells["MontantReste"].Style.BackColor = System.Drawing.Color.White;

                    DGVPaiement.Rows[cpt].Cells["RemiseAmount"].Style.ForeColor = System.Drawing.Color.Green;
                    DGVPaiement.Rows[cpt].Cells["RemiseAmount"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    DGVPaiement.Rows[cpt].Cells["RemiseAmount"].Style.BackColor = System.Drawing.Color.White;

                    if (DGVPaiement.Rows[cpt].Cells["MontantReste"].Value.Equals("0"))
                    {
                        DGVPaiement.Rows[cpt].Cells["Etudiants"].Style.ForeColor = System.Drawing.Color.Maroon;

                        DGVPaiement.Rows[cpt].Cells["InscriptionID"].Style.ForeColor = System.Drawing.Color.Maroon;
                        DGVPaiement.Rows[cpt].Cells["Classes"].Style.ForeColor = System.Drawing.Color.Maroon;
                        DGVPaiement.Rows[cpt].Cells["DatePaie"].Style.ForeColor = System.Drawing.Color.Maroon;
                        DGVPaiement.Rows[cpt].Cells["ModePaie"].Style.ForeColor = System.Drawing.Color.Maroon;
                    }
                }
                this.DGVPaiement.Columns["MontantVerse"].PinPosition = PinnedColumnPosition.Right;
                this.DGVPaiement.Columns["MontantReste"].PinPosition = PinnedColumnPosition.Right; 
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur ChargementData_Paiement", MessageBoxButtons.OK);
            }

        }
        void Initializer() {
            lblfiliere.Text = "...";
            lblclasse.Text = "...";
            lblCapacite.Text = "...";
            lbltypeformation.Text = "...";
            pictImage.Image = ALCHANYSCHOOL.Properties.Resources.student_icon_512;
            lblCategorie.Text = "...";
            lblpension.Text = "...";
            lblRemise.Text = "...";
            lblrestante.Text = "...";
            txtmontantVerse.Text = "0";
            lblTT_Remise.Text = "...";
            txtmontantPayer.Text = "0";
            txtResteA_Payer.Text = "0";
        }
        void ChargementData_ModePaiement()
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM P_ModePaiement order by Id_ModePaie").ExecuteReader();
                ComboTypePaiement.Items.Clear();
                               while (dataReader.Read())
                {
                    ComboTypePaiement.Items.Add(dataReader["Id_ModePaie"].ToString() + "-" + dataReader["Libelle"].ToString());
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur Chargement des Parametrage du mode paiement", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public String getAvanceMontant(String Id_Inscription)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT Avance FROM F_Paiements WHERE Id_Inscrip='" + Id_Inscription + "' AND Exercice='" + Settings.Default["Exercice"] + "'");
                while (dataReader.Read())
                {
                    return dataReader["Avance"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return "";
        }
        public Boolean  IsSolder(String Id_Inscription)
        {
            try
            {
                SqlDataReader dataReader;
                decimal Montant_Net;
                decimal Avance;
                decimal RemiseTT;
                dataReader = MyObjConnection.GetReader("SELECT Montant_Net,Avance,RemiseTT FROM F_Paiements WHERE Id_Inscrip='" + Id_Inscription + "' AND Exercice='" + Settings.Default["Exercice"] + "'");
                while (dataReader.Read())
                {
                    Montant_Net = decimal.Floor(decimal.Parse(dataReader["Montant_Net"].ToString()));
                    Avance = decimal.Floor(decimal.Parse(dataReader["Avance"].ToString()));
                    RemiseTT = decimal.Floor(decimal.Parse(dataReader["RemiseTT"].ToString()));
                    if (Montant_Net==(Avance+RemiseTT))
                        return true;
                    else
                        return false;                    
                }
                dataReader.Close();
                return false;
            }
            catch (Exception ex)
            {
                return false ;
            }       
        }
        public String getIdClasse(String intituleClasse)
        {

            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Classes WHERE Intitule='" + intituleClasse + "'");
                while (dataReader.Read())
                {
                    return dataReader["Code"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return "";
        }
        void ChargementData_Classes()
        {
            try
            {
               
                radDListClasse.Items.Clear();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT DISTINCT Intitule  FROM F_Inscriptions Fi INNER JOIN F_Classes  FC ON FI.ClasseId =FC.Code  AND FI.Statut=1").ExecuteReader();
                while (dataReader.Read())
                {                   
                    radDListClasse.Items.Add(dataReader["Intitule"].ToString());
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur Chargement des classes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }      
        private void MasterTemplate_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            String path = Dossier.FullName.ToString();
            InscriptionID = "";
            if (e.RowIndex>=0)
            {
                String Remise = DGVEtudiants.Rows[e.RowIndex].Cells["Discompte"].Value.ToString();
                 float  RemiseValue; 
                if (Remise != "" && Remise != "%")
                {
                    lblRemise.Text = Remise;
                    lblRemiseVue.Visible = true ;
                    lblRemise.Visible = true ;
                    lblTT_Remise.Visible = true;
                    lblIntituleRemise.Visible = true;
                }
                else
                {
                    lblRemiseVue.Visible = false;
                    lblRemise.Visible  = false;
                    lblTT_Remise.Visible = false ;
                    lblIntituleRemise.Visible = false;
                    lblRemise.Text="...";
                    lblTT_Remise.Text ="...";
                }

                GroupePension(DGVEtudiants.Rows[e.RowIndex].Cells["Groupe"].Value.ToString());
                lblfiliere.Text = DGVEtudiants.Rows[e.RowIndex].Cells["Section"].Value.ToString();
                lblclasse.Text = DGVEtudiants.Rows[e.RowIndex].Cells["IntituleClasse"].Value.ToString();
                lblCapacite.Text = DGVEtudiants.Rows[e.RowIndex].Cells["Capacite"].Value.ToString();
                int PlaceRestante=int.Parse(DGVEtudiants.Rows[e.RowIndex].Cells["Capacite"].Value.ToString()) - DGVEtudiants.Rows.Count;
                lblrestante.Text = PlaceRestante.ToString ();
                lbltypeformation.Text = DGVEtudiants.Rows[e.RowIndex].Cells["TypeFormation"].Value.ToString();          
                if (File.Exists (path + @"\" + DGVEtudiants.Rows[e.RowIndex].Cells["ProfilPro"].Value.ToString ())) {
                pictImage.ImageLocation = path + @"\" + DGVEtudiants.Rows[e.RowIndex].Cells["ProfilPro"].Value.ToString ();
                    } else {
                    pictImage.Image  = ALCHANYSCHOOL.Properties.Resources.Copy_of_stock_people;
                    }
                InscriptionID = DGVEtudiants.Rows[e.RowIndex].Cells["InscriptionID"].Value.ToString();
                String MontantPayer_Avance = getAvanceMontant(InscriptionID);
                double MontantNet = double.Parse(lblpension.Text);

                if (MontantPayer_Avance!="")
                {                                        
                    double MontantAvance = double.Parse(MontantPayer_Avance);                  
                    txtmontantPayer.Text = MontantAvance.ToString();

                    if (lblRemise.Text != "..." && MontantNet > MontantAvance)
                    {
                        RemiseValue = int.Parse(lblRemise.Text.Split('%')[0].ToString());
                        RemiseValue = RemiseValue / 100;
                        MontantRemise = decimal.Ceiling (Convert.ToDecimal ((MontantNet * RemiseValue)));
                        lblTT_Remise.Text = decimal.Ceiling (MontantRemise).ToString ();
                        txtResteA_Payer.Text = decimal.Ceiling (Convert.ToDecimal (MontantNet) - (Convert.ToDecimal (MontantAvance) + MontantRemise)).ToString ();
                    }
                    else
                    {
                        txtResteA_Payer.Text = (MontantNet - MontantAvance).ToString();
                    }
                    btnSave.Text = "&Modifier";
                }
                else
                {
                    txtmontantPayer.Text = "0.0";
                    if (lblRemise.Text != "..." )
                    {
                        RemiseValue = int.Parse(lblRemise.Text.Split('%')[0].ToString());
                        RemiseValue = RemiseValue / 100;
                        MontantRemise = decimal.Ceiling(Convert.ToDecimal((MontantNet * RemiseValue)));
                        lblTT_Remise.Text = decimal.Ceiling (MontantRemise).ToString ();
                        txtResteA_Payer.Text = decimal.Ceiling (Convert.ToDecimal (MontantNet) - MontantRemise).ToString ();
                    }
                    else
                    {
                        txtResteA_Payer.Text = lblpension.Text;
                    }
                    btnSave.Text = "&Enregistrer";
                }

                if (IsSolder(InscriptionID) || !DGVEtudiants.Rows[e.RowIndex].Cells["infosLibre"].Value.ToString().Equals(""))
                {
                    btnSave.Enabled = false;
                    txtmontantVerse.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                    txtmontantVerse.Enabled = true;
                }
            }        
        }
        public void GroupePension(String CodeGroupe)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM P_Pension where CodeCategorie='" + CodeGroupe + "'");
                while (dataReader.Read())
                {
                   String[] Montant=dataReader["Montant"].ToString().Split('.');
                    lblCategorie.Text = dataReader["Intitule"].ToString();
                    lblpension.Text = Montant[0];
                }
                dataReader.Close();              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur selection  categorie pension", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void radDListClasse_TextChanged(object sender, EventArgs e)
        {
            ChargementData_Edutiants(getIdClasse(radDListClasse.SelectedItem.Text));
        }
        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtmontantPayer_Click(object sender, EventArgs e)
        {
            txtmontantVerse.Focus();
        }
        private void txtResteA_Payer_Click(object sender, EventArgs e)
        {
            txtmontantVerse.Focus();
        }
        private void txtmontantVerse_TextChanged(object sender, EventArgs e)
        {
            double MontantNet =  double.Parse (txtResteA_Payer.Text);//double.Parse(txtmontantPayer.Text) +
            if (txtmontantVerse .Text=="")
            {
                txtmontantVerse.Text = "0";
                txtmontantVerse.Focus();
                return;
            }
            if (double.Parse(txtmontantVerse .Text)>MontantNet)
            {
                txtmontantVerse.BackColor = System.Drawing.Color.Pink;
                btnSave.Enabled = false;
            }
            else
            {
                txtmontantVerse.BackColor = System.Drawing.Color.White;
                btnSave.Enabled = true;
            }

        }

        private void txtmontantVerse_KeyPress (object sender, KeyPressEventArgs e) {
            char ch = e.KeyChar;
            if (ch == 46 && txtmontantVerse.Text.IndexOf (".") != -1) {
                e.Handled = true;
                return;
                }
            if (!char.IsDigit (ch) && ch != 8 && ch !=46) {
                e.Handled = true;
                }
            }    
    }
}
