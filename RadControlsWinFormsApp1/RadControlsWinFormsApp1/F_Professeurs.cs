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
using System.Text.RegularExpressions;
namespace ALCHANYSCHOOL
{
    public partial class F_Professeurs : Telerik.WinControls.UI.RadForm
    {
        Connection MyObjConnection;
		

        public F_Professeurs()
        {
            InitializeComponent();
            MyObjConnection = new Connection (Settings.Default["ServerName"].ToString (), Settings.Default["DataBase"].ToString (), Settings.Default["UserName"].ToString (), Settings.Default["KEY"].ToString ());

        }

        private void RadForm_Professeurs_Load(object sender, EventArgs e)
        {
            //F_Parents fp= new F_Parents();
            //fp.HidingElement(false);
            ChargementData_Professeurs();
            ComboxUploaderFonction();
        }
        public void ComboxUploaderFonction()
        {
            try
            {
                int index = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM P_Fonction ").ExecuteReader();
                while (dataReader.Read())
                {
                    CombFonctions.MultiColumnComboBoxElement.Rows.AddNew();
                    index = CombFonctions.MultiColumnComboBoxElement.Rows.Count - 1;
                    CombFonctions.MultiColumnComboBoxElement.Rows[index].Cells["Code"].Value = dataReader["Code"].ToString();
                    CombFonctions.MultiColumnComboBoxElement.Rows[index].Cells["Fonction"].Value = dataReader["Fonction"].ToString();
                    CombFonctions.MultiColumnComboBoxElement.Rows[index].Cells["Montant"].Value =double.Parse( dataReader["Salaire"].ToString());    
                }
                dataReader.Close();
               // this.CombFonctions.MultiColumnComboBoxElement.Columns["Montant"].PinPosition = PinnedColumnPosition.Right;
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur de chargement des fonctions", MessageBoxButtons.OK);
            }
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
        public void ControleValiditer () {
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
            }
        private void btnSave_Click (object sender, EventArgs e)
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
                DirectoryInfo Dossier = new DirectoryInfo("PhotoProfesseurs");
                ControleValiditer ();
				if (btnSave.Text.Equals ("&Modifier"))
					{
                        Query = "UPDATE F_Professeurs SET NomP='" + txtnom.Text + "',PrenomP='" + txtprenom.Text
                                                          + "',DOBP='" + DatePicDOB.Value.ToString("yyyy-MM-dd") + "',CNI='" + txtCin.Text + "',CodeF='" + CombFonctions.Text
                                                          + "',Telephone1=" + CheckIsNull(txttelDomiciel.Text) + ",Telephone2=" + CheckIsNull(txtmobile.Text)
                                                          + ",Email='" + txtemail.Text + "',Adresse='" + txtadresse.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                          + "',VilleP='" + rDListVille.Text + "',Diplome='" + rDListDiplome.Text
                                                          + "',Status='" + rdListStatut.Text + "',Remarques='" + rTremarques.Text.Replace("'", "''").Replace(@"\", @"\\")
                                                          + "',ProfilPro='" + txtmatricule.Text + ".jpg',Sex='" + Sex
                                                          + "' WHERE MatriculeP='" + txtmatricule.Text + "'";
					MyObjConnection.Execute (Query);
					MessageBox.Show ("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				else
					{
                       
                     if (!Dossier.Exists)Dossier.Create();
					Query = "INSERT INTO F_Professeurs(MatriculeP,NomP,PrenomP,DOBP,CNI,Telephone1,Telephone2,Email,Adresse,VilleP,Diplome,Status,Remarques,ProfilPro,Sex,CodeF) VALUES('" + txtmatricule.Text + "','" + txtnom.Text
                        + "','" + txtprenom.Text + "','" + DatePicDOB.Value.ToString("yyyy-MM-dd") + "','" + txtCin.Text + "'," + CheckIsNull(txttelDomiciel.Text) + "," + CheckIsNull(txtmobile.Text)
                        + ",'" + txtemail.Text + "','" + txtadresse.Text.Replace("'", "''").Replace(@"\", @"\\")
                        + "','" + rDListVille.Text + "','" + rDListDiplome.Text + "','" + rdListStatut.Text + "','" + rTremarques.Text.Replace("'", "''").Replace(@"\", @"\\") + "','" + txtmatricule.Text + ".jpg','" + Sex + "','" + CombFonctions.Text + "')";
					MyObjConnection.Execute (Query);
					MessageBox.Show ("Enregistrement effectuée avec Succès!", "Enregistrement de la Classe", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
              
                if (PorvenanceImage == "Repertoire")
                {

                    String path = Dossier.FullName.ToString();
                    pictImage.Image.Save(path + @"\" + txtmatricule.Text + ".jpg", ImageFormat.Jpeg);
                }
				ChargementData_Professeurs();
				CleanTexts ();
				}
			catch (Exception ex)
				{

				MessageBox.Show (ex.Message, "erreur creation de la classe ", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
            }
        void ChargementData_Professeurs()
        {
            try
            {
                int cpt = 0;
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Professeurs ").ExecuteReader();
                if (DGVProfesseurs.Rows.Count > 0)
                {
                    DGVProfesseurs.Rows.Clear();
                }
                while (dataReader.Read())
                {
                    DGVProfesseurs.Rows.AddNew();
                    cpt = DGVProfesseurs.RowCount - 1;
                    DGVProfesseurs.Rows[cpt].Cells["MatriculeP"].Value = dataReader["MatriculeP"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["NomP"].Value = dataReader["NomP"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["PrenomP"].Value = dataReader["PrenomP"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["DOBP"].Value = dataReader["DOBP"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["CNI"].Value = dataReader["CNI"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Telephone1"].Value = dataReader["Telephone1"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Telephone2"].Value = dataReader["Telephone2"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Email"].Value = dataReader["Email"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Adresse"].Value = dataReader["Adresse"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["VilleP"].Value = dataReader["VilleP"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Diplome"].Value = dataReader["Diplome"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Status"].Value = dataReader["Status"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Remarques"].Value = dataReader["Remarques"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["ProfilPro"].Value = dataReader["ProfilPro"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["Sex"].Value = dataReader["Sex"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["CodeF"].Value = dataReader["CodeF"].ToString();
                    DGVProfesseurs.Rows[cpt].Cells["FonctionLue"].Value = GetFonction(dataReader["CodeF"].ToString());
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur ChargementData_Professeurs", MessageBoxButtons.OK);
            }

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
        public String PorvenanceImage;
		private void btnOpenUrl_Click (object sender, EventArgs e)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog ();
				openFileDialog.InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				openFileDialog.Filter = "Fichiers Image (*.JPG)|*.JPG|(*.PNG)|*.PNG";
				if (openFileDialog.ShowDialog (this) == DialogResult.OK)
					{
					    string FileName = openFileDialog.FileName;
					    pictImage.ImageLocation = FileName;
                        PorvenanceImage = "Repertoire";
					}
			}
        
        private void btnWecam_Click(object sender, EventArgs e)
        {
            F_MyCamera f_ex = new F_MyCamera(txtmatricule.Text, "F_PROFESSEURS");                    
            f_ex.ShowDialog();
            pictImage.ImageLocation = f_ex.s;
            PorvenanceImage = "Webcam";
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

        DirectoryInfo Dossier = new DirectoryInfo("PhotoProfesseurs");
        
        private void MasterTemplate_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {         
             try
             {
                 int cpt = e.RowIndex;
                 String path = Dossier.FullName.ToString();

                     txtmatricule.Text = DGVProfesseurs.Rows[cpt].Cells["MatriculeP"].Value.ToString();
                     txtnom.Text = DGVProfesseurs.Rows[cpt].Cells["NomP"].Value.ToString();
                     txtprenom.Text = DGVProfesseurs.Rows[cpt].Cells["PrenomP"].Value.ToString();
                     DatePicDOB.Text = DGVProfesseurs.Rows[cpt].Cells["DOBP"].Value.ToString();

                     txtCin.Text = DGVProfesseurs.Rows[cpt].Cells["CNI"].Value.ToString();
                     txttelDomiciel.Text = DGVProfesseurs.Rows[cpt].Cells["Telephone1"].Value.ToString();
                     txtmobile.Text = DGVProfesseurs.Rows[cpt].Cells["Telephone2"].Value.ToString();
                     txtemail.Text = DGVProfesseurs.Rows[cpt].Cells["Email"].Value.ToString();
                     txtadresse.Text = DGVProfesseurs.Rows[cpt].Cells["Adresse"].Value.ToString();
                     rDListVille.Text = DGVProfesseurs.Rows[cpt].Cells["VilleP"].Value.ToString();

                     rDListDiplome.Text = DGVProfesseurs.Rows[cpt].Cells["Diplome"].Value.ToString();
                     rdListStatut.Text = DGVProfesseurs.Rows[cpt].Cells["Status"].Value.ToString();
                     rTremarques.Text = DGVProfesseurs.Rows[cpt].Cells["Remarques"].Value.ToString();                   
                     if (File.Exists (path + @"\" + DGVProfesseurs.Rows[cpt].Cells["ProfilPro"].Value.ToString ())) {
                     pictImage.ImageLocation = path + @"\" + DGVProfesseurs.Rows[cpt].Cells["ProfilPro"].Value.ToString ();
                         } else {
                         pictImage.Image  = ALCHANYSCHOOL.Properties.Resources.Copy_of_stock_people;
                         }
                     CheckSex(DGVProfesseurs.Rows[cpt].Cells["Sex"].Value.ToString());
                     btnSave.Text = "&Modifier";
                     DGVdocuments.Rows.Clear();
                     DGVdocuments.Columns["OpenUrl"].HeaderText = "";
                     ChargementData_Documents(txtmatricule.Text);
                     txtFonctionLibelle.Text = DGVProfesseurs.Rows[cpt].Cells["FonctionLue"].Value.ToString();
                     CombFonctions.Text = DGVProfesseurs.Rows[cpt].Cells["CodeF"].Value.ToString();


             }
             catch (Exception ex)
             {
                 
                
             }
        }
        public string GetFonction(String CodeFonction)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT Fonction FROM P_Fonction where Code='" + CodeFonction + "'");
                while (dataReader.Read())
                {
                    return dataReader["Fonction"].ToString();
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
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
            else {
                rbtnMale.IsChecked = false;
                rbtnFemale.IsChecked = false;
            }
        
        }
        private void CleanTexts()
        {
        
            txtmatricule.Text = "";
            txtnom.Text = "";
            txtprenom.Text = "";
            DatePicDOB.Value = DateTime.Now;

            txtCin.Text = "";
            txttelDomiciel.Text = "";
            txtmobile.Text = "";
            txtemail.Text = "";
            txtadresse.Text = "";
            rDListVille.Text = "";

            rDListDiplome.Text = "";
            rdListStatut.Text = "";
            rTremarques.Text = "";
            pictImage.Image = ALCHANYSCHOOL.Properties.Resources.images;
            CheckSex("");
            DGVdocuments.Rows.Clear();
        }
        private void linklblClear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
              CleanTexts();
              btnSave.Text = "&Enregistrer";
            this.radPageViewPage3.Enabled = false;
        }

        private void radPageViewPage3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Experiance" ,"Ex", MessageBoxButtons.OK);
        }

        private void radPageViewPage4_Enter(object sender, EventArgs e)
        {
            MessageBox.Show("Infos Fincace", "Ex", MessageBoxButtons.OK);
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
                        MyObjConnection.Execute ("DELETE FROM F_Documents WHERE lienDoc='" + DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value.ToString () + "' AND LINKTO='PROFESSEURS'");
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

        void ChargementData_Documents(String Matricule_Prof)
        {
            try
            {
                int cpt = 0;
                String NomFichier;
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Documents WHERE LINKTO='PROFESSEURS' ").ExecuteReader();

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
        Boolean CheckExistingData(String DataCheck)
        {
            Boolean status = false;
            try
            {
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT lienDoc FROM F_Documents WHERE lienDoc='" + DataCheck + "' AND LINKTO='PROFESSEURS'").ExecuteReader();
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
                            Query = "INSERT INTO F_Documents(IntituleDoc,lienDoc,linkTo)Values('" +txtmatricule.Text +"-"+ NomFichier + "','" + CheminFichier + "','PROFESSEURS')";
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
        int RowIndexSelected;
        private void DGVdocuments_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            var Chemin = DGVdocuments.Rows[e.RowIndex].Cells["OpenUrl"].Value;
            if (Chemin != null)
            {
                DGVdocuments.Columns["OpenUrl"].HeaderText = "Mes Documents => [{" + Path.GetFileName(DGVdocuments.Rows[e.RowIndex].Cells["OpenUrl"].Value.ToString()) + "}]";
                DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            }
        }

        private void DGVdocuments_CurrentRowChanged(object sender, Telerik.WinControls.UI.CurrentRowChangedEventArgs e)
        {
            RowIndexSelected = e.CurrentRow.Index;
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

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSupppression_Click(object sender, EventArgs e)
        {
            
        }
    }
}
