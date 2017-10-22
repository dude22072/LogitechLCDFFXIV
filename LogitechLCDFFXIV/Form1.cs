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
        int locale = 0;
        String playerName = "TESTUSER";
        private static FFXIV ffxiv;
        double currentHP = 10000, maxHP = 15000, currentMP = 10000, maxMP = 15000, currentTP = 1000, maxTP = 1000, currentCP, maxCP, currentGP, maxGP, expGLD, expPGL, expMRD, expLNC, expARC, expROG, expCNJ, expTHM, expACN, expCPT, expBSM, expARM, expGSM, expLTW, expWVR, expALC, expCUL, expMIN, expBTN, expFSH, expDRK, expAST, expMCH, expSAM, expRDM;
        byte job, pjob, level, plevel, title, levelGLD, levelPGL, levelMRD, levelLNC, levelARC, levelROG, levelCNJ, levelTHM, levelACN, levelCPT, levelBSM, levelARM, levelGSM, levelLTW, levelWVR, levelALC, levelCUL, levelMIN, levelBTN, levelFSH, levelDRK, levelAST, levelMCH, levelSAM, levelRDM;
        double x, y, z;
        short STR, bSTR, DEX, bDEX, VIT, bVIT, INT, bINT, MND, bMND, PIE, bPIE, resWater, resLightning, resEarth, resWind, resIce, resFire, resBlunt, resPiercing, resSlashing, statTenacity, statDefense, statControl, statSpellSpeed, statSkillSpeed, statDetermination, statHealingPotency, statAttackPotency, statCritRate, statDirectHit, statAttackPower, statMagicDefense, statEvasion, statGathering, statPerception;
        System.Timers.Timer infoTimer;
        /*strings for when a tell is recived*/
        //public static volatile string tellUser, tellMessage;
        /*other ints*/
        static int currentDisplayMode = -1, maxDisplayMode = 3, curentScrollIndex = 0, maxScrollIndex = 0, curentScrollIndexColor = 0, maxScrollIndexColor = 0, currentBackground = 0;

        public static BGRAMap mainMap = new BGRAMap(320, 240, 255, 255, 255, 064);
        
        //public static byte[] test = new byte[320 * 240 * 4];

        String[][] localization = {
            new String[]{ "Minimise to tray", "トレイに最小化" },
            new String[]{ "Connect", "コネクト" },
            new String[]{ "Could not find FFXIV instance.", "インスタンスのFFXIVが見つかりませんでした。" },
            new String[]{ "Could not find your player. Please make sure you're logged in.", "" },
            new String[]{ "Running in tray. Double click tray icon to maximize.", "" },
            new String[]{ "res/ARR EN SMALL.png", "res/ARR JP SMALL.png"}
        };
        String[][] jobLocalization = {
            new String[]{ "GLD", "剣"},
            new String[]{ "PGL", "格"},
            new String[]{ "MRD", "斧"},
            new String[]{ "LNC", "槍"},
            new String[]{ "ARC", "弓"},
            new String[]{ "ROG", "双"},
            new String[]{ "CNJ", "幻"},
            new String[]{ "THM", "呪"},
            new String[]{ "ACN", "巴"},
            new String[]{ "CPT", "木工師"},
            new String[]{ "BSM", "鍛冶師"},
            new String[]{ "ARM", "甲冑師"},
            new String[]{ "GSM", "彫金師"},
            new String[]{ "LTW", "革細工師"},
            new String[]{ "WVR", "裁縫師"},
            new String[]{ "ALC", "錬金術師"},
            new String[]{ "CUL", "調理師"},
            new String[]{ "MIN", "採掘師"},
            new String[]{ "BTN", "園芸師"},
            new String[]{ "FSH", "漁師"},
            new String[]{ "DRK", "暗"},
            new String[]{ "AST", "占"},
            new String[]{ "MCH", "機"},
            new String[]{ "SAM", "侍"},
            new String[]{ "RDM", "赤"},
        };

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

            mainMap.setMap(BGRATools.fillColorMap(mainMap.getMap().Length, 255, 255, 255, 065));
            LogitechLCD.LogiLcdInit("FFXIV", LogitechLCD.LcdType.Mono | LogitechLCD.LcdType.Color);
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

        private void rbLangEnglish_CheckedChanged(object sender, EventArgs e)
        {
            locale = 0;
            updateLocalization();
        }

        private void rbLangJapanese_CheckedChanged(object sender, EventArgs e)
        {
            locale = 1;
            updateLocalization();
        }

        private void updateLocalization()
        {
            cbTray.Text = localization[0][locale];
            btnConnect.Text = localization[1][locale];
            trayIcon.BalloonTipText = localization[4][locale];
            updateBackground(0);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ffxiv = new FFXIV(dx11, locale);
            if(ffxiv._initiated)
            {
                    btnConnect.Enabled = false;
                    infoTimer = new System.Timers.Timer();
                    infoTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetCharacterInfo);
                    infoTimer.Interval = 1000;
                    infoTimer.Enabled = true;
                    infoTimer.Start();
            }
            else
            {
                MessageBox.Show(localization[2][locale], this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void GetCharacterInfo(object source, System.Timers.ElapsedEventArgs e)
        {
            Sharlayan.Core.PlayerEntity player = Reader.GetPlayerInfo().PlayerEntity;
            playerName = player.Name;
            FFXIV.Character charInfo = new FFXIV.Character(playerName);

            if(charInfo == null)
            {
                MessageBox.Show(localization[3][locale], this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnConnect.Enabled = true;
                infoTimer.Stop();
            }
            
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

            #region Stats
            STR = player.Strength;
            bSTR = player.BaseStrength;
            DEX = player.Dexterity;
            bDEX = player.BaseDexterity;
            VIT = player.Vitality;
            bVIT = player.BaseVitality;
            INT = player.Intelligence;
            bINT = player.BaseIntelligence;
            MND = player.Mind;
            bMND = player.BaseMind;
            PIE = player.Piety;
            bPIE = player.BasePiety;
            //TODO
            statTenacity = player.Tenacity;
            statDefense = player.Defense;
            statControl = player.Control;
            statSpellSpeed = player.SpellSpeed;
            statSkillSpeed = player.SkillSpeed;
            statDetermination = player.Determination;
            statHealingPotency = player.HealingMagicPotency;
            statAttackPotency = player.AttackMagicPotency;
            statCritRate = player.CriticalHitRate;
            statDirectHit = player.DirectHit;
            statAttackPower = player.AttackPower;
            statMagicDefense = player.MagicDefense;
            statEvasion = player.Evasion;
            statGathering = player.Gathering;
            statPerception = player.Perception;
            #endregion
            #region Resistances
            resWater = player.WaterResistance;
            resLightning = player.LightningResistance;
            resEarth = player.EarthResistance;
            resWind = player.WindResistance;
            resIce = player.IceResistance;
            resFire = player.FireResistance;
            resBlunt = player.BluntResistance;
            resPiercing = player.PiercingResistance;
            resSlashing = player.SlashingResistance;
            #endregion
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
            expSAM = player.SAM_CurrentEXP;
            expRDM = player.RDM_CurrentEXP;
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
            levelSAM = player.SAM;
            levelRDM = player.RDM;
            #endregion

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

                        /*Monochrome*/
                        LogitechLCD.LogiLcdMonoSetText(0, "                          ");
                        LogitechLCD.LogiLcdMonoSetText(1, "         Level Up!        ");
                        LogitechLCD.LogiLcdMonoSetText(2, "          " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + " " + level +  "          ");
                        LogitechLCD.LogiLcdMonoSetText(3, "                          ");
                        LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundBlank);

                        /*Color*/
                        LogitechLCD.LogiLcdColorSetText(0, "");
                        LogitechLCD.LogiLcdColorSetText(1, "");
                        LogitechLCD.LogiLcdColorSetText(2, "         Level Up!          ");
                        LogitechLCD.LogiLcdColorSetText(3, "");
                        LogitechLCD.LogiLcdColorSetText(4, "");
                        LogitechLCD.LogiLcdColorSetText(5, "");
                        LogitechLCD.LogiLcdColorSetText(6, "");
                        LogitechLCD.LogiLcdColorSetText(7, "");
                        LogitechLCD.LogiLcdColorSetBackground(mainMap.getMap());

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

                        /*Monochrome*/
                        LogitechLCD.LogiLcdMonoSetText(0, "                          ");
                        LogitechLCD.LogiLcdMonoSetText(1, "        Job Change        ");
                        LogitechLCD.LogiLcdMonoSetText(2, "           " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + "            ");
                        LogitechLCD.LogiLcdMonoSetText(3, "                          ");
                        LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundBlank);

                        /*Color*/
                        LogitechLCD.LogiLcdColorSetText(0, "");
                        LogitechLCD.LogiLcdColorSetText(1, "");
                        LogitechLCD.LogiLcdColorSetText(2, "         Job Change         ");
                        LogitechLCD.LogiLcdColorSetText(3, "");
                        LogitechLCD.LogiLcdColorSetText(4, "            " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job)  + "             ");
                        LogitechLCD.LogiLcdColorSetText(5, "");
                        LogitechLCD.LogiLcdColorSetText(6, "");
                        LogitechLCD.LogiLcdColorSetText(7, "");
                        LogitechLCD.LogiLcdColorSetBackground(mainMap.getMap());

                        timerAnimations.Start();
                    }
                }
                if (!isDoingAnimation)
                {
                    updateCurrentDisplay(currentDisplayMode);
                }


                LogitechLCD.LogiLcdUpdate();
            }
            
            if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton0) || LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorLeft))
            {
                if (currentDisplayMode > 0) { currentDisplayMode--; } else { currentDisplayMode = maxDisplayMode; }
                curentScrollIndex = 0;
                curentScrollIndexColor = 0;
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton1) || LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorRight))
            {
                if (currentDisplayMode < maxDisplayMode) { currentDisplayMode++; } else { currentDisplayMode = 0; }
                curentScrollIndex = 0;
                curentScrollIndexColor = 0;
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton2))
            {
                if (curentScrollIndex > 0) { curentScrollIndex--; }
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.MonoButton3))
            {
                if (curentScrollIndex < maxScrollIndex) { curentScrollIndex++; }
            }
            else if ( LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorUp))
            {
                if (curentScrollIndexColor > 0) { curentScrollIndexColor--; }
            }
            else if (LogitechLCD.LogiLcdIsButtonPressed(LogitechLCD.Buttons.ColorDown))
            {
                if (curentScrollIndexColor < maxScrollIndexColor) { curentScrollIndexColor++; }
            }
        }

        private void updateBackground(int bg)
        {
            if (currentBackground != bg)
            {
                switch(bg)
                {
                    case 0:
                        mainMap.setMap(BGRATools.fillColorMap(mainMap.getMap().Length, 255, 255, 255, 065));
                        currentBackground = 0;
                        break;
                    case 1:
                        mainMap.setMap(BGRATools.drawToMap(mainMap.getMap(), 320, 240, new Bitmap(localization[5][locale]), 0, 75));
                        currentBackground = 1;
                        break;
                    default: break;
                }
            }
        }

        private void updateCurrentDisplay(int dispMode)
        {
            LogitechLCD.LogiLcdColorSetBackground(mainMap.getMap());
            LogitechLCD.LogiLcdColorSetTitle("Final Fantasy XIV");
            if (dispMode == -1) /*Initial Screen*/
            {
                updateBackground(1);
                /*Monochrome*/
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0,  "");
                LogitechLCD.LogiLcdMonoSetText(1,  "     Final Fantasy XIV    ");
                LogitechLCD.LogiLcdMonoSetText(2,  "          Online          ");

                /*Color*/
                LogitechLCD.LogiLcdColorSetText(0, "");
                LogitechLCD.LogiLcdColorSetText(1, "");
                LogitechLCD.LogiLcdColorSetText(2, "");
                LogitechLCD.LogiLcdColorSetText(3, "");
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "");
                LogitechLCD.LogiLcdColorSetText(6, "");
                LogitechLCD.LogiLcdColorSetBackground(mainMap.getMap());

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
                    LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundFixed);
                }

            }
            else if (dispMode == 0) /*First Tab*/
            {
                updateBackground(0);
                /*Monochrome*/
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, playerName.PadRight(21) + " " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + level);
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
                    string strMP = "MP: " + currentMP + "/" + maxMP, strTP = "TP: " + currentTP;
                    strMP = strMP.PadRight(18, ' ');
                    LogitechLCD.LogiLcdMonoSetText(2, strMP + " " + strTP);
                }
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundFixed);

                /*Color*/
                maxScrollIndexColor = 0;
                LogitechLCD.LogiLcdColorSetText(0, playerName.PadRight(23) + " " + Enum.GetName(typeof(Sharlayan.Core.Enums.Actor.Job), job) + level);
                LogitechLCD.LogiLcdColorSetText(1, "HP: " + currentHP + "/" + maxHP);
                LogitechLCD.LogiLcdColorSetText(2, "");
                if (job == 16 || job == 17 || job == 18)
                {
                    LogitechLCD.LogiLcdColorSetText(3, "GP: " + currentGP + "/" + maxGP);
                    ColorTab1.drawTab1(2, currentGP, maxGP, currentHP, maxHP, currentTP, maxTP);
                }
                else if (job >= 8 && job <= 15)
                {
                    LogitechLCD.LogiLcdColorSetText(3, "CP: " + currentCP + "/" + maxCP);
                    ColorTab1.drawTab1(1, currentCP, maxCP, currentHP, maxHP, currentTP, maxTP);
                }
                else
                {
                    LogitechLCD.LogiLcdColorSetText(3, "MP: " + currentMP + "/" + maxMP);
                    ColorTab1.drawTab1(0, currentMP, maxMP, currentHP, maxHP, currentTP, maxTP);
                }
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "TP: " + currentTP + "/" + maxTP);
                LogitechLCD.LogiLcdColorSetText(6, "");
                LogitechLCD.LogiLcdColorSetText(7, "");

            }
            else if (dispMode == 1) /*Second Tab*/
            {
                updateBackground(0);
                String[] rows;
                switch(locale)
                {
                    case 0:
                        rows = new String[] {
                            jobLocalization[0][locale] + levelGLD.ToString().PadLeft(2, '0') + "  " + jobLocalization[1][locale] + levelPGL.ToString().PadLeft(2, '0') + "  " + jobLocalization[2][locale] + levelMRD.ToString().PadLeft(2, '0') + "  " + jobLocalization[3][locale] + levelLNC.ToString().PadLeft(2, '0'),
                            jobLocalization[4][locale] + levelARC.ToString().PadLeft(2, '0') + "  " + jobLocalization[5][locale] + levelROG.ToString().PadLeft(2, '0') + "  " + jobLocalization[6][locale] + levelCNJ.ToString().PadLeft(2, '0') + "  " + jobLocalization[7][locale] + levelTHM.ToString().PadLeft(2, '0'),
                            jobLocalization[8][locale] + levelACN.ToString().PadLeft(2, '0') + "  " + jobLocalization[9][locale] + levelCPT.ToString().PadLeft(2, '0') + "  " + jobLocalization[10][locale] + levelBSM.ToString().PadLeft(2, '0') + "  " + jobLocalization[0][locale] + levelARM.ToString().PadLeft(2, '0'),
                            jobLocalization[12][locale] + levelGSM.ToString().PadLeft(2, '0') + "  " + jobLocalization[13][locale] + levelLTW.ToString().PadLeft(2, '0') + "  " + jobLocalization[14][locale] + levelWVR.ToString().PadLeft(2, '0') + "  " + jobLocalization[15][locale] + levelALC.ToString().PadLeft(2, '0'),
                            jobLocalization[16][locale] + levelCUL.ToString().PadLeft(2, '0') + "  " + jobLocalization[17][locale] + levelMIN.ToString().PadLeft(2, '0') + "  " + jobLocalization[18][locale] + levelBTN.ToString().PadLeft(2, '0') + "  " + jobLocalization[19][locale] + levelFSH.ToString().PadLeft(2, '0'),
                            jobLocalization[20][locale] + levelDRK.ToString().PadLeft(2, '0') + "  " + jobLocalization[21][locale] + levelAST.ToString().PadLeft(2, '0') + "  " + jobLocalization[22][locale] + levelMCH.ToString().PadLeft(2, '0') + "  " + jobLocalization[23][locale] + levelSAM.ToString().PadLeft(2, '0'),
                            jobLocalization[24][locale] + levelRDM.ToString().PadLeft(2, '0')
                        };
                        break;
                    case 1:
                        rows = new String[] {
                            jobLocalization[0][locale] + levelGLD.ToString().PadLeft(2, '0') + " " + jobLocalization[1][locale] + levelPGL.ToString().PadLeft(2, '0') + " " + jobLocalization[2][locale] + levelMRD.ToString().PadLeft(2, '0') + " " + jobLocalization[3][locale] + levelLNC.ToString().PadLeft(2, '0') + " " + jobLocalization[4][locale] + levelARC.ToString().PadLeft(2, '0'),
                            jobLocalization[5][locale] + levelROG.ToString().PadLeft(2, '0') + " " + jobLocalization[6][locale] + levelCNJ.ToString().PadLeft(2, '0') + " " + jobLocalization[7][locale] + levelTHM.ToString().PadLeft(2, '0') + " " + jobLocalization[8][locale] + levelACN.ToString().PadLeft(2, '0') + " " + jobLocalization[20][locale] + levelDRK.ToString().PadLeft(2, '0'),
                            jobLocalization[21][locale] + levelAST.ToString().PadLeft(2, '0') + " " +jobLocalization[22][locale] + levelMCH.ToString().PadLeft(2, '0') + " " + jobLocalization[23][locale] + levelSAM.ToString().PadLeft(2, '0') + " " + jobLocalization[24][locale] + levelRDM.ToString().PadLeft(2, '0'),
                            jobLocalization[9][locale] + levelCPT.ToString().PadLeft(2, '0') + jobLocalization[10][locale] + levelBSM.ToString().PadLeft(2, '0') + jobLocalization[11][locale] + levelARM.ToString().PadLeft(2, '0'),
                            jobLocalization[12][locale] + levelGSM.ToString().PadLeft(2, '0') + jobLocalization[13][locale] + levelLTW.ToString().PadLeft(2, '0') + jobLocalization[14][locale] + levelWVR.ToString().PadLeft(2, '0'),
                            jobLocalization[15][locale] + levelALC.ToString().PadLeft(2, '0') + jobLocalization[16][locale] + levelCUL.ToString().PadLeft(2, '0') + jobLocalization[17][locale] + levelMIN.ToString().PadLeft(2, '0'),
                            jobLocalization[18][locale] + levelBTN.ToString().PadLeft(2, '0') + jobLocalization[19][locale] + levelFSH.ToString().PadLeft(2, '0')
                        };
                        break;
                    default:
                        rows = new String[]{};
                        break;
                }
                

                /*Monochrome*/
                maxScrollIndex = 4;
                LogitechLCD.LogiLcdMonoSetText(0, rows[0+curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(1, rows[1 + curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(2, rows[2 + curentScrollIndex]);
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundScrollable);

                /*Color*/
                maxScrollIndexColor = 0;
                LogitechLCD.LogiLcdColorSetText(0, " " + rows[0]);
                LogitechLCD.LogiLcdColorSetText(1, " " + rows[1]);
                LogitechLCD.LogiLcdColorSetText(2, " " + rows[2]);
                LogitechLCD.LogiLcdColorSetText(3, " " + rows[3]);
                LogitechLCD.LogiLcdColorSetText(4, " " + rows[4]);
                LogitechLCD.LogiLcdColorSetText(5, " " + rows[5]);
                LogitechLCD.LogiLcdColorSetText(6, " " + rows[6]);
                LogitechLCD.LogiLcdColorSetText(7, "");
                LogitechLCD.LogiLcdColorSetBackground(mainMap.getMap());


            }
            else if (dispMode == 2) /*Third Tab*/
            {
                updateBackground(0);
                /*Monochrome*/
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, "Tab 3");
                LogitechLCD.LogiLcdMonoSetText(1, "");
                LogitechLCD.LogiLcdMonoSetText(2, "");
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundFixed);

                /*Color*/
                maxScrollIndexColor = 0;
                LogitechLCD.LogiLcdColorSetText(0, "Tab 3");
                LogitechLCD.LogiLcdColorSetText(1, "");
                LogitechLCD.LogiLcdColorSetText(2, "");
                LogitechLCD.LogiLcdColorSetText(3, "");
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "");
                LogitechLCD.LogiLcdColorSetText(6, "");
                LogitechLCD.LogiLcdColorSetText(7, "");
                LogitechLCD.LogiLcdColorSetBackground(mainMap.getMap());
            }
            else if (dispMode == 3) /*Fourth Tab*/
            {
                updateBackground(0);
                /*Monochrome*/
                maxScrollIndex = 0;
                LogitechLCD.LogiLcdMonoSetText(0, "Tab 4");
                LogitechLCD.LogiLcdMonoSetText(1, "");
                LogitechLCD.LogiLcdMonoSetText(2, "");
                LogitechLCD.LogiLcdMonoSetText(3, "");
                LogitechLCD.LogiLcdMonoSetBackground(LogitechLCD.lcdBackroundFixed);

                /*Color*/
                maxScrollIndexColor = 0;
                LogitechLCD.LogiLcdColorSetText(0, "Tab 4");
                LogitechLCD.LogiLcdColorSetText(1, "");
                LogitechLCD.LogiLcdColorSetText(2, "");
                LogitechLCD.LogiLcdColorSetText(3, "");
                LogitechLCD.LogiLcdColorSetText(4, "");
                LogitechLCD.LogiLcdColorSetText(5, "");
                LogitechLCD.LogiLcdColorSetText(6, "");
                LogitechLCD.LogiLcdColorSetText(7, "");
                LogitechLCD.LogiLcdColorSetBackground(mainMap.getMap());
            }
        }
        private void timerAnimations_Tick(object sender, EventArgs e)
        {
            isDoingAnimation = false;
            timerAnimations.Stop();
        }
    }
}
