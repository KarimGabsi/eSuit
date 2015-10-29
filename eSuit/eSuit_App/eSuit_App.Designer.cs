namespace eSuit_App
{
    partial class eSuit_App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbHitPlaces = new System.Windows.Forms.ComboBox();
            this.tbVolts = new System.Windows.Forms.TrackBar();
            this.tbDuration = new System.Windows.Forms.TrackBar();
            this.lblHitPlaces = new System.Windows.Forms.Label();
            this.lblVolts = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.btnExecuteHit = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // cbHitPlaces
            // 
            this.cbHitPlaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbHitPlaces.FormattingEnabled = true;
            this.cbHitPlaces.Location = new System.Drawing.Point(12, 41);
            this.cbHitPlaces.Name = "cbHitPlaces";
            this.cbHitPlaces.Size = new System.Drawing.Size(260, 21);
            this.cbHitPlaces.TabIndex = 0;
            // 
            // tbVolts
            // 
            this.tbVolts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVolts.Location = new System.Drawing.Point(12, 104);
            this.tbVolts.Name = "tbVolts";
            this.tbVolts.Size = new System.Drawing.Size(260, 45);
            this.tbVolts.TabIndex = 1;
            this.tbVolts.Scroll += new System.EventHandler(this.tbVolts_Scroll);
            // 
            // tbDuration
            // 
            this.tbDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDuration.Location = new System.Drawing.Point(12, 179);
            this.tbDuration.Name = "tbDuration";
            this.tbDuration.Size = new System.Drawing.Size(260, 45);
            this.tbDuration.TabIndex = 2;
            this.tbDuration.Scroll += new System.EventHandler(this.tbDuration_Scroll);
            // 
            // lblHitPlaces
            // 
            this.lblHitPlaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHitPlaces.AutoSize = true;
            this.lblHitPlaces.Location = new System.Drawing.Point(108, 25);
            this.lblHitPlaces.Name = "lblHitPlaces";
            this.lblHitPlaces.Size = new System.Drawing.Size(55, 13);
            this.lblHitPlaces.TabIndex = 3;
            this.lblHitPlaces.Text = "Hit Places";
            // 
            // lblVolts
            // 
            this.lblVolts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVolts.AutoSize = true;
            this.lblVolts.Location = new System.Drawing.Point(108, 88);
            this.lblVolts.Name = "lblVolts";
            this.lblVolts.Size = new System.Drawing.Size(30, 13);
            this.lblVolts.TabIndex = 4;
            this.lblVolts.Text = "Volts";
            // 
            // lblDuration
            // 
            this.lblDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(108, 163);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(47, 13);
            this.lblDuration.TabIndex = 5;
            this.lblDuration.Text = "Duration";
            // 
            // btnExecuteHit
            // 
            this.btnExecuteHit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecuteHit.Location = new System.Drawing.Point(12, 226);
            this.btnExecuteHit.Name = "btnExecuteHit";
            this.btnExecuteHit.Size = new System.Drawing.Size(75, 23);
            this.btnExecuteHit.TabIndex = 6;
            this.btnExecuteHit.Text = "Hit";
            this.btnExecuteHit.UseVisualStyleBackColor = true;
            this.btnExecuteHit.Click += new System.EventHandler(this.btnExecuteHit_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(93, 231);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status";
            // 
            // eSuit_App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnExecuteHit);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.lblVolts);
            this.Controls.Add(this.lblHitPlaces);
            this.Controls.Add(this.tbDuration);
            this.Controls.Add(this.tbVolts);
            this.Controls.Add(this.cbHitPlaces);
            this.Name = "eSuit_App";
            this.Text = "eSuit App";
            this.Load += new System.EventHandler(this.eSuit_App_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbVolts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbHitPlaces;
        private System.Windows.Forms.TrackBar tbVolts;
        private System.Windows.Forms.TrackBar tbDuration;
        private System.Windows.Forms.Label lblHitPlaces;
        private System.Windows.Forms.Label lblVolts;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Button btnExecuteHit;
        private System.Windows.Forms.Label lblStatus;
    }
}

