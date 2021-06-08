namespace WpfImageProcess.Services.Interface
{
    public interface IBrowseService
    {
        string OpenFile(string caption, string filter = @"All files (*.*)|*.*");
    }
}
