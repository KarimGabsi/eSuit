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
using System.ServiceModel.Channels;

namespace eSuit_Manager
{
    public partial class eSuitManager : Form
    {
        WebServiceHost host;
        ServiceEndpoint ep;
        ServiceDebugBehavior sdb;
        NotifyIcon eSuitIcon;

        private System.Windows.Forms.Timer t;

        public eSuitManager()
        {
            InitializeComponent();

            eSuitIcon = new NotifyIcon();
            eSuitIcon.DoubleClick += eSuitIcon_DoubleClick;

            // Attach a context menu.
            eSuitIcon.ContextMenuStrip = IconMenu();

            eSuitIcon.BalloonTipText = "eSuit Manager is running...";
            eSuitIcon.BalloonTipTitle = "eSuit Manager";
            eSuitIcon.BalloonTipIcon = ToolTipIcon.None;

            eSuitIcon.Icon = Resources.eSuit;

            host = new WebServiceHost(typeof(eSuitService), new Uri("http://localhost:6969/esuit"));
            ep = host.AddServiceEndpoint(typeof(IeSuitService), new WebHttpBinding(WebHttpSecurityMode.None), "");
            
            ep.EndpointBehaviors.Add(new CorsBehaviorAttribute());
            foreach (var operation in ep.Contract.Operations)
            {
                //add support for cors (and for operation to be able to not  
                //invoke the operation if we have a preflight cors request)  
                operation.Behaviors.Add(new CorsBehaviorAttribute());
            }  
            sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;
            
            host.Open();
        }

        private void eSuitManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            host.Close();
            eSuitIcon.Dispose();
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

        private void eSuitManager_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                eSuitIcon.Visible = true;
                eSuitIcon.ShowBalloonTip(5000);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                eSuitIcon.Visible = false;
            }
        }

        private void eSuitIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip IconMenu()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
