using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ALCHANYSCHOOL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //  Application.Run(new F_Parents());        
           //Application.Run(new F_MyCamera("TEST","NUUU"));    
                try {
                    /* */ FrmPlashScreem frS = new FrmPlashScreem();
                           frS.Show();
                           Application.Run();
                    } catch (Exception ex) {
                        MessageBox.Show (ex.Message, "erreur program 404!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
        }
    }
}
