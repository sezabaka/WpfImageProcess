using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfImageProcess.Services.Interface;

namespace WpfImageProcess.ViewModels
{
    public class CheckViewModel : ViewModelBase
    {
        private readonly IBrowseService _browse;

        private WriteableBitmap img;
        public WriteableBitmap Img
        {
            get { return img; }
            set
            {
                img = value;
                OnPropertyChanged("Img");
            }
        }

        private WriteableBitmap img1;
        public WriteableBitmap Img1
        {
            get { return img1; }
            set
            {
                img1 = value;
                OnPropertyChanged("Img1");
            }
        }

        public CheckViewModel(IBrowseService browse)
        {
            _browse = browse;
        }

        private RelayCommand _browseCommand;
        public RelayCommand BrowseCommand => _browseCommand ??=
            new RelayCommand(_ => Browse());

        private void Browse()
        {
            string path = _browse.OpenFile("Open Image", "Image File|*.jpg;*.png;*.bmp");

            if (!string.IsNullOrWhiteSpace(path))
            {
                BitmapImage image = new BitmapImage(new System.Uri(path));
                BitmapSource bS = new FormatConvertedBitmap(image, PixelFormats.Bgr32, null, 0);

                Img = new WriteableBitmap(bS);
            }
        }

        private RelayCommand _browse1Command;
        public RelayCommand Browse1Command => _browse1Command ??=
            new RelayCommand(_ => Browse1());

        /*private void Browse1()
        {
            string path = _browse.OpenFile("Open Image", "Image File|*.jpg;*.png;*.bmp");
            if (!string.IsNullOrWhiteSpace(path))
            {
                BitmapImage image = new BitmapImage(new System.Uri(path));
                BitmapSource bS = new FormatConvertedBitmap(image, PixelFormats.Bgr32, null, 0);

                Img1 = new WriteableBitmap(bS);
            }
        }*/

        private void Browse1()
        {
            string path = @"F:\VSCode\Img\Identify";

            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                SaveLable(files[i], path, i);
            }
        }

        private void SaveLable(string path, string root, int i)
        {
            BitmapImage image = new BitmapImage(new System.Uri(path));
            BitmapSource bS = new FormatConvertedBitmap(image, PixelFormats.Bgr32, null, 0);

            var wb = new WriteableBitmap(bS);

            int h = wb.PixelHeight;
            int w = wb.PixelWidth;

            int[] pixelData = new int[h * w];

            int strike = 4 * w;
            wb.CopyPixels(pixelData, strike, 0);

            int[,] res = RemoveColor(pixelData);

            int[] arr = Make1DArray<int>(res);

            int[,] his = MakeArrayHistogram(arr, root.Replace("Identify", "IdenLabel1"), i);

            CreateThumbnail(path.Replace("Identify", "IdenLabel"), res, root.Replace("Identify", "IdenLabel"), i);
            CreateThumbnail(path.Replace("Identify", "IdenLabel1"), his, root.Replace("Identify", "IdenLabel1"), i);
        }

