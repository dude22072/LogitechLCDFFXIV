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
            // pnlDXVersion
            // 
            this.pnlDXVersion.Controls.Add(this.rbDX11);
            this.pnlDXVersion.Controls.Add(this.rbDX9);
            this.pnlDXVersion.Location = new System.Drawing.Point(12, 12);
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
            this.btnConnect.Location = new System.Drawing.Point(177, 13);
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
            this.cbTray.Location = new System.Drawing.Point(16, 54);
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
            this.ClientSize = new System.Drawing.Size(280, 99);
            this.Controls.Add(this.cbTray);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.pnlDXVersion);
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
        private System.Windows.Forms.Panel pnlDXVersion;
        private System.Windows.Forms.RadioButton rbDX9;
        private System.Windows.Forms.RadioButton rbDX11;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckBox cbTray;
        private System.Windows.Forms.Timer timerUpdateLCD;
        private System.Windows.Forms.Timer timerAnimations;
    }
}

