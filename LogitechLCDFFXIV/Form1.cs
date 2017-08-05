using Sharlayan;
using Sharlayan.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LogitechLCDFFXIV
{
    public partial class Form1 : Form
    {
        Boolean dx11 = false, isDoingAnimation = false;
        String first, last;
        private static FFXIV ffxiv;
        private FFXIV.Character charInfo;
        double currentHP, maxHP, currentMP, maxMP, currentTP, maxTP, currentCP, maxCP, currentGP, maxGP, expGLD, expPGL, expMRD, expLNC, expARC, expROG, expCNJ, expTHM, expACN, expCPT, expBSM, expARM, expGSM, expLTW, expWVR, expALC, expCUL, expMIN, expBTN, expFSH, expDRK, expAST, expMCH, expSAM, expRDM;
        byte job, pjob, level, plevel, title, levelGLD, levelPGL, levelMRD, levelLNC, levelARC, levelROG, levelCNJ, levelTHM, levelACN, levelCPT, levelBSM, levelARM, levelGSM, levelLTW, levelWVR, levelALC, levelCUL, levelMIN, levelBTN, levelFSH, levelDRK, levelAST, levelMCH, levelSAM, levelRDM;
        double x, y, z;
        /*strings for when a tell is recived*/
        public static volatile string tellUser, tellMessage;
        /*other ints*/
        static int currentDisplayMode = -1, curentScrollIndex = 0, maxScrollIndex = 0;

        public static double map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogitechLCD.LogiLcdInit("FFXIV", LogitechLCD.LcdType.Mono);
            trayIcon.BalloonTipText = "Running in tray. Double click tray icon to maximize.";
            trayIcon.BalloonTipTitle = "FFXIV LCD Applet";
            trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            trayIcon.Icon = this.Icon;
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

            Debug.WriteLine(Inventories.ToArray().Length.ToString());
            Debug.WriteLine(Inventories.ToArray().ToString());

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
        private void timerUpdateLCD_Tick(object sender, EventArgs e)
        {
            if (LogitechLCD.LogiLcdIsConnected(LogitechLCD.LcdType.Mono) /*|| Logitech.LogiLcdIsConnected(Logitech.LcdType.Color)*/)
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
                        timerAnimations.Start();
                    }
                }
                if (!isDoingAnimation)
                {
                    updateCurrentDisplay(currentDisplayMode);
                }


                LogitechLCD.LogiLcdUpdate();
            }
            if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton0))
            {
                if (currentDisplayMode < 3) { currentDisplayMode++; } else { currentDisplayMode = 0; }
                curentScrollIndex = 0;
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton1))
            {

            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton2))
            {
                if (curentScrollIndex < maxScrollIndex) { curentScrollIndex++; }
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton3))
            {
                if(curentScrollIndex>0) { curentScrollIndex--; }
            }
        }

        private void updateCurrentDisplay(int dispMode)
        {
            if (dispMode == -1) /*Initial Screen*/
            {
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, "");
                LogitechLCD.LogiLcdMonoSetText(1, "     Final Fantasy XIV    ");
                LogitechLCD.LogiLcdMonoSetText(2, "          Online          ");
                if (btnConnect.Enabled)
                {
                    LogitechLCD.LogiLcdMonoSetText(3, "   Awaiting Connection... ");
                    LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundBlank);
                }
                else
                {
                    LogitechLCD.LogiLcdMonoSetText(3, "");
                    LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneButton);
                }
                
            }
            else if (dispMode == 0) /*First Tab*/
            {
                maxScrollIndex = 0;
                //TODO:HP, MP, EXP
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
            }
            else if (dispMode == 1) /*Second Tab*/
            {
                maxScrollIndex = 4;
                string row1 = "GLD" + levelGLD.ToString().PadLeft(2, '0') + "  PGL" + levelPGL.ToString().PadLeft(2, '0') + "  MRD" + levelMRD.ToString().PadLeft(2, '0') + "  LNC" + levelLNC.ToString().PadLeft(2, '0');
                string row2 = "ARC" + levelARC.ToString().PadLeft(2, '0') + "  ROG" + levelROG.ToString().PadLeft(2, '0') + "  CNJ" + levelCNJ.ToString().PadLeft(2, '0') + "  THM" + levelTHM.ToString().PadLeft(2, '0');
                string row3 = "ACN" + levelACN.ToString().PadLeft(2, '0') + "  CPT" + levelCPT.ToString().PadLeft(2, '0') + "  BSM" + levelBSM.ToString().PadLeft(2, '0') + "  ARM" + levelARM.ToString().PadLeft(2, '0');
                string row4 = "GSM" + levelGSM.ToString().PadLeft(2, '0') + "  LTW" + levelLTW.ToString().PadLeft(2, '0') + "  WVR" + levelWVR.ToString().PadLeft(2, '0') + "  ALC" + levelALC.ToString().PadLeft(2, '0');
                string row5 = "CUL" + levelCUL.ToString().PadLeft(2, '0') + "  MIN" + levelMIN.ToString().PadLeft(2, '0') + "  BTN" + levelBTN.ToString().PadLeft(2, '0') + "  FSH" + levelFSH.ToString().PadLeft(2, '0');
                string row6 = "DRK" + levelDRK.ToString().PadLeft(2, '0') + "  AST" + levelAST.ToString().PadLeft(2, '0') + "  MCH" + levelMCH.ToString().PadLeft(2, '0') + "  SAM" + levelSAM.ToString().PadLeft(2, '0');
                string row7 = "RDM" + levelRDM.ToString().PadLeft(2, '0');


                String[] rows = new String[] { row1, row2, row3, row4, row5, row6, row7 };
                LogitechLCD.LogiLcdMonoSetText(0, rows[0+curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(1, rows[1 + curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(2, rows[2 + curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneThreeFourButton);
            }
            else if (dispMode == 2) /*Third Tab*/
            {
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, "Tab 3");
                LogitechLCD.LogiLcdMonoSetText(1, "");
                LogitechLCD.LogiLcdMonoSetText(2, "");
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneButton);
            }
            else if (dispMode == 3) /*Fourth Tab*/
            {
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, "Tab 4");
                LogitechLCD.LogiLcdMonoSetText(1, "");
                LogitechLCD.LogiLcdMonoSetText(2, "");
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundOneButton);
            }
        }
        private void timerAnimations_Tick(object sender, EventArgs e)
        {
            isDoingAnimation = false;
            timerAnimations.Stop();
        }
    }
}
