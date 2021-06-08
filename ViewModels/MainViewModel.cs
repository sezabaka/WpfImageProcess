using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfImageProcess.FactoryMethods;
using WpfImageProcess.Services.Interface;
using WpfImageProcess.Utils;

namespace WpfImageProcess.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IBrowseService _browse;
        private List<ImageMethod> factories;
        public List<ImageMethod> Factories
        {
            get { return factories; }
            set
            {
                factories = value;
                OnPropertyChanged("Factories");
            }
        }

        private ImageMethod selectedFactory;
        public ImageMethod SelectedFactory
        {
            get { return selectedFactory; }
            set
            {
                selectedFactory = value;
                OnPropertyChanged("SelectedFactory");
            }
        }

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

        public MainViewModel(IBrowseService browse)
        {
            _browse = browse;

            factories = ReflectionHelper.CreateAllInstancesOf<ImageMethod>().ToList();
            if (factories.Any())
            {
                SelectedFactory = factories[0];
            }
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

        
        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand => _filterCommand ??=
            new RelayCommand(_ => Filter(), _ => CanFilter());

        private void Filter()
        {
            ImageMethod method = selectedFactory;
            IProcess process = method.GetProcess();
            Img = process.ImageProcess(img);
        }

        private bool CanFilter()
        {
            return img != null;
        }
    }
}
