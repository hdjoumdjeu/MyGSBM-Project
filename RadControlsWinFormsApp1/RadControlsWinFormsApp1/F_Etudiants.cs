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
using System.Text.RegularExpressions;
namespace ALCHANYSCHOOL
{
    public partial class F_Etudiants : Telerik.WinControls.UI.RadForm
    {
        Connection MyObjConnection;
        public F_Etudiants()
        {
            InitializeComponent();
            String EndExercice = Settings.Default["Exercice"].ToString().Split('-')[1].ToString();
            String StartExercice = Settings.Default["Exercice"].ToString().Split('-')[0].ToString();
            DateTime DateMax =new DateTime (int.Parse (EndExercice), 01, 31);
            DateTime DateMin =new DateTime (int.Parse (StartExercice), 01, 01);
            DateTimeAbs.MaxDate = DateMax;
            DateTimeAbs.MinDate = DateMin;
            MyObjConnection = new Connection(Settings.Default["ServerName"].ToString(), Settings.Default["DataBase"].ToString(), Settings.Default["UserName"].ToString(), Settings.Default["KEY"].ToString());

        }
        private string CheckIsNull(string ValueRead)
        {

            if (ValueRead.Equals(""))
            {
                return "Null";
            }
            else
            {
                return ValueRead;
            }
        }
        DirectoryInfo Dossier = new DirectoryInfo("PhotoEtudiants");
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String Query = "";
                if (txtnom.Text.Equals ("")|| txtprenom.Text.Equals ("") || Sex.Equals ("")|| txtmatricule.Text.Equals ("") || rDListVille.Text.Equals ("")||txtadresse.Text.Equals ("")) {
                    if (!isValidEmail (txtemail.Text)) {
                        MessageBox.Show ("Email Incorrect!", "Controle du Email", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        } else {
                        MessageBox.Show ("Veillez remplir les champs(*) SVP!", "Validite des champs", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    return;
                    }
                if (!isValidEmail (txtemail.Text) && txtemail.Text !="") {
                    MessageBox.Show ("Email Incorrect!", "Controle du Email", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                    }        
                if (btnSave.Text.Equals("&Modifier"))
                {
                    Query = "UPDATE F_Etudiants SET NomE='" + txtnom.Text + "',PrenomE='" + txtprenom.Text
                                                      + "',DOBE='" + DOB.Value.ToString("yyyy-MM-dd") + "',CNI='" + txtcin.Text
                                                      + "',TelephoneE=" + CheckIsNull(txtdomicile.Text) + ",TelephoneMobileE=" + CheckIsNull(txtmobile.Text)
                                                      + ",EmailE='" + txtemail.Text + "',AdresseE='" + txtadresse.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                      + "',VilleE='" + rDListVille.Text 
                                                      + "',RemarqueE='" + txtremarques.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                      + "',ProfilPro='" + txtmatricule.Text + ".jpg',GenderE='" + Sex + "',NiveauScolaireE='" + rDListeNiveau.Text 
                                                      + "' WHERE Matricule='" + txtmatricule.Text + "'";
                    MyObjConnection.Execute(Query);
                    
                    //**************************************************************Tuteur etudiants**********************************************************************
                    if (MyObjConnection.IsRecord("SELECT * FROM F_Tuteurs where EtudiantsId='" + txtmatricule.Text + "'"))
                    {
                        if (IsTulaireNo_Null()) {
                            Query = "UPDATE F_Tuteurs SET PrenomP='" + txtPprenom.Text + "',PrenomM='" + txtMprenom.Text
                                                         + "',NomP='" + txtPnom.Text + "',NomM='" + txtMnom.Text
                                                         + "',TelP=" + CheckIsNull(txtmobileP.Text) + ",TelM=" + CheckIsNull(txtMmobile.Text)
                                                         + ",EmailP='" + txtemailP.Text + "',EmailM='" + txtMemail.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                         + "',ProfessionP='" + txtProfession.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                         + "',ProfessionM='" + txtMprofession.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                         + "' WHERE EtudiantsId='" + txtmatricule.Text + "'";
                            MyObjConnection.Execute(Query);
                        }
                       
                    }
                    else  {
                        if (IsTulaireNo_Null())
                        {
                            Query = "INSERT INTO F_Tuteurs(PrenomP,PrenomM,NomP,NomM,TelP,TelM,EmailP,EmailM,ProfessionP,ProfessionM,EtudiantsId)Values('" + txtPprenom.Text + "','" + txtMprenom.Text + "','" + txtPnom.Text +
                                                                                                                                                     "','" + txtMnom.Text + "'," + CheckIsNull(txtmobileP.Text) + "," + CheckIsNull(txtMmobile.Text) +
                                                                                                                                                     ",'" + txtemailP.Text + "','" + txtMemail.Text + "','" + txtProfession.Text +
                                                                                                                                                     "','" + txtMprofession.Text + "','" + txtmatricule.Text + "')";
                           MyObjConnection.Execute(Query);
                        }                                                         
                    }
                    //*************************************************************************************************************************************************************
                    
                   //*******************************************************Medication etudiant************************************************************************************
                    if (MyObjConnection.IsRecord("SELECT * FROM F_Medications where EtudiantId='" + txtmatricule.Text + "'"))
                    {
                        if (IsMedicationNo_Null()) {
                            Query = "UPDATE F_Medications SET NomeMedecin='" + txtnomMed.Text + "',GSM='" + txtgsm.Text
                                                         + "',Telephone=" + CheckIsNull(txtmobileHos.Text)
                                                         + ",Observation='" + txtObservationMed.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                         + "' WHERE EtudiantId='" + txtmatricule.Text + "'";
                            MyObjConnection.Execute(Query);
                        
                        }
                       
                    }
                    else
                    {
                        if (IsMedicationNo_Null())
                        {
                            Query = "INSERT INTO F_Medications(NomeMedecin,GSM,Telephone,Observation,EtudiantId)Values('" + txtnomMed.Text + "','" + txtgsm.Text + "'," + CheckIsNull(txtmobileHos.Text) +
                                                                                                                                           ",'" + txtObservationMed.Text.Replace("'", "''").Replace(@"\", @"\\") +
                                                                                                                                          "','" + txtmatricule.Text + "')";
                            MyObjConnection.Execute(Query); 
                        }
                    }
                    //*************************************************************************************************************************************************************
                    //*******************************************************Ets Origine etudiant************************************************************************************
                    if (MyObjConnection.IsRecord("SELECT * FROM F_EtablissementsOrigine where EtudiantsId='" + txtmatricule.Text + "'"))
                    {
                        if (IsEtsOriginaireNo_Null())
                        {
                            Query = "UPDATE F_EtablissementsOrigine SET NomEts='" + txtNomEtsOriginal.Text + "',EmailEts='" + txtEmailEts_O.Text
                                                         + "',TelephoneEts=" + CheckIsNull(txtTelEtsO.Text) + ",OriginaireEts='" + chbIsOriginaire.Checked
                                                         + "',AdresseEts='" + txtAdresseEts_O.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                         + "' WHERE EtudiantsId='" + txtmatricule.Text + "'";
                            MyObjConnection.Execute(Query);

                        }

                    }
                    else
                    {
                        if (IsEtsOriginaireNo_Null())
                        {
                            Query = "INSERT INTO F_EtablissementsOrigine(NomEts,TelephoneEts,EmailEts,AdresseEts,OriginaireEts,EtudiantsId)Values('" + txtNomEtsOriginal.Text.Replace("'", "''").Replace(@"\", @"\\") + "'," + CheckIsNull(txtTelEtsO.Text) + ",'" + txtEmailEts_O.Text +
                                                                                                                                           "','" + txtAdresseEts_O.Text.Replace("'", "''").Replace(@"\", @"\\") + "','" + chbIsOriginaire.Checked + "','" + txtmatricule.Text + "')";
                            MyObjConnection.Execute(Query);
                        }
                    }
                    //*************************************************************************************************************************************************************
                    MessageBox.Show("Modification effectuée avec Succès!", "Modification-Etudiants", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    if (!Dossier.Exists) Dossier.Create();
                    //***********************************************************************Creation de l'etudiant*******************************************************************
                    Query = "INSERT INTO F_Etudiants(Matricule,NomE,PrenomE,DOBE,GenderE,CNI,TelephoneE,TelephoneMobileE,EmailE,AdresseE,VilleE,NiveauScolaireE,RemarqueE,ProfilPro) VALUES('" + txtmatricule.Text + "','" + txtnom.Text
                        + "','" + txtprenom.Text + "','" + DOB.Value.ToString("yyyy-MM-dd") +"','" + Sex +  "','" + txtcin.Text + "'," + CheckIsNull(txtdomicile.Text) + "," + CheckIsNull(txtmobile.Text)
                        + ",'" + txtemail.Text + "','" + txtadresse.Text.Replace("'", "''").Replace(@"\", @"\\")
                        + "','" + rDListVille.Text + "','" + rDListeNiveau.Text + "','" + txtremarques.Text.Replace("'", "''").Replace(@"\", @"\\") + "','" + txtmatricule.Text + ".jpg')";
                    if(txtmatricule.Text.Equals("")){
                        Lbl1.Visible=true;

                        label1.Visible = true;
                        label2.Visible = true;
                        label3.Visible = true;
                        label4.Visible = true;
                        label5.Visible = true;
                        label6.Visible = true;
                        return ;
                    }
                    
                    MyObjConnection.Execute(Query);
                   //*******************************************************************************************************************************************************************
                    //*********************************************************************************attachement du tuteur a l'etudiant***********************************************
                    if (IsTulaireNo_Null())
                    {
                        Query = "INSERT INTO F_Tuteurs(PrenomP,PrenomM,NomP,NomM,TelP,TelM,EmailP,EmailM,ProfessionP,ProfessionM,EtudiantsId)Values('" + txtPprenom.Text + "','" + txtMprenom.Text +"','" + txtPnom.Text + 
                                                                                                                                                 "','" + txtMnom.Text + "'," + CheckIsNull(txtmobileP.Text) + "," + CheckIsNull(txtMmobile.Text) + 
                                                                                                                                                 ",'" + txtemailP.Text + "','" + txtMemail.Text + "','"+ txtProfession.Text+
                                                                                                                                                 "','"+txtMprofession.Text +"','"+txtmatricule.Text +"')";
                        MyObjConnection.Execute(Query);                        
                    }
                    //******************************************************************************************************************************************************************
                    //***************************************************Attachement d'un dossier medicale a l'etudiant*****************************************************************
                    if (IsMedicationNo_Null())
                    {
                        Query = "INSERT INTO F_Medications(NomeMedecin,GSM,Telephone,Observation,EtudiantId)Values('" + txtnomMed.Text + "','" + txtgsm.Text + "'," + CheckIsNull(txtmobileHos.Text) +
                                                                                                                                       ",'" + txtObservationMed.Text.Replace("'", "''").Replace(@"\", @"\\") +
                                                                                                                                      "','" + txtmatricule.Text + "')";
                        MyObjConnection.Execute(Query);
                    }
                    //*******************************************************************************************************************************************************************
                    //***************************************************Attachement de la provenance de l'etudiant*****************************************************************
                    if (IsEtsOriginaireNo_Null())
                    {
                        Query = "INSERT INTO F_EtablissementsOrigine(NomEts,TelephoneEts,EmailEts,AdresseEts,OriginaireEts,EtudiantsId)Values('" + txtNomEtsOriginal.Text.Replace("'", "''").Replace(@"\", @"\\") + "'," + CheckIsNull(txtTelEtsO.Text) + ",'" + txtEmailEts_O.Text +
                                                                                                                                       "','" + txtAdresseEts_O.Text.Replace("'", "''").Replace(@"\", @"\\") +"','"+chbIsOriginaire.Checked +"','" + txtmatricule.Text + "')";
                        MyObjConnection.Execute(Query);
                    }
                    //*******************************************************************************************************************************************************************


                    MessageBox.Show("Enregistrement effectuée avec Succès!", "Enregistrement-Etudiants", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (PorvenanceImage == "Repertoire")
                {

                    String path = Dossier.FullName.ToString();
                    pictImage.Image.Save(path + @"\" + txtmatricule.Text + ".jpg", ImageFormat.Jpeg);
                }
                ChargementData_Edutiants();
                CleanTexts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur creation de la classe ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private Boolean IsTulaireNo_Null() 
        {
            Boolean IsValue=false;
            if (txtPnom.Text !="" || txtPnom.Text!="" ||
                                     txtMnom.Text!="" || txtMprenom.Text !="")
            {
                IsValue = true;
            }

            return IsValue;         
        }

        private Boolean IsMedicationNo_Null()
        {
            Boolean IsValue = false;
            if (txtnomMed.Text != "" || txtgsm.Text != "" ||
                                     txtmobileHos.Text != "" || txtObservationMed.Text != "")
            {
                IsValue = true;
            }

            return IsValue;
        }
        private Boolean IsEtsOriginaireNo_Null()
        {
         
            Boolean IsValue = false;
            if (txtNomEtsOriginal.Text != "" || txtEmailEts_O.Text != "" ||
                                     txtTelEtsO.Text != "" || txtAdresseEts_O.Text != "")
            {
                IsValue = true;
            }
            else if (chbIsOriginaire.Checked)
            {
                IsValue = true;
            }

            return IsValue;
        }

        public static bool isValidEmail (string inputEmail) {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + 
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex (strRegex);
            if (re.IsMatch (inputEmail))
                return (true);
            else
                return (false);
            }
        void ChargementData_Edutiants()
        {
            try
            {
                int cpt = 0;
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                {
                    MyObjConnection.Con.Open();
                }         
                SqlDataReader dataReader;

                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Etudiants ").ExecuteReader();
                if (DGVEtudiants.Rows.Count > 0)
                {
                    DGVEtudiants.Rows.Clear();
                }
                while (dataReader.Read())
                {
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
                    if (!dataReader["infosLibre"].ToString().Equals(""))
                    {
                        DGVEtudiants.Rows[cpt].Cells["NomE"].Style.ForeColor = System.Drawing.Color.Brown;
                        DGVEtudiants.Rows[cpt].Cells["NomE"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        DGVEtudiants.Rows[cpt].Cells["NomE"].Style.BackColor = System.Drawing.Color.Yellow;

                        DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.BackColor = System.Drawing.Color.Yellow ;
                        DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.ForeColor = System.Drawing.Color.Brown;
                        DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        DGVEtudiants.Rows[cpt].Cells["Matricule"].Style.BackColor = System.Drawing.Color.Gainsboro;
                        lbl_change_classe.Enabled = false;
                        lbl_Abandonner.Enabled = false;
                    }
                    else
                    {
                        lbl_change_classe.Enabled = true ;
                        lbl_Abandonner.Enabled = true;
                    }
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur ChargementData_Etudiants", MessageBoxButtons.OK);
            }

        }

        String Sex;
        private void rbtnMale_Click(object sender, EventArgs e)
        {
            Sex = "Masculin";
        }

        private void rbtnFemale_Click(object sender, EventArgs e)
        {
            Sex = "Feminin";
        }

        private void CheckSex(string Sexvalue)
        {
            if (Sexvalue.Equals("Feminin"))
            {
                rbtnFemale.IsChecked = true;
                Sex = "Feminin";
            }
            else if (Sexvalue.Equals("Masculin"))
            {
                rbtnMale.IsChecked = true;
                Sex = "Masculin";
            }
            else
            {
                rbtnMale.IsChecked = false;
                rbtnFemale.IsChecked = false;
            }
        }
        private void CleanTexts()
        {

            txtmatricule.Text = "";
            txtnom.Text = "";
            txtprenom.Text = "";
            DOB.Value = DateTime.Now;

            txtcin.Text = "";
            txtdomicile.Text = "";
            txtmobile.Text = "";
            txtemail.Text = "";
            txtadresse.Text = "";
            rDListVille.Text = "";
            rDListeNiveau.Text = "";       
            txtremarques.Text = "";
            pictImage.Image = ALCHANYSCHOOL.Properties.Resources.student_icon_512;
            CheckSex("");
            DGVdocuments.Rows.Clear();

            txtPprenom.Text= ""; 
            txtMprenom.Text = ""; 
            txtPnom.Text = "";
             txtMnom.Text= "";  
            txtmobileP.Text= "";  
            txtMmobile.Text = "";
            txtemailP.Text = ""; 
            txtMemail.Text = ""; 
            txtProfession.Text = ""; 
            txtMprofession.Text = "";

            txtnomMed.Text = "";
            txtgsm.Text = "";
            txtmobileHos.Text = "";
            txtObservationMed.Text = "";

            txtNomEtsOriginal.Text = "";
            txtEmailEts_O.Text = "";
            txtTelEtsO.Text = "";
            txtAdresseEts_O.Text = "";
            chbIsOriginaire.Checked = false;
            rGpB_1.Text = "Fiche Etudiant :";
            lblClasse.Text = "...";
            lbl_Classe_Id.Text = "...";
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (DGVdocuments.Rows.Count > 0)
                {
                    var Chemin = DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value;
                    if (Chemin != null)
                    {
                        Process.Start(Chemin.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur ouverture du documenets", MessageBoxButtons.OK);
            }    
        }
        int RowIndexSelected;
        private void DGVdocuments_CurrentRowChanged(object sender, Telerik.WinControls.UI.CurrentRowChangedEventArgs e)
        {
            
        }

        private void DGVdocuments_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            
            try {
                var Chemin = DGVdocuments.Rows[e.RowIndex].Cells["OpenUrl"].Value;
                if (Chemin != null) {
                    DGVdocuments.Columns["OpenUrl"].HeaderText = "Mes Documents => [{" + Path.GetFileName (DGVdocuments.Rows[e.RowIndex].Cells["OpenUrl"].Value.ToString ()) + "}]";
                    DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Style.Font = new System.Drawing.Font ("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    }
                } catch (Exception ex) {
                
                }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DGVdocuments.Rows.AddNew();
        }

        private void btnSuppression_Click(object sender, EventArgs e)
        {
            
            try {
            if (RowIndexSelected >= 0) {
                if (CheckExistingData (DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value.ToString ())) {
                    if (MessageBox.Show ("Voulez-vous reelement supprime ce document?", "Suppression de document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        MyObjConnection.Execute ("DELETE FROM F_Documents WHERE lienDoc='" + DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value.ToString () + "' AND LINKTO='ETUDIANTS'");
                        DGVdocuments.Columns["OpenUrl"].HeaderText = "Mes Documents";
                        DGVdocuments.Rows[RowIndexSelected].Delete ();
                        MessageBox.Show ("Suppression  effectuée avec Succès!", "Supprimer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    } else {
                    DGVdocuments.Rows[RowIndexSelected].Delete ();
                    }
                }

                } catch (Exception ex) {
                              
                }
        }
        Boolean CheckExistingData(String DataCheck)
        {
            Boolean status = false;
            try
            {
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT lienDoc FROM F_Documents WHERE lienDoc='" + DataCheck + "' AND LINKTO='ETUDIANTS'").ExecuteReader();
                while (dataReader.Read())
                {
                    status = true;
                }
                dataReader.Close();
                return status;
            }
            catch (Exception)
            {

                return status;
            }

        }

        private void btnSaveDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                String CheminFichier = "";
                String NomFichier = "";
                String Query = "";
                int LineEnregistre = 0, LineNonEnregistrer = 0;
                for (int i = 0; i <= DGVdocuments.Rows.Count - 1; i++)
                {
                    var Chemin = DGVdocuments.Rows[i].Cells["OpenUrl"].Value;
                    if (Chemin != null)
                    {
                        if (!CheckExistingData(DGVdocuments.Rows[i].Cells["OpenUrl"].Value.ToString()))
                        {
                            LineEnregistre += 1;
                            CheminFichier = DGVdocuments.Rows[i].Cells["OpenUrl"].Value.ToString();
                            NomFichier = Path.GetFileName(CheminFichier);
                            Query = "INSERT INTO F_Documents(IntituleDoc,lienDoc,linkTo)Values('" + txtmatricule.Text + "-" + NomFichier + "','" + CheminFichier + "','ETUDIANTS')";
                            MyObjConnection.Execute(Query);
                        }
                        else
                        {
                            LineNonEnregistrer += 1;
                        }
                    }

                }

                MessageBox.Show(string.Format("{0}", LineEnregistre) + " Line(s) Enregistrement effectuée avec Succès! \n " + string.Format("{0}", LineNonEnregistrer) + " Duplication(s).", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur de gestion dans se formulaire", MessageBoxButtons.OK);
            }
        }
        public String PorvenanceImage;
        private void RbOpenUrl_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Fichiers Image (*.JPG)|*.JPG|(*.PNG)|*.PNG";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                pictImage.ImageLocation = FileName;
                PorvenanceImage = "Repertoire";
            }
        }

        private void RbUpWebam_Click(object sender, EventArgs e)
        {
            F_MyCamera f_ex = new F_MyCamera(txtmatricule.Text, "F_ETUDIANTS");
            f_ex.ShowDialog();
            pictImage.ImageLocation = f_ex.s;
            PorvenanceImage = "Webcam";
        }

        private void linklblClear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CleanTexts();
            btnSave.Text = "&Enregistrer";
        }

        private void BtnSupprimer_Click(object sender, EventArgs e)
        {

        }

        private void Btnfermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_Etudiants_Load(object sender, EventArgs e)
        {
            //F_Parents fp = new F_Parents();
            //fp.HidingElement(false);
            ChargementData_Edutiants();
            ChargementData_Classes();
            ControleExerciceActif("");
        }
        void ControleExerciceActif(String Infos) {

            if (Settings.Default["IsCurrentExercice"].Equals("False") || Infos!="")
            {
                btnSave.Enabled = false;
                lbl_Pre_Inscription.Enabled = false;
                lbl_change_classe.Enabled = false;
                lbl_Transferer_Classe.Enabled = false;
                lbl_Abandonner.Enabled = false;
                lbl_CreationAbs.Enabled = false;
                BtnAddFile.Enabled = false;
                btnSuppressionDoc.Enabled = false;
                btnSaveDocuments.Enabled = false;
                RbUpWebam.Enabled = false;
                RbOpenUrl.Enabled = false;
            }
            else
            {
                if (Infos.Equals(""))
                {
                    /**/ btnSave.Enabled = true ;
                   if( DGV_EtudiantInscript.RowCount>0)
                         lbl_Pre_Inscription.Enabled = false;
                    else
                       lbl_Pre_Inscription.Enabled = true;
                lbl_change_classe.Enabled = true;
                lbl_Transferer_Classe.Enabled = true;
                lbl_Abandonner.Enabled = true;
                lbl_CreationAbs.Enabled = true;
                BtnAddFile.Enabled = true;
                btnSuppressionDoc.Enabled = true;
                btnSaveDocuments.Enabled = true;
                RbUpWebam.Enabled = true;
                RbOpenUrl.Enabled = true;
                }
               
            }
        }
        void ChargementData_Classes()
        {
            try
            {
                rDListeNiveau.Items.Clear();
                radDListClasse.Items.Clear();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Classes ").ExecuteReader();               
                while (dataReader.Read())
                {
                    rDListeNiveau.Items.Add(dataReader["Intitule"].ToString());
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
            try
            {
                int cpt = e.RowIndex;

                String path = Dossier.FullName.ToString();
                if (cpt>=0) {

                    txtmatricule.Text = DGVEtudiants.Rows[cpt].Cells["Matricule"].Value.ToString ();
                    txtnom.Text = DGVEtudiants.Rows[cpt].Cells["NomE"].Value.ToString ();
                    txtprenom.Text = DGVEtudiants.Rows[cpt].Cells["Prenome"].Value.ToString ();
                    DOB.Text = DGVEtudiants.Rows[cpt].Cells["DOBE"].Value.ToString ();

                    txtcin.Text = DGVEtudiants.Rows[cpt].Cells["CNI"].Value.ToString ();
                    txtdomicile.Text = DGVEtudiants.Rows[cpt].Cells["TelephoneE"].Value.ToString ();
                    txtmobile.Text = DGVEtudiants.Rows[cpt].Cells["TelephoneMobileE"].Value.ToString ();
                    txtemail.Text = DGVEtudiants.Rows[cpt].Cells["EmailE"].Value.ToString ();
                    txtadresse.Text = DGVEtudiants.Rows[cpt].Cells["AdresseE"].Value.ToString ();
                    rDListVille.Text = DGVEtudiants.Rows[cpt].Cells["VilleE"].Value.ToString ();

                    if (DGVEtudiants.Rows[cpt].Cells["NiveauScolaireE"].Value.ToString ()!="") {
                        rDListeNiveau.Text = DGVEtudiants.Rows[cpt].Cells["NiveauScolaireE"].Value.ToString ();
                        }
                    txtremarques.Text = DGVEtudiants.Rows[cpt].Cells["RemarqueE"].Value.ToString ();
                    if (!DGVEtudiants.Rows[cpt].Cells["infosLibre"].Value.ToString ().Equals ("")) {
                        lbl_change_classe.Enabled = false;
                        lbl_Abandonner.Enabled = false;
                        lbl_CreationAbs.Enabled = false;
                        lbl_Transferer_Classe.Enabled = false;
                        } else {
                        lbl_change_classe.Enabled = true;
                        lbl_Abandonner.Enabled = true;
                        lbl_CreationAbs.Enabled = true;
                        lbl_Transferer_Classe.Enabled = true;
                        }
                    if (File.Exists (path + @"\" + DGVEtudiants.Rows[cpt].Cells["ProfilPro"].Value.ToString ())) {
                        pictImage.ImageLocation = path + @"\" + DGVEtudiants.Rows[cpt].Cells["ProfilPro"].Value.ToString ();
                        } else {
                        pictImage.Image  = ALCHANYSCHOOL.Properties.Resources.images;
                        }

                    CheckSex (DGVEtudiants.Rows[cpt].Cells["GenderE"].Value.ToString ());
                    btnSave.Text = "&Modifier";
                    DGVdocuments.Rows.Clear ();
                    if (DGVdocuments.Rows.Count>0) {
                        DGVdocuments.Columns["OpenUrl"].HeaderText = "";
                        }
                    ChargementData_Documents (txtmatricule.Text);
                    TuteurSelected (txtmatricule.Text);
                    MedicationSelected (txtmatricule.Text);
                    EtsOrigineSelected (txtmatricule.Text);
                    AbsenceSelected (txtmatricule.Text);
                    rGpB_1.Text = "Fiche Etudiant : " + txtmatricule.Text;
                    DGVdocuments.Columns["OpenUrl"].HeaderText = "";
                    ControleExerciceActif (DGVEtudiants.Rows[cpt].Cells["infosLibre"].Value.ToString ());
                    }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur de selection sur eleve", MessageBoxButtons.OK);
            }
        }
        public String getIdClasse(String intituleClasse){

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
        void AbsenceSelected(String Etudiant_Id)
        {
            try
            {
                DGVLIstAbs.Rows.Clear();
                DGV_EtudiantInscript.Rows.Clear();
                SqlDataReader dataReader;
                Boolean IsData = false;
                int cpt;

                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Inscriptions Fi INNER JOIN F_Classes FC ON FI.CLASSEID=FC.CODE AND FI.ETUDIANTID='" + Etudiant_Id + "' AND FI.EXERCICE='" + Settings.Default["Exercice"] + "'");
                while (dataReader.Read())
                {
                    DGV_EtudiantInscript.Rows.AddNew();
                    cpt = DGV_EtudiantInscript.RowCount - 1;

                    DGV_EtudiantInscript.Rows[cpt].Cells["DateInscription"].Value =DateTime.Parse ( dataReader["DateInscription"].ToString()).ToShortDateString();
                    DGV_EtudiantInscript.Rows[cpt].Cells["Classe"].Value = dataReader["Intitule"].ToString();
                    switch (dataReader["Statut"].ToString())
                    {
                        case "1":
                            DGV_EtudiantInscript.Rows[cpt].Cells["Statut"].Value = "Pre-Inscrit";
                            break;
                        case "2":
                            DGV_EtudiantInscript.Rows[cpt].Cells["Statut"].Value = "Inscrit";
                            break;
                        case "3":
                            DGV_EtudiantInscript.Rows[cpt].Cells["Statut"].Value = "1ere Tranche Solde";
                            break;
                        case "4":
                            DGV_EtudiantInscript.Rows[cpt].Cells["Statut"].Value = "Pension Solde";
                            break;
                        default:
                            break;
                    }
                    lbl_Classe_Id.Text = dataReader["ClasseId"].ToString();
                    lblClasse.Text = dataReader["Intitule"].ToString();
                    lbl_Pre_Inscription.Enabled = false;
                    IsData = true;
                    ChargementData_Absence(txtmatricule.Text, lbl_Classe_Id.Text);
                }
                dataReader.Close();
                if (IsData == false)
                {
                    lbl_Classe_Id.Text = "...";
                    lblClasse.Text = "...";
                    lbl_Pre_Inscription.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur dans la fonction de selection des absences", MessageBoxButtons.OK);

            }
        }
        void TuteurSelected(String Etudiant_Id) {

            try
            {
                Boolean IsData = false;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Tuteurs where EtudiantsId='" + Etudiant_Id + "'");
                while (dataReader.Read())
                {
                    txtPprenom.Text = dataReader["PrenomP"].ToString();
                    txtMprenom.Text = dataReader["PrenomM"].ToString();
                    txtPnom.Text = dataReader["NomP"].ToString();
                    txtMnom.Text = dataReader["NomM"].ToString();
                    txtmobileP.Text = dataReader["TelP"].ToString();
                    txtMmobile.Text = dataReader["TelM"].ToString();
                    txtemailP.Text = dataReader["EmailP"].ToString();
                    txtMemail.Text = dataReader["EmailM"].ToString();
                    txtProfession.Text = dataReader["ProfessionP"].ToString();
                    txtMprofession.Text = dataReader["ProfessionM"].ToString();
                    IsData = true;
                }
                dataReader.Close();
                if (IsData==false )
                {
                    txtPprenom.Text = "";
                    txtMprenom.Text = "";
                    txtPnom.Text = "";
                    txtMnom.Text = "";
                    txtmobileP.Text = "";
                    txtMmobile.Text = "";
                    txtemailP.Text = "";
                    txtMemail.Text = "";
                    txtProfession.Text = "";
                    txtMprofession.Text = "";
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "erreur Tuteur selected affectation", MessageBoxButtons.OK);  
               
            } 
        }
        void MedicationSelected(String Etudiant_Id)
        {

            try
            {
                SqlDataReader dataReader;
                Boolean IsData = false;
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Medications where EtudiantId='" + Etudiant_Id + "'");
                while (dataReader.Read())
                {
                    txtnomMed.Text = dataReader["NomeMedecin"].ToString();
                    txtgsm.Text = dataReader["GSM"].ToString();
                    txtmobileHos.Text = dataReader["Telephone"].ToString();
                    txtObservationMed.Text = dataReader["Observation"].ToString();
                    IsData = true;
                }
                dataReader.Close();
                if (IsData == false)
                {
                    txtnomMed.Text = "";
                    txtgsm.Text = "";
                    txtmobileHos.Text = "";
                    txtObservationMed.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur medication selected affectation", MessageBoxButtons.OK);

            }


        }
        void EtsOrigineSelected(String Etudiant_Id)
        {

            try
            {
                SqlDataReader dataReader;
                Boolean IsData = false;
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_EtablissementsOrigine where EtudiantsId='" + Etudiant_Id + "'");
                while (dataReader.Read())
                {
                    txtNomEtsOriginal.Text = dataReader["NomEts"].ToString();
                    txtEmailEts_O.Text = dataReader["EmailEts"].ToString();
                    txtTelEtsO.Text = dataReader["TelephoneEts"].ToString();
                    txtAdresseEts_O.Text = dataReader["AdresseEts"].ToString();                  
                    if (dataReader["OriginaireEts"].ToString().Equals("True"))
                    {
                        chbIsOriginaire.Checked = true;
                    }
                    else
                    {
                        chbIsOriginaire.Checked = false; 
                    }
                    IsData = true;
                }
                dataReader.Close();
                if (IsData == false)
                {
                    txtNomEtsOriginal.Text = "";
                    txtEmailEts_O.Text = "";
                    txtTelEtsO.Text = "";
                    txtAdresseEts_O.Text = "";                                       
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur medication selected affectation", MessageBoxButtons.OK);

            }


        }
        void ChargementData_Documents(String Matricule_Prof)
        {
            try
            {
                int cpt = 0;
                String NomFichier;
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Documents WHERE LINKTO='ETUDIANTS' ").ExecuteReader();

                while (dataReader.Read())
                {
                    NomFichier = Path.GetFileName(dataReader["lienDoc"].ToString());
                    if (dataReader["IntituleDoc"].ToString().Equals(Matricule_Prof + "-" + NomFichier))
                    {
                        DGVdocuments.Rows.AddNew();
                        cpt = DGVdocuments.RowCount - 1;
                        DGVdocuments.Rows[cpt].Cells["OpenUrl"].Value = dataReader["lienDoc"].ToString();
                    }

                }
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur ChargementData_Documents", MessageBoxButtons.OK);
            }
        }
        private void rbtnMale_Click_1(object sender, EventArgs e)
        {
            Sex = "Masculin";
        }

        private void rbtnFemale_Click_1(object sender, EventArgs e)
        {
            Sex = "Feminin";
        }

        private void chbIsOriginaire_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (chbIsOriginaire.Checked)
            {
                txtNomEtsOriginal.Enabled = false;
                txtEmailEts_O.Enabled = false;
                txtTelEtsO.Enabled = false;
                txtAdresseEts_O.Enabled = false;
                txtNomEtsOriginal.Text = "";
                txtEmailEts_O.Text = "";
                txtTelEtsO.Text = "";
                txtAdresseEts_O.Text = "";
            }
            else
            {
                txtNomEtsOriginal.Enabled = true;
                txtEmailEts_O.Enabled = true;
                txtTelEtsO.Enabled = true;
                txtAdresseEts_O.Enabled = true;
            }
        }
  
        private void lbl_CreationAbs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = String.Empty;
                String Message="";
                //***************************************************Traitement des absence de l'etudiant*****************************************************************
                if (ComboxTypeAbs.Text != "" && txtmatricule.Text !="")
                {
                    if (AbsTimeEnd.Value.Value.TimeOfDay >= AbsTimeStart.Value.Value.TimeOfDay)
                    {
                        if (lbl_CreationAbs.Text == "Creation d'absence")
                        {
                        Query = "INSERT INTO F_Absences(ClasseId,DateAbs,HeureDebut,HeureFin,ModifsAbs,TypeAbs,EtudiandId,Exercice)Values('" + lbl_Classe_Id.Text + "','" + DateTimeAbs.Value.ToString ("yyyy-MM-dd") +
                          "','" + AbsTimeStart.Value.Value.TimeOfDay + "','" + AbsTimeEnd.Value.Value.TimeOfDay + "','" + txtModifAbs.Text.Replace("'", "''").Replace(@"\", @"\\") +
                          "','" + ComboxTypeAbs.Text + "','" + txtmatricule.Text + "','"+Settings.Default["Exercice"]+"')";
                            Message = "Enregistrement Absence de l'etudiant " + txtnom.Text;
                        }
                        else
                        {
                            Query = "UPDATE F_Absences SET HeureDebut='" + AbsTimeStart.Value.Value.TimeOfDay + "',HeureFin='" + AbsTimeEnd.Value.Value.TimeOfDay 
                                + "',ModifsAbs='" + txtModifAbs.Text.Replace("'", "''").Replace(@"\", @"\\") +
                                "',TypeAbs='" + ComboxTypeAbs.Text + "'  WHERE EtudiandId='" + txtmatricule.Text +
                                "' AND DateAbs='" + DateTimeAbs.Value.ToString("yyyy-MM-dd") + "' AND ClasseId='" + lbl_Classe_Id.Text + "'";
                            Message = "Modification de l'absence de l'etudiant " + txtnom.Text;
                        }
                      
                        MyObjConnection.Execute(Query);
                        ChargementData_Absence(txtmatricule.Text, lbl_Classe_Id.Text);
                        IntialeValueAbsence();                       
                        MessageBox.Show(Message  + " effectué avec Succès!", "Enregistrement Absence", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Imposible d'effectuer ce traitement \n l'heure de debut[{ " + AbsTimeStart.Value.Value.TimeOfDay + " }] ne doit pas etre superieur à l'heure fin[{ " + AbsTimeEnd.Value.Value.TimeOfDay + " }].", "Verification Heure d'absence.", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    }
                }
                else {

                    MessageBox.Show("Imposible d'effectuer ce traitement \n veillez selectioner le type d'absence et l'etudiant SVP.", "Verification Type d'absence.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                }
                //*******************************************************************************************************************************************************************
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur ChargementData_Absence", MessageBoxButtons.OK, MessageBoxIcon.Error);             
            }
        }
        void IntialeValueAbsence() {
            txtModifAbs.Text = "";
            ComboxTypeAbs.Text = "";
            AbsTimeEnd.Value = DateTime.Now;
            AbsTimeStart.Value = DateTime.Now;
            DateTimeAbs.Value = DateTime.Now;
            lbl_CreationAbs.Text = "Creation d'absence";
        }
        void ChargementData_Absence(String Matricule_Id,String Classe_Id)
        {
            try
            {
                DGVLIstAbs.Rows.Clear();
                int cpt = 0;                        
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Absences WHERE ClasseId='" + Classe_Id + "' AND  EtudiandId='" + Matricule_Id + "' AND Exercice='" + Settings.Default["Exercice"] + "'").ExecuteReader();
                while (dataReader.Read())
                {
                    DGVLIstAbs.Rows.AddNew();
                    cpt = DGVLIstAbs.RowCount - 1;
                    var Heure_Absence = DateTime.Parse(dataReader["HeureFin"].ToString()) - DateTime.Parse(dataReader["HeureDebut"].ToString());
                    var DateAbsence = DateTime.Parse(dataReader["DateAbs"].ToString()).ToShortDateString();
                    DGVLIstAbs.Rows[cpt].Cells["DateAbsence"].Value = DateAbsence;
                    DGVLIstAbs.Rows[cpt].Cells["HeureD"].Value = dataReader["HeureDebut"].ToString();
                    DGVLIstAbs.Rows[cpt].Cells["HeureF"].Value = dataReader["HeureFin"].ToString();
                    DGVLIstAbs.Rows[cpt].Cells["TypeA"].Value = dataReader["TypeAbs"].ToString();
                    DGVLIstAbs.Rows[cpt].Cells["Modif"].Value = dataReader["ModifsAbs"].ToString();
                    DGVLIstAbs.Rows[cpt].Cells["NbreHAbs"].Value = Heure_Absence.Hours + " H:" + Heure_Absence.Minutes+" Mm";
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur ChargementData_Absence", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DGVLIstAbs_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
            if (e.RowIndex>=0) {
                        lbl_CreationAbs.Text = "Modification d'absence";
                        DateTimeAbs.Value = DateTime.Parse (DGVLIstAbs.Rows[e.RowIndex].Cells["DateAbsence"].Value.ToString ());
                        AbsTimeStart.Value = DateTime.Parse (DGVLIstAbs.Rows[e.RowIndex].Cells["HeureD"].Value.ToString ());
                        AbsTimeEnd.Value = DateTime.Parse (DGVLIstAbs.Rows[e.RowIndex].Cells["HeureF"].Value.ToString ());
                        ComboxTypeAbs.Text = DGVLIstAbs.Rows[e.RowIndex].Cells["TypeA"].Value.ToString ();
                        txtModifAbs.Text = DGVLIstAbs.Rows[e.RowIndex].Cells["Modif"].Value.ToString ();               
                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur ChargementData_Absence", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
        String Classe_Id;
        private void radDListClasse_TextChanged(object sender, EventArgs e)
        {
            Classe_Id = getIdClasse(radDListClasse.SelectedItem.Text);
            lbl_classeId.Text = " ID : "+Classe_Id;
        }

        private void lbl_Abandonner_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = String.Empty;
                if (DGV_EtudiantInscript.Rows.Count>0)
                {
                    Query = "UPDATE F_Etudiants SET infosLibre='Abandonner' WHERE Matricule='" + txtmatricule.Text + "'";
                    if (MessageBox.Show("Etes-vous vraiment sure de cet action d'abandon sur cet eleve?","Operation d'abandon",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                          MyObjConnection.Execute(Query);
                          ChargementData_Edutiants();
                    }             
                }
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show(ex.Message, "erreur lors de l'operation d'abandon", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbl_change_classe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = String.Empty;
                if (DGV_EtudiantInscript.Rows.Count > 0  && Classe_Id != "")
                {
                    Query = "UPDATE F_Inscriptions SET ClasseId='" + Classe_Id + "' WHERE EtudiantId='" + txtmatricule.Text + "'";
                    if (MessageBox.Show("Etes-vous vraiment sure de cette operation?", "Operation de changement de classe", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MyObjConnection.Execute(Query);
                        ChargementData_Edutiants();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur ChargementData_Absence", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbl_Pre_Inscription_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = String.Empty;
                if (txtmatricule.Text != "" && txtnom.Text != "" && Classe_Id != "")//fonction de test d'existance a implemente.
                {
                    Query = "INSERT INTO F_Inscriptions(DateInscription,Statut,ClasseId,EtudiantId,Exercice)Values('" + DateTime.Now.ToString("yyyy-MM-dd") + "',1,'" + Classe_Id + "','" + txtmatricule.Text + "','" + Settings.Default["Exercice"]  + "')";
                    if (MessageBox.Show("Etes-vous vraiment sure de cette operation de Pre-Inscription?", "Operation de Pre-Inscription dans une classe", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MyObjConnection.Execute(Query);
                        ChargementData_Edutiants();
                    }                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Operation de Pre-Inscription dans une classe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lbl_Transferer_Classe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {
                String Query = String.Empty;
                if (txtmatricule.Text != "" && txtnom.Text != "" && Classe_Id != "")//fonction de test d'existance a implemente.
                {
                    Query = "INSERT INTO F_Inscriptions(DateInscription,Statut,ClasseId,EtudiantId,Exercice)Values('" + DateTime.Now.ToString("yyyy-MM-dd") + "',1,'" + Classe_Id + "','" + txtmatricule.Text + "','"+ Settings.Default["Exercice"] +"')";
                    if (MessageBox.Show("Souhaitez vous vraiment transferer cet etudiant?", "Operation de transfer dans une classe", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MyObjConnection.Execute(Query);
                        ChargementData_Edutiants();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Operation de Pre-Inscription dans une classe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DGVdocuments_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            RowIndexSelected = e.RowIndex;
            if (DGVdocuments.Rows.Count > 0)
            {
                var Chemin = DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value;
                if (Chemin != null)
                {
                    DGVdocuments.Columns["OpenUrl"].HeaderText = "Mes Documents => [{" + Path.GetFileName(DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value.ToString()) + "}]";
                    DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }     
        }
    }

}
