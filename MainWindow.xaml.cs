using System.Windows;
using WpfImageProcess.Services;
using WpfImageProcess.Services.Interface;
using WpfImageProcess.ViewModels;

namespace WpfImageProcess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IBrowseService browse = new BrowseService();
            this.DataContext = new MainViewModel(browse);
        }
    }
}
