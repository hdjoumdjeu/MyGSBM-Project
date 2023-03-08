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
namespace ALCHANYSCHOOL
{
    public partial class F_Exercice1 : Telerik.WinControls.UI.RadForm
    {
        Connection MyObjConnection;
        public F_Exercice1()
        {
            InitializeComponent();
            MyObjConnection = new Connection(Settings.Default["ServerName"].ToString(), Settings.Default["DataBase"].ToString(), Settings.Default["UserName"].ToString(), Settings.Default["KEY"].ToString());

        }

        private void btnFermer_Click(object sender, EventArgs e)
            {
            this.Close();
            }

        Boolean PeriodeOuverte;
        Boolean CheckExistingData(String DateDebut,String Datefin)
            {
            Boolean status = false;
            try
                {
                     PeriodeOuverte = false;
                    if (MyObjConnection.Con.State == ConnectionState.Closed)
                        MyObjConnection.Con.Open();
                    SqlDataReader dataReader;

                    dataReader = MyObjConnection.GetCommand("SELECT CONVERT(varchar(4),DateDebut,23) AS DateDebut,CONVERT(varchar(4),Datefin,23) AS Datefin,Estatus  FROM F_EXERCICES").ExecuteReader();
                    while (dataReader.Read())
                    {
                    String Estatus = dataReader["Estatus"].ToString();
                    int YearStartLue = Convert.ToInt16(dataReader["DateDebut"]);
                    int YearEndLue = Convert.ToInt16(dataReader["Datefin"]);

                    int YearStartArg = Convert.ToInt16(Convert.ToDateTime(DateDebut).ToString("yyyy"));
                    int YearEndArg = Convert.ToInt16(Convert.ToDateTime(Datefin).ToString("yyyy"));

                    if (YearStartLue == YearStartArg && YearEndLue== YearEndArg)
                        {
                                   status = true;
                        }
                    if (Estatus == "True")
                        {
                            PeriodeOuverte = true;
                        }
                    }
                    dataReader.Close();
                    return status;
                }
            catch (Exception)
                {

                return status;
                }

            }
        private void btncreer_Click(object sender, EventArgs e)
            {
            try
                {
                        string Query = "";
                        int YearStart = Convert.ToInt16(DateDebut.Value.ToString("yyyy"));
                        int YearEnd   = Convert.ToInt16(DateFin.Value.ToString("yyyy")); 
                    
                        if (YearStart>YearEnd)
                        {
                            GpboxExercice.Text = "Periode Scolaire : Inacceptable.";
                            MessageBox.Show(GpboxExercice.Text, "Controle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if ((YearEnd>YearStart) && (YearEnd-YearStart)==1)
                        {
                            GpboxExercice.Text = "Periode Scolaire : Acceptable [" + DateDebut.Value.ToString("yyyy") + " - " + DateFin.Value.ToString("yyyy") + "]";
                            Query = "INSERT INTO F_EXERCICES(Estatus,DateDebut,Datefin) VALUES('" + chkexercice.Checked + "','" + DateDebut.Value.ToString("yyyy-MM-dd") + "','" + DateFin.Value.ToString("yyyy-MM-dd") + "')";

                            if (!CheckExistingData(DateDebut.Value.ToString("yyyy-MM-dd"), DateFin.Value.ToString("yyyy-MM-dd")))
                                {
                                if (!PeriodeOuverte)
                                    {
                                          MyObjConnection.Execute(Query);
                                          MessageBox.Show("Enregistrement effectuée avec Succès!", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                          ChargementData_Exercices ();
                                    }
                                else
                                    {
                                          MessageBox.Show("Une periode scolaire est ouverte !", "Controle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }                                    
                                }
                            else
                                {
                                    GpboxExercice.Text = " Cette Periode Scolaire : [" + DateDebut.Value.ToString("yyyy") + " - " + DateFin.Value.ToString("yyyy") + "] existe déjà.";
                                    MessageBox.Show(GpboxExercice.Text, "Controle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }                     
                        }
                    else
                        {
                            GpboxExercice.Text = "Periode Scolaire : Inacceptable.";
                            MessageBox.Show(GpboxExercice.Text, "Controle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                 }
            catch (Exception ex)
                {

                        MessageBox.Show(ex.Message, "erreur creation de l'exercice", MessageBoxButtons.OK);
                }
            }

        void ChargementData_Exercices()
            {
            try
                {
                int cpt = 0;
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand("SELECT * FROM F_EXERCICES ").ExecuteReader();
                if (  DGVExercice.Rows.Count>0) {
                       DGVExercice.Rows.Clear ();
                    }
                    while (dataReader.Read())
                        {
                        DGVExercice.Rows.AddNew();
                        cpt = DGVExercice.RowCount - 1;
                        DGVExercice.Rows[cpt].Cells["Code"].Value = dataReader["Code"].ToString();
                        DGVExercice.Rows[cpt].Cells["DebutExercice"].Value = Convert.ToDateTime(dataReader["DateDebut"].ToString()).ToString("yyyy");
                        DGVExercice.Rows[cpt].Cells["FinExercice"].Value = Convert.ToDateTime(dataReader["DateFin"].ToString()).ToString("yyyy");
                        DGVExercice.Rows[cpt].Cells["Statut"].Value = dataReader["Estatus"].ToString();
                        }
                dataReader.Close();
                }
            catch (Exception ex)
                {

                MessageBox.Show(ex.Message, "erreur ChargementData_Documents", MessageBoxButtons.OK);
                }

            }

        private void F_Exercice1_Load(object sender, EventArgs e)
            {
                ChargementData_Exercices();
            }

        private void btnModifier_Click (object sender, EventArgs e) {
            
             if (!txtcodeExercice.Text.Equals (""))
                {
                      String Query = "UPDATE F_EXERCICES SET Estatus='False'";
                      MyObjConnection.Execute (Query);
                      Query = "UPDATE F_EXERCICES SET Estatus='" + chkexercice.Checked + "' WHERE Code=" + txtcodeExercice.Text;
                      MyObjConnection.Execute (Query);
                      MessageBox.Show ("Action de la periode scolaire effectuée avec Succès!", "Cloration de la periode d'exercice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      ChargementData_Exercices ();
                }
                else
                {
                      MessageBox.Show ("Veillez a selectionner l'exercice a cloture SVP!", "Controle de fermeture d'exercice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        private void DGVExercice_CellClick (object sender, Telerik.WinControls.UI.GridViewCellEventArgs e) {
                try {
                        if (e.RowIndex>=0)
                            txtcodeExercice.Text = DGVExercice.Rows[e.RowIndex].Cells["Code"].Value.ToString ();
                    } catch (Exception ex) {
                         MessageBox.Show (ex.Message, "erreur .....", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
    }
}
