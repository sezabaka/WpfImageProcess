using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfImageProcess.FactoryMethods.Methods
{
    public class InvertMethod : IProcess
    {
        public WriteableBitmap ImageProcess(WriteableBitmap bitmap)
        {
            int h = bitmap.PixelHeight;
            int w = bitmap.PixelWidth;

            int[] pixelData = new int[h * w];

            int strike = 4 * w;
            bitmap.CopyPixels(pixelData, strike, 0);

            for (int i = 0; i < w * h; i++)
            {
                pixelData[i] = pixelData[i] ^ 0x00ffffff;
            }

            bitmap.WritePixels(new Int32Rect(0, 0, w, h), pixelData, strike, 0);

            return bitmap;
        }
    }
}
