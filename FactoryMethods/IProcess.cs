using System.Windows.Media.Imaging;

namespace WpfImageProcess.FactoryMethods
{
    public interface IProcess
    {
        WriteableBitmap ImageProcess(WriteableBitmap bitmap);
    }
}
