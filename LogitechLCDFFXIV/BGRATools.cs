using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogitechLCDFFXIV
{
    class BGRATools
    {
        
        public static byte[] drawToMap(byte[] map, int mapWidth, int mapHeight, byte[] toDraw, int toDrawWidth, int toDrawHeight, int xStart, int yStart, int alphaThreshold = 100)
        {
            byte[] toReturn = map;
            for (int y = 0; y < toDrawHeight; y++)
            {
                for (int x = 0; x < toDrawWidth; x++)
                {
                    if (toDraw[(y * (toDrawWidth * 4)) + x * 4 + 3] > 0)
                    {
                        if (toDraw[(y * (toDrawWidth * 4)) + x * 4 + 3] > alphaThreshold)
                        {
                            toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 0] = toDraw[(y * (toDrawWidth * 4)) + x * 4 + 0];
                            toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 1] = toDraw[(y * (toDrawWidth * 4)) + x * 4 + 1];
                            toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 2] = toDraw[(y * (toDrawWidth * 4)) + x * 4 + 2];
                            toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 3] = toDraw[(y * (toDrawWidth * 4)) + x * 4 + 3];
                        }
                        else
                        {
                            toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 0] = (byte)((toDraw[(y * (toDrawWidth * 4)) + x * 4 + 0] + toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 0]) / 2);
                            toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 1] = (byte)((toDraw[(y * (toDrawWidth * 4)) + x * 4 + 1] + toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 1]) / 2);
                            toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 2] = (byte)((toDraw[(y * (toDrawWidth * 4)) + x * 4 + 2] + toReturn[((yStart * mapWidth * 4) + (y * (mapWidth * 4))) + ((xStart * 4) + (x * 4)) + 2]) / 2);
                        }
                    }
                }
            }
            return toReturn;
        }
        public static byte[] drawToMap(byte[] map, int mapWidth, int mapHeight, System.Drawing.Image toDraw, int xStart, int yStart, int alphaThreshold = 100)
        {
            return drawToMap(map, mapWidth, mapHeight, ImageToBGRA(toDraw), toDraw.Width, toDraw.Height, xStart, yStart, alphaThreshold);
        }

        public static byte[] ImageToBGRA(System.Drawing.Image imageIn)
        {
            System.Drawing.Bitmap useme = new System.Drawing.Bitmap(imageIn);
            byte[] bgra = new byte[imageIn.Width * imageIn.Height * 4];
            for (int y = 0; y < imageIn.Height; y++)
            {
                for (int x = 0; x < imageIn.Width; x++)
                {
                    System.Drawing.Color pxl = useme.GetPixel(x, y);
                    bgra[(y * (imageIn.Width * 4)) + x * 4 + 0] = pxl.B;
                    bgra[(y * (imageIn.Width * 4)) + x * 4 + 1] = pxl.G;
                    bgra[(y * (imageIn.Width * 4)) + x * 4 + 2] = pxl.R;
                    bgra[(y * (imageIn.Width * 4)) + x * 4 + 3] = pxl.A;
                }
            }
            return bgra;
        }

        public static byte[] fillBlankMap(int Length)
        {
            byte[] toReturn = new byte[Length];
            for (int i = 0; i < toReturn.Length; i++)
            {
                toReturn[i] = 0;
            }
            return toReturn;
        }

        public static byte[] fillColorMap(int Length, byte B, byte G, byte R, byte A)
        {
            byte[] toReturn = new byte[Length];
            for (int iter = 0; iter < toReturn.Length / 4; iter++)
            {
                toReturn[4 * iter + 0] = B;
                toReturn[4 * iter + 1] = G;
                toReturn[4 * iter + 2] = R;
                toReturn[4 * iter + 3] = A;

            }
            return toReturn;
        }
    }
}
