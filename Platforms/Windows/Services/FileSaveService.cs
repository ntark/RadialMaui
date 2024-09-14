using RadialMaui.Interfaces;
using RadialMaui.Platforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Platforms
{
    public class FileSaveService : IFileSaveService
    {
        public string DownloadFile(string filename, HttpResponseMessage response)
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            var fileBytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();

            var filePath = Path.Combine(folderPath, filename);

            using var fileWriteStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            fileWriteStream.WriteAsync(fileBytes, 0, fileBytes.Length).GetAwaiter().GetResult();

            return filePath;
        }
    }
}
