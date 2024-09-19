using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RadialMaui.Interfaces;
using RadialMaui.Models;
using RadialMaui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RadialMaui.ViewModels
{
    public partial class GCodeControllerViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;

        #region observable Properties

        [ObservableProperty]
        string? status;

        [ObservableProperty]
        string? latestFile;

        [ObservableProperty]
        ImageSource? previewImageSource;

        [ObservableProperty]
        string gCodePreviewButtonText = "Upload";

        [ObservableProperty]
        RadialToXyFields radialToXy = new RadialToXyFields();

        [ObservableProperty]
        XyToRadialFields xyToRadial = new XyToRadialFields();

        #endregion

        #region Private Fields

        FileResult? RadialToXyFileResult = null;
        FileResult? XyToRadialFileResult = null;

        #endregion

        public GCodeControllerViewModel(
            HttpClient httpClient,
            IFileService fileService
            )
        {
            _httpClient = httpClient;
            _fileService = fileService;

            RadialToXy.FileName = "Choose a file";
            XyToRadial.FileName = "Choose a file";

            _ = ReCheck();
        }

        [RelayCommand]
        async Task ReCheck()
        {
            try
            {
                IsBusy = true;
                Status = "checking";

                var response = await _httpClient.PostAsync("drawing", null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    Status = responseContent == "true" ? "On" : "Off";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Status = "Error";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SetDrawing(bool drawing)
        {
            try
            {
                IsBusy = true;
                Status = drawing ? "Starting" : "Stopping";

                var values = new Dictionary<string, string?>()
                {
                    { "drawing", drawing ? "true" : "false"}
                };

                var queryParams = APIUtil.EncodeQueryParams(values);

                var response = await _httpClient.PostAsync($"setDrawing?{queryParams}", null);

                if (response.IsSuccessStatusCode)
                {
                    await ReCheck().ConfigureAwait(false);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Status = "Error";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task StartDrawing()
        {
            await SetDrawing(true);
        }

        [RelayCommand]
        async Task StopDrawing()
        {
            await SetDrawing(false);
        }

        [RelayCommand]
        async Task PickFileRadialToXy()
        {
            try
            {
                var result = await FilePicker.PickAsync();

                if (result == null)
                {
                    return;
                }

                var fileName = result.FileName;

                RadialToXyFileResult = result;
                RadialToXy.FileName = fileName;
                RadialToXy.ConvertButtonEnabled = true;
            }
            catch (Exception ex)
            {
                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task RadialToXY()
        {
            try
            {
                IsBusy = true;
                if (RadialToXyFileResult == null)
                {
                    throw new Exception("No file selected");
                }

                var values = new Dictionary<string, string?>()
                {
                    { "radialSteps", RadialToXy.RadialSteps},
                    { "angleSteps", RadialToXy.AngleSteps},
                };

                using var stream = await RadialToXyFileResult.OpenReadAsync();

                var form = APIUtil.MultipartFileForm(RadialToXyFileResult.FileName, stream);

                var queryParams = APIUtil.EncodeQueryParams(values);

                var response = await _httpClient.PostAsync($"radToXy?{queryParams}", form);

                if (response.IsSuccessStatusCode)
                {
                    var filePath = await _fileService.HandleDownload(response);

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
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task PickFileXyToRadial()
        {
            try
            {
                var result = await FilePicker.PickAsync();

                if (result == null)
                {
                    return;
                }

                var fileName = result.FileName;

                XyToRadialFileResult = result;
                XyToRadial.FileName = fileName;
                XyToRadial.ConvertButtonEnabled = true;
            }
            catch (Exception ex)
            {
                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task XYToRadial()
        {
            try
            {
                IsBusy = true;
                if (XyToRadialFileResult == null)
                {
                    throw new Exception("No file selected");
                }

                using var stream = await XyToRadialFileResult.OpenReadAsync();

                var form = APIUtil.MultipartFileForm(XyToRadialFileResult.FileName, stream);

                var values = new Dictionary<string, string?>()
                {
                    { "maxDistance", XyToRadial.MaxDistance},
                    { "radialSteps", XyToRadial.RadialSteps},
                    { "angleSteps", XyToRadial.AngleSteps},
                };

                var queryParams = APIUtil.EncodeQueryParams(values);

                var response = await _httpClient.PostAsync($"xyToRad?{queryParams}", form);

                if (response.IsSuccessStatusCode)
                {
                    var filePath = await _fileService.HandleDownload(response);

                    UIUtil.DisplayPopup("Success", $"file {Path.GetFileName(filePath)} {Environment.NewLine}was inserted into printer queue and was saved to {Environment.NewLine}{filePath}", "OK");
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
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task FetchLatest()
        {
            try
            {
                IsBusy = true;
                var response = await _httpClient.PostAsync($"fetchLatest", null);

                if (response.IsSuccessStatusCode)
                {
                    LatestFile = await response.Content.ReadAsStringAsync();
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
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GCodePreviewButton()
        {
            try
            {
                IsBusy = true;
                var result = await FilePicker.PickAsync();

                if (result == null)
                {
                    return;
                }

                GCodePreviewButtonText = "Working...";

                using var stream = await result.OpenReadAsync();
                var fileName = result.FileName;

                var form = new MultipartFormDataContent();

                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                form.Add(fileContent, "file", fileName);

                var response = await _httpClient.PostAsync($"gcodePreview", form);

                if (response.IsSuccessStatusCode)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    PreviewImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
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
            finally
            {
                IsBusy = false;
                GCodePreviewButtonText = "Upload";
            }

        }
    }
}
