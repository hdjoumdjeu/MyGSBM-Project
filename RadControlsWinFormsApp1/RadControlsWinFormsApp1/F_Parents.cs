using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ALCHANYSCHOOL.Properties;
namespace ALCHANYSCHOOL
{
    public partial  class F_Parents : Telerik.WinControls.UI.RadRibbonForm
    {
        private string DirectoryIniFilePath = Application.StartupPath + "\\ConnectAPI.ini";
        public F_Parents()
        {
            InitializeComponent();
            
            var MyIni = new IniFilePro(DirectoryIniFilePath);
            Settings.Default["ServerName"]       = MyIni.Read("ServerName");
            Settings.Default["UserName"]         = MyIni.Read("UserName");
            Settings.Default["DataBase"]         = MyIni.Read("DataBase");
            Settings.Default["KEY"]              = EncryptDecrypt.DecryptCipherTextToPlainText(MyIni.Read("KEY"));
            Settings.Default["ENTETEPAGE"]       = MyIni.Read("ENTETE-PAGE", "LINK");
            Settings.Default["PIEDPAGE"]         = MyIni.Read("PIED-PAGE", "LINK");
            Settings.Default["LOGO"]             = MyIni.Read("LOGO", "LINK");
            Settings.Default["StatusImpression"] = MyIni.Read("StatusImpression", "LINK");
        }
       
        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            new F_Parents();
            F_Ecole fr = new F_Ecole();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);           
        }
        public void HidingElement(bool etat ) {

            this.Gp1.Visible = etat;
            this.Gp4.Visible = etat;
            this.Gp5.Visible = etat;
        }

      
        private void radButtonElement2_Click(object sender, EventArgs e)
        {
           // F_Filiers fr = new F_Filiers();
            F_Classes fr = new F_Classes ();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            F_Professeurs fr = new F_Professeurs();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void radButtonElement4_Click(object sender, EventArgs e)
        {
           
        }

        private void radRibbonBarGroup8_Click(object sender, EventArgs e)
        {

        }

        private void radButtonElement8_Click(object sender, EventArgs e)
        {
            F_Etudiants fr = new F_Etudiants();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }
     
        private void RadRibbonForm1_Load(object sender, EventArgs e)
        {
            radGwMatiers.Rows.Add(1);
            radGwMatiers.Rows[0].Cells["Matieres"].Value = "EPS";
            radGwMatiers.Rows.Add(1);
            radGwMatiers.Rows[1].Cells["Matieres"].Value = "Anglais";
            radGwMatiers.Rows.Add(1);
            radGwMatiers.Rows[2].Cells["Matieres"].Value = "Francais";
            radGwMatiers.Rows.Add(1);
            radGwMatiers.Rows[3].Cells["Matieres"].Value = "Mathematique";
            radGwMatiers.Rows.Add(1);
            radGwMatiers.Rows[4].Cells["Matieres"].Value = "Geographie";




            radGwClasse.Rows.Add(1);
            radGwClasse.Rows[0].Cells["Classe"].Value = "CM2";
            radGwClasse.Rows[0].Cells["EleveI"].Value = "25";
            radGwClasse.Rows[0].Cells["Places"].Value = "12";
            
            radGwClasse.Rows.Add(1);
            radGwClasse.Rows[1].Cells["Classe"].Value = "CM1";
            radGwClasse.Rows[1].Cells["EleveI"].Value = "10";
            radGwClasse.Rows[1].Cells["Places"].Value = "3";
            
            radGwClasse.Rows.Add(1);
            radGwClasse.Rows[2].Cells["Classe"].Value = "CE2";
            radGwClasse.Rows[2].Cells["EleveI"].Value = "15";
            radGwClasse.Rows[2].Cells["Places"].Value = "6";
            
            radGwClasse.Rows.Add(1);
            radGwClasse.Rows[3].Cells["Classe"].Value = "CE1";
            radGwClasse.Rows[3].Cells["EleveI"].Value = "18";
            radGwClasse.Rows[3].Cells["Places"].Value = "4";
        }

        private void radButtonElement5_Click(object sender, EventArgs e)
        {
            HidingElement(true);
        }

        private void radButtonElement11_Click(object sender, EventArgs e)
        {

            F_Configuration fr = new F_Configuration();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }

        private void radRibbonBarGroup2_Click (object sender, EventArgs e)
            {

            }

        private void radButtonElement9_Click(object sender, EventArgs e)
        {
            F_Paiement fr = new F_Paiement();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void radButtonElement7_Click(object sender, EventArgs e)
        {
            F_Classes fr = new F_Classes();
            fr.MdiParent = this;
            fr.Show();
            HidingElement(false);
        }

        private void radRibbonBarGroup7_Click(object sender, EventArgs e)
        {

        }
    }
}
