using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogitechLCDFFXIV
{
    class ColorTab1
    {
        private static byte[] tab1 = new byte[320 * 240 * 4];

        private static double map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private static double barLengthGenerator(double current, double max)
        {
            //0, 311
            return map(current, 0, max, 0, 311);
        }

        public static byte[] ImageToBGRA(System.Drawing.Image imageIn)
        {
            System.Drawing.Bitmap useme = new System.Drawing.Bitmap(imageIn);
            byte[] bgra = new byte[307200];
            for (int y = 0; y < 240; y++)
            {
                for (int x = 0; x < 320; x++)
                {
                    System.Drawing.Color pxl = useme.GetPixel(x, y);
                    bgra[(y * (320 * 4)) + x * 4 + 0] = pxl.B;
                    bgra[(y * (320 * 4)) + x * 4 + 1] = pxl.G;
                    bgra[(y * (320 * 4)) + x * 4 + 2] = pxl.R;
                    bgra[(y * (320 * 4)) + x * 4 + 3] = pxl.A;
                }
            }
            return bgra;
        }

        private static void generateMainBackground()
        {
            System.Drawing.Image tab1i = System.Drawing.Image.FromFile("res/tab1.png");
            tab1 = ImageToBGRA(tab1i);
        }

        /// <summary>
        /// Generate the HP, MP/CP/GP, and TP bars
        /// </summary>
        /// <param name="type">0=MP, 1=CP, 2=GP</param>
        /// <param name="currentType">MP/CP/GP</param>
        /// <param name="maxType">MP/CP/GP</param>
        private static void generateStatusBars(int type, double currentType, double maxType, double currentHP, double maxHP, double currentTP, double maxTP)
        {
            byte[] modify = tab1;
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
            for (int x = 0; x < hpLength; x++)
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
                double mpLength = barLengthGenerator(currentType, maxType);
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
            else if (type == 1)
            {
                double cpLength = barLengthGenerator(currentType, maxType);
                for (int x = 0; x < cpLength; x++)
                {
                    int start = 5 * 4 + 141 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 142 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 143 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 144 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 145 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 146 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 147 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 148 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 149 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 150 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 151 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 152 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 153 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 154 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 155 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                }
            }
            else if (type == 2)
            {
                double gpLength = barLengthGenerator(currentType, maxType);
                for (int x = 0; x < gpLength; x++)
                {
                    int start = 5 * 4 + 141 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 142 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 143 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 144 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 145 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 146 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 147 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 148 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 149 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 150 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 151 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 152 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 153 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 154 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                    start = 5 * 4 + 155 * 4 * 320;
                    modify[(start + (4 * x)) + 0] = 0;
                    modify[(start + (4 * x)) + 1] = 0;
                    modify[(start + (4 * x)) + 2] = 0;
                    modify[(start + (4 * x)) + 3] = 255;
                }
            }
        }

        public static void drawTab1(int type, double currentType, double maxType, double currentHP, double maxHP, double currentTP, double maxTP)
        {
            generateMainBackground();
            generateStatusBars(type, currentType, maxType, currentHP, maxHP, currentTP, maxTP);
            LogitechLCD.LogiLcdColorSetBackground(tab1);
        }
    }
}
