using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfImageProcess.FactoryMethods.Methods
{
    public class AddRectMethod : IProcess
    {
        // 800 * 567 -- 16 * 9 -- 50 * 63
        public WriteableBitmap ImageProcess(WriteableBitmap bitmap)
        {
            //const int width = 50;
            //const int height = 63;

            int h = bitmap.PixelHeight;
            int w = bitmap.PixelWidth;

            int[] pixelData = new int[h * w];

            int strike = 4 * w;
            bitmap.CopyPixels(pixelData, strike, 0);

            int[,] arr2D = To2DArray(pixelData, h, w);
            /*
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int[,] img = new int[62, 50];

                    for (int m = 0; m < 62; m++)
                    {
                        for (int n = 0; n < 50; n++)
                        {
                            img[m, n] = arr2D[129 + (int)(i * 62.5) + m, 78 + j * 50 + n];
                        }
                    }

                    int c = i * 16 + (j + 1);

                    CreateThumbnail(c.ToString("D3"), img);
                }
            }*/

            for (int j = 0; j < 17; j++)
            {
                for (int i = 0; i < h; i++)
                {
                    arr2D[i, 78 + j * 50] = 0x00ff0000;
                }
            }

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    arr2D[129 + (int)(j * 62.5), i] = 0x00ff0000;
                }
            }

            int[] arr = Make1DArray<int>(arr2D);
            bitmap.WritePixels(new Int32Rect(0, 0, w, h), arr, strike, 0);

            return bitmap;
        }

        void CreateThumbnail(string filename, int[,] arr)
        {
            string path = @"F:\VSCode\Img\";

            if (filename != string.Empty)
            {
                using (FileStream stream5 = new FileStream(path + filename + ".png", FileMode.Create))
                {
                    PngBitmapEncoder encoder5 = new PngBitmapEncoder();
                    var bitmap = BitmapFrame.Create(arr.GetLength(1), arr.GetLength(0), 96, 96, PixelFormats.Bgr32,
                        null, Make1DArray(arr), 4 * arr.GetLength(1));
                    var frame = BitmapFrame.Create(bitmap);
                    encoder5.Frames.Add(frame);
                    encoder5.Save(stream5);
                }
            }
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

        public T[,] To2DArray<T>(IEnumerable<T> source, int rows, int columns)
        {
            if (source == null)
                throw new ArgumentException("source");
            if (rows < 0 || columns < 0)
                throw new ArgumentException("rows and columns must be positive intergers.");
            var results = new T[rows, columns];
            if (columns == 0 || rows == 0)
            {
                return results;
            }

            int column = 0, row = 0;
            foreach (T element in source)
            {
                if (column >= columns)
                {
                    column = 0;
                    if (++row >= rows)
                    {
                        throw new InvalidOperationException("Sequence elements do not fit the array.");
                    }
                }
                results[row, column++] = element;
            }

            return results;
        }
    }
}
