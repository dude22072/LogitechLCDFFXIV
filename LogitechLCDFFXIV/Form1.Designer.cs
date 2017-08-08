namespace LogitechLCDFFXIV
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
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblCharName = new System.Windows.Forms.Label();
            this.lblCharFirst = new System.Windows.Forms.Label();
            this.lblCharLast = new System.Windows.Forms.Label();
            this.txtCharFirst = new System.Windows.Forms.TextBox();
            this.txtCharLast = new System.Windows.Forms.TextBox();
            this.pnlDXVersion = new System.Windows.Forms.Panel();
            this.rbDX11 = new System.Windows.Forms.RadioButton();
            this.rbDX9 = new System.Windows.Forms.RadioButton();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cbTray = new System.Windows.Forms.CheckBox();
            this.timerUpdateLCD = new System.Windows.Forms.Timer(this.components);
            this.timerAnimations = new System.Windows.Forms.Timer(this.components);
            this.pnlDXVersion.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.Text = "FFXI LCD Applet";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // lblCharName
            // 
            this.lblCharName.AutoSize = true;
            this.lblCharName.Location = new System.Drawing.Point(13, 13);
            this.lblCharName.Name = "lblCharName";
            this.lblCharName.Size = new System.Drawing.Size(140, 20);
            this.lblCharName.TabIndex = 0;
            this.lblCharName.Text = "Character\'s Name:";
            // 
            // lblCharFirst
            // 
            this.lblCharFirst.AutoSize = true;
            this.lblCharFirst.Location = new System.Drawing.Point(32, 43);
            this.lblCharFirst.Name = "lblCharFirst";
            this.lblCharFirst.Size = new System.Drawing.Size(44, 20);
            this.lblCharFirst.TabIndex = 1;
            this.lblCharFirst.Text = "First:";
            // 
            // lblCharLast
            // 
            this.lblCharLast.AutoSize = true;
            this.lblCharLast.Location = new System.Drawing.Point(32, 70);
            this.lblCharLast.Name = "lblCharLast";
            this.lblCharLast.Size = new System.Drawing.Size(44, 20);
            this.lblCharLast.TabIndex = 2;
            this.lblCharLast.Text = "Last:";
            // 
            // txtCharFirst
            // 
            this.txtCharFirst.Location = new System.Drawing.Point(82, 37);
            this.txtCharFirst.Name = "txtCharFirst";
            this.txtCharFirst.Size = new System.Drawing.Size(131, 26);
            this.txtCharFirst.TabIndex = 3;
            // 
            // txtCharLast
            // 
            this.txtCharLast.Location = new System.Drawing.Point(82, 70);
            this.txtCharLast.Name = "txtCharLast";
            this.txtCharLast.Size = new System.Drawing.Size(131, 26);
            this.txtCharLast.TabIndex = 4;
            // 
            // pnlDXVersion
            // 
            this.pnlDXVersion.Controls.Add(this.rbDX11);
            this.pnlDXVersion.Controls.Add(this.rbDX9);
            this.pnlDXVersion.Location = new System.Drawing.Point(268, 37);
            this.pnlDXVersion.Name = "pnlDXVersion";
            this.pnlDXVersion.Size = new System.Drawing.Size(159, 36);
            this.pnlDXVersion.TabIndex = 5;
            // 
            // rbDX11
            // 
            this.rbDX11.AutoSize = true;
            this.rbDX11.Location = new System.Drawing.Point(76, 4);
            this.rbDX11.Name = "rbDX11";
            this.rbDX11.Size = new System.Drawing.Size(75, 24);
            this.rbDX11.TabIndex = 1;
            this.rbDX11.Text = "DX11";
            this.rbDX11.UseVisualStyleBackColor = true;
            this.rbDX11.CheckedChanged += new System.EventHandler(this.rbDX11_CheckedChanged);
            // 
            // rbDX9
            // 
            this.rbDX9.AutoSize = true;
            this.rbDX9.Checked = true;
            this.rbDX9.Location = new System.Drawing.Point(4, 4);
            this.rbDX9.Name = "rbDX9";
            this.rbDX9.Size = new System.Drawing.Size(66, 24);
            this.rbDX9.TabIndex = 0;
            this.rbDX9.TabStop = true;
            this.rbDX9.Text = "DX9";
            this.rbDX9.UseVisualStyleBackColor = true;
            this.rbDX9.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(337, 107);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(90, 35);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cbTray
            // 
            this.cbTray.AutoSize = true;
            this.cbTray.Checked = true;
            this.cbTray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTray.Location = new System.Drawing.Point(9, 113);
            this.cbTray.Name = "cbTray";
            this.cbTray.Size = new System.Drawing.Size(144, 24);
            this.cbTray.TabIndex = 7;
            this.cbTray.Text = "Minimize to tray";
            this.cbTray.UseVisualStyleBackColor = true;
            // 
            // timerUpdateLCD
            // 
            this.timerUpdateLCD.Enabled = true;
            this.timerUpdateLCD.Tick += new System.EventHandler(this.timerUpdateLCD_Tick);
            // 
            // timerAnimations
            // 
            this.timerAnimations.Interval = 3000;
            this.timerAnimations.Tick += new System.EventHandler(this.timerAnimations_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 156);
            this.Controls.Add(this.cbTray);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.pnlDXVersion);
            this.Controls.Add(this.txtCharLast);
            this.Controls.Add(this.txtCharFirst);
            this.Controls.Add(this.lblCharLast);
            this.Controls.Add(this.lblCharFirst);
            this.Controls.Add(this.lblCharName);
            this.Name = "Form1";
            this.Text = "Dude22072\'s FFXIV Logitech GamePanel App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_OnClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.pnlDXVersion.ResumeLayout(false);
            this.pnlDXVersion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Label lblCharName;
        private System.Windows.Forms.Label lblCharFirst;
        private System.Windows.Forms.Label lblCharLast;
        private System.Windows.Forms.TextBox txtCharFirst;
        private System.Windows.Forms.TextBox txtCharLast;
        private System.Windows.Forms.Panel pnlDXVersion;
        private System.Windows.Forms.RadioButton rbDX9;
        private System.Windows.Forms.RadioButton rbDX11;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckBox cbTray;
        private System.Windows.Forms.Timer timerUpdateLCD;
        private System.Windows.Forms.Timer timerAnimations;
    }
}

