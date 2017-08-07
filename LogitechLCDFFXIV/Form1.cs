using Sharlayan;
using Sharlayan.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

#if DEBUG
using System.Diagnostics;
#endif

namespace LogitechLCDFFXIV
{
    public partial class Form1 : Form
    {
        Boolean dx11 = false, isDoingAnimation = false;
        String first, last;
        private static FFXIV ffxiv;
        private FFXIV.Character charInfo;
        double currentHP, maxHP, currentMP, maxMP, currentTP, maxTP, currentCP, maxCP, currentGP, maxGP, expGLD, expPGL, expMRD, expLNC, expARC, expROG, expCNJ, expTHM, expACN, expCPT, expBSM, expARM, expGSM, expLTW, expWVR, expALC, expCUL, expMIN, expBTN, expFSH, expDRK, expAST, expMCH, expSAM, expRDM;
        byte job = 21, pjob, level = 99, plevel, title, levelGLD, levelPGL, levelMRD, levelLNC, levelARC, levelROG, levelCNJ, levelTHM, levelACN, levelCPT, levelBSM, levelARM, levelGSM, levelLTW, levelWVR, levelALC, levelCUL, levelMIN, levelBTN, levelFSH, levelDRK, levelAST, levelMCH, levelSAM, levelRDM;
        double x, y, z;
        /*strings for when a tell is recived*/
        public static volatile string tellUser, tellMessage;
        /*other ints*/
        static int currentDisplayMode = -1, maxDisplayMode = 3, curentScrollIndex = 0, maxScrollIndex = 0;

        byte[] test = new byte[320 * 240 * 4];

        public static double map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trayIcon.BalloonTipText = "Running in tray. Double click tray icon to maximize.";
            trayIcon.BalloonTipTitle = "FFXIV LCD Applet";
            trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            trayIcon.Icon = this.Icon;

            for (int iter = 0; iter < test.Length/4; iter++)
            {
                test[4*iter + 0] = 255; //Blue
                test[4*iter + 1] = 255; //Green
                test[4*iter + 2] = 255; //Red
                test[4*iter + 3] = 064; //Alpha
                
            }
            
        }

