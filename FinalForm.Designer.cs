namespace Final_Kinect
{
    partial class FinalForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinalForm));
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.initializeButton = new System.Windows.Forms.Button();
            this.greenLightPictureBox = new System.Windows.Forms.PictureBox();
            this.yellowLightPictureBox = new System.Windows.Forms.PictureBox();
            this.redLightPictureBox = new System.Windows.Forms.PictureBox();
            this.sessionElapsedTimeTextBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.warningLabel = new System.Windows.Forms.Label();
            this.notAllowedLabel = new System.Windows.Forms.Label();
            this.lowerLimitSmallMovementTextBox = new System.Windows.Forms.TextBox();
            this.lowerLimitLargeMovementTextBox = new System.Windows.Forms.TextBox();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.notesLabel = new System.Windows.Forms.Label();
            this.setLimitsButton = new System.Windows.Forms.Button();
            this.movementProbeButton = new System.Windows.Forms.Button();
            this.noContingencyButton = new System.Windows.Forms.Button();
            this.instructionButton = new System.Windows.Forms.Button();
            this.shapeButton = new System.Windows.Forms.Button();
            this.currentLimitsLabel = new System.Windows.Forms.Label();
            this.sessionTimeLabel = new System.Windows.Forms.Label();
            this.conditionTimeLabel = new System.Windows.Forms.Label();
            this.conditionElapsedTimeTextBox = new System.Windows.Forms.TextBox();
            this.scanButton = new System.Windows.Forms.Button();
            this.stepUpButton = new System.Windows.Forms.Button();
            this.stepDownButton = new System.Windows.Forms.Button();
            this.bodyPictureBox = new System.Windows.Forms.PictureBox();
            this.headTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.neckTextBox = new System.Windows.Forms.TextBox();
            this.spineMidTextBox = new System.Windows.Forms.TextBox();
            this.spineShoulderTextBox = new System.Windows.Forms.TextBox();
            this.spineBaseTextBox = new System.Windows.Forms.TextBox();
            this.shoulderLeftTextBox = new System.Windows.Forms.TextBox();
            this.shoulderRightTextBox = new System.Windows.Forms.TextBox();
            this.elbowLeftTextBox = new System.Windows.Forms.TextBox();
            this.elbowRightTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenLightPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yellowLightPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redLightPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bodyPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(739, 3);
            this.axWindowsMediaPlayer1.Margin = new System.Windows.Forms.Padding(4);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(486, 435);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.stopButton);
            this.panel1.Controls.Add(this.startButton);
            this.panel1.Controls.Add(this.initializeButton);
            this.panel1.Controls.Add(this.greenLightPictureBox);
            this.panel1.Controls.Add(this.yellowLightPictureBox);
            this.panel1.Controls.Add(this.redLightPictureBox);
            this.panel1.Location = new System.Drawing.Point(2436, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 879);
            this.panel1.TabIndex = 1;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(50, 771);
            this.stopButton.Margin = new System.Windows.Forms.Padding(4);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(148, 40);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(52, 702);
            this.startButton.Margin = new System.Windows.Forms.Padding(4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(148, 40);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // initializeButton
            // 
            this.initializeButton.Location = new System.Drawing.Point(54, 833);
            this.initializeButton.Margin = new System.Windows.Forms.Padding(4);
            this.initializeButton.Name = "initializeButton";
            this.initializeButton.Size = new System.Drawing.Size(148, 40);
            this.initializeButton.TabIndex = 58;
            this.initializeButton.Text = "Initialize";
            this.initializeButton.UseVisualStyleBackColor = true;
            this.initializeButton.Click += new System.EventHandler(this.initializeButton_Click);
            // 
            // greenLightPictureBox
            // 
            this.greenLightPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("greenLightPictureBox.Image")));
            this.greenLightPictureBox.Location = new System.Drawing.Point(52, 523);
            this.greenLightPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.greenLightPictureBox.Name = "greenLightPictureBox";
            this.greenLightPictureBox.Size = new System.Drawing.Size(75, 75);
            this.greenLightPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.greenLightPictureBox.TabIndex = 2;
            this.greenLightPictureBox.TabStop = false;
            // 
            // yellowLightPictureBox
            // 
            this.yellowLightPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("yellowLightPictureBox.Image")));
            this.yellowLightPictureBox.Location = new System.Drawing.Point(52, 285);
            this.yellowLightPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.yellowLightPictureBox.Name = "yellowLightPictureBox";
            this.yellowLightPictureBox.Size = new System.Drawing.Size(75, 75);
            this.yellowLightPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.yellowLightPictureBox.TabIndex = 1;
            this.yellowLightPictureBox.TabStop = false;
            // 
            // redLightPictureBox
            // 
            this.redLightPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("redLightPictureBox.Image")));
            this.redLightPictureBox.Location = new System.Drawing.Point(52, 46);
            this.redLightPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.redLightPictureBox.Name = "redLightPictureBox";
            this.redLightPictureBox.Size = new System.Drawing.Size(75, 75);
            this.redLightPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.redLightPictureBox.TabIndex = 0;
            this.redLightPictureBox.TabStop = false;
            // 
            // sessionElapsedTimeTextBox
            // 
            this.sessionElapsedTimeTextBox.Font = new System.Drawing.Font("Times New Roman", 10.125F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionElapsedTimeTextBox.Location = new System.Drawing.Point(1634, 1325);
            this.sessionElapsedTimeTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.sessionElapsedTimeTextBox.Name = "sessionElapsedTimeTextBox";
            this.sessionElapsedTimeTextBox.Size = new System.Drawing.Size(238, 39);
            this.sessionElapsedTimeTextBox.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.progressBar.Location = new System.Drawing.Point(1444, 998);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1268, 75);
            this.progressBar.TabIndex = 2;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(2490, 1479);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(150, 34);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(2490, 1552);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(150, 40);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 4;
            this.pictureBox5.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1440, 885);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1258, 109);
            this.label9.TabIndex = 37;
            this.label9.Text = "  P     R     O     M     I     S     E";
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.Location = new System.Drawing.Point(1438, 1113);
            this.warningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(127, 31);
            this.warningLabel.TabIndex = 38;
            this.warningLabel.Text = "Warning:";
            // 
            // notAllowedLabel
            // 
            this.notAllowedLabel.AutoSize = true;
            this.notAllowedLabel.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notAllowedLabel.Location = new System.Drawing.Point(1438, 1183);
            this.notAllowedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.notAllowedLabel.Name = "notAllowedLabel";
            this.notAllowedLabel.Size = new System.Drawing.Size(167, 31);
            this.notAllowedLabel.TabIndex = 39;
            this.notAllowedLabel.Text = "Not Allowed:";
            // 
            // lowerLimitSmallMovementTextBox
            // 
            this.lowerLimitSmallMovementTextBox.Font = new System.Drawing.Font("Times New Roman", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lowerLimitSmallMovementTextBox.Location = new System.Drawing.Point(1626, 1108);
            this.lowerLimitSmallMovementTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.lowerLimitSmallMovementTextBox.Name = "lowerLimitSmallMovementTextBox";
            this.lowerLimitSmallMovementTextBox.Size = new System.Drawing.Size(148, 39);
            this.lowerLimitSmallMovementTextBox.TabIndex = 40;
            // 
            // lowerLimitLargeMovementTextBox
            // 
            this.lowerLimitLargeMovementTextBox.Font = new System.Drawing.Font("Times New Roman", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lowerLimitLargeMovementTextBox.Location = new System.Drawing.Point(1626, 1173);
            this.lowerLimitLargeMovementTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.lowerLimitLargeMovementTextBox.Name = "lowerLimitLargeMovementTextBox";
            this.lowerLimitLargeMovementTextBox.Size = new System.Drawing.Size(148, 39);
            this.lowerLimitLargeMovementTextBox.TabIndex = 41;
            // 
            // notesTextBox
            // 
            this.notesTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.notesTextBox.Location = new System.Drawing.Point(1416, 1479);
            this.notesTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.Size = new System.Drawing.Size(1030, 146);
            this.notesTextBox.TabIndex = 47;
            // 
            // notesLabel
            // 
            this.notesLabel.AutoSize = true;
            this.notesLabel.Location = new System.Drawing.Point(1410, 1448);
            this.notesLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(68, 25);
            this.notesLabel.TabIndex = 48;
            this.notesLabel.Text = "Notes";
            // 
            // setLimitsButton
            // 
            this.setLimitsButton.Location = new System.Drawing.Point(1444, 1246);
            this.setLimitsButton.Margin = new System.Windows.Forms.Padding(6);
            this.setLimitsButton.Name = "setLimitsButton";
            this.setLimitsButton.Size = new System.Drawing.Size(196, 44);
            this.setLimitsButton.TabIndex = 49;
            this.setLimitsButton.Text = "Update Limits";
            this.setLimitsButton.UseVisualStyleBackColor = true;
            this.setLimitsButton.Click += new System.EventHandler(this.setLimitsButton_Click);
            // 
            // movementProbeButton
            // 
            this.movementProbeButton.Location = new System.Drawing.Point(1818, 1108);
            this.movementProbeButton.Margin = new System.Windows.Forms.Padding(6);
            this.movementProbeButton.Name = "movementProbeButton";
            this.movementProbeButton.Size = new System.Drawing.Size(192, 44);
            this.movementProbeButton.TabIndex = 50;
            this.movementProbeButton.Text = "Movement Probe";
            this.movementProbeButton.UseVisualStyleBackColor = true;
            this.movementProbeButton.Click += new System.EventHandler(this.movementProbeButton_Click);
            // 
            // noContingencyButton
            // 
            this.noContingencyButton.Location = new System.Drawing.Point(1818, 1173);
            this.noContingencyButton.Margin = new System.Windows.Forms.Padding(6);
            this.noContingencyButton.Name = "noContingencyButton";
            this.noContingencyButton.Size = new System.Drawing.Size(192, 44);
            this.noContingencyButton.TabIndex = 51;
            this.noContingencyButton.Text = "No Contingency";
            this.noContingencyButton.UseVisualStyleBackColor = true;
            this.noContingencyButton.Click += new System.EventHandler(this.noContingencyButton_Click);
            // 
            // instructionButton
            // 
            this.instructionButton.Location = new System.Drawing.Point(2056, 1108);
            this.instructionButton.Margin = new System.Windows.Forms.Padding(6);
            this.instructionButton.Name = "instructionButton";
            this.instructionButton.Size = new System.Drawing.Size(192, 44);
            this.instructionButton.TabIndex = 52;
            this.instructionButton.Text = "Instruction";
            this.instructionButton.UseVisualStyleBackColor = true;
            this.instructionButton.Click += new System.EventHandler(this.instructionButton_Click);
            // 
            // shapeButton
            // 
            this.shapeButton.Location = new System.Drawing.Point(2056, 1171);
            this.shapeButton.Margin = new System.Windows.Forms.Padding(6);
            this.shapeButton.Name = "shapeButton";
            this.shapeButton.Size = new System.Drawing.Size(192, 44);
            this.shapeButton.TabIndex = 53;
            this.shapeButton.Text = "Shape";
            this.shapeButton.UseVisualStyleBackColor = true;
            this.shapeButton.Click += new System.EventHandler(this.shapeButton_Click);
            // 
            // currentLimitsLabel
            // 
            this.currentLimitsLabel.AutoSize = true;
            this.currentLimitsLabel.Location = new System.Drawing.Point(1674, 1256);
            this.currentLimitsLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.currentLimitsLabel.Name = "currentLimitsLabel";
            this.currentLimitsLabel.Size = new System.Drawing.Size(151, 25);
            this.currentLimitsLabel.TabIndex = 54;
            this.currentLimitsLabel.Text = "Current Limits:";
            // 
            // sessionTimeLabel
            // 
            this.sessionTimeLabel.AutoSize = true;
            this.sessionTimeLabel.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionTimeLabel.Location = new System.Drawing.Point(1438, 1331);
            this.sessionTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sessionTimeLabel.Name = "sessionTimeLabel";
            this.sessionTimeLabel.Size = new System.Drawing.Size(176, 31);
            this.sessionTimeLabel.TabIndex = 55;
            this.sessionTimeLabel.Text = "Session Time:";
            // 
            // conditionTimeLabel
            // 
            this.conditionTimeLabel.AutoSize = true;
            this.conditionTimeLabel.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conditionTimeLabel.Location = new System.Drawing.Point(1410, 1387);
            this.conditionTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.conditionTimeLabel.Name = "conditionTimeLabel";
            this.conditionTimeLabel.Size = new System.Drawing.Size(208, 31);
            this.conditionTimeLabel.TabIndex = 57;
            this.conditionTimeLabel.Text = "Condition Time:";
            // 
            // conditionElapsedTimeTextBox
            // 
            this.conditionElapsedTimeTextBox.Font = new System.Drawing.Font("Times New Roman", 10.125F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conditionElapsedTimeTextBox.Location = new System.Drawing.Point(1634, 1381);
            this.conditionElapsedTimeTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.conditionElapsedTimeTextBox.Name = "conditionElapsedTimeTextBox";
            this.conditionElapsedTimeTextBox.Size = new System.Drawing.Size(238, 39);
            this.conditionElapsedTimeTextBox.TabIndex = 56;
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(1944, 1379);
            this.scanButton.Margin = new System.Windows.Forms.Padding(4);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(148, 40);
            this.scanButton.TabIndex = 6;
            this.scanButton.Text = "Scan";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // stepUpButton
            // 
            this.stepUpButton.Location = new System.Drawing.Point(2302, 1108);
            this.stepUpButton.Margin = new System.Windows.Forms.Padding(6);
            this.stepUpButton.Name = "stepUpButton";
            this.stepUpButton.Size = new System.Drawing.Size(192, 44);
            this.stepUpButton.TabIndex = 59;
            this.stepUpButton.Text = "Step Up";
            this.stepUpButton.UseVisualStyleBackColor = true;
            this.stepUpButton.Click += new System.EventHandler(this.stepUpButton_Click);
            // 
            // stepDownButton
            // 
            this.stepDownButton.Location = new System.Drawing.Point(2302, 1173);
            this.stepDownButton.Margin = new System.Windows.Forms.Padding(6);
            this.stepDownButton.Name = "stepDownButton";
            this.stepDownButton.Size = new System.Drawing.Size(192, 44);
            this.stepDownButton.TabIndex = 60;
            this.stepDownButton.Text = "Step Down";
            this.stepDownButton.UseVisualStyleBackColor = true;
            this.stepDownButton.Click += new System.EventHandler(this.stepDownButton_Click);
            // 
            // bodyPictureBox
            // 
            this.bodyPictureBox.Location = new System.Drawing.Point(110, 52);
            this.bodyPictureBox.Margin = new System.Windows.Forms.Padding(6);
            this.bodyPictureBox.Name = "bodyPictureBox";
            this.bodyPictureBox.Size = new System.Drawing.Size(1226, 1263);
            this.bodyPictureBox.TabIndex = 61;
            this.bodyPictureBox.TabStop = false;
            this.bodyPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.bodyPictureBox_Paint);
            // 
            // headTextBox
            // 
            this.headTextBox.Location = new System.Drawing.Point(191, 1362);
            this.headTextBox.Name = "headTextBox";
            this.headTextBox.Size = new System.Drawing.Size(203, 31);
            this.headTextBox.TabIndex = 62;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 1365);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 25);
            this.label1.TabIndex = 63;
            this.label1.Text = "Head";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 1433);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 25);
            this.label2.TabIndex = 64;
            this.label2.Text = "Neck";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(479, 1504);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 25);
            this.label3.TabIndex = 65;
            this.label3.Text = "SpineBase";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(493, 1371);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 25);
            this.label4.TabIndex = 66;
            this.label4.Text = "SpineMid";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(889, 1368);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 25);
            this.label5.TabIndex = 67;
            this.label5.Text = "ShoulderLeft";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(442, 1436);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(153, 25);
            this.label6.TabIndex = 68;
            this.label6.Text = "SpineShoulder";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(903, 1567);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 25);
            this.label7.TabIndex = 69;
            this.label7.Text = "ElbowRight";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(875, 1436);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(148, 25);
            this.label8.TabIndex = 70;
            this.label8.Text = "ShoulderRight";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(917, 1498);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 25);
            this.label10.TabIndex = 71;
            this.label10.Text = "ElbowLeft";
            // 
            // neckTextBox
            // 
            this.neckTextBox.Location = new System.Drawing.Point(189, 1430);
            this.neckTextBox.Name = "neckTextBox";
            this.neckTextBox.Size = new System.Drawing.Size(203, 31);
            this.neckTextBox.TabIndex = 72;
            // 
            // spineMidTextBox
            // 
            this.spineMidTextBox.Location = new System.Drawing.Point(601, 1368);
            this.spineMidTextBox.Name = "spineMidTextBox";
            this.spineMidTextBox.Size = new System.Drawing.Size(203, 31);
            this.spineMidTextBox.TabIndex = 73;
            // 
            // spineShoulderTextBox
            // 
            this.spineShoulderTextBox.Location = new System.Drawing.Point(601, 1430);
            this.spineShoulderTextBox.Name = "spineShoulderTextBox";
            this.spineShoulderTextBox.Size = new System.Drawing.Size(203, 31);
            this.spineShoulderTextBox.TabIndex = 74;
            // 
            // spineBaseTextBox
            // 
            this.spineBaseTextBox.Location = new System.Drawing.Point(601, 1501);
            this.spineBaseTextBox.Name = "spineBaseTextBox";
            this.spineBaseTextBox.Size = new System.Drawing.Size(203, 31);
            this.spineBaseTextBox.TabIndex = 75;
            // 
            // shoulderLeftTextBox
            // 
            this.shoulderLeftTextBox.Location = new System.Drawing.Point(1029, 1365);
            this.shoulderLeftTextBox.Name = "shoulderLeftTextBox";
            this.shoulderLeftTextBox.Size = new System.Drawing.Size(203, 31);
            this.shoulderLeftTextBox.TabIndex = 76;
            // 
            // shoulderRightTextBox
            // 
            this.shoulderRightTextBox.Location = new System.Drawing.Point(1029, 1433);
            this.shoulderRightTextBox.Name = "shoulderRightTextBox";
            this.shoulderRightTextBox.Size = new System.Drawing.Size(203, 31);
            this.shoulderRightTextBox.TabIndex = 77;
            // 
            // elbowLeftTextBox
            // 
            this.elbowLeftTextBox.Location = new System.Drawing.Point(1029, 1495);
            this.elbowLeftTextBox.Name = "elbowLeftTextBox";
            this.elbowLeftTextBox.Size = new System.Drawing.Size(203, 31);
            this.elbowLeftTextBox.TabIndex = 78;
            // 
            // elbowRightTextBox
            // 
            this.elbowRightTextBox.Location = new System.Drawing.Point(1029, 1564);
            this.elbowRightTextBox.Name = "elbowRightTextBox";
            this.elbowRightTextBox.Size = new System.Drawing.Size(203, 31);
            this.elbowRightTextBox.TabIndex = 79;
            // 
            // FinalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(2898, 1783);
            this.Controls.Add(this.elbowRightTextBox);
            this.Controls.Add(this.elbowLeftTextBox);
            this.Controls.Add(this.shoulderRightTextBox);
            this.Controls.Add(this.shoulderLeftTextBox);
            this.Controls.Add(this.spineBaseTextBox);
            this.Controls.Add(this.spineShoulderTextBox);
            this.Controls.Add(this.spineMidTextBox);
            this.Controls.Add(this.neckTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.headTextBox);
            this.Controls.Add(this.bodyPictureBox);
            this.Controls.Add(this.stepDownButton);
            this.Controls.Add(this.stepUpButton);
            this.Controls.Add(this.scanButton);
            this.Controls.Add(this.conditionTimeLabel);
            this.Controls.Add(this.conditionElapsedTimeTextBox);
            this.Controls.Add(this.sessionTimeLabel);
            this.Controls.Add(this.currentLimitsLabel);
            this.Controls.Add(this.sessionElapsedTimeTextBox);
            this.Controls.Add(this.shapeButton);
            this.Controls.Add(this.instructionButton);
            this.Controls.Add(this.noContingencyButton);
            this.Controls.Add(this.movementProbeButton);
            this.Controls.Add(this.setLimitsButton);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.lowerLimitLargeMovementTextBox);
            this.Controls.Add(this.lowerLimitSmallMovementTextBox);
            this.Controls.Add(this.notAllowedLabel);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FinalForm";
            this.Text = "Version 3 (Final Version)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FinalForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FinalForm_FormClosed);
            this.Load += new System.EventHandler(this.FinalForm_Load);
            this.VisibleChanged += new System.EventHandler(this.FinalForm_Load);
            this.ParentChanged += new System.EventHandler(this.FinalForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenLightPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yellowLightPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redLightPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bodyPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox greenLightPictureBox;
        private System.Windows.Forms.PictureBox yellowLightPictureBox;
        private System.Windows.Forms.PictureBox redLightPictureBox;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox sessionElapsedTimeTextBox;
        public System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Label notAllowedLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox lowerLimitSmallMovementTextBox;
        private System.Windows.Forms.TextBox lowerLimitLargeMovementTextBox;
        private System.Windows.Forms.TextBox notesTextBox;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.Button setLimitsButton;
        private System.Windows.Forms.Button movementProbeButton;
        private System.Windows.Forms.Button noContingencyButton;
        private System.Windows.Forms.Button instructionButton;
        private System.Windows.Forms.Button shapeButton;
        private System.Windows.Forms.Label currentLimitsLabel;
        private System.Windows.Forms.Label sessionTimeLabel;
        private System.Windows.Forms.Label conditionTimeLabel;
        private System.Windows.Forms.TextBox conditionElapsedTimeTextBox;
        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.Button initializeButton;
        private System.Windows.Forms.Button stepUpButton;
        private System.Windows.Forms.Button stepDownButton;
        private System.Windows.Forms.PictureBox bodyPictureBox;
        private System.Windows.Forms.TextBox headTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox neckTextBox;
        private System.Windows.Forms.TextBox spineMidTextBox;
        private System.Windows.Forms.TextBox spineShoulderTextBox;
        private System.Windows.Forms.TextBox spineBaseTextBox;
        private System.Windows.Forms.TextBox shoulderLeftTextBox;
        private System.Windows.Forms.TextBox shoulderRightTextBox;
        private System.Windows.Forms.TextBox elbowLeftTextBox;
        private System.Windows.Forms.TextBox elbowRightTextBox;
    }
}