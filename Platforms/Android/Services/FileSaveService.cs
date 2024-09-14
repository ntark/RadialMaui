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
            var writeStatus = Permissions.CheckStatusAsync<Permissions.StorageWrite>().GetAwaiter().GetResult();

            if (writeStatus != PermissionStatus.Granted)
            {
                writeStatus = Permissions.RequestAsync<Permissions.StorageWrite>().GetAwaiter().GetResult();
            }

            var readStatus = Permissions.CheckStatusAsync<Permissions.StorageRead>().GetAwaiter().GetResult();

            if (readStatus != PermissionStatus.Granted)
            {
                readStatus = Permissions.RequestAsync<Permissions.StorageRead>().GetAwaiter().GetResult();
            }

            if (readStatus != PermissionStatus.Granted || writeStatus != PermissionStatus.Granted)
            {
                throw new Exception("No rights to save to file storage");
            }

            var folder = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);

            if (folder == null)
            {
                throw new Exception("download folder not found");
            }
            
            return folder.AbsolutePath;
        }
    }
}
