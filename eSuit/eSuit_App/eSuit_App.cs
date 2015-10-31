using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using eSuitLibraryNET35;

namespace eSuit_App
{
    public partial class eSuit_App : Form
    {
        eSuit _eSuit;
        public eSuit_App()
        {
            InitializeComponent();
            
            //Fill combobox with possible hitplaces
            var hitplaces = Enum.GetValues(typeof(HitPlaces));
            foreach (var hp in hitplaces)
            {
                cbHitPlaces.Items.Add(hp);
            }

            tbVolts.Minimum = 1;
            tbVolts.Maximum = 60;
            tbDuration.SmallChange = 1;
            tbDuration.LargeChange = 10;
            tbVolts.Value = 15;

            tbDuration.Minimum = 10;
            tbDuration.Maximum = 3000;
            tbDuration.SmallChange = 1;
            tbDuration.LargeChange = 100;
            tbDuration.TickFrequency = 100;
            tbDuration.Value = 10;

            btnExecuteHit.Enabled = false;
            lblVolts.Text = "Volts: " + tbVolts.Value.ToString() + "v";
            lblDuration.Text = "Duration: " + tbDuration.Value.ToString() + "ms";


        }

        private void eSuit_App_Load(object sender, EventArgs e)
        {
            _eSuit = new eSuit();

            Timer t = new Timer();
            t.Interval = 100;
            t.Tick += t_Tick;
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (_eSuit.connected())
            {
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Connected on " + _eSuit.currentPort();
                btnExecuteHit.Enabled = true;
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Disconnected";
                btnExecuteHit.Enabled = false;
            }
        }

        private void btnExecuteHit_Click(object sender, EventArgs e)
        {
            _eSuit.ExecuteHit((HitPlaces)Enum.Parse(typeof(HitPlaces), cbHitPlaces.SelectedItem.ToString()), tbVolts.Value, tbDuration.Value);
        }

        private void tbVolts_Scroll(object sender, EventArgs e)
        {
            lblVolts.Text = "Volts: " + tbVolts.Value.ToString() + "v";
        }

        private void tbDuration_Scroll(object sender, EventArgs e)
        {
            lblDuration.Text = "Duration: " + tbDuration.Value.ToString() + "ms";
        }


    }
}
