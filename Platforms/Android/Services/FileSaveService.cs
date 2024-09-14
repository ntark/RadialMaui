using Android.App;
using Android.Content;
using Android.OS;
using RadialMaui.Interfaces;
using RadialMaui.Platforms;
using System.Formats.Asn1;
using Environment = Android.OS.Environment;

namespace RadialMaui.Platforms
{
    public class FileSaveService : IFileSaveService
    {
        public string DownloadFile(string filename, HttpResponseMessage response)
        {
            var context = Android.App.Application.Context;
            var downloadManager = (DownloadManager?)context.GetSystemService(Context.DownloadService);

            if (downloadManager == null)
            {
                throw new Exception("unable to access download manager");
            }

            var url = $"https://kvash.tar.ge/File?fileName={filename}";

            var request = new DownloadManager.Request(Android.Net.Uri.Parse(url));
            request.SetTitle(filename);
            request.SetDescription("downloading ...");
            request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            request.SetDestinationInExternalPublicDir(Environment.DirectoryDownloads, filename);

            downloadManager.Enqueue(request);

            return Path.Join(Environment.DirectoryDownloads, filename);
        }
    }
}
