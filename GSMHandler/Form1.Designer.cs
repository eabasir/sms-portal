namespace GSMHandler
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.grdSim = new System.Windows.Forms.DataGridView();
            this.btnGetSim = new System.Windows.Forms.Button();
            this.btnRunService = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grdSim)).BeginInit();
            this.SuspendLayout();
            // 
            // grdSim
            // 
            this.grdSim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSim.Dock = System.Windows.Forms.DockStyle.Left;
            this.grdSim.Location = new System.Drawing.Point(0, 0);
            this.grdSim.Name = "grdSim";
            this.grdSim.Size = new System.Drawing.Size(394, 391);
            this.grdSim.TabIndex = 0;
            // 
            // btnGetSim
            // 
            this.btnGetSim.Location = new System.Drawing.Point(473, 187);
            this.btnGetSim.Name = "btnGetSim";
            this.btnGetSim.Size = new System.Drawing.Size(224, 53);
            this.btnGetSim.TabIndex = 1;
            this.btnGetSim.Text = "دریافت لیست سیم کارت";
            this.btnGetSim.UseVisualStyleBackColor = true;
            this.btnGetSim.Click += new System.EventHandler(this.btnGetSim_Click);
            // 
            // btnRunService
            // 
            this.btnRunService.Location = new System.Drawing.Point(473, 267);
            this.btnRunService.Name = "btnRunService";
            this.btnRunService.Size = new System.Drawing.Size(224, 48);
            this.btnRunService.TabIndex = 2;
            this.btnRunService.Text = "راه اندازی سرویس";
            this.btnRunService.UseVisualStyleBackColor = true;
            this.btnRunService.Click += new System.EventHandler(this.btnRunService_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(536, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Copy Right @2017";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Location = new System.Drawing.Point(539, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(83, 128);
            this.panel1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 391);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRunService);
            this.Controls.Add(this.btnGetSim);
            this.Controls.Add(this.grdSim);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "سرویس ارتباط با مودم GSM";
            ((System.ComponentModel.ISupportInitialize)(this.grdSim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdSim;
        private System.Windows.Forms.Button btnGetSim;
        private System.Windows.Forms.Button btnRunService;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
    }
}

