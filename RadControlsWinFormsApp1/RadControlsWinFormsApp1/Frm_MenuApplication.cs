using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ALCHANYSCHOOL.Properties;
using System.Data.SqlClient;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using Telerik.Charting;
using System.Drawing.Drawing2D;
using Telerik.WinControls;
using System.IO;
namespace ALCHANYSCHOOL
{
    public partial class Frm_MenuApplication : Form
    {
        //private string DirectoryIniFilePath = Application.StartupPath + "\\ConnectAPI.ini";
        Connection MyObjConnection;
        private DataModel data = new DataModel();
        private SmartLabelsController smartLabelsController;
        private int childFormNumber = 0;
        public static Frm_MenuApplication  fpr = null;
        public String PictureProfil;
        public Frm_MenuApplication()
        {
            InitializeComponent();
            MyObjConnection = new Connection(Settings.Default["ServerName"].ToString(), Settings.Default["DataBase"].ToString(), Settings.Default["UserName"].ToString(), Settings.Default["KEY"].ToString());
            lblUserConneter.Text = Settings.Default["UserConnecter"].ToString();
            this.radChartView1.LabelFormatting += radChartView1_LabelFormatting;
            // this.InitializePie();
            this.radChartView1.View.Margin = new Padding(20);
            this.radChartView1.Title = "EFFECTIFS INSCRIT PAR CLASSES";
            this.radChartView1.ChartElement.TitlePosition = TitlePosition.Top;
            this.radChartView1.ChartElement.TitleElement.TextAlignment = ContentAlignment.MiddleCenter;
            this.smartLabelsController = new SmartLabelsController();
            this.radChartView1.Controllers.Add(this.smartLabelsController);
            //--------------------------------------
            this.radChartView2.LabelFormatting += radChartView1_LabelFormatting;
            this.radChartView2.Controllers.Add(this.smartLabelsController);

            this.radDropDownList1.SelectedIndexChanged += radDropDownList1_SelectedIndexChanged;
            List<string> chartTypes = new List<string>();
            chartTypes.Add("Pie");
            chartTypes.Add("Bar");
            this.radDropDownList1.DataSource = chartTypes;
            fpr = this;
        }
        void ChargementData_Paiement()
        {
            try
            {
                int cpt = 0;
                SqlDataReader dataReader;
                double MontantNetTT = 0;
                double MontantRecuTT = 0;
                double MontantResteTT = 0;
                double RemiseAmount = 0;
                double RemiseAmountTT = 0;
                dataReader = MyObjConnection.GetCommand("SELECT (Montant_Net*Dsicompte)/100 as MontantRemise,Montant_Net,Avance,Dsicompte,Intitule,NomE,Libelle FROM F_Inscriptions FI INNER JOIN F_Paiements FP ON FI.IdInsp=FP.Id_Inscrip INNER JOIN F_Etudiants FE  ON FE.Matricule =FI.EtudiantId INNER JOIN P_ModePaiement PM ON PM.Id_ModePaie=FP.Id_mode_paie INNER JOIN F_Classes FC ON FC.Code=FI.ClasseId  AND FI.EXERCICE='" + Settings.Default["Exercice"] + "'").ExecuteReader();
                if (DGVPaiement.Rows.Count > 0)
                {
                    DGVPaiement.Rows.Clear();
                }
                while (dataReader.Read())
                {
                    DGVPaiement.Rows.AddNew();
                    cpt = DGVPaiement.RowCount - 1;
                    DGVPaiement.Rows[cpt].Cells["Etudiants"].Value = dataReader["NomE"].ToString();
                    DGVPaiement.Rows[cpt].Cells["Classes"].Value = dataReader["Intitule"].ToString();
                    DGVPaiement.Rows[cpt].Cells["ModePaie"].Value = dataReader["Libelle"].ToString();
                    DGVPaiement.Rows[cpt].Cells["MontantNet"].Value = double.Parse(dataReader["Montant_Net"].ToString());
                    DGVPaiement.Rows[cpt].Cells["MontantVerse"].Value = double.Parse(dataReader["Avance"].ToString());
                    if (dataReader["MontantRemise"].ToString()!="")
                    {
                        RemiseAmount = double.Parse(dataReader["MontantRemise"].ToString());
                    }
                    else
                    {
                        RemiseAmount = 0;
                    }
                    DGVPaiement.Rows[cpt].Cells["RemiseAmount"].Value = RemiseAmount;
                    DGVPaiement.Rows[cpt].Cells["MontantReste"].Value = double.Parse(dataReader["Montant_Net"].ToString()) - double.Parse(dataReader["Avance"].ToString()) - RemiseAmount;
                    RemiseAmountTT = RemiseAmountTT + RemiseAmount;
                    //Cumul des montant TT ici
                    MontantNetTT = MontantNetTT + double.Parse(dataReader["Montant_Net"].ToString());
                    MontantRecuTT = MontantRecuTT + double.Parse(dataReader["Avance"].ToString());
                    MontantResteTT = MontantResteTT + double.Parse(DGVPaiement.Rows[cpt].Cells["MontantReste"].Value.ToString());

                    DGVPaiement.Rows[cpt].Cells["MontantNet"].Style.ForeColor = System.Drawing.Color.Brown;
                    DGVPaiement.Rows[cpt].Cells["MontantNet"].Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    DGVPaiement.Rows[cpt].Cells["MontantNet"].Style.BackColor = System.Drawing.Color.White;

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
                        DGVPaiement.Rows[cpt].Cells["Classes"].Style.ForeColor = System.Drawing.Color.Maroon;
                        DGVPaiement.Rows[cpt].Cells["ModePaie"].Style.ForeColor = System.Drawing.Color.Maroon;
                        
                    }
                }
                this.DGVPaiement.Columns["MontantVerse"].PinPosition = PinnedColumnPosition.Right;
                this.DGVPaiement.Columns["MontantReste"].PinPosition = PinnedColumnPosition.Right; 

                lblMtnNetTT.Text = String.Format("{0,12:0,000.00}", Convert.ToDouble(MontantNetTT.ToString()));
                lblMtnRecuTT.Text = String.Format("{0,12:0,000.00}", Convert.ToDouble(MontantRecuTT.ToString()));
                lblMtnResteTT.Text = String.Format("{0,12:0,000.00}", Convert.ToDouble(MontantResteTT.ToString()));
                lblApresRemise.Text = String.Format("{0,12:0,000.00}", Convert.ToDouble(RemiseAmountTT.ToString()));
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "erreur ChargementData_Paiement", MessageBoxButtons.OK);
            }

        }
        private void btnF_classe_Click(object sender, EventArgs e)
        {
            F_Classes fr = new F_Classes();
            fr.TopMost = true;
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }
        public void HidingElement(bool etat)
        {

            this.P1.Visible = etat;
            this.P2.Visible = etat;
            this.P3.Visible = etat;
            this.P4.Visible = etat;
            this.P5.Visible = etat;
            this.P6.Visible = etat;
        }
        public string CountInscit() {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT count(*) AS COUNT  FROM F_Inscriptions Fi INNER JOIN F_Classes  FC ON FI.ClasseId =FC.Code  AND FI.Statut=1 AND FI.EXERCICE='" + Settings.Default["Exercice"] + "'");
                while (dataReader.Read())
                {
                    return dataReader["COUNT"].ToString();
                }
                return "0";
            }
            catch (Exception )
            {
                return "0";
            }         
        }
        /*[ENSEIGNANT COMPTAGE]*/
        public string CountEnseignants()
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT Count(*) AS COUNT  FROM F_Professeurs");
                while (dataReader.Read())
                {
                    return dataReader["COUNT"].ToString();
                }
                return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        /*[ELEVE COMPTAGE]*/
        public string CountELEVE()
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT Count(*) AS COUNT  FROM F_Etudiants");
                while (dataReader.Read())
                {
                    return dataReader["COUNT"].ToString();
                }
                return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        /*[CLASSES COMPTAGE]*/
        public string CountClasse()
        {
            try
            {
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetReader("SELECT Count(*) AS COUNT  FROM F_CLASSES");
                while (dataReader.Read())
                {
                    return dataReader["COUNT"].ToString();
                }
                return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        DirectoryInfo Dossier = new DirectoryInfo ("PhotoProfesseurs");
        private void Form1_Load(object sender, EventArgs e)
        {
            String path = Dossier.FullName.ToString ();
            lblCount_1.Text ="  "+ CountInscit();
            lblcount_2.Text = "  " + CountEnseignants();
            lblcount_3.Text = "  " + CountELEVE();
            lblcount_4.Text = "  " + CountClasse();
            if (PictureProfil!=""){
               
                if (File.Exists (path + @"\" + PictureProfil)) {
                    Bitmap pictureProfils=new Bitmap (path + @"\" + PictureProfil);
                    PictureProfilL.BackgroundImage   =pictureProfils;
                    } else {
                    PictureProfilL.BackgroundImage   =ALCHANYSCHOOL.Properties.Resources.Copy_of_stock_people;
                    }
                }     
            lblExercice.Text = "Année Scolaire : " + Settings.Default["Exercice"] ;
            ChargementData_Paiement();
        }

        private void button9_Click(object sender, EventArgs e)
        {
           // new F_Parents();
            F_Ecole fr = new F_Ecole();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);  
        }

        private void button8_Click(object sender, EventArgs e)
        {
            F_Professeurs fr = new F_Professeurs();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            F_Etudiants fr = new F_Etudiants();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            F_Paiement fr = new F_Paiement();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            F_Configuration fr = new F_Configuration();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }
        Boolean IsClique = false ;
        private void button6_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close() ;
            }
            if (IsClique ==false )
            {
                this.P1.Visible = true;
                this.P2.Visible = true;
                this.P3.Visible = true;
                this.P4.Visible = true;
                this.P5.Visible = true;
                this.P6.Visible = true;
                IsClique = true ;
            }
            else
            {
                this.P1.Visible = false;
                this.P2.Visible = false;
                this.P3.Visible = false;
                this.P4.Visible = false;
                this.P5.Visible = false;
                this.P6.Visible = false;
                IsClique = false;
            }
            ChargementData_Paiement ();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
            F_Authenification Authenification = new F_Authenification();
            Authenification.Show();
        }

        private void radChartView1_LabelFormatting(object sender, ChartViewLabelFormattingEventArgs e)
        {
            /*
            if (this.radCheckBox2.Checked)
            {
                if (this.radCheckBox4.Checked)
                {
                    e.LabelElement.BorderColor = ((DataPointElement)e.LabelElement.Parent).BackColor;
                }
                else
                {
                    e.LabelElement.BorderColor = Color.Black;
                }

                e.LabelElement.BackColor = Color.White;
            }
            else
            {
                e.LabelElement.ResetValue(LabelElement.BorderColorProperty, Telerik.WinControls.ValueResetFlags.Local);
                e.LabelElement.ResetValue(LabelElement.BorderWidthProperty, Telerik.WinControls.ValueResetFlags.Local);
                e.LabelElement.ResetValue(LabelElement.BackColorProperty, Telerik.WinControls.ValueResetFlags.Local);
            }*/
            e.LabelElement.ResetValue(LabelElement.BorderColorProperty, Telerik.WinControls.ValueResetFlags.Local);
            e.LabelElement.ResetValue(LabelElement.BorderWidthProperty, Telerik.WinControls.ValueResetFlags.Local);
            e.LabelElement.ResetValue(LabelElement.BackColorProperty, Telerik.WinControls.ValueResetFlags.Local);
        }

        private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            this.radChartView1.AreaType = ChartAreaType.Polar;
            this.radChartView1.View.Margin = new Padding(40);
            this.radChartView1.ChartElement.LegendPosition = LegendPosition.Right;
            this.radChartView1.ChartElement.LegendElement.StackElement.Orientation = Orientation.Vertical;
            //----------------------------------------
            this.radChartView2.AreaType = ChartAreaType.Polar;
            this.radChartView2.View.Margin = new Padding(40);
            this.radChartView2.ChartElement.LegendPosition = LegendPosition.Right;
            this.radChartView2.ChartElement.LegendElement.StackElement.Orientation = Orientation.Vertical;


            SmartLabelsStrategyBase strategy = null;
            this.smartLabelsController.Strategy = null;
            if (this.radDropDownList1.SelectedItem.Text == "Pie")
            {
                this.radChartView1.View.Margin = new Padding(60, 0, 50, 0);
                this.radChartView2.View.Margin = new Padding(60, 0, 50, 0);
                this.InitializePie();
                this.InitializePensionPie();
                strategy = new PieTwoLabelColumnsStrategy();
            }else if (this.radDropDownList1.SelectedItem.Text == "Bar")
            {
                this.InitializeBar();
                this.InitializeBarPension();
                strategy = new VerticalAdjusmentLabelsStrategy();
            }
            this.smartLabelsController.Strategy = strategy;
        }
        private void radSpinEditor1_ValueChanged(object sender, EventArgs e)
        {
            foreach (ChartViewController controller in this.radChartView1.Controllers)
            {
                SmartLabelsController control = controller as SmartLabelsController;

                if (control != null)
                {
                  //  control.Strategy.DistanceToLabel = (int)this.radSpinEditor1.Value;
                    this.radChartView1.View.PerformRefresh(this.radChartView1.View, false);
                }
            }
        }
        private void InitializeBar()
        {
            this.radChartView1.AreaType = ChartAreaType.Cartesian;
            Boolean IsData = false;
            this.radChartView1.ShowGrid = true;
            SqlDataReader dataReader;
            dataReader = MyObjConnection.GetReader("SELECT COUNT(*)AS INSCRIT_PAR_CLASSE,Intitule  FROM F_Inscriptions FI INNER JOIN F_Classes FC ON FI.ClasseId=FC.Code  AND FI.Statut=1 AND FI.EXERCICE='" + Settings.Default["Exercice"] + "' GROUP BY FC.Code,FC.Intitule ");
            foreach (KeyValuePair<double, object> dataItem in this.data.GetBarData(dataReader))
            {
                BarSeries bar = new BarSeries();
                bar.ShowLabels = true;
                bar.LegendTitle = dataItem.Value.ToString();
                bar.DrawLinesToLabels = true;// this.radCheckBox1.Checked;
                bar.SyncLinesToLabelsColor = true;// this.radCheckBox3.Checked;

                BarSeries smartBar = new BarSeries();
                smartBar.ShowLabels = true;
                smartBar.LegendTitle = dataItem.Value.ToString();
                smartBar.DrawLinesToLabels = true;// this.radCheckBox1.Checked;

                CategoricalDataPoint point = new CategoricalDataPoint(dataItem.Key, dataItem.Value);
                point.Label = string.Format("{0}E ", point.Value);
                bar.DataPoints.Add(point);
                this.radChartView1.Series.Add(bar);
                IsData = true;
            }
            if (IsData)
              this.radChartView1.Axes[0].LabelFitMode = AxisLabelFitMode.MultiLine;
        }
        private void InitializeBarPension()
        {
            this.radChartView2.AreaType = ChartAreaType.Cartesian;

            this.radChartView2.ShowGrid = true;
            SqlDataReader dataReader;
            dataReader = MyObjConnection.GetReader("SELECT (Montant_Net*Dsicompte)/100 as MontantRemise,Montant_Net,Avance,Dsicompte,Intitule,NomE,Libelle FROM F_Inscriptions FI INNER JOIN F_Paiements FP ON FI.IdInsp=FP.Id_Inscrip INNER JOIN F_Etudiants FE  ON FE.Matricule =FI.EtudiantId INNER JOIN P_ModePaiement PM ON PM.Id_ModePaie=FP.Id_mode_paie INNER JOIN F_Classes FC ON FC.Code=FI.ClasseId AND FI.EXERCICE='" + Settings.Default["Exercice"] + "'");
            foreach (KeyValuePair<double, object> dataItem in this.data.GetBar_PensionData(dataReader))
            {
                BarSeries bar = new BarSeries();
                bar.ShowLabels = true;
                bar.LegendTitle = dataItem.Value.ToString();
                bar.DrawLinesToLabels = true;// this.radCheckBox1.Checked;
                bar.SyncLinesToLabelsColor = true;// this.radCheckBox3.Checked;

                BarSeries smartBar = new BarSeries();
                smartBar.ShowLabels = true;
                smartBar.LegendTitle = dataItem.Value.ToString();
                smartBar.DrawLinesToLabels = true;// this.radCheckBox1.Checked;

                CategoricalDataPoint point = new CategoricalDataPoint(dataItem.Key, dataItem.Value);
                point.Label = string.Format("{0} ", point.Value);
                bar.DataPoints.Add(point);

                /*point = new CategoricalDataPoint(dataItem.Key, dataItem.Value);
                point.Label = string.Format("{0}E - {1}", point.Value, point.Category);
                smartBar.DataPoints.Add(point);*/

                this.radChartView2.Series.Add(bar);
            }

            this.radChartView2.Axes[0].LabelFitMode = AxisLabelFitMode.MultiLine;
        }
        private void InitializePie()
        {
            this.radChartView1.AreaType = ChartAreaType.Pie;
            SqlDataReader dataReader;
            dataReader = MyObjConnection.GetReader("SELECT COUNT(*)AS INSCRIT_PAR_CLASSE,Intitule  FROM F_Inscriptions FI INNER JOIN F_Classes FC ON FI.ClasseId=FC.Code  AND FI.Statut=1 AND FI.EXERCICE='" + Settings.Default["Exercice"] + "' GROUP BY FC.Code,FC.Intitule ");

            PieSeries pie = new PieSeries();
            pie.LabelMode = PieLabelModes.Horizontal ;
            pie.ShowLabels = true;
            pie.DrawLinesToLabels = true;// this.radCheckBox1.Checked;
            pie.SyncLinesToLabelsColor = true;// this.radCheckBox3.Checked;

            PieSeries smartPie = new PieSeries();
            smartPie.LabelMode = PieLabelModes.Horizontal;
            smartPie.ShowLabels = true;
            smartPie.DrawLinesToLabels = true;// this.radCheckBox1.Checked;
            smartPie.SyncLinesToLabelsColor = true;// this.radCheckBox3.Checked;

            foreach (KeyValuePair<double, object> dataItem in this.data.GetBarData(dataReader))
            {
                PieDataPoint point = new PieDataPoint(dataItem.Key, dataItem.Value.ToString());
                point.Label = dataItem.Value.ToString();
                pie.DataPoints.Add(point);

                point = new PieDataPoint(dataItem.Key, dataItem.Value.ToString());
                point.Label = dataItem.Value.ToString();
                smartPie.DataPoints.Add(point);
            }

            this.radChartView1.Series.Add(pie);
          //  this.radCheckBox1.Checked = true;
        }
        private void InitializePensionPie()
        {
            this.radChartView2.AreaType = ChartAreaType.Pie;
            SqlDataReader dataReader;
            dataReader = MyObjConnection.GetReader("SELECT (Montant_Net*Dsicompte)/100 as MontantRemise,Montant_Net,Avance,Dsicompte,Intitule,NomE,Libelle FROM F_Inscriptions FI INNER JOIN F_Paiements FP ON FI.IdInsp=FP.Id_Inscrip INNER JOIN F_Etudiants FE  ON FE.Matricule =FI.EtudiantId INNER JOIN P_ModePaiement PM ON PM.Id_ModePaie=FP.Id_mode_paie INNER JOIN F_Classes FC ON FC.Code=FI.ClasseId AND FI.EXERCICE='" + Settings.Default["Exercice"] + "'");

            PieSeries pie = new PieSeries();
            pie.LabelMode = PieLabelModes.Horizontal;
            pie.ShowLabels = true;
            pie.DrawLinesToLabels = true;// this.radCheckBox1.Checked;
            pie.SyncLinesToLabelsColor = true;// this.radCheckBox3.Checked;

            PieSeries smartPie = new PieSeries();
            smartPie.LabelMode = PieLabelModes.Horizontal;
            smartPie.ShowLabels = true;
            smartPie.DrawLinesToLabels = true;// this.radCheckBox1.Checked;
            smartPie.SyncLinesToLabelsColor = true;// this.radCheckBox3.Checked;

            foreach (KeyValuePair<double, object> dataItem in this.data.GetBar_PensionData(dataReader))
            {
                PieDataPoint point = new PieDataPoint(dataItem.Key, dataItem.Value.ToString());
                point.Label = dataItem.Value.ToString();
                pie.DataPoints.Add(point);

                point = new PieDataPoint(dataItem.Key, dataItem.Value.ToString());
                point.Label = dataItem.Value.ToString();
                smartPie.DataPoints.Add(point);
            }
            this.radChartView2.Series.Add(pie);
          //  this.radCheckBox2.Checked = true;
        }

		private void button14_Click (object sender, EventArgs e)
			{
				Frm_Process fr = new Frm_Process ();
				fr.MdiParent = this;
				fr.Show ();
				HidingElement (false);
			}

		private void button10_Click (object sender, EventArgs e)
			{
				AboutBox1 fr = new AboutBox1 ();
				fr.MdiParent = this;
				fr.Show ();
				HidingElement (false);
			}

        private void btnUser_Click (object sender, EventArgs e)
            {
                 F_UTILISATEURS fr = new F_UTILISATEURS ();
                 fr.MdiParent = this;
                 fr.Show ();
                 HidingElement (false);
            }

        private void button5_Click (object sender, EventArgs e) {

            }

        private void button2_Click (object sender, EventArgs e) {
                  F_Filiers fr = new F_Filiers ();
                  fr.MdiParent = this;
                  fr.Show ();
                  HidingElement (false);
            }    
    }
}
