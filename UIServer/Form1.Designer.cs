namespace UIServer
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnReadInbox = new System.Windows.Forms.Button();
            this.grdInbox = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblCharCount = new System.Windows.Forms.Label();
            this.txtWorkingIndex = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFinishIndex = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtStartIndex = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnReadCurrentMessage = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnReadAtivation = new System.Windows.Forms.Button();
            this.btnReadNumbers = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.grdInfo = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnUSSDRead = new System.Windows.Forms.Button();
            this.btnSendUSSD = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUSSDAnswer = new System.Windows.Forms.TextBox();
            this.txtUSSDRequest = new System.Windows.Forms.TextBox();
            this.txtSimNumber = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInbox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.btnReadInbox);
            this.tabPage2.Controls.Add(this.grdInbox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(855, 444);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Inbox";
            // 
            // btnReadInbox
            // 
            this.btnReadInbox.Location = new System.Drawing.Point(673, 375);
            this.btnReadInbox.Name = "btnReadInbox";
            this.btnReadInbox.Size = new System.Drawing.Size(128, 23);
            this.btnReadInbox.TabIndex = 34;
            this.btnReadInbox.Text = "Read Last Message";
            this.btnReadInbox.UseVisualStyleBackColor = true;
            this.btnReadInbox.Click += new System.EventHandler(this.btnReadInbox_Click);
            // 
            // grdInbox
            // 
            this.grdInbox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInbox.Location = new System.Drawing.Point(29, 21);
            this.grdInbox.Name = "grdInbox";
            this.grdInbox.Size = new System.Drawing.Size(772, 333);
            this.grdInbox.TabIndex = 32;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(25, 75);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(863, 470);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.lblCharCount);
            this.tabPage1.Controls.Add(this.txtWorkingIndex);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.txtFinishIndex);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.txtStartIndex);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.btnActivate);
            this.tabPage1.Controls.Add(this.btnSend);
            this.tabPage1.Controls.Add(this.btnReadCurrentMessage);
            this.tabPage1.Controls.Add(this.txtMessage);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.btnReadAtivation);
            this.tabPage1.Controls.Add(this.btnReadNumbers);
            this.tabPage1.Controls.Add(this.btnRegister);
            this.tabPage1.Controls.Add(this.grdInfo);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(855, 444);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Send";
            // 
            // lblCharCount
            // 
            this.lblCharCount.AutoSize = true;
            this.lblCharCount.Location = new System.Drawing.Point(289, 403);
            this.lblCharCount.Name = "lblCharCount";
            this.lblCharCount.Size = new System.Drawing.Size(71, 13);
            this.lblCharCount.TabIndex = 55;
            this.lblCharCount.Text = "تعداد کاراکتر:";
            // 
            // txtWorkingIndex
            // 
            this.txtWorkingIndex.Location = new System.Drawing.Point(788, 176);
            this.txtWorkingIndex.Name = "txtWorkingIndex";
            this.txtWorkingIndex.Size = new System.Drawing.Size(56, 20);
            this.txtWorkingIndex.TabIndex = 54;
            this.txtWorkingIndex.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(721, 178);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 53;
            this.label11.Text = "Working:";
            // 
            // txtFinishIndex
            // 
            this.txtFinishIndex.Location = new System.Drawing.Point(788, 149);
            this.txtFinishIndex.Name = "txtFinishIndex";
            this.txtFinishIndex.Size = new System.Drawing.Size(56, 20);
            this.txtFinishIndex.TabIndex = 52;
            this.txtFinishIndex.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(721, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 51;
            this.label10.Text = "Finish";
            // 
            // txtStartIndex
            // 
            this.txtStartIndex.Location = new System.Drawing.Point(788, 123);
            this.txtStartIndex.Name = "txtStartIndex";
            this.txtStartIndex.Size = new System.Drawing.Size(56, 20);
            this.txtStartIndex.TabIndex = 50;
            this.txtStartIndex.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(721, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Start:";
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(557, 398);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(75, 23);
            this.btnActivate.TabIndex = 48;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(638, 398);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 47;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnReadCurrentMessage
            // 
            this.btnReadCurrentMessage.Location = new System.Drawing.Point(719, 269);
            this.btnReadCurrentMessage.Name = "btnReadCurrentMessage";
            this.btnReadCurrentMessage.Size = new System.Drawing.Size(128, 25);
            this.btnReadCurrentMessage.TabIndex = 46;
            this.btnReadCurrentMessage.Text = "Read Current Message";
            this.btnReadCurrentMessage.UseVisualStyleBackColor = true;
            this.btnReadCurrentMessage.Click += new System.EventHandler(this.btnReadCurrentMessage_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(18, 269);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtMessage.Size = new System.Drawing.Size(695, 99);
            this.txtMessage.TabIndex = 44;
            this.txtMessage.Text = "سلام این یک تست است.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 243);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 45;
            this.label8.Text = "Message:";
            // 
            // btnReadAtivation
            // 
            this.btnReadAtivation.Location = new System.Drawing.Point(719, 93);
            this.btnReadAtivation.Name = "btnReadAtivation";
            this.btnReadAtivation.Size = new System.Drawing.Size(128, 23);
            this.btnReadAtivation.TabIndex = 36;
            this.btnReadAtivation.Text = "Read Activation Status";
            this.btnReadAtivation.UseVisualStyleBackColor = true;
            this.btnReadAtivation.MouseCaptureChanged += new System.EventHandler(this.btnReadAtivation_Click);
            // 
            // btnReadNumbers
            // 
            this.btnReadNumbers.Location = new System.Drawing.Point(719, 51);
            this.btnReadNumbers.Name = "btnReadNumbers";
            this.btnReadNumbers.Size = new System.Drawing.Size(128, 23);
            this.btnReadNumbers.TabIndex = 33;
            this.btnReadNumbers.Text = "Read Writen Numbers";
            this.btnReadNumbers.UseVisualStyleBackColor = true;
            this.btnReadNumbers.Click += new System.EventHandler(this.btnReadNumbers_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(422, 398);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(129, 23);
            this.btnRegister.TabIndex = 32;
            this.btnRegister.Text = "Register Numbers";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // grdInfo
            // 
            this.grdInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInfo.Location = new System.Drawing.Point(18, 51);
            this.grdInfo.Name = "grdInfo";
            this.grdInfo.Size = new System.Drawing.Size(695, 177);
            this.grdInfo.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(175, 398);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 29;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Controls.Add(this.btnUSSDRead);
            this.tabPage3.Controls.Add(this.btnSendUSSD);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.txtUSSDAnswer);
            this.tabPage3.Controls.Add(this.txtUSSDRequest);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(855, 444);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "USSD";
            // 
            // btnUSSDRead
            // 
            this.btnUSSDRead.Location = new System.Drawing.Point(334, 361);
            this.btnUSSDRead.Name = "btnUSSDRead";
            this.btnUSSDRead.Size = new System.Drawing.Size(75, 23);
            this.btnUSSDRead.TabIndex = 43;
            this.btnUSSDRead.Text = "Read";
            this.btnUSSDRead.UseVisualStyleBackColor = true;
            this.btnUSSDRead.Click += new System.EventHandler(this.btnUSSDRead_Click);
            // 
            // btnSendUSSD
            // 
            this.btnSendUSSD.Location = new System.Drawing.Point(431, 361);
            this.btnSendUSSD.Name = "btnSendUSSD";
            this.btnSendUSSD.Size = new System.Drawing.Size(75, 23);
            this.btnSendUSSD.TabIndex = 42;
            this.btnSendUSSD.Text = "Send";
            this.btnSendUSSD.UseVisualStyleBackColor = true;
            this.btnSendUSSD.Click += new System.EventHandler(this.btnSendUSSD_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Answer:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Request:";
            // 
            // txtUSSDAnswer
            // 
            this.txtUSSDAnswer.Location = new System.Drawing.Point(105, 105);
            this.txtUSSDAnswer.Multiline = true;
            this.txtUSSDAnswer.Name = "txtUSSDAnswer";
            this.txtUSSDAnswer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtUSSDAnswer.Size = new System.Drawing.Size(401, 235);
            this.txtUSSDAnswer.TabIndex = 1;
            this.txtUSSDAnswer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUSSDRequest
            // 
            this.txtUSSDRequest.Location = new System.Drawing.Point(105, 57);
            this.txtUSSDRequest.Name = "txtUSSDRequest";
            this.txtUSSDRequest.Size = new System.Drawing.Size(401, 20);
            this.txtUSSDRequest.TabIndex = 0;
            this.txtUSSDRequest.Text = "*140*11#";
            // 
            // txtSimNumber
            // 
            this.txtSimNumber.Location = new System.Drawing.Point(199, 46);
            this.txtSimNumber.Name = "txtSimNumber";
            this.txtSimNumber.Size = new System.Drawing.Size(56, 20);
            this.txtSimNumber.TabIndex = 41;
            this.txtSimNumber.Text = "1";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(58, 45);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 20);
            this.txtPort.TabIndex = 37;
            this.txtPort.Text = "3000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "SIM:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(667, 46);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 39;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(287, 52);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 13);
            this.lblStatus.TabIndex = 38;
            this.lblStatus.Text = "Not Connected";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Port:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 565);
            this.Controls.Add(this.txtSimNumber);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SMS Portal Modem Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInbox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtSimNumber;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSendUSSD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUSSDAnswer;
        private System.Windows.Forms.TextBox txtUSSDRequest;
        private System.Windows.Forms.Button btnUSSDRead;
        private System.Windows.Forms.DataGridView grdInbox;
        private System.Windows.Forms.Button btnReadInbox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblCharCount;
        private System.Windows.Forms.TextBox txtWorkingIndex;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtFinishIndex;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtStartIndex;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnReadCurrentMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnReadAtivation;
        private System.Windows.Forms.Button btnReadNumbers;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.DataGridView grdInfo;
        private System.Windows.Forms.Label label3;
    }
}

