namespace TSWMod
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblRDStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoadRDCalibration = new System.Windows.Forms.Button();
            this.lblTSWStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupTSW = new System.Windows.Forms.GroupBox();
            this.lblMap = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panelRdControls = new System.Windows.Forms.Panel();
            this.lblIndependentBrakeValue = new System.Windows.Forms.Label();
            this.lblAutoBrakeValue = new System.Windows.Forms.Label();
            this.lblThrottleValue = new System.Windows.Forms.Label();
            this.lblReverserValue = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLoadCalibrationPrompt = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblActiveTrain = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupTSW.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelRdControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblRDStatus);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnLoadRDCalibration);
            this.groupBox1.Controls.Add(this.lblTSWStatus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // lblRDStatus
            // 
            this.lblRDStatus.AutoSize = true;
            this.lblRDStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblRDStatus.Location = new System.Drawing.Point(343, 22);
            this.lblRDStatus.Name = "lblRDStatus";
            this.lblRDStatus.Size = new System.Drawing.Size(80, 20);
            this.lblRDStatus.TabIndex = 4;
            this.lblRDStatus.Text = "RDStatus";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(259, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Raildriver:";
            // 
            // btnLoadRDCalibration
            // 
            this.btnLoadRDCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnLoadRDCalibration.Location = new System.Drawing.Point(570, 17);
            this.btnLoadRDCalibration.Name = "btnLoadRDCalibration";
            this.btnLoadRDCalibration.Size = new System.Drawing.Size(195, 30);
            this.btnLoadRDCalibration.TabIndex = 2;
            this.btnLoadRDCalibration.Text = "Load RailDriver Calibration";
            this.btnLoadRDCalibration.UseVisualStyleBackColor = true;
            this.btnLoadRDCalibration.Click += new System.EventHandler(this.btnLoadRDCalibration_Click);
            // 
            // lblTSWStatus
            // 
            this.lblTSWStatus.AutoSize = true;
            this.lblTSWStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblTSWStatus.Location = new System.Drawing.Point(60, 22);
            this.lblTSWStatus.Name = "lblTSWStatus";
            this.lblTSWStatus.Size = new System.Drawing.Size(91, 20);
            this.lblTSWStatus.TabIndex = 1;
            this.lblTSWStatus.Text = "TSWStatus";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "TSW:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupTSW
            // 
            this.groupTSW.Controls.Add(this.lblActiveTrain);
            this.groupTSW.Controls.Add(this.label7);
            this.groupTSW.Controls.Add(this.lblMap);
            this.groupTSW.Location = new System.Drawing.Point(12, 73);
            this.groupTSW.Name = "groupTSW";
            this.groupTSW.Size = new System.Drawing.Size(776, 73);
            this.groupTSW.TabIndex = 1;
            this.groupTSW.TabStop = false;
            this.groupTSW.Text = "TSW";
            // 
            // lblMap
            // 
            this.lblMap.AutoSize = true;
            this.lblMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblMap.Location = new System.Drawing.Point(10, 20);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(37, 18);
            this.lblMap.TabIndex = 0;
            this.lblMap.Text = "Map";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelRdControls);
            this.groupBox2.Controls.Add(this.lblLoadCalibrationPrompt);
            this.groupBox2.Location = new System.Drawing.Point(12, 152);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 73);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RailDriver";
            // 
            // panelRdControls
            // 
            this.panelRdControls.Controls.Add(this.lblIndependentBrakeValue);
            this.panelRdControls.Controls.Add(this.lblAutoBrakeValue);
            this.panelRdControls.Controls.Add(this.lblThrottleValue);
            this.panelRdControls.Controls.Add(this.lblReverserValue);
            this.panelRdControls.Controls.Add(this.label6);
            this.panelRdControls.Controls.Add(this.label5);
            this.panelRdControls.Controls.Add(this.label4);
            this.panelRdControls.Controls.Add(this.label2);
            this.panelRdControls.Location = new System.Drawing.Point(10, 12);
            this.panelRdControls.Name = "panelRdControls";
            this.panelRdControls.Size = new System.Drawing.Size(760, 55);
            this.panelRdControls.TabIndex = 1;
            // 
            // lblIndependentBrakeValue
            // 
            this.lblIndependentBrakeValue.AutoSize = true;
            this.lblIndependentBrakeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblIndependentBrakeValue.Location = new System.Drawing.Point(442, 24);
            this.lblIndependentBrakeValue.Name = "lblIndependentBrakeValue";
            this.lblIndependentBrakeValue.Size = new System.Drawing.Size(18, 20);
            this.lblIndependentBrakeValue.TabIndex = 7;
            this.lblIndependentBrakeValue.Text = "0";
            // 
            // lblAutoBrakeValue
            // 
            this.lblAutoBrakeValue.AutoSize = true;
            this.lblAutoBrakeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblAutoBrakeValue.Location = new System.Drawing.Point(315, 24);
            this.lblAutoBrakeValue.Name = "lblAutoBrakeValue";
            this.lblAutoBrakeValue.Size = new System.Drawing.Size(18, 20);
            this.lblAutoBrakeValue.TabIndex = 6;
            this.lblAutoBrakeValue.Text = "0";
            // 
            // lblThrottleValue
            // 
            this.lblThrottleValue.AutoSize = true;
            this.lblThrottleValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblThrottleValue.Location = new System.Drawing.Point(134, 24);
            this.lblThrottleValue.Name = "lblThrottleValue";
            this.lblThrottleValue.Size = new System.Drawing.Size(18, 20);
            this.lblThrottleValue.TabIndex = 5;
            this.lblThrottleValue.Text = "0";
            // 
            // lblReverserValue
            // 
            this.lblReverserValue.AutoSize = true;
            this.lblReverserValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblReverserValue.Location = new System.Drawing.Point(18, 24);
            this.lblReverserValue.Name = "lblReverserValue";
            this.lblReverserValue.Size = new System.Drawing.Size(18, 20);
            this.lblReverserValue.TabIndex = 4;
            this.lblReverserValue.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(443, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Independent Brake";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(316, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Auto Brake";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Throttle/Dynamic Brake";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Reverser";
            // 
            // lblLoadCalibrationPrompt
            // 
            this.lblLoadCalibrationPrompt.AutoSize = true;
            this.lblLoadCalibrationPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblLoadCalibrationPrompt.Location = new System.Drawing.Point(271, 31);
            this.lblLoadCalibrationPrompt.Name = "lblLoadCalibrationPrompt";
            this.lblLoadCalibrationPrompt.Size = new System.Drawing.Size(197, 18);
            this.lblLoadCalibrationPrompt.TabIndex = 0;
            this.lblLoadCalibrationPrompt.Text = "Please Load Calibration First";
            this.lblLoadCalibrationPrompt.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label7.Location = new System.Drawing.Point(9, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 18);
            this.label7.TabIndex = 1;
            this.label7.Text = "Active Train:";
            // 
            // lblActiveTrain
            // 
            this.lblActiveTrain.AutoSize = true;
            this.lblActiveTrain.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblActiveTrain.Location = new System.Drawing.Point(103, 44);
            this.lblActiveTrain.Name = "lblActiveTrain";
            this.lblActiveTrain.Size = new System.Drawing.Size(80, 18);
            this.lblActiveTrain.TabIndex = 2;
            this.lblActiveTrain.Text = "ActiveTrain";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 615);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupTSW);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "TSW Raildriver Connector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupTSW.ResumeLayout(false);
            this.groupTSW.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panelRdControls.ResumeLayout(false);
            this.panelRdControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblRDStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoadRDCalibration;
        private System.Windows.Forms.Label lblTSWStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupTSW;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblLoadCalibrationPrompt;
        private System.Windows.Forms.Panel panelRdControls;
        private System.Windows.Forms.Label lblIndependentBrakeValue;
        private System.Windows.Forms.Label lblAutoBrakeValue;
        private System.Windows.Forms.Label lblThrottleValue;
        private System.Windows.Forms.Label lblReverserValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblActiveTrain;
        private System.Windows.Forms.Label label7;
    }
}

