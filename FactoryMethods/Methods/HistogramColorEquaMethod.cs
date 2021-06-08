using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfImageProcess.FactoryMethods.Methods
{
    public class HistogramColorEquaMethod : IProcess
    {
        public WriteableBitmap ImageProcess(WriteableBitmap bitmap)
        {
            int h = bitmap.PixelHeight;
            int w = bitmap.PixelWidth;

            int[] pixelData = new int[h * w];

            int strike = 4 * w;
            bitmap.CopyPixels(pixelData, strike, 0);

            int[] res = MakeHistogramEquaColor(pixelData);

            bitmap.WritePixels(new Int32Rect(0, 0, w, h), res, strike, 0);
            return bitmap;
        }

        public static int[] MakeHistogramEquaColor(int[] input)
        {
            int h = input.Length;

            int[] freR = new int[256];
            int[] freG = new int[256];
            int[] freB = new int[256];

            for (int i = 0; i < 256; i++)
            {
                freR[i] = 0;
                freG[i] = 0;
                freB[i] = 0;
            }

            for (int i = 0; i < h; i++)
            {
                byte R = (byte)((input[i] & 0x00ff0000) >> 16);
                byte G = (byte)((input[i] & 0x0000ff00) >> 8);
                byte B = (byte)(input[i] & 0x000000ff);

                freR[R]++;
                freG[G]++;
                freB[B]++;
            }

            int[] cdfR = new int[256];
            int[] cdfG = new int[256];
            int[] cdfB = new int[256];

            int totalR = 0;
            int totalG = 0;
            int totalB = 0;

            for (int i = 0; i < 256; i++)
            {
                totalR += freR[i];
                cdfR[i] = totalR;

                totalG += freG[i];
                cdfG[i] = totalG;

                totalB += freB[i];
                cdfB[i] = totalB;
            }

            int minR = cdfR.Min();
            int minG = cdfG.Min();
            int minB = cdfB.Min();

            double cdfminR = (double)(h - minR) / (double)255;
            double cdfminG = (double)(h - minG) / (double)255;
            double cdfminB = (double)(h - minB) / (double)255;

            for (int i = 0; i < h; i++)
            {
                byte R = (byte)((input[i] & 0x00ff0000) >> 16);
                byte G = (byte)((input[i] & 0x0000ff00) >> 8);
                byte B = (byte)(input[i] & 0x000000ff);

                byte nR = (byte)Math.Round((double)(cdfR[R] - minR) / cdfminR);
                byte nG = (byte)Math.Round((double)(cdfG[G] - minG) / cdfminG);
                byte nB = (byte)Math.Round((double)(cdfB[B] - minB) / cdfminB);

                input[i] = (nR << 16) + (nG << 8) + nB;
            }

            return input;
        }
    }
}
