using System;
using Microsoft.Win32;
using WpfImageProcess.Services.Interface;

namespace WpfImageProcess.Services
{
    public class BrowseService : IBrowseService
    {
        public string OpenFile(string caption, string filter = "All files (*.*)|*.*")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dialog.Title = caption;
            dialog.Filter = filter;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true) return dialog.FileName;

            return string.Empty;
        }
    }
}
