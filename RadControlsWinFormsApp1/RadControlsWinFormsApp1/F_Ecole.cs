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
using System.Diagnostics;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
namespace ALCHANYSCHOOL
{
    public partial class F_Ecole : Telerik.WinControls.UI.RadForm
    {
        Connection MyObjConnection;
        public F_Ecole()
        {
            InitializeComponent();
            String EndExercice = Settings.Default["Exercice"].ToString().Split('-')[1].ToString();
            String StartExercice = Settings.Default["Exercice"].ToString().Split('-')[0].ToString();
            DateTime DateMax =new DateTime(int.Parse(EndExercice),01,31);
            DateTime DateMin =new DateTime (int.Parse (StartExercice), 01, 01);
            DateDepense.MaxDate = DateMax;
            DateDepense.MinDate = DateMin;
            MyObjConnection = new Connection(Settings.Default["ServerName"].ToString(), Settings.Default["DataBase"].ToString(), Settings.Default["UserName"].ToString(), Settings.Default["KEY"].ToString());       
        }

        private void F_Ecole_Load(object sender, EventArgs e)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT* FROM F_Ecole").ExecuteReader();                        
                while (dataReader.Read())
                {
                    txtnom.Text = dataReader["SchoolName"].ToString();
                    txtnomAbrege.Text = dataReader["NomAbrege"].ToString();
                    txtresponsable.Text = dataReader["Responsable"].ToString();
                    txttel1.Text = dataReader["Telephone1"].ToString();
                    txttel2.Text = dataReader["Telephone2"].ToString();
                    txtmobile.Text = dataReader["TelephoneMobile"].ToString();
                    txtfax.Text = dataReader["FaxSchool"].ToString();
                    txtemail.Text = dataReader["Email"].ToString();
                    txtautorisation.Text = dataReader["NoAutorisation"].ToString();
                    txtadresse.Text = dataReader["Adresse"].ToString();
                    DropDownListVille.Text = dataReader["ville"].ToString();
                    txtInfosLegales.Text = dataReader["InfosLega"].ToString();
                    DateDelivree.Value = Convert.ToDateTime(dataReader["DateCreation"]);
                    Settings.Default["CurrentID"] = dataReader["IdSchool"].ToString();
                    btnEnregistre.Text = "Modifier";
                }
                dataReader.Close();
                GrpFiche.Text = "Fiche de l'école : " + txtnomAbrege.Text.ToUpper();
                if (!Settings.Default["LOGO"].Equals(""))
                {
                    PicLogo.Image = Image.FromFile(Settings.Default["LOGO"].ToString());
                }
                if (!Settings.Default["ENTETEPAGE"].Equals("") || !Settings.Default["PIEDPAGE"].Equals(""))
                {
                    PicEntete.BackgroundImage = Image.FromFile(Settings.Default["ENTETEPAGE"].ToString());
                    PicPied.BackgroundImage = Image.FromFile(Settings.Default["PIEDPAGE"].ToString());
                }
                if (Settings.Default["StatusImpression"].Equals("YES"))
                    CheckStatusImpression.Checked = true;
                else
                    CheckStatusImpression.Checked = false;
                ChargementData_Documents();
                ChargementData_DiscompteP();
                ChargementData_PensionParametre();
                ChargementData_ModePaiement();
                ChargementData_FonctionSalaire();
                ComboxPersonnel();
                ChargementData_Depense();
				ChargementData_Vehicule ();
				ChargementData_Supplement ();
            }
            catch (Exception ex)
            {

                //throw new Exception(ex.Message);
                MessageBox.Show (ex.Message, "erreur de chargement fiche de l'ecole", MessageBoxButtons.OK);
            }
        }
        public void ComboxPersonnel() 
        {
            try
            {
                 int index = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Professeurs ").ExecuteReader();
                while (dataReader.Read())
                {
                    CombPersonnels.MultiColumnComboBoxElement.Rows.AddNew();
                    index = CombPersonnels.MultiColumnComboBoxElement.Rows.Count - 1;
                    CombPersonnels.MultiColumnComboBoxElement.Rows[index].Cells["MatriculeP"].Value = dataReader["MatriculeP"].ToString();
                    CombPersonnels.MultiColumnComboBoxElement.Rows[index].Cells["NomP"].Value = dataReader["NomP"].ToString();

                    CombPersonnels.MultiColumnComboBoxElement.Rows[index].Cells["Telephone1"].Value = dataReader["Telephone1"].ToString();
                    CombPersonnels.MultiColumnComboBoxElement.Rows[index].Cells["PrenomP"].Value = dataReader["PrenomP"].ToString();
                    CombPersonnels.MultiColumnComboBoxElement.Rows[index].Cells["CNI"].Value = dataReader["CNI"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur de chargement du personnels", MessageBoxButtons.OK);
            }     
        }
        private void btnEnregistre_Click(object sender, EventArgs e)
        {
            try
            {
                string Query = "";               
                if (btnEnregistre.Text.Equals("Enregistrer"))
                {
                    Query = "INSERT INTO F_Ecole(SchoolName,NomAbrege,Responsable,Telephone1,Telephone2,TelephoneMobile,FaxSchool,Email,NoAutorisation,Adresse,ville,InfosLega,DateCreation) VALUES('" + txtnom.Text.Replace("'", "''").Replace(@"\", @"\\")
                   + "','" + txtnomAbrege.Text.Trim() + "','" + txtresponsable.Text + "'," + CheckIsNull(txttel1.Text)
                   + "," + CheckIsNull(txttel2.Text) + "," + CheckIsNull(txtmobile.Text)
                   + "," + CheckIsNull(txtfax.Text) + ",'" + txtemail.Text + "','" + txtautorisation.Text + "','" + txtadresse.Text.Replace("'", "''").Replace(@"\", @"\\")
                   + "','" + DropDownListVille.Text + "','" + txtInfosLegales.Text.Replace("'", "''").Replace(@"\", @"\\") + "','" + Convert.ToDateTime(DateDelivree.Value) + "')";
                    MyObjConnection.Execute(Query);
                    MessageBox.Show("Enregistrement effectuée avec Succès!", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Query = "UPDATE F_Ecole SET SchoolName='" + txtnom.Text.Replace("'", "''").Replace(@"\", @"\\") + "',NomAbrege='" + txtnomAbrege.Text
                        + "',Responsable='" + txtresponsable.Text + "',Telephone1=" + CheckIsNull(txttel1.Text) + ",Telephone2=" + CheckIsNull(txttel2.Text)
                        + ",TelephoneMobile=" + CheckIsNull(txtmobile.Text) + ",FaxSchool=" + CheckIsNull(txtfax.Text)
                        + ",Email='" + txtemail.Text + "',NoAutorisation='" + txtautorisation.Text + "',Adresse='" + txtadresse.Text.Replace("'", "''").Replace(@"\", @"\\")
                        + "',ville='" + DropDownListVille.Text + "',InfosLega='" + txtInfosLegales.Text.Replace("'", "''").Replace(@"\", @"\\") + "',DateCreation='" + Convert.ToDateTime(DateDelivree.Value) + "' WHERE IdSchool=" + Settings.Default["CurrentID"];
                   MyObjConnection.Execute(Query);
                    MessageBox.Show("Modification effectuée avec Succès!", "Modification",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur de gestion dans se formulaire", MessageBoxButtons.OK);
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

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void radPageViewPage4_Enter(object sender, EventArgs e)
        {
            btnEnregistre.Enabled=false ;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DGVdocuments.Rows.AddNew();
        }

        private void btnSaveDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                String CheminFichier = "";
                String NomFichier = "";
                String Query = "";
                int LineEnregistre=0, LineNonEnregistrer = 0;
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
                            Query = "INSERT INTO F_Documents(IntituleDoc,lienDoc,linkTo)Values('" + NomFichier + "','" + CheminFichier + "','ECOLE')";
                            MyObjConnection.Execute(Query);
                        }
                        else
                        {
                            LineNonEnregistrer += 1;
                        }                   
                    }
                                                                 
                }

                MessageBox.Show(string.Format("{0}", LineEnregistre) + " Line(s) Enregistrement effectuée avec Succès! \n " + string.Format("{0}", LineNonEnregistrer) +" Duplication(s).", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show(ex.Message, "erreur de gestion dans se formulaire", MessageBoxButtons.OK);
            }
        }
            
        private void radPageViewPage2_Enter(object sender, EventArgs e)
        {
            btnEnregistre.Enabled = true;
        }
        int RowIndexSelected;
        private void radButton3_Click(object sender, EventArgs e)
        {
            if (RowIndexSelected>=0)
            {
                if (CheckExistingData(DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value.ToString()))
                {
                    if (MessageBox.Show("Voulez-vous reelement supprime ce document?", "Suppression de document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        MyObjConnection.Execute("DELETE FROM F_Documents WHERE lienDoc='" + DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value.ToString() + "'");
                        DGVdocuments.Columns["OpenUrl"].HeaderText = "Mes Documents";
                        DGVdocuments.Rows[RowIndexSelected].Delete();
                        MessageBox.Show("Suppression  effectuée avec Succès!", "Supprimer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                        DGVdocuments.Rows[RowIndexSelected].Delete();
                }             
            }        
        }
        Boolean CheckExistingData(String DataCheck) {
            Boolean status = false;
            try
            {
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT lienDoc FROM F_Documents WHERE lienDoc='" + DataCheck + "'").ExecuteReader();
                while (dataReader.Read())
                {
                   status= true;
                }
                dataReader.Close();
                return status;                
            }
            catch (Exception)
            {
                return status; 
            }           
        }

        void ChargementData_Documents()
        {
            try
            {
                int cpt=0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT lienDoc FROM F_Documents WHERE linkTo='ECOLE' ").ExecuteReader();              
                while (dataReader.Read())
                {
                    DGVdocuments.Rows.AddNew();
                    cpt = DGVdocuments.RowCount - 1;
                    DGVdocuments.Rows[cpt].Cells["OpenUrl"].Value = dataReader["lienDoc"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur ChargementData_Documents", MessageBoxButtons.OK);
            }
        }
        private void DGVdocuments_CurrentRowChanged(object sender, Telerik.WinControls.UI.CurrentRowChangedEventArgs e)
        {         
            RowIndexSelected = e.CurrentRow.Index;
            if (DGVdocuments.Rows.Count > 0)
            {
                var Chemin = DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value;
                if (Chemin!=null)
                {
                    DGVdocuments.Columns["OpenUrl"].HeaderText = "Mes Documents => [{" + Path.GetFileName(DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Value.ToString()) + "}]";
                    DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }        
        }

        private void DGVdocuments_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            var Chemin = DGVdocuments.Rows[e.RowIndex].Cells["OpenUrl"].Value;
            if (Chemin != null)
            {
                DGVdocuments.Columns["OpenUrl"].HeaderText = "Mes Documents => [{" + Path.GetFileName(DGVdocuments.Rows[e.RowIndex].Cells["OpenUrl"].Value.ToString()) + "}]";
                DGVdocuments.Rows[RowIndexSelected].Cells["OpenUrl"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular , System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
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
                        Process.Start(Chemin.ToString ());
                  }
                }
              }
            catch (Exception ex)
              {           
                  MessageBox.Show(ex.Message, "erreur ouverture du documenets", MessageBoxButtons.OK);
              }    
        }

        private void txtnomAbrege_TextChanged(object sender, EventArgs e)
        {
           GrpFiche.Text = "Fiche de l'école : " + txtnomAbrege.Text.ToUpper() ;
        }

        private void btnExercice_Click(object sender, EventArgs e)
            {
                F_Exercice1 f_ex=new F_Exercice1() ;
                f_ex.ShowDialog();
            }
        private void DpListGenreTuteurs_TextChanged(object sender, EventArgs e)
        {
            if (DpListGenreTuteurs.Text  !="")
            {
                 ChargementTuteur(DpListGenreTuteurs.SelectedItem.Text);
            }       
        }   
        void  ChargementTuteur(String Gendre)
        {
            try
            {
                SqlDataReader dataReader;
                if ( rDListTuteurs.Items.Count>0)
                {
                    rDListTuteurs.Items.Clear();
                    lbl_eleve_nom.Text = "...";
                    lbl_Classe_Id.Text = "...";
                }              
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Tuteurs");
                while (dataReader.Read())
                {       
                     if (Gendre.Equals("Mère"))
                     {
                         rDListTuteurs.Items.Add(dataReader["NomM"].ToString());
                     }
                     else
                     {
                         rDListTuteurs.Items.Add(dataReader["NomP"].ToString());
                     }                   
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Chargement Parents",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        public String getIdEleve_Parent(String NomParent)
        {
            try
            {
                SqlDataReader dataReader;
                String Query = String.Empty;
                if (DpListGenreTuteurs.SelectedItem.Text.Equals("Mère"))
                {
                    Query = "SELECT * FROM F_Tuteurs WHERE NomM='" + NomParent + "'";
                }
                else
                {
                    Query = "SELECT * FROM F_Tuteurs WHERE NomP='" + NomParent + "'";
                }
                dataReader = MyObjConnection.GetReader(Query);
                while (dataReader.Read())
                {
                    return dataReader["EtudiantsId"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return "";
        }
        public String getIdParent_Eleve(String Matricule)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Tuteurs WHERE EtudiantsId='" + Matricule + "'");
                while (dataReader.Read())
                {               
                    if (!dataReader["NomP"].ToString().Equals(""))
                    {
                        Gender = "Père";
                        return dataReader["NomP"].ToString();
                    }
                    else if (!dataReader["NomM"].ToString().Equals(""))
                    {
                        Gender = "Mère";
                        return dataReader["NomM"].ToString();
                    }
                    else
                    {
                        return String.Empty;
                    }                   
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return "";
        }
        String MatriculeEleve;
        private void rDListTuteurs_TextChanged(object sender, EventArgs e)
        {
            if (rDListTuteurs.Items.Count>0)
            {
                   MatriculeEleve=getIdEleve_Parent(rDListTuteurs.SelectedItem.Text);
                   lbl_eleve_nom.Text =getIdEleve_Nom(MatriculeEleve);
                   lbl_Classe_Id.Text = getIdEleve_ClasseInscrit(MatriculeEleve);
            }
            
        }
        public String getIdEleve_Nom(String Matricule)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Etudiants WHERE Matricule='" + Matricule + "'");
                while (dataReader.Read())
                {
                    return dataReader["NomE"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            return "";
        }
        public String getIdEleve_ClasseInscrit(String Matricule)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM F_Inscriptions Fi INNER JOIN F_Classes FC ON FI.CLASSEID=FC.CODE AND FI.ETUDIANTID='" + Matricule + "'");
                while (dataReader.Read())
                {
                    return dataReader["Intitule"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return "";
        }

        private void lbl_discoAP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (nuDiscompte.Value > 0 )
                {
                    if (MatriculeEleve != "" &&  MatriculeEleve != null)
                    {
                        String Query = "UPDATE F_Etudiants SET Dsicompte=" + nuDiscompte.Value + " WHERE Matricule='" + MatriculeEleve + "'";
                       MyObjConnection.Execute(Query);
                        MessageBox.Show("Discompte appliquer avec Succès!", "Discompte ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Veullez Selectionner l'eleve sur lequel le discompte sera appliquer SVP", "Controle de discompte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }                 
                }
                else
                {
                    MessageBox.Show("Veullez a renseigner une valeur de discompte SVP", "Controle de discompte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                ChargementData_DiscompteP();
                DpListGenreTuteurs.Enabled = true ;
                rDListTuteurs.Enabled = true ;
            }
            catch (Exception ex)
            {
                
               MessageBox.Show(ex.Message,"Chargement Parents",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        String Gender;
        void ChargementData_DiscompteP()
        {
            try
            {
                int cpt = 0;
                if (MyObjConnection.Con.State == ConnectionState.Closed) {
                    MyObjConnection.Con.Open ();
                    } 
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Etudiants WHERE Dsicompte<>0").ExecuteReader();
                if (DGV_DISCOMPTE.Rows.Count > 0)
                    DGV_DISCOMPTE.Rows.Clear();
                while (dataReader.Read())
                {
                    DGV_DISCOMPTE.Rows.AddNew();
                    cpt = DGV_DISCOMPTE.RowCount - 1;
                    DGV_DISCOMPTE.Rows[cpt].Cells["Classe"].Value = getIdEleve_ClasseInscrit(dataReader["Matricule"].ToString());
                    DGV_DISCOMPTE.Rows[cpt].Cells["Discompte"].Value = dataReader["Dsicompte"].ToString()+" %";
                    DGV_DISCOMPTE.Rows[cpt].Cells["Parent"].Value =getIdParent_Eleve(dataReader["Matricule"].ToString());
                    DGV_DISCOMPTE.Rows[cpt].Cells["Eleve"].Value = dataReader["NomE"].ToString();
                    DGV_DISCOMPTE.Rows[cpt].Cells["Gender"].Value = Gender;
                    DGV_DISCOMPTE.Rows[cpt].Cells["Discompte"].Style.ForeColor = System.Drawing.Color.Brown;
                    DGV_DISCOMPTE.Rows[cpt].Cells["Discompte"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    DGV_DISCOMPTE.Rows[cpt].Cells["Discompte"].Style.BackColor = System.Drawing.Color.Yellow;
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Parametrage des discomptes", MessageBoxButtons.OK);
            }

        }

        private void MasterTemplate_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
            if (e.RowIndex>=0) {
                nuDiscompte.Value = int.Parse (DGV_DISCOMPTE.Rows[e.RowIndex].Cells["Discompte"].Value.ToString ().Replace (" %", ""));
                DpListGenreTuteurs.SelectedValue  = DGV_DISCOMPTE.Rows[e.RowIndex].Cells["Gender"].Value.ToString ();
                rDListTuteurs.SelectedValue = DGV_DISCOMPTE.Rows[e.RowIndex].Cells["Parent"].Value.ToString ();
                DpListGenreTuteurs.Enabled = false;
                rDListTuteurs.Enabled = false;
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur lors d'affectation des donnees", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbl_ApplicationCP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = "";
                if (lbl_ApplicationCP.Text.Equals("&Modifier"))
                {
                    Query = "UPDATE P_Pension SET Intitule='" + txtintitue.Text + "',Montant=" + txtmontant.Text + " WHERE CodeCategorie='" + txtcode.Text + "'";
                    MyObjConnection.Execute(Query);
                    MessageBox.Show("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (txtmontant.Text != "" && txtintitue.Text != "")
                    {
                        Query = "INSERT INTO P_Pension(CodeCategorie,Intitule,Montant) VALUES('" + txtcode.Text + "','" + txtintitue.Text + "'," + txtmontant.Text + ")";
                        MyObjConnection.Execute(Query);
                        MessageBox.Show("Enregistrement effectuée avec Succès!", "Enregistrement de la Classe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
               }
                ChargementData_PensionParametre();
                CleanTexts();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur creation de la classe ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void CleanTexts() {

            txtcode.Text = "";
            txtintitue.Text = "";
            txtmontant.Text = "0";
            lbl_ApplicationCP.Text = "Appliquer";
            txtcode.Enabled = true;
        }
        void ChargementData_PensionParametre()
        {
            try
            {
                int cpt = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM P_Pension order by CodeCategorie").ExecuteReader();
                if (DGV_PESIONP.Rows.Count > 0)
                    DGV_PESIONP.Rows.Clear();
                while (dataReader.Read())
                {
                    DGV_PESIONP.Rows.AddNew();
                    cpt = DGV_PESIONP.RowCount - 1;
                    DGV_PESIONP.Rows[cpt].Cells["CodeCategorie"].Value = dataReader["CodeCategorie"].ToString();
                    DGV_PESIONP.Rows[cpt].Cells["Intitule"].Value = dataReader["Intitule"].ToString();
                    DGV_PESIONP.Rows[cpt].Cells["Montant"].Value =double.Parse(dataReader["Montant"].ToString());                   
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Chargement des Parametrage de la pension", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ChargementData_ModePaiement()
        {
            try
            {
                int cpt = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM P_ModePaiement order by Id_ModePaie").ExecuteReader();
                if (DGV_MODEPAIEMENT.Rows.Count > 0)
                    DGV_MODEPAIEMENT.Rows.Clear();
                while (dataReader.Read())
                {
                    DGV_MODEPAIEMENT.Rows.AddNew();
                    cpt = DGV_MODEPAIEMENT.RowCount - 1;
                    DGV_MODEPAIEMENT.Rows[cpt].Cells["Code"].Value = dataReader["Id_ModePaie"].ToString();
                    DGV_MODEPAIEMENT.Rows[cpt].Cells["Libelle"].Value = dataReader["Libelle"].ToString();
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Chargement des Parametrage du mode paiement", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void MasterTemplate_CellClick_1(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
                  
           try {
                    if (e.RowIndex>=0) {
                             txtcode.Text = DGV_PESIONP.Rows[e.RowIndex].Cells["CodeCategorie"].Value.ToString ();
                             txtintitue.Text = DGV_PESIONP.Rows[e.RowIndex].Cells["Intitule"].Value.ToString ();
                             txtmontant.Text = DGV_PESIONP.Rows[e.RowIndex].Cells["Montant"].Value.ToString ();
                             txtcode.Enabled = false;
                             lbl_ApplicationCP.Text = "&Modifier";
                        }

               } catch (Exception ex) {
               
                              MessageBox.Show(ex.Message, "erreur .....", MessageBoxButtons.OK, MessageBoxIcon.Error);

               }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CleanTexts();
        }

        private void lbl_AnnulerDisco_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rDListTuteurs.Items.Clear();
            DpListGenreTuteurs.SelectedText="";
            lbl_eleve_nom.Text = "...";
            lbl_Classe_Id.Text = "...";

            DpListGenreTuteurs.Enabled = true ;
            rDListTuteurs.Enabled = true;
        }
        void CleanTextFC()
        {
            txtFonction.Text = "";
            txtSalaire.Text = "0";
            lblApplyFS.Text = "&Enregistre";
        }
        private void lblApplyFS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = "";
                if (lblApplyFS.Text.Equals("&Modifier"))
                {
                    Query = "UPDATE P_Fonction SET Fonction='" + txtFonction.Text + "',Salaire=" + txtSalaire.Text + " WHERE Code='" + txtCodeF.Text + "'";
                    MyObjConnection.Execute(Query);
                    MessageBox.Show("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (txtSalaire.Text != "" && txtFonction.Text != "")
                    {
                        Query = "INSERT INTO P_Fonction(Code,Fonction,Salaire) VALUES('" + txtCodeF.Text + "','" + txtFonction.Text + "'," + txtSalaire.Text + ")";
                        MyObjConnection.Execute(Query);
                        MessageBox.Show("Enregistrement effectuée avec Succès!", "Enregistrement de la Classe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                ChargementData_FonctionSalaire();
                if ( DGV_FONCSalaire.Rows.Count>=10)
                {
                    txtCodeF.Text = txtnomAbrege.Text.Trim() + "FC" + (DGV_FONCSalaire.Rows.Count+1).ToString();
                }
                else
                {
                    txtCodeF.Text = txtnomAbrege.Text.Trim() + "FC0" + (DGV_FONCSalaire.Rows.Count+1).ToString();
                }             
                CleanTextFC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur creation de la fonction/Salaire ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ChargementData_FonctionSalaire()
        {
            try
            {
                int cpt = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM P_Fonction order by Code").ExecuteReader();
                if (DGV_FONCSalaire.Rows.Count > 0)
                    DGV_FONCSalaire.Rows.Clear();
                while (dataReader.Read())
                {
                    DGV_FONCSalaire.Rows.AddNew();
                    cpt = DGV_FONCSalaire.RowCount - 1;
                    DGV_FONCSalaire.Rows[cpt].Cells["Code"].Value = dataReader["Code"].ToString();
                    DGV_FONCSalaire.Rows[cpt].Cells["Fonction"].Value = dataReader["Fonction"].ToString();
                    DGV_FONCSalaire.Rows[cpt].Cells["Salaire"].Value =double.Parse(dataReader["Salaire"].ToString());
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Chargement des fonction/Salaire", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DGV_FONCSalaire_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                if (DGV_FONCSalaire.Rows.Count > 0 && e.RowIndex>=0)
                {
                    txtCodeF.Text = DGV_FONCSalaire.Rows[e.RowIndex].Cells["Code"].Value.ToString();
                    txtFonction.Text = DGV_FONCSalaire.Rows[e.RowIndex].Cells["Fonction"].Value.ToString();
                    txtSalaire.Text = DGV_FONCSalaire.Rows[e.RowIndex].Cells["Salaire"].Value.ToString();
                    lblApplyFS.Text = "&Modifier";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Chargement des fonction/Salaire", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lblbAnnuler_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DGV_FONCSalaire.Rows.Count >= 10)
            {
                txtCodeF.Text = txtnomAbrege.Text.Trim() + "FC" + (DGV_FONCSalaire.Rows.Count + 1).ToString();
            }
            else
            {
                txtCodeF.Text = txtnomAbrege.Text.Trim() + "FC0" + (DGV_FONCSalaire.Rows.Count + 1).ToString();
            }
            CleanTextFC();
        }

        private void DGV_MODEPAIEMENT_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (DGV_MODEPAIEMENT.Rows.Count > 0 && e.RowIndex >= 0)
            {
                txtcodeMP.Text = DGV_MODEPAIEMENT.Rows[e.RowIndex].Cells["Code"].Value.ToString();
                txtLibelle.Text = DGV_MODEPAIEMENT.Rows[e.RowIndex].Cells["Libelle"].Value.ToString();
                linkLabel3.Text = "&Modifier";
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = "";
                if (linkLabel3.Text.Equals("&Modifier"))
                {
                    Query = "UPDATE P_ModePaiement SET Libelle='" + txtLibelle.Text.Replace("'", "''").Replace(@"\", @"\\") + "' WHERE Code='" + txtcodeMP.Text + "'";
                    MyObjConnection.Execute(Query);
                    MessageBox.Show("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (txtmontant.Text != "" && txtintitue.Text != "")
                    {
                        Query = "INSERT INTO P_ModePaiement(Code,Libelle) VALUES('" + txtcodeMP.Text + "','" + txtLibelle.Text.Replace("'", "''").Replace(@"\", @"\\") + "')";
                        MyObjConnection.Execute(Query);
                        MessageBox.Show("Enregistrement effectuée avec Succès!", "Enregistrement du Mode Paiement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                ChargementData_ModePaiement();
                txtcodeMP.Text="";
                txtLibelle.Text = "";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur creation de la classe ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtcodeMP.Text = "";
            txtLibelle.Text = "";
        }
        String CodeDepense;
        private void lblDepense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String Query = "";
                if (lblDepense.Text.Equals("&Modifier"))
                {
                    Query = "UPDATE F_Depense SET intitule='" + txtIntituleDep.Text.Replace("'", "''").Replace(@"\", @"\\") + "',details='" + txtdetailDepense.Text.Replace("'", "''").Replace(@"\", @"\\") + "',Montant=" + txtMontnDepenser.Text + ",DateDP='" + DateDepense.Value.ToString("yyyy-MM-dd") + "' WHERE Id_depense=" + CodeDepense;
                    MyObjConnection.Execute(Query);
                    MessageBox.Show("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (txtMontnDepenser.Text != "" && txtIntituleDep.Text != "" && txtdetailDepense.Text != "" && CombPersonnels.Text != "")
                    {
                        Query = "INSERT INTO F_Depense(intitule,details,MatriculeP,Montant,Exercice,DateDP) VALUES('" + txtIntituleDep.Text.Replace("'", "''").Replace(@"\", @"\\") + "','" + txtdetailDepense.Text.Replace("'", "''").Replace(@"\", @"\\") + "','" + CombPersonnels.Text + "'," + txtMontnDepenser.Text + ",'" + Settings.Default["Exercice"] + "','" + DateDepense.Value.ToString("yyyy-MM-dd") + "')";
                        MyObjConnection.Execute(Query);
                        MessageBox.Show("Enregistrement effectuée avec Succès!", "Enregistrement d'une depense", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                txtIntituleDep.Text = string.Empty;
                txtdetailDepense.Text = string.Empty;
                txtMontnDepenser.Text = "0";
                CombPersonnels.Text = string.Empty;
                CodeDepense = string.Empty;
                ChargementData_Depense();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur creation de la depense ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ChargementData_Depense()
        {
            try
            {
                int cpt = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Depense where Exercice='" + Settings.Default["Exercice"] + "'").ExecuteReader();
                if (DGV_DEPENSE.Rows.Count > 0)
                    DGV_DEPENSE.Rows.Clear();
                while (dataReader.Read())
                {
                    
                    DGV_DEPENSE.Rows.AddNew();
                    cpt = DGV_DEPENSE.RowCount - 1;
                    DGV_DEPENSE.Rows[cpt].Cells["Code"].Value = dataReader["Id_depense"].ToString();
                    DGV_DEPENSE.Rows[cpt].Cells["IntituleDepense"].Value = dataReader["intitule"].ToString();
                    DGV_DEPENSE.Rows[cpt].Cells["details"].Value = dataReader["details"].ToString();
                    DGV_DEPENSE.Rows[cpt].Cells["Personne"].Value =GetPersonnel(dataReader["MatriculeP"].ToString());
                    DGV_DEPENSE.Rows[cpt].Cells["DateDepense"].Value =DateTime.Parse( dataReader["DateDP"].ToString()).ToShortDateString();
                    DGV_DEPENSE.Rows[cpt].Cells["Matricule"].Value = dataReader["MatriculeP"].ToString();
                    DGV_DEPENSE.Rows[cpt].Cells["MontantDepense"].Value = double.Parse(dataReader["Montant"].ToString());
                }
                dataReader.Close();
                this.DGV_DEPENSE.Columns["MontantDepense"].PinPosition = PinnedColumnPosition.Right;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Chargement des depenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public string GetPersonnel(String Matricule)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT NomP FROM F_Professeurs where MatriculeP='" + Matricule + "'");
                while (dataReader.Read())
                {
                    return dataReader["NomP"].ToString();
                }
                return String.Empty ;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
        private void DGV_DEPENSE_CellClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (DGV_DEPENSE.Rows.Count > 0 && e.RowIndex >= 0)
                {
                    CodeDepense = DGV_DEPENSE.Rows[e.RowIndex].Cells["Code"].Value.ToString();
                    txtIntituleDep.Text = DGV_DEPENSE.Rows[e.RowIndex].Cells["IntituleDepense"].Value.ToString();
                    txtdetailDepense.Text = DGV_DEPENSE.Rows[e.RowIndex].Cells["details"].Value.ToString();
                    CombPersonnels.Text = DGV_DEPENSE.Rows[e.RowIndex].Cells["Personne"].Value.ToString();
                    DateDepense.Text = DGV_DEPENSE.Rows[e.RowIndex].Cells["DateDepense"].Value.ToString();
                    txtMontnDepenser.Text = DGV_DEPENSE.Rows[e.RowIndex].Cells["MontantDepense"].Value.ToString();
                    lblDepense.Text = "&Modifier";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "erreur Chargement des depenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtIntituleDep.Text = string.Empty;
            txtdetailDepense.Text = string.Empty;
            txtMontnDepenser.Text = "0";
            CombPersonnels.Text = string.Empty;
            CodeDepense = string.Empty;
        }

        private void GrpFiche_Click(object sender, EventArgs e)
        {

        }
		int VehiculeID;
		private void lbl_VehiculeSave_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
			{
			try
				{
				String Query = "";
				if (lbl_VehiculeSave.Text.Equals ("&Modifier"))
					{
					Query = "UPDATE F_VEHICULE SET Libelle='" + txtnomV.Text + "',killometrage=" + numKillometrage.Value + ",Model='" + txtmodel.Text + "' WHERE Id=" + VehiculeID;
					MyObjConnection.Execute (Query);
					MessageBox.Show ("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				else
					{
					if (txtnomV.Text != "" && txtmodel.Text != "" && numKillometrage.Value>=1)
						{
						Query = "INSERT INTO F_VEHICULE(Libelle,Model,killometrage) VALUES('" + txtnomV.Text + "','" + txtmodel.Text + "'," + numKillometrage.Value + ")";
						MyObjConnection.Execute (Query);
						MessageBox.Show ("Enregistrement effectuée avec Succès!", "Enregistrement ", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
						ChargementData_Vehicule ();
						txtnomV.Text = "";
						numKillometrage.Value = 0;
						txtmodel.Text = "";
						lbl_VehiculeSave.Text = "Enregistrer";
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur creation de la Transports", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		void ChargementData_Vehicule ()
			{
			try
				{
				int cpt = 0;
				SqlDataReader dataReader;
				dataReader = MyObjConnection.GetCommand ("SELECT * FROM F_VEHICULE ").ExecuteReader ();
				if (DGV_VEHICULE .Rows.Count > 0)
					DGV_VEHICULE.Rows.Clear ();
				while (dataReader.Read ())
					{
					DGV_VEHICULE.Rows.AddNew ();
					cpt = DGV_VEHICULE.RowCount - 1;
					DGV_VEHICULE.Rows[cpt].Cells["ID"].Value = dataReader["Id"].ToString ();
					DGV_VEHICULE.Rows[cpt].Cells["NomVehicule"].Value = dataReader["Libelle"].ToString ();
					DGV_VEHICULE.Rows[cpt].Cells["ModelVehicule"].Value = dataReader["Model"].ToString ();
					DGV_VEHICULE.Rows[cpt].Cells["MileVehicule"].Value = int.Parse (dataReader["killometrage"].ToString ());
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur Chargement des Transports", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}

		private void lbl_QuitterV_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
			{
					txtnomV.Text = "";
					numKillometrage.Value = 0;
					txtmodel.Text = "";
					lbl_VehiculeSave.Text = "Enregistrer";
			}

		private void MasterTemplate_CellClick_2 (object sender, GridViewCellEventArgs e)
			{
				try
					{
					if (DGV_VEHICULE.Rows.Count > 0 && e.RowIndex >= 0)
						{
							VehiculeID =int.Parse(DGV_VEHICULE.Rows[e.RowIndex].Cells["ID"].Value.ToString ());
							txtnomV .Text = DGV_VEHICULE.Rows[e.RowIndex].Cells["NomVehicule"].Value.ToString ();
							txtmodel .Text = DGV_VEHICULE.Rows[e.RowIndex].Cells["ModelVehicule"].Value.ToString ();
							numKillometrage.Value =int.Parse( DGV_VEHICULE.Rows[e.RowIndex].Cells["MileVehicule"].Value.ToString ());
							lbl_VehiculeSave.Text = "&Modifier";
						}
					}
				catch (Exception ex)
					{
						MessageBox.Show (ex.Message, "erreur Chargement des Transports", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
			}
		int SupplementID;
		private void lbl_SaveSup_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
			{
			try
				{
				String Query = "";
				if (lbl_SaveSup.Text.Equals ("&Modifier"))
					{
					Query = "UPDATE F_SUPPLEMENTS SET Libelle='" + txtlibelleSup.Text + "',Montant=" + txtmontantSup.Text  + " WHERE Id=" + SupplementID;
					MyObjConnection.Execute (Query);
					MessageBox.Show ("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				else
					{
					if (txtlibelleSup.Text != "" && txtmontantSup.Text != "")
						{
						Query = "INSERT INTO F_SUPPLEMENTS(Libelle,Montant) VALUES('" + txtlibelleSup.Text + "'," + txtmontantSup.Text + ")";
						MyObjConnection.Execute (Query);
						MessageBox.Show ("Enregistrement effectuée avec Succès!", "Enregistrement ", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				ChargementData_Supplement ();
				txtlibelleSup.Text = "";
				txtmontantSup.Text = "0";
				lbl_SaveSup.Text = "Enregistrer";
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur creation de la Supplements", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
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
					}
				dataReader.Close ();
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur Chargement des Supplements", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		private void MasterTemplate_CellClick_3 (object sender, GridViewCellEventArgs e)
			{
			try
				{
				if (DGV_SUPPLEMENT.Rows.Count > 0 && e.RowIndex >= 0)
					{
						SupplementID = int.Parse (DGV_SUPPLEMENT.Rows[e.RowIndex].Cells["ID"].Value.ToString ());
						txtlibelleSup.Text = DGV_SUPPLEMENT.Rows[e.RowIndex].Cells["LibelleSup"].Value.ToString ();
						txtmontantSup.Text = DGV_SUPPLEMENT.Rows[e.RowIndex].Cells["MontantSup"].Value.ToString ();
						lbl_SaveSup.Text = "&Modifier";
					}
				}
			catch (Exception ex)
				{
				MessageBox.Show (ex.Message, "erreur Chargement des Supplements", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

		private void lbl_FermerSup_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
			{
				txtlibelleSup.Text = "";
				txtmontantSup.Text = "0";
				lbl_SaveSup.Text = "Enregistrer";
			}

        private void txtMontnDepenser_KeyPress (object sender, KeyPressEventArgs e) {
            char ch = e.KeyChar;
            if (ch == 46 && txtMontnDepenser.Text.IndexOf (".") != -1) {
                e.Handled = true;
                return;
                }
            if (!char.IsDigit (ch) && ch != 8 && ch !=46) {
                e.Handled = true;
                }
            }

        private void txtSalaire_KeyPress (object sender, KeyPressEventArgs e) {
            char ch = e.KeyChar;
            if (ch == 46 && txtSalaire.Text.IndexOf (".") != -1) {
                e.Handled = true;
                return;
                }
            if (!char.IsDigit (ch) && ch != 8 && ch !=46) {
                e.Handled = true;
                }
            }

        private void txtmontant_KeyPress (object sender, KeyPressEventArgs e) {
            char ch = e.KeyChar;
            if (ch == 46 && txtmontant.Text.IndexOf (".") != -1) {
                e.Handled = true;
                return;
                }
            if (!char.IsDigit (ch) && ch != 8 && ch !=46) {
                e.Handled = true;
                }
            }

        private void txtmontantSup_KeyPress (object sender, KeyPressEventArgs e) {
            char ch = e.KeyChar;
            if (ch == 46 && txtmontantSup.Text.IndexOf (".") != -1) {
                e.Handled = true;
                return;
                }
            if (!char.IsDigit (ch) && ch != 8 && ch !=46) {
                e.Handled = true;
                }
            }
    }
}
