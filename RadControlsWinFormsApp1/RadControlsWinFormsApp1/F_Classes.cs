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
namespace ALCHANYSCHOOL
{
    public partial class F_Classes : Telerik.WinControls.UI.RadForm
    {
        Connection MyObjConnection;
        public F_Classes()
        {
            InitializeComponent();
            MyObjConnection = new Connection (Settings.Default["ServerName"].ToString (), Settings.Default["DataBase"].ToString (), Settings.Default["UserName"].ToString (), Settings.Default["KEY"].ToString ());

        }

        private void btnSave_Click (object sender, EventArgs e)
            {
            try
                {
                String Query = "";
                if (btnSave.Text.Equals ("&Modifier"))
                    {
                        Query = "UPDATE F_Classes SET Section='" + rDListSection.Text + "',Intitule='" + txtintitule.Text + "',TypeFormation='" + rDlistTypeFormation.Text + "',ModeFormation='" + rDListModeFormation.Text + "',Capacite=" + numericUpDownCapacite.Value + ",Groupe='" + CombGroupeList.Text + "' WHERE Code='" + txtcode.Text + "'";
                    MyObjConnection.Execute (Query);
                    MessageBox.Show ("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                else
                    {

                        Query = "INSERT INTO F_Classes(Code,Section,TypeFormation,ModeFormation,Intitule,Capacite,Groupe) VALUES('" + txtcode.Text + "','" + rDListSection.Text + "','" + rDlistTypeFormation.Text + "','" + rDListModeFormation.Text + "','" + txtintitule.Text + "'," + numericUpDownCapacite.Value + ",'" + CombGroupeList.Text + "')";
                    MyObjConnection.Execute (Query);
                    MessageBox.Show ("Enregistrement effectuée avec Succès!", "Enregistrement de la Classe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                ChargementData_Classes ();
                CleanTexts();
                }
            catch (Exception ex)
                {

                MessageBox.Show (ex.Message, "erreur creation de la classe ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        void ChargementGroupePension()
        {
            try
            {
                CombGroupeList.Items.Clear();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM P_Pension");
                while (dataReader.Read())
                {
                    CombGroupeList.Items.Add(dataReader["CodeCategorie"].ToString());
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chargement Groupe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    public  String  IntitueGroupePension(String CodeGroupe)
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT * FROM P_Pension where CodeCategorie='" + CodeGroupe  + "'");
                while (dataReader.Read())
                {
                    return dataReader["Intitule"].ToString();
                }
                dataReader.Close();
                return "";
            }
            catch (Exception ex)
            {
                return "Error 301 "+ex;
            }
        }

        void ChargementData_Classes ()
            {
            try
                {
                int cpt = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_Classes order by Code").ExecuteReader();
                if (DGVClasse.Rows.Count > 0)
                        DGVClasse.Rows.Clear ();
                while (dataReader.Read ())
                    {
                        DGVClasse.Rows.AddNew ();
                        cpt = DGVClasse.RowCount - 1;
                        DGVClasse.Rows[cpt].Cells["CodeClasse"].Value = dataReader["Code"].ToString ();
                        DGVClasse.Rows[cpt].Cells["Intitule"].Value = dataReader["Intitule"].ToString ();
                        DGVClasse.Rows[cpt].Cells["TypeFr"].Value = dataReader["TypeFormation"].ToString ();
                        DGVClasse.Rows[cpt].Cells["modeFr"].Value = dataReader["ModeFormation"].ToString ();
                        DGVClasse.Rows[cpt].Cells["Section"].Value = dataReader["Section"].ToString ();
                        DGVClasse.Rows[cpt].Cells["Capacite"].Value = dataReader["Capacite"].ToString();
                        DGVClasse.Rows[cpt].Cells["Groupe"].Value = dataReader["Groupe"].ToString();

                    }
                        dataReader.Close ();
                }
            catch (Exception ex)
                {

                MessageBox.Show (ex.Message, "erreur Chargement des classes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        private void btnFermer_Click (object sender, EventArgs e)
            {
                this.Close ();
            }
        private void DGVClasse_CellClick (object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
            {
                try
                    {

                        int cpt = e.RowIndex;
                        var Capacite=1;
                        if (e.RowIndex>=0)
                        {
                            if (DGVClasse.Rows[e.RowIndex].Cells["Capacite"].Value.ToString() != "")
                            {
                                Capacite = int.Parse(DGVClasse.Rows[e.RowIndex].Cells["Capacite"].Value.ToString());
                            }
                            txtcode.Text = DGVClasse.Rows[e.RowIndex].Cells["CodeClasse"].Value.ToString();
                            txtintitule.Text = DGVClasse.Rows[e.RowIndex].Cells["Intitule"].Value.ToString();
                            rDlistTypeFormation.Text = DGVClasse.Rows[e.RowIndex].Cells["TypeFr"].Value.ToString();
                            rDListModeFormation.Text = DGVClasse.Rows[e.RowIndex].Cells["modeFr"].Value.ToString();
                            rDListSection.Text = DGVClasse.Rows[e.RowIndex].Cells["Section"].Value.ToString();
                            CombGroupeList.Text = DGVClasse.Rows[e.RowIndex].Cells["Groupe"].Value.ToString();
                            numericUpDownCapacite.Value = Capacite;
                            btnSave.Text = "&Modifier";
                        }
                        
                        
                    }
                catch (Exception ex)
                    {
                     MessageBox.Show (ex.Message, "erreur est survenu lors de cette action", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }

        private void btnSupprimer_Click (object sender, EventArgs e)
            {
                if (MessageBox.Show ("Voulez-vous reelement supprime cette classe?", "Suppression de classe", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MyObjConnection.Execute ("DELETE FROM F_Classes WHERE Code='" + txtcode.Text + "'");
                        MessageBox.Show ("Suppression  effectuée avec Succès!", "Supprimer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ChargementData_Classes ();
                    }
            }

        private void linklblClear_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
            {
                CleanTexts();
                btnSave.Text = "&Enregistrer";
            }
         private void CleanTexts()
         {
                 txtcode.Text = "";
                 txtintitule.Text = "";
                 rDListSection.Text = "";
                 rDlistTypeFormation.Text = "";
                 rDListModeFormation.Text = "";
                 numericUpDownCapacite.Value = 1;
                 CombGroupeList.Text = "";
                 lbl_intitule_Gp.Text = "...";
         }
        private void F_Classes_Load (object sender, EventArgs e)
            {
                 ChargementData_Classes ();
                 ChargementGroupePension();
            }

        private void DGVClasse_Click(object sender, EventArgs e)
        {
           
        }

        private void CombGroupeList_TextChanged(object sender, EventArgs e)
        {
            lbl_intitule_Gp.Text=IntitueGroupePension(CombGroupeList.Text);
        }
    }
}
