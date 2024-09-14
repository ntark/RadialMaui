using RadialMaui.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Util
{
    public class FileUtil : IFileUtil
    {
        private readonly IFileSaveService _fileSaveService;

        public FileUtil(IFileSaveService fileSaveService)
        { 
            _fileSaveService = fileSaveService;
        }

        public async Task<string> HandleDownload(HttpResponseMessage response)
        {
            var contentDisposition = response.Content.Headers.ContentDisposition;
            var responseFileName = contentDisposition?.FileName?.Trim('"');

            if (string.IsNullOrEmpty(responseFileName))
            {
                throw new Exception("returned file has no name");
            }

            var fileBytes = await response.Content.ReadAsByteArrayAsync();

            var downloadPath = _fileSaveService.GetDownloadPath();
            var filePath = Path.Combine(downloadPath, responseFileName);

            using var fileWriteStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            await fileWriteStream.WriteAsync(fileBytes, 0, fileBytes.Length);

            return filePath;
        }
    }
}
