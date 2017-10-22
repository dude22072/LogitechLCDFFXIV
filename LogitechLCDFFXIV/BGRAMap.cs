using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogitechLCDFFXIV.BGRATools;

namespace LogitechLCDFFXIV
{
    public class BGRAMap
    {
        private int height, width;
        private byte[] map;

        public BGRAMap(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.map = new byte[width * height * 4];
            fillBlankMap(this.map.Length);
        }
        public BGRAMap(int width, int height, byte B, byte G, byte R)
        {
            this.width = width;
            this.height = height;
            this.map = new byte[width * height * 4];
            this.map = fillColorMap(this.map.Length, B, G, R, 255);
        }
        public BGRAMap(int width, int height, byte B, byte G, byte R, byte A)
        {
            this.width = width;
            this.height = height;
            this.map = new byte[width * height * 4];
            this.map = fillColorMap(this.map.Length, B, G, R, A);
        }
        public BGRAMap(Bitmap imageIn)
        {
            width = imageIn.Width;
            height = imageIn.Width;
            map = ImageToBGRA(imageIn);
        }

        public byte[] getMap()
        {
            return this.map;
        }
        public void setMap(byte[] newMap)
        {
            this.map = newMap;
        }

    }
}
