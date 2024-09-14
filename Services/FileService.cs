using RadialMaui.Interfaces;
using RadialMaui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Services
{
    public class FileService : IFileService
    {
        private readonly IFileSaveService _fileSaveService;

        public FileService(IFileSaveService fileSaveService)
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

        public async Task ConvertFile(string endpoint, HttpClient httpClient)
        {
            try
            {
                var result = await FilePicker.PickAsync();

                if (result == null)
                {
                    return;
                }

                var fileName = result.FileName;
                using var stream = await result.OpenReadAsync();

                var form = APIUtil.MultipartFileForm(fileName, stream);

                var response = await httpClient.PostAsync(endpoint, form);

                if (response.IsSuccessStatusCode)
                {
                    var filePath = await HandleDownload(response);
                    UIUtil.DisplayPopup("Success", $"file {Path.GetFileName(filePath)} {Environment.NewLine}was saved to {Environment.NewLine}{filePath}", "OK");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }
    }
}