        private void Form1_OnClosing(object sender, FormClosingEventArgs e)
        {
            trayIcon.Visible = false;
            LogitechLCD.LogiLcdShutdown();

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                if (cbTray.Checked)
                {
                    trayIcon.Visible = true;
                    trayIcon.ShowBalloonTip(250);
                    this.Hide();
                }
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                trayIcon.Visible = false;
            }
        }

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            ShowWindow(this.Handle, 0x09);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dx11 = false;
        }

        private void rbDX11_CheckedChanged(object sender, EventArgs e)
        {
            dx11 = true;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (LogitechLCD.LogiLcdInit("FFXIV Color", LogitechLCD.LcdType.Color))
            {
                btnMono.Enabled = false;
                btnColor.Enabled = false;
            }
        }

        public static byte[] ImageToBGRA(Image imageIn)
        {
            Bitmap useme = new Bitmap(imageIn);
            byte[] bgra = new byte[307200];
            for (int y = 0; y < 240; y++)
            {
                for (int x = 0; x < 320; x++)
                {
                    Color pxl = useme.GetPixel(x, y);
                    bgra[(y * (320*4)) + x*4 + 0] = pxl.B;
                    bgra[(y * (320*4)) + x*4 + 1] = pxl.G;
                    bgra[(y * (320*4)) + x*4 + 2] = pxl.R;
                    bgra[(y * (320*4)) + x*4 + 3] = pxl.A;
                }
            }
            return bgra;
        }

        private void btnMono_Click(object sender, EventArgs e)
        {
            if(LogitechLCD.LogiLcdInit("FFXIV Mono", LogitechLCD.LcdType.Mono))
            {
                btnMono.Enabled = false;
                btnColor.Enabled = false;
            }  
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ffxiv = new FFXIV(dx11);
            if(ffxiv._initiated)
            {
                charInfo = new FFXIV.Character(txtCharFirst.Text, txtCharLast.Text);
                if (charInfo.stats != null)
                {
                    btnConnect.Enabled = false;
                    first = txtCharFirst.Text;
                    last = txtCharLast.Text;
                    txtCharFirst.Enabled = false;
                    txtCharLast.Enabled = false;
                    System.Timers.Timer infoTimer = new System.Timers.Timer();
                    infoTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetCharacterInfo);
                    infoTimer.Interval = 1000;
                    infoTimer.Enabled = true;
                    infoTimer.Start();
                }
                else
                {
                    MessageBox.Show("Could not find character \"" + txtCharFirst.Text + " " + txtCharLast.Text + "\".", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            else
            {
                MessageBox.Show("Could not find FFXIV instance.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void GetCharacterInfo(object source, System.Timers.ElapsedEventArgs e)
        {
            FFXIV.Character charInfo = new FFXIV.Character(first, last);
            PlayerInfoReadResult readResult = Reader.GetPlayerInfo();
            Sharlayan.Core.PlayerEntity player = readResult.PlayerEntity;
            InventoryReadResult readResultInv = Reader.GetInventoryItems();
            List<Sharlayan.Core.InventoryEntity> Inventories = readResultInv.InventoryEntities;
#if DEBUG
            Debug.WriteLine(Inventories.ToArray().Length.ToString());
            Debug.WriteLine(Inventories.ToArray().ToString());
#endif
            currentHP = charInfo.stats.HPCurrent;
            maxHP = charInfo.stats.HPMax;
            currentMP = charInfo.stats.MPCurrent;
            maxMP = charInfo.stats.MPMax;
            currentTP = charInfo.stats.TPCurrent;
            maxTP = charInfo.stats.TPMax;
            currentCP = charInfo.stats.CPCurrent;
            maxCP = charInfo.stats.CPMax;
            currentGP = charInfo.stats.GPCurrent;
            maxGP = charInfo.stats.GPMax;
            job = charInfo.stats.JobID;
            level = charInfo.stats.Level;
            x = charInfo.stats.X;
            y = charInfo.stats.Y;
            z = charInfo.stats.Z;
            title = charInfo.stats.Title;

            #region EXP
            expGLD = player.GLD_CurrentEXP;
            expPGL = player.PGL_CurrentEXP;
            expMRD = player.MRD_CurrentEXP;
            expLNC = player.LNC_CurrentEXP;
            expARC = player.ARC_CurrentEXP;
            expROG = player.ROG_CurrentEXP;
            expCNJ = player.CNJ_CurrentEXP;
            expTHM = player.THM_CurrentEXP;
            expACN = player.ACN_CurrentEXP;
            expCPT = player.CPT_CurrentEXP;
            expBSM = player.BSM_CurrentEXP;
            expARM = player.ARM_CurrentEXP;
            expGSM = player.GSM_CurrentEXP;
            expLTW = player.LTW_CurrentEXP;
            expWVR = player.WVR_CurrentEXP;
            expALC = player.ALC_CurrentEXP;
            expCUL = player.CUL_CurrentEXP;
            expMIN = player.MIN_CurrentEXP;
            expBTN = player.BTN_CurrentEXP;
            expFSH = player.FSH_CurrentEXP;
            expDRK = player.DRK_CurrentEXP;
            expAST = player.AST_CurrentEXP;
            expMCH = player.MCH_CurrentEXP;
            expSAM = 0;
            expRDM = 0;
            #endregion
            #region Levels
            levelGLD = player.GLD;
            levelPGL = player.PGL;
            levelMRD = player.MRD;
            levelLNC = player.LNC;
            levelARC = player.ARC;
            levelROG = player.ROG;
            levelCNJ = player.CNJ;
            levelTHM = player.THM;
            levelACN = player.ACN;
            levelCPT = player.CPT;
            levelBSM = player.BSM;
            levelARM = player.ARM;
            levelGSM = player.GSM;
            levelLTW = player.LTW;
            levelWVR = player.WVR;
            levelALC = player.ALC;
            levelCUL = player.CUL;
            levelMIN = player.MIN;
            levelBTN = player.BTN;
            levelFSH = player.FSH;
            levelDRK = player.DRK;
            levelAST = player.AST;
            levelMCH = player.MCH;
            levelSAM = 0;
            levelRDM = 0;
            #endregion
        }

        private double barLengthGenerator(double current, double max)
        {
            //0, 311
            return map(current, 0, max, 0, 311);
        }
        /// <summary>
        /// Generate the HP, MP/CP/GP, and TP bars
        /// </summary>
        /// <param name="type">0=MP, 1=CP, 2=GP</param>
        private void generateStatusBars(int type)
        {
            Image tab1 = Image.FromFile("res/tab1.png");
            byte[] modify = ImageToBGRA(tab1);
            //Hp Bar
            //2, 69, 78
            //23.186.148
            //32.234.173
            //23.214.153///
            //1.171.113
            //39.186.122
            //0.73.42
            //5, 94+
            double hpLength = barLengthGenerator(currentHP, maxHP);
            for (int x = 0; x<hpLength; x++)
            {
                int start = 5 * 4 + 94 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 2;
                modify[(start + (4 * x)) + 1] = 69;
                modify[(start + (4 * x)) + 2] = 78;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 95 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 2;
                modify[(start + (4 * x)) + 1] = 69;
                modify[(start + (4 * x)) + 2] = 78;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 96 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 23;
                modify[(start + (4 * x)) + 1] = 186;
                modify[(start + (4 * x)) + 2] = 148;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 97 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 23;
                modify[(start + (4 * x)) + 1] = 186;
                modify[(start + (4 * x)) + 2] = 148;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 98 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 32;
                modify[(start + (4 * x)) + 1] = 234;
                modify[(start + (4 * x)) + 2] = 173;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 99 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 32;
                modify[(start + (4 * x)) + 1] = 234;
                modify[(start + (4 * x)) + 2] = 173;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 100 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 23;
                modify[(start + (4 * x)) + 1] = 214;
                modify[(start + (4 * x)) + 2] = 153;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 101 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 23;
                modify[(start + (4 * x)) + 1] = 214;
                modify[(start + (4 * x)) + 2] = 153;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 102 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 23;
                modify[(start + (4 * x)) + 1] = 214;
                modify[(start + (4 * x)) + 2] = 153;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 103 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 1;
                modify[(start + (4 * x)) + 1] = 171;
                modify[(start + (4 * x)) + 2] = 113;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 104 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 1;
                modify[(start + (4 * x)) + 1] = 171;
                modify[(start + (4 * x)) + 2] = 113;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 105 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 39;
                modify[(start + (4 * x)) + 1] = 186;
                modify[(start + (4 * x)) + 2] = 122;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 106 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 39;
                modify[(start + (4 * x)) + 1] = 183;
                modify[(start + (4 * x)) + 2] = 122;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 107 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 73;
                modify[(start + (4 * x)) + 2] = 42;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 108 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 73;
                modify[(start + (4 * x)) + 2] = 42;
                modify[(start + (4 * x)) + 3] = 255;
            }
            //TP Bar
            //0.68.90
            //20.179.199
            //24.220.244
            //21.203.233///
            //0.160.192
            //42.165.193
            //0.63.85
            //5,190+
            double tpLength = barLengthGenerator(currentTP, maxTP);
            for (int x = 0; x < tpLength; x++)
            {
                int start = 5 * 4 + 190 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 68;
                modify[(start + (4 * x)) + 2] = 90;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 191 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 68;
                modify[(start + (4 * x)) + 2] = 90;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 192 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 20;
                modify[(start + (4 * x)) + 1] = 179;
                modify[(start + (4 * x)) + 2] = 199;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 193 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 20;
                modify[(start + (4 * x)) + 1] = 179;
                modify[(start + (4 * x)) + 2] = 199;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 194 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 24;
                modify[(start + (4 * x)) + 1] = 220;
                modify[(start + (4 * x)) + 2] = 244;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 195 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 24;
                modify[(start + (4 * x)) + 1] = 220;
                modify[(start + (4 * x)) + 2] = 244;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 196 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 21;
                modify[(start + (4 * x)) + 1] = 203;
                modify[(start + (4 * x)) + 2] = 233;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 197 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 21;
                modify[(start + (4 * x)) + 1] = 203;
                modify[(start + (4 * x)) + 2] = 233;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 198 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 21;
                modify[(start + (4 * x)) + 1] = 203;
                modify[(start + (4 * x)) + 2] = 233;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 199 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 160;
                modify[(start + (4 * x)) + 2] = 192;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 201 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 160;
                modify[(start + (4 * x)) + 2] = 192;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 202 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 42;
                modify[(start + (4 * x)) + 1] = 165;
                modify[(start + (4 * x)) + 2] = 193;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 203 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 42;
                modify[(start + (4 * x)) + 1] = 165;
                modify[(start + (4 * x)) + 2] = 192;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 204 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 63;
                modify[(start + (4 * x)) + 2] = 85;
                modify[(start + (4 * x)) + 3] = 255;
                start = 5 * 4 + 205 * 4 * 320;
                modify[(start + (4 * x)) + 0] = 0;
                modify[(start + (4 * x)) + 1] = 63;
                modify[(start + (4 * x)) + 2] = 85;
                modify[(start + (4 * x)) + 3] = 255;
            }
            if (type == 0)
            {
                //MP Bar
                //28, 42, 92, 255
                //129, 88, 180, 255
                //166, 109, 224, 255
                //141, 77, 206, 255
                //105, 49, 162, 255
                //109, 57, 151, 255
                //47, 19, 89, 255
                //5,141+
                double mpLength = barLengthGenerator(currentMP, maxMP);
                for (int x = 0; x < mpLength; x++)
                {
                    int start = 5 * 4 + 141 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 28;
                    modify[(start + (4 * x)) + 1] = 42;
                    modify[(start + (4 * x)) + 2] = 92;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 142 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 28;
                    modify[(start + (4 * x)) + 1] = 42;
                    modify[(start + (4 * x)) + 2] = 92;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 143 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 129;
                    modify[(start + (4 * x)) + 1] = 88;
                    modify[(start + (4 * x)) + 2] = 180;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 144 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 129;
                    modify[(start + (4 * x)) + 1] = 88;
                    modify[(start + (4 * x)) + 2] = 180;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 145 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 166;
                    modify[(start + (4 * x)) + 1] = 109;
                    modify[(start + (4 * x)) + 2] = 224;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 146 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 166;
                    modify[(start + (4 * x)) + 1] = 109;
                    modify[(start + (4 * x)) + 2] = 224;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 147 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 141;
                    modify[(start + (4 * x)) + 1] = 77;
                    modify[(start + (4 * x)) + 2] = 206;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 148 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 141;
                    modify[(start + (4 * x)) + 1] = 77;
                    modify[(start + (4 * x)) + 2] = 206;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 149 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 141;
                    modify[(start + (4 * x)) + 1] = 77;
                    modify[(start + (4 * x)) + 2] = 206;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 150 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 105;
                    modify[(start + (4 * x)) + 1] = 49;
                    modify[(start + (4 * x)) + 2] = 162;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 151 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 105;
                    modify[(start + (4 * x)) + 1] = 49;
                    modify[(start + (4 * x)) + 2] = 162;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 152 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 109;
                    modify[(start + (4 * x)) + 1] = 57;
                    modify[(start + (4 * x)) + 2] = 151;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 153 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 109;
                    modify[(start + (4 * x)) + 1] = 57;
                    modify[(start + (4 * x)) + 2] = 151;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 154 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 47;
                    modify[(start + (4 * x)) + 1] = 19;
                    modify[(start + (4 * x)) + 2] = 89;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 155 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 47;
                    modify[(start + (4 * x)) + 1] = 19;
                    modify[(start + (4 * x)) + 2] = 89;
                    modify[(start + (4 * x)) + 3] = 255;
                }
            }
            else if (type==1)
            {

            }
            else if (type==2)
            {

            }

            LogitechLCD.LogiLcdColorSetBackground(modify);
        }

        private void timerUpdateLCD_Tick(object sender, EventArgs e)
        {
            if (LogitechLCD.LogiLcdIsConnected(LogitechLCD.LcdType.Mono) || LogitechLCD.LogiLcdIsConnected(LogitechLCD.LcdType.Color))
            {
                if ((level != plevel) && (job == pjob))
                {
                    //Do level up animation
                    if (isDoingAnimation == false)
                    {
                        isDoingAnimation = true;
                        plevel = level;
                        LogitechLCD.LogiLcdMonoSetText(0, "                          ");
                        LogitechLCD.LogiLcdMonoSetText(1, "         Level Up!        ");
                        LogitechLCD.LogiLcdMonoSetText(2, "          " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + " " + level +  "          ");
                        LogitechLCD.LogiLcdMonoSetText(3, "                          ");
                        LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundBlank);

                        LogitechLCD.LogiLcdColorSetText(0, "Tab 3");
                        LogitechLCD.LogiLcdColorSetText(1, "");
                        LogitechLCD.LogiLcdColorSetText(2, "         Level Up!          ");
                        LogitechLCD.LogiLcdColorSetText(3, "");
                        LogitechLCD.LogiLcdColorSetText(4, "");
                        LogitechLCD.LogiLcdColorSetText(5, "");
                        LogitechLCD.LogiLcdColorSetText(6, "");
                        LogitechLCD.LogiLcdColorSetText(7, "");
                        LogitechLCD.LogiLcdColorSetBackground(test);

                        timerAnimations.Start();
                    }
                    
                }
                if(job != pjob)
                {
                    //do job change animation
                    if (isDoingAnimation == false)
                    {
                        isDoingAnimation = true;
                        pjob = job;
                        plevel = level;
                        LogitechLCD.LogiLcdMonoSetText(0, "                          ");
                        LogitechLCD.LogiLcdMonoSetText(1, "        Job Change        ");
                        LogitechLCD.LogiLcdMonoSetText(2, "           " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + "            ");
                        LogitechLCD.LogiLcdMonoSetText(3, "                          ");
                        LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundBlank);

                        LogitechLCD.LogiLcdColorSetText(0, "");
                        LogitechLCD.LogiLcdColorSetText(1, "");
                        LogitechLCD.LogiLcdColorSetText(2, "         Job Change         ");
                        LogitechLCD.LogiLcdColorSetText(3, "");
                        LogitechLCD.LogiLcdColorSetText(4, "            " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job)  + "             ");
                        LogitechLCD.LogiLcdColorSetText(5, "");
                        LogitechLCD.LogiLcdColorSetText(6, "");
                        LogitechLCD.LogiLcdColorSetText(7, "");
                        LogitechLCD.LogiLcdColorSetBackground(test);

                        timerAnimations.Start();
                    }
                }
                if (!isDoingAnimation)
                {
                    updateCurrentDisplay(currentDisplayMode);
                }


                LogitechLCD.LogiLcdUpdate();
            }
            if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton0) || LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorRight))
            {
                if (currentDisplayMode < maxDisplayMode) { currentDisplayMode++; } else { currentDisplayMode = 0; }
                curentScrollIndex = 0;
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorLeft))
            {
                if (currentDisplayMode > 0) { currentDisplayMode--; } else { currentDisplayMode = maxDisplayMode; }
                curentScrollIndex = 0;
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton1))
            {

            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton2) || LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorUp))
            {
                if (curentScrollIndex > 0) { curentScrollIndex--; }
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton3) || LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorDown))
            {
                if (curentScrollIndex < maxScrollIndex) { curentScrollIndex++; }
            }
        }

        private void updateCurrentDisplay(int dispMode)
        {
            LogitechLCD.LogiLcdColorSetBackground(test);
            LogitechLCD.LogiLcdColorSetTitle("Final Fantasy XIV");
            if (dispMode == -1) /*Initial Screen*/
            {
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0,  "");
                LogitechLCD.LogiLcdMonoSetText(1,  "     Final Fantasy XIV    ");
                LogitechLCD.LogiLcdMonoSetText(2,  "          Online          ");

                LogitechLCD.LogiLcdColorSetText(0, "");
                LogitechLCD.LogiLcdColorSetText(1, "");
                LogitechLCD.LogiLcdColorSetText(2, "      Final Fantasy XIV     ");
                LogitechLCD.LogiLcdColorSetText(3, "           Online           ");
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "");
                LogitechLCD.LogiLcdColorSetText(6, "");

                if (btnConnect.Enabled)
                {
                    LogitechLCD.LogiLcdMonoSetText(3,  "   Awaiting Connection... ");
                    LogitechLCD.LogiLcdColorSetText(7, "    Awaiting Connection...  ");
                    LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundBlank);
                    
                }
                else
                {
                    LogitechLCD.LogiLcdMonoSetText(3,  "");
                    LogitechLCD.LogiLcdColorSetText(7, "");
                    LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneButton);
                }
                
            }
            else if (dispMode == 0) /*First Tab*/
            {
                maxScrollIndex = 0;

                string name = first + " " + last;
                LogitechLCD.LogiLcdMonoSetText(0, name.PadRight(21) + " " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + level);
                LogitechLCD.LogiLcdMonoSetText(1, "HP: " + currentHP + "/" + maxHP);
                if(job == 16 || job == 17 || job == 18)
                {
                    LogitechLCD.LogiLcdMonoSetText(2, "GP: " + currentGP + "/" + maxGP);
                }
                else if (job >= 8 && job <= 15)
                {
                    LogitechLCD.LogiLcdMonoSetText(2, "CP: " + currentCP + "/" + maxCP);
                }
                else
                {
                    LogitechLCD.LogiLcdMonoSetText(2, "MP: " + currentMP + "/" + maxMP);
                }
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneButton);


                LogitechLCD.LogiLcdColorSetText(0, name.PadRight(23) + " " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + level);
                LogitechLCD.LogiLcdColorSetText(1, "HP: " + currentHP + "/" + maxHP);
                LogitechLCD.LogiLcdColorSetText(2, "");
                if (job == 16 || job == 17 || job == 18)
                {
                    LogitechLCD.LogiLcdColorSetText(3, "GP: " + currentGP + "/" + maxGP);
                    generateStatusBars(2);
                }
                else if (job >= 8 && job <= 15)
                {
                    LogitechLCD.LogiLcdColorSetText(3, "CP: " + currentCP + "/" + maxCP);
                    generateStatusBars(1);
                }
                else
                {
                    LogitechLCD.LogiLcdColorSetText(3, "MP: " + currentMP + "/" + maxMP);
                    generateStatusBars(0);
                }
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "TP: " + currentTP + "/" + maxTP);
                LogitechLCD.LogiLcdColorSetText(6, "");
                LogitechLCD.LogiLcdColorSetText(7, "");
                

            }
            else if (dispMode == 1) /*Second Tab*/
            {
                maxScrollIndex = 4;
                String[] rows = new String[] {
                    "GLD" + levelGLD.ToString().PadLeft(2, '0') + "  PGL" + levelPGL.ToString().PadLeft(2, '0') + "  MRD" + levelMRD.ToString().PadLeft(2, '0') + "  LNC" + levelLNC.ToString().PadLeft(2, '0'),
                    "ARC" + levelARC.ToString().PadLeft(2, '0') + "  ROG" + levelROG.ToString().PadLeft(2, '0') + "  CNJ" + levelCNJ.ToString().PadLeft(2, '0') + "  THM" + levelTHM.ToString().PadLeft(2, '0'),
                    "ACN" + levelACN.ToString().PadLeft(2, '0') + "  CPT" + levelCPT.ToString().PadLeft(2, '0') + "  BSM" + levelBSM.ToString().PadLeft(2, '0') + "  ARM" + levelARM.ToString().PadLeft(2, '0'),
                    "GSM" + levelGSM.ToString().PadLeft(2, '0') + "  LTW" + levelLTW.ToString().PadLeft(2, '0') + "  WVR" + levelWVR.ToString().PadLeft(2, '0') + "  ALC" + levelALC.ToString().PadLeft(2, '0'),
                    "CUL" + levelCUL.ToString().PadLeft(2, '0') + "  MIN" + levelMIN.ToString().PadLeft(2, '0') + "  BTN" + levelBTN.ToString().PadLeft(2, '0') + "  FSH" + levelFSH.ToString().PadLeft(2, '0'),
                    "DRK" + levelDRK.ToString().PadLeft(2, '0') + "  AST" + levelAST.ToString().PadLeft(2, '0') + "  MCH" + levelMCH.ToString().PadLeft(2, '0') + "  SAM" + levelSAM.ToString().PadLeft(2, '0'),
                    "RDM" + levelRDM.ToString().PadLeft(2, '0')
                };
                LogitechLCD.LogiLcdMonoSetText(0, rows[0+curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(1, rows[1 + curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(2, rows[2 + curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneThreeFourButton);

                LogitechLCD.LogiLcdColorSetText(0, rows[0]);
                LogitechLCD.LogiLcdColorSetText(1, rows[1]);
                LogitechLCD.LogiLcdColorSetText(2, rows[2]);
                LogitechLCD.LogiLcdColorSetText(3, rows[3]);
                LogitechLCD.LogiLcdColorSetText(4, rows[4]);
                LogitechLCD.LogiLcdColorSetText(5, rows[5]);
                LogitechLCD.LogiLcdColorSetText(6, rows[6]);
                LogitechLCD.LogiLcdColorSetText(7, "");
            }
            else if (dispMode == 2) /*Third Tab*/
            {
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, "Tab 3");
                LogitechLCD.LogiLcdMonoSetText(1, "");
                LogitechLCD.LogiLcdMonoSetText(2, "");
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneButton);

                LogitechLCD.LogiLcdColorSetText(0, "Tab 3");
                LogitechLCD.LogiLcdColorSetText(1, "");
                LogitechLCD.LogiLcdColorSetText(2, "");
                LogitechLCD.LogiLcdColorSetText(3, "");
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "");
                LogitechLCD.LogiLcdColorSetText(6, "");
                LogitechLCD.LogiLcdColorSetText(7, "");
            }
            else if (dispMode == 3) /*Fourth Tab*/
            {
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, "Tab 4");
                LogitechLCD.LogiLcdMonoSetText(1, "");
                LogitechLCD.LogiLcdMonoSetText(2, "");
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneButton);

                LogitechLCD.LogiLcdColorSetText(0, "Tab 4");
                LogitechLCD.LogiLcdColorSetText(1, "");
                LogitechLCD.LogiLcdColorSetText(2, "");
                LogitechLCD.LogiLcdColorSetText(3, "");
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "");
                LogitechLCD.LogiLcdColorSetText(6, "");
                LogitechLCD.LogiLcdColorSetText(7, "");
            }
        }
        private void timerAnimations_Tick(object sender, EventArgs e)
        {
            isDoingAnimation = false;
            timerAnimations.Stop();
        }
    }
}
