using System.Windows;
using WpfImageProcess.Services;
using WpfImageProcess.Services.Interface;
using WpfImageProcess.ViewModels;

namespace WpfImageProcess
{
    /// <summary>
    /// Interaction logic for CheckWindow.xaml
    /// </summary>
    public partial class CheckWindow : Window
    {
        public CheckWindow()
        {
            InitializeComponent();
            IBrowseService browse = new BrowseService();
            this.DataContext = new CheckViewModel(browse);
        }
    }
}
