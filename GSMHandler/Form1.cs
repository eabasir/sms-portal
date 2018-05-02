using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSMHandler
{
    public partial class Form1 : Form
    {

        string DIR_APP = string.Empty;
        string DIR_EXECUTABLE = string.Empty;
        string EXECUTABLE_FILE_NAME = "GSMConnector.exe";


        public Form1()
        {
            InitializeComponent();
            DIR_APP = Path.GetDirectoryName(Application.ExecutablePath);
            DIR_EXECUTABLE = Path.Combine(DIR_APP, "Executable");

            if (!Directory.Exists(DIR_EXECUTABLE))
            {
                MessageBox.Show("Executable folder not exists in necessary path. Check bin folder");
            }

            if (!File.Exists(Path.Combine(DIR_EXECUTABLE, EXECUTABLE_FILE_NAME)))
            {
                MessageBox.Show("Executable file (" + EXECUTABLE_FILE_NAME + ") not exists in necessary path. Check bin/Executable folder");
            }
            new Logger();



        }

        private void btnGetSim_Click(object sender, EventArgs e)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {

                var q = (from x in db.SIMs
                         select new
                         {
                             x.TFId,
                             x.TFNumber,
                             x.TFPort,
                         }).ToList();


                grdSim.DataSource = q;


            }
        }

        private void btnRunService_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdSim.RowCount != 0)
                {
                    for (int i = 0; i < grdSim.RowCount; i++)
                    {


                        var p = Process.Start(Path.Combine(DIR_EXECUTABLE, EXECUTABLE_FILE_NAME), "-" + grdSim.Rows[i].Cells[0].Value.ToString());
                    }


                }
                else
                    MessageBox.Show("There is no sim card to work...");
            }
            catch (Exception ex)
            {
                Logger.Log(SMSPortalCross.Messages.HANDLER_RUN_SERVICE, ex);

            }


        }
    }
}
