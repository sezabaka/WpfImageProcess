using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfImageProcess.FactoryMethods.Methods
{
    public class GrayScaleMethod : IProcess
    {
        public WriteableBitmap ImageProcess(WriteableBitmap bitmap)
        {
            int h = bitmap.PixelHeight;
            int w = bitmap.PixelWidth;

            int[] pixelData = new int[h * w];
            int[,] ma = new int[h, w];
            int strike = 4 * w;

            bitmap.CopyPixels(pixelData, strike, 0);

            for (int i = 0; i < w * h; i++)
            {
                Color co = new Color();
                co.B = (byte)(pixelData[i] & 0x000000ff);
                co.G = (byte)((pixelData[i] & 0x0000ff00) >> 8);
                co.R = (byte)((pixelData[i] & 0x00ff0000) >> 16);

                byte t = (byte)(0.299 * co.R + 0.587 * co.G + 0.114 * co.B);
                co.B = t;
                co.R = t;
                co.G = t;

                pixelData[i] = pixelData[i] = co.R * 256 * 256 + co.G * 256 + co.B;
            }

            bitmap.WritePixels(new Int32Rect(0, 0, w, h), pixelData, strike, 0);

            return bitmap;
        }
    }
}
