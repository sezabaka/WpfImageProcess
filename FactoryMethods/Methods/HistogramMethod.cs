using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfImageProcess.FactoryMethods.Methods
{
    public class HistogramMethod : IProcess
    {
        public WriteableBitmap ImageProcess(WriteableBitmap bitmap)
        {
            int h = bitmap.PixelHeight;
            int w = bitmap.PixelWidth;

            int[] pixelData = new int[h * w];

            int strike = 4 * w;
            bitmap.CopyPixels(pixelData, strike, 0);

            int[,] res = MakeArrayHistogram(pixelData);
            int nh = res.GetLength(0);
            int nw = res.GetLength(1);

            int[] arr = Make1DArray(res);

            WriteableBitmap wb = new WriteableBitmap(nw, nh, 96, 96, PixelFormats.Bgr32, null);

            wb.WritePixels(new Int32Rect(0, 0, nw, nh), arr, 4 * nw, 0);

            return wb;
        }

        private T[] Make1DArray<T>(T[,] input)
        {
            int h = input.GetLength(0);
            int w = input.GetLength(1);
            T[] output = new T[h * w];

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    output[i * w + j] = input[i, j];
                }
            }
            return output;
        }

        public int[,] MakeArrayHistogram(int[] input)
        {
            int[,] output = new int[256, 256];
            int[] countR = new int[256];
            int[] countG = new int[256];
            int[] countB = new int[256];

            for (int i = 0; i < 256; i++)
            {
                countR[i] = 0;
                countG[i] = 0;
                countB[i] = 0;
            }

            for (int i = 0; i < input.Length; i++)
            {
                byte R = (byte)((input[i] & 0x00ff0000) >> 16);
                byte G = (byte)((input[i] & 0x0000ff00) >> 8);
                byte B = (byte)(input[i] & 0x000000ff);

                countR[R]++;
                countG[G]++;
                countB[B]++;
            }

            int max = Math.Max(Math.Max(countR.Max(), countG.Max()), countB.Max());

            for (int i = 0; i < 256; i++)
            {
                countR[i] = (int)(((float)countR[i] / (float)max) * 256);
                countG[i] = (int)(((float)countG[i] / (float)max) * 256);
                countB[i] = (int)(((float)countB[i] / (float)max) * 256);
            }

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if (y == countR[x])
                    {
                        output[255 - y, x] = 0x00ff0000;
                    }
                    else if (y == countG[x])
                    {
                        output[255 - y, x] = 0x0000ff00;
                    }
                    else if (y == countB[x])
                    {
                        output[255 - y, x] = 0x000000ff;
                    }
                    else
                    {
                        output[255 - y, x] = 0x00ffffff;
                    }
                }
            }

            return output;
        }
    }
}
