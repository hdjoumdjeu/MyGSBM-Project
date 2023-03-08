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
using Excel=Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Threading;
namespace ALCHANYSCHOOL
{
    public partial class F_Filiers : Telerik.WinControls.UI.RadForm
    {
        Connection MyObjConnection;
        public int RowsCount;
        public Boolean ChkHead;
        public F_Filiers()
        {
            InitializeComponent();
            MyObjConnection = new Connection (Settings.Default["ServerName"].ToString (), Settings.Default["DataBase"].ToString (), Settings.Default["UserName"].ToString (), Settings.Default["KEY"].ToString ());         
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            //backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;  //Tell the user how the process went
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true; //Allow for the process to be cancelled
        }

        private void btnQuitter_Click(object sender, EventArgs e)
		{
           this.Close();
        }
        public String CheckTypeInput (String InputValue) 
            {
                    double number;
                    DateTime Date;
                    string ReturnStringValue="";
                    char[] charsToTrim = { ' ' };
                    if (double.TryParse (InputValue, out number)) {
                        if (number >= 0 || number < 0) {
                        ReturnStringValue= InputValue;
                            }
                        } else if (DateTime.TryParse (InputValue, out Date)) {
                        ReturnStringValue= "'"+ InputValue +"'";
                        } else {
                        ReturnStringValue= "'"+ InputValue +"'";
                        }
                    ReturnStringValue=ReturnStringValue.TrimStart (charsToTrim);
                    return  ReturnStringValue;
            }
		private void btnSave_Click(object sender, EventArgs e)
			{
            try
                {
                var ValueLue = txtFileName.Text ;
            
                
                     //String Query="";
                     //if (btnTraitement.Text.Equals("&Modifier"))
                     //    {
                     //    Query = "UPDATE F_Filieres SET IntituleF='" + txtniveau.Text + "',CapaciteF=" + txtcapacite.Text + ",DureeF=" + txtduree.Text + ",Section='" + drListSection.Text + "' WHERE Id_Filliere='" + txtPathFile.Text + "'";
                     //       MyObjConnection.Execute (Query);
                     //       MessageBox.Show ("Modification effectuée avec Succès!", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     //    }
                     //else
                     //    {
                     //        Query = "INSERT INTO F_Filieres(Id_Filliere,IntituleF,CapaciteF,DureeF,Section) VALUES('" + txtPathFile.Text + "','" + txtniveau.Text + "'," + txtcapacite.Text + "," + txtduree.Text + ",'" + drListSection.Text + "')";
                     //        MyObjConnection.Execute (Query);
                     //        MessageBox.Show ("Enregistrement effectuée avec Succès!", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     //    }
                     //var test="";
               
                    // ChargementData_Filiers ();
                }
            catch (Exception ex )
                {

                MessageBox.Show (ex.Message, "erreur creation de l'exercice", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
			
			}
        void ChargementALLColumnName (String TableName)
            {
            try
                {
                int cpt = 0;
                if (MyObjConnection.Con.State == ConnectionState.Closed)
                    MyObjConnection.Con.Open ();
                SqlDataReader dataReader;
                dataReader = MyObjConnection.GetCommand ("SELECT  * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='"+ TableName +"'").ExecuteReader ();
                if (DGVImportData.Rows.Count>0)
                    {
                         DGVImportData.Rows.Clear ();
                    }
                while (dataReader.Read ())
                    {
                    DGVImportData.Rows.AddNew ();
                    cpt = DGVImportData.RowCount - 1;
                          DGVImportData.Rows[cpt].Cells["ChampToImport"].Value =dataReader["COLUMN_NAME"].ToString ();
                          DGVImportData.Rows[cpt].Cells["choix"].Value="false";
                          ///DGVImportData.Rows[cpt].Cells["valeurDefaut"].Value="";
                    
                    }
                dataReader.Close ();
                DGVImportData.ReadOnly=false;
                }
            catch (Exception ex)
                {

                MessageBox.Show (ex.Message, "erreur Chargement des colonens", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        private void F_Filiers_Load (object sender, EventArgs e)
            {
                    DGVImportData.ReadOnly=false;
                    ChkHead=MyChkHead.Checked;
            }
        
        private void DGVFiliere_CellEndEdit (object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
            {
            txtPathFile.Text = DGVFiliere.Rows[e.RowIndex].Cells["Code"].Value.ToString ();
            }

        private void DGVFiliere_CellClick (object sender, Telerik.WinControls.UI.GridViewCellEventArgs e) {

            }

        private void btnClean_Click (object sender, EventArgs e)
            {
                        OpenFileDialog openFileDialog = new OpenFileDialog ();
                        openFileDialog.InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
                        openFileDialog.Filter = "Excel File |*.xlsx";
                        if (openFileDialog.ShowDialog (this) == DialogResult.OK) {
                            string FileName = openFileDialog.FileName;
                            txtPathFile.Text = FileName;
                            }
            }

        private void btnSupprimer_Click (object sender, EventArgs e)
            {
            if (MessageBox.Show ("Voulez-vous reelement supprime cette filiere?", "Suppression de filiere", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MyObjConnection.Execute ("DELETE FROM F_Filieres WHERE Id_Filliere='" + txtPathFile.Text  + "'");
                    MessageBox.Show ("Suppression  effectuée avec Succès!", "Supprimer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   // ChargementALLColumnName ();
                }
            }
        public void ReadExcelData ()
            {
                       Excel.Application xlApp = new Excel.Application ();
                       Excel.Workbook xlWorkbook = xlApp.Workbooks.Open (@""+txtPathFile.Text);
                       Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[txtFileName.Text];
                       Excel.Range xlRange = xlWorksheet.UsedRange;
                       string CollectiorImporter="";
                       int cpte=0,ColPosition=1;
                      // ListVJournal.Items.Clear ();
                       this.Invoke (new MethodInvoker (delegate
                       {
                             ListVJournal.Items.Clear ();
                       }));
                       for (int i = 0; i <numRows.Value; i++) {                   
                           for (int j = 0; j <numCols.Value; j++) {                         
                               //new line
                               //write the value to the console                
                                 if (xlRange.Cells[rowPosition, ColPosition] != null && xlRange.Cells[rowPosition, ColPosition].Value2 != null)
                                     DGVDataFromExcel.Rows[i].Cells[j].Value=xlRange.Cells[rowPosition, ColPosition].Value2.ToString ();
                                 CollectiorImporter+=" "+ DGVDataFromExcel.Rows[i].Cells[j].Value;
                                 ColPosition++;
                               }
                           ColPosition=1;
                           rowPosition++;
                                 //Thread.Sleep (100);
                                 backgroundWorker1.ReportProgress (i+1);
                                 cpte++;
                                 if (InvokeRequired ) {
                                 this.Invoke (new MethodInvoker (delegate
                                     {
                                        lblStatus.Text = "Process "+ (i+1) +"/"+numRows.Value;   
                                        ListVJournal.Items.Add ("Line =>["+ (i+1) +"] Collections donnees importent : "+CollectiorImporter);
                                         CollectiorImporter=""; 
                                     }));
                                     }
                            
                           }
                       //cleanup
                       GC.Collect ();
                       GC.WaitForPendingFinalizers ();
                       Marshal.ReleaseComObject (xlRange);
                       Marshal.ReleaseComObject (xlWorksheet);
                       xlWorkbook.Close ();
                       Marshal.ReleaseComObject (xlWorkbook);
                       xlApp.Quit ();
                       Marshal.ReleaseComObject (xlApp);          
            }
        public int rowPosition;
        private void btnLireExcelFIle_Click (object sender, EventArgs e) {
                      Excel.Application xlApp = new Excel.Application ();
                      Excel.Workbook xlWorkbook = xlApp.Workbooks.Open (@""+txtPathFile.Text);
                      Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[txtFileName.Text];
                      Excel.Range xlRange = xlWorksheet.UsedRange;
                      lblStatus.Text = "...";   
                      DGVDataFromExcel.ColumnCount=(int)numCols.Value;
                      DGVDataFromExcel.RowCount=(int)numRows.Value;

                       ChkHead=false;
                      if (MyChkHead.Checked) {
                          ChkHead=true;
                          rowPosition=2;
                          }
                      for (int i = 0; i < numRows.Value; i++) {
                          for (int j = 0; j < numCols.Value; j++) {                                              
                                        if (xlRange.Cells[i+1, j+1] != null && xlRange.Cells[i+1, j+1].Value2 != null){                             
                                             if (ChkHead)
                                               DGVDataFromExcel.Columns[j].HeaderText=xlRange.Cells[i+1, j+1].Value2.ToString ()+"=>["+ (j)+"]";                                  
                                         }                               
                                     }
                                   ChkHead=false;
                                       break;
                          }
                      //cleanup
                      GC.Collect ();
                      GC.WaitForPendingFinalizers ();
                      //rule of thumb for releasing com objects:
                      //release com objects to fully kill excel process from running in the background
                      Marshal.ReleaseComObject (xlRange);
                      Marshal.ReleaseComObject (xlWorksheet);
                      //close and release
                      xlWorkbook.Close ();
                      Marshal.ReleaseComObject (xlWorkbook);
                      //quit and release
                      xlApp.Quit ();
                      Marshal.ReleaseComObject (xlApp);    
                      progressBar1.Maximum =(int)numRows.Value;            
                      btnLireExcelFIle.Enabled=false;
                      backgroundWorker1.RunWorkerAsync ();
            }
        private void backgroundWorker1_DoWork (object sender, DoWorkEventArgs e) {                  
                     ReadExcelData ();            
            }
        private void backgroundWorker1_ProgressChanged (object sender, ProgressChangedEventArgs e) {
                progressBar1.Value = e.ProgressPercentage;
            }
        private void backgroundWorker1_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e) {
                if (e.Cancelled)
                    {
                         lblStatus.Text = "Process was cancelled";
                    } else if (e.Error != null) 
                    {
                         lblStatus.Text = "There was an error running the process. The thread aborted";
                    } else 
                    {
                          lblStatus.Text = "Process was completed";                      
                          btnLireExcelFIle.Enabled=true;
                    }
            }
        public String TableName;
        private void CmbImportFile_SelectedIndexChanged (object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e) {
            TableName="";
            switch (CmbImportFile.Text ) {
                case "Fiche Enseignants":
                    TableName="F_Professeurs";
                    ChargementALLColumnName (TableName);
            break ;
                case "Fiche Etudiants":
                     TableName="F_Etudiants";
                     ChargementALLColumnName (TableName);
            break;
             case "Fiche Classes":
                    TableName="F_Classes";
                    ChargementALLColumnName (TableName);
            break;
             case "Fiche Etudiants Inscris":
            TableName="F_Inscriptions";
            ChargementALLColumnName (TableName);
            break;
                default:
                    break;
                }

            }
        public int ColumnCount () {
            int ColCountToImport=0;
            for (int i = 0; i <= DGVImportData.Rows.Count-1; i++) {
                if (DGVImportData.Rows[i].Cells["choix"].Value!=null) {
                    if (DGVImportData.Rows[i].Cells["choix"].Value.Equals (true)) {
                        ColCountToImport=ColCountToImport+1;
                        }
                    }
                }
            return ColCountToImport;
            }
        public void PreparationData ( out  String  ListeChamps  ) 
            {
            String[] ListeDataCollection;
            String ExcelCorrespondance,DefaultValue;
            ListeChamps="";
            int cpte=0;
            char[] charsToTrim = { ' ' };
            DGVReadyImport.RowCount =DGVDataFromExcel.RowCount;
            try {
                for (int iRow = 0; iRow <= DGVDataFromExcel.Rows.Count-1; iRow++) 
                    {
                        for (int col = 0; col <= DGVReadyImport.ColumnCount-1; col++) 
                            {
                                 ListeDataCollection=DGVReadyImport.Columns[col].HeaderText.Split ('=');
                                 ExcelCorrespondance=ListeDataCollection[1].Replace ('>', ' ');
                                 DefaultValue=ListeDataCollection[2].Replace ('>', ' ');

                                 if (iRow==0 && MyChkHead.Checked) {
                                     if (col==DGVReadyImport.ColumnCount-1) {
                                         ListeChamps+=ListeDataCollection[0].Replace ('>', ' ');
                                         } else {
                                         ListeChamps+=ListeDataCollection[0].Replace ('>', ' ')+",";
                                         }
                                     }                           
                                 if (ExcelCorrespondance.Trim ()!="Null" && DefaultValue.Trim ()=="Null") 
                                   {
                                   DGVReadyImport.Rows[cpte].Cells[col].Value= DGVDataFromExcel.Rows[iRow].Cells[Convert.ToInt16 (ExcelCorrespondance)].Value;
                                   } else if (DefaultValue.Trim ()!="Null")
                                   {
                                     //get default data instead of data present on the excel file...
                                     DGVReadyImport.Rows[cpte].Cells[col].Value=DefaultValue.TrimStart(charsToTrim);
                                   } else {
                                     //no data to import....
                                   }
                            }
                        cpte++;
                    }                        
                } catch (Exception ex) {
                    MessageBox.Show (ex.Message, "erreur program importation donnees!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }         
            }
        public Boolean ImportDateToServerTable (string ListColumms, string TableName, string mode) 
            {
            try {

                  String ListValueCollected="";
                  ListVJournal.Items.Clear ();
                     for (int iRow = 0; iRow <= DGVReadyImport.Rows.Count-1; iRow++) 
                        {
                            for (int col = 0; col < DGVReadyImport.ColumnCount; col++) 
                                { //RECUPERATION DES VALEURS PRESENT DANS LA COLONNES DE LA iRow Line.
                                   
                                    try {
                                          if (col==DGVReadyImport.ColumnCount-1) {
                                              ListValueCollected+=CheckTypeInput (DGVReadyImport.Rows[iRow].Cells[col].Value.ToString ());
                                              } else {
                                              var Value=DGVReadyImport.Rows[iRow].Cells[col].Value;
                                              if (Value==null) {
                                                  ListValueCollected+="'',";
                                                  } else {

                                                  ListValueCollected+=CheckTypeInput (Value.ToString ().Trim())+",";
                                                  }
                                              }
                                        } catch (Exception ex) {
                                            MessageBox.Show (ex.Message, "erreur program ImportDateToServerTable/ Exception genere dans la boucle colonne", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                }
                         //nous devons insere la iRow line dans la base de donne et vide la valeur du champ ListValueCollected.
                            String Query = "INSERT INTO "+TableName +"("+ListColumms+") VALUES("+ListValueCollected+")";
                            MyObjConnection.Execute (Query);
                            ListVJournal.Items.Add (Query);
                            ListVJournal.SelectedIndex  =ListVJournal.Items.Count-1;
                            ListValueCollected="";
                        }

                } catch (Exception ex) {
                        MessageBox.Show (ex.Message, "erreur program ImportDateToServerTable!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                    MessageBox.Show ("Import effectuez avec succes !", "erreur program ImportDateToServerTable!", MessageBoxButtons.OK, MessageBoxIcon.Information);           
                 return true;
              
            }
        public string ListInsertField;
        private void radButton1_Click (object sender, EventArgs e) {
            int cpte=0;
            try {
                       if (DGVImportData.Rows.Count > 0)
                         {
                            ListVJournal.Items.Clear ();
                            if (btnImport.Text =="&Pre-import") 
                                {
                                    DGVReadyImport.Rows.Clear ();                              
                                    DGVReadyImport.ColumnCount=ColumnCount ();
                                    for (int i = 0; i <=DGVImportData.Rows.Count-1; i++) 
                                        {
                                            if (DGVImportData.Rows[i].Cells["choix"].Value.Equals (true)) 
                                                {
                                                if (cpte<DGVReadyImport.ColumnCount) {
                                                    if (DGVImportData.Rows[i].Cells["ValeurDefaut"].Value !=null) {
                                                        DGVReadyImport.Columns[cpte].HeaderText=DGVImportData.Rows[i].Cells["ChampToImport"].Value +"=>Null=>"+DGVImportData.Rows[i].Cells["ValeurDefaut"].Value;
                                                        } else if (DGVImportData.Rows[i].Cells["CodeCorrespondanceExcel"].Value !=null) {
                                                        DGVReadyImport.Columns[cpte].HeaderText=DGVImportData.Rows[i].Cells["ChampToImport"].Value +"=>"+ DGVImportData.Rows[i].Cells["CodeCorrespondanceExcel"].Value+"=>Null";
                                                        }
                                                    cpte++;
                                                    }
                                                }
                                        }
                                    if (DGVDataFromExcel.Rows.Count > 0) {                         
                                            PreparationData (out ListInsertField);                          
                                        }
                                    btnImport.Text ="&Lancer-import";
                                } else 
                                {
                                        ImportDateToServerTable (ListInsertField, TableName, "Creation");
                                        btnImport.Text ="&Pre-import";
                                }                    
                         }
                } catch (Exception ex) {
                MessageBox.Show (ex.Message, "erreur program Lancer-import!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
       }     
    }
}