        void CreateThumbnail(string filename, int[,] arr, string root, int i)
        {
            string path = root + "\\" + (i / 4 + 1).ToString("D2") + "-" + (i % 4) + ".png";
            if (filename != string.Empty)
            {
                using (FileStream stream5 = new FileStream(path, FileMode.Create))
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

        public int[,] MakeArrayHistogram(int[] input, string path, int k)
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

            var cR = GetIndex(countR);
            var cG = GetIndex(countG);
            var cB = GetIndex(countB);

            string s = $"{path}\\{(k / 4 + 1).ToString("D2")}-{k % 4}-{cR}-{cG}-{cB}.txt";
            File.WriteAllText(s, "");

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

        private int GetIndex(int[] intList)
        {
            /*int indexMax
                = !intList.Any() ? -1 :
                intList.Where((i, x) => x > 0)
                .Select((value, index) => new { Value = value, Index = index })
                .Aggregate((a, b) => (a.Value > b.Value) ? a : b)
                .Index + 1;*/

            var indexMax = intList.Where((i, x) => x > 0).Select((value, index) => new { Value = value, Index = index + 1 })
                .OrderByDescending(x => x.Value).Take(2);

            var res = indexMax.Sum(x => x.Index * x.Value);

            return res;
        }

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand => _filterCommand ??=
            new RelayCommand(_ => Filter(), _ => CanFilter());

        private void Filter()
        {
            int h = img.PixelHeight;
            int w = img.PixelWidth;

            int[] pixelData = new int[h * w];

            int strike = 4 * w;
            img.CopyPixels(pixelData, strike, 0);

            int[,] res = RemoveColor(pixelData);
            int[] arr = Make1DArray<int>(res);

            int nh = res.GetLength(0);
            int nw = res.GetLength(1);

            WriteableBitmap wb = new WriteableBitmap(nw, nh, 96, 96, PixelFormats.Bgr32, null);

            wb.WritePixels(new Int32Rect(0, 0, nw, nh), arr, 4 * nw, 0);
            Img = wb;
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

        List<int> l = new List<int>();

        private bool FilterColor(int color, int filter)
        {
            byte R = (byte)((color & 0x00ff0000) >> 16);
            byte G = (byte)((color & 0x0000ff00) >> 8);
            byte B = (byte)(color & 0x000000ff);

            byte xR = (byte)((filter & 0x00ff0000) >> 16);
            byte xG = (byte)((filter & 0x0000ff00) >> 8);
            byte xB = (byte)(filter & 0x000000ff);
            byte num = 10;

            if (xR > R - num && xR < R + num && xG > G - num && xG < G + num && xB > B - num && xB < B + num)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int[,] RemoveColor(int[] arr)
        {
            int[,] res = new int[52, 40];

            int[,] temp = To2DArray<int>(arr, 62, 50);

            /*if (!l.Any())
            {
                l = MakeList(temp[5, 5]);
            }

            List<string> s = new List<string>();

            for (int i = 0; i < l.Count; i++)
            {
                s.Add(l[i].ToString("X8"));
            }*/

            //File.WriteAllLines(@"F:\aa.txt", s);

            for (int i = 0; i < 52; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    //if (!l.Any(x => x == temp[i + 5, j + 5]))
                    if (FilterColor(temp[i + 5, j + 5], temp[5, 5]))
                        res[i, j] = temp[i + 5, j + 5];
                }
            }

            return res;
        }

        private List<int> MakeList(int color)
        {
            List<int> res = new List<int>();
            List<int> temp = new List<int>();

            res.Add(color);

            for (int i = 1; i < 15; i++)
            {
                temp.Add(0x00010000 * i);
                temp.Add(0x00000100 * i);
                temp.Add(0x00000001 * i);
            }

            int count = temp.Count;

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    int k = temp[i] + temp[j];

                    if (!temp.Any(x => x == k))
                        temp.Add(k);
                }
            }

            count = temp.Count;

            for (int i = 0; i < count; i++)
            {
                res.Add(color + temp[i]);
                res.Add(color - temp[i]);
            }

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    res.Add(color - temp[i] + temp[j]);
                    res.Add(color + temp[i] - temp[j]);
                }
            }

            return res.Distinct().ToList();
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

        private bool CanFilter()
        {
            return img != null;
        }

        private RelayCommand _checkCommand;
        public RelayCommand CheckCommand => _checkCommand ??=
            new RelayCommand(_ => Check(), _ => CanCheck());

        private void Check()
        {
            int h = img.PixelHeight;
            int w = img.PixelWidth;

            int[] pixelData = new int[h * w];

            int strike = 4 * w;
            img.CopyPixels(pixelData, strike, 0);

            int[] pixelData1 = new int[h * w];
            img1.CopyPixels(pixelData1, strike, 0);

            for (int i = 0; i < h * w; i++)
            {
                if (pixelData[i] != pixelData1[i])
                {
                    break;
                }
            }
        }

        private bool CanCheck()
        {
            return img != null && img1 != null;
        }
    }
}
