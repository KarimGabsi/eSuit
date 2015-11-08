using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using eSuit_Service;

namespace eSuit_Manager
{
    public partial class eSuitManager : Form
    {
        WebServiceHost host;
        ServiceEndpoint ep;
        ServiceDebugBehavior sdb;

        private System.Windows.Forms.Timer t;

        public eSuitManager()
        {
            InitializeComponent();

            host = new WebServiceHost(typeof(eSuitService), new Uri("http://localhost:6969/"));
            ep = host.AddServiceEndpoint(typeof(IeSuitService), new WebHttpBinding(), "");
            sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;
            host.Open();
        }

        private void eSuitManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            host.Close();
        }

        private void eSuitManager_Load(object sender, EventArgs e)
        {
            t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += t_Tick;
            t.Start();
        }
        private void t_Tick(object sender, EventArgs e)
        {
            eSuitService ess = new eSuitService();

            if (ess.Connected())
            {
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "eSuit connected on " + ess.CurrentPort();
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "eSuit disconnected";
            }

            txtDebug.Text = ess.GetLog();
            txtDebug.SelectionStart = txtDebug.Text.Length;
            txtDebug.ScrollToCaret();
        }
    }
}
