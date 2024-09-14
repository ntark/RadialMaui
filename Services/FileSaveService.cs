//using RadialMaui.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//#if ANDROID
//using Android.OS;
//using Environment = Android.OS.Environment;
//#elif WINDOWS

//#else
//throw new NotImplementedException();
//#endif

//namespace RadialMaui.Services
//{
//    public class FileSaveService : IFileSaveService
//    {
//        string GetDownloadPath()
//        {
//#if ANDROID
//            var folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            
//            if (folder == null)
//            {
//                throw new Exception("download folder not found");
//            }
            
//            return folder.AbsolutePath;
//#elif WINDOWS
//            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
//#else
//            throw new NotImplementedException();
//#endif
//        }
//    }
//}
