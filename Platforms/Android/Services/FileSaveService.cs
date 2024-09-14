using Android.OS;
using RadialMaui.Interfaces;
using RadialMaui.Platforms;
using Environment = Android.OS.Environment;

namespace RadialMaui.Platforms
{
    public class FileSaveService : IFileSaveService
    {
        public string GetDownloadPath()
        {
            var folder = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads);
            
            if (folder == null)
            {
                throw new Exception("download folder not found");
            }
            
            return folder.AbsolutePath;
        }
    }
}
