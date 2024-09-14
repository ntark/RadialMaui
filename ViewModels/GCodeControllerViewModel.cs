using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RadialMaui.Interfaces;
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
    public partial class GCodeControllerViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly IFileUtil _fileUtil;

        private readonly string BaseUrl = "https://kvash.tar.ge/GCode/";

        #region observable Properties

        [ObservableProperty]
        string maxDistanceDefault;

        [ObservableProperty]
        string radialStepsDefault;

        [ObservableProperty]
        string angleStepsDefault;

        [ObservableProperty]
        string? status;

        [ObservableProperty]
        string latestFile;

        [ObservableProperty]
        ImageSource previewImageSource;

        [ObservableProperty]
        string gCodePreviewButtonText;

        #region Radial To XY
        [ObservableProperty]
        string? radialToXyFileName;

        [ObservableProperty]
        string? radialToXyRadialSteps;

        [ObservableProperty]
        string? radialToXyAngleSteps;
        #endregion

        #region Radial To XY
        [ObservableProperty]
        string? xyToRadialFileName;

        [ObservableProperty]
        string? xyToRadialMaxDistance;

        [ObservableProperty]
        string? xyToRadialRadialSteps;

        [ObservableProperty]
        string? xyToRadialAngleSteps;
        #endregion

        #endregion

        #region Private Fields

        FileResult? RadialToXyFileResult = null;
        FileResult? XyToRadialFileResult = null;

        #endregion

        public GCodeControllerViewModel(
            HttpClient httpClient,
            IFileUtil fileUtil
            )
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _fileUtil = fileUtil;


            MaxDistanceDefault = "0.1";
            RadialStepsDefault = "-3500";
            AngleStepsDefault = "27800";

            RadialToXyFileName = "Choose a file";
            XyToRadialFileName = "Choose a file";

            LatestFile = "Check to see";

            PreviewImageSource = ImageSource.FromFile("gcodepreviewplaceholder.png");

            GCodePreviewButtonText = "Upload";

            _ = ReCheck();
        }

        [RelayCommand]
        async Task ReCheck()
        {
            try
            {
                Status = "checking...";

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
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        private async Task SetDrawing(bool drawing)
        {
            try
            {
                Status = drawing ? "Starting..." : "Stopping...";

                var values = new Dictionary<string, string>()
                {
                    { "drawing", drawing ? "true" : "false"}
                };

                var queryParams = APIUtil.EncodeQueryParams(values);

                var response = await _httpClient.PostAsync($"setDrawing?{queryParams}", null);

                if (response.IsSuccessStatusCode)
                {
                    _ = ReCheck();
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
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
                RadialToXyFileName = fileName;
            }
            catch (Exception ex)
            {
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task RadialToXY()
        {
            try
            {
                if (RadialToXyFileResult == null)
                {
                    throw new Exception("No file selected");
                }

                using var stream = await RadialToXyFileResult.OpenReadAsync();
                var fileName = RadialToXyFileResult.FileName;

                var form = new MultipartFormDataContent();

                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                form.Add(fileContent, "file", fileName);

                var radialSteps = string.IsNullOrEmpty(RadialToXyRadialSteps) ? RadialStepsDefault : RadialToXyRadialSteps;
                var angleSteps = string.IsNullOrEmpty(RadialToXyAngleSteps) ? AngleStepsDefault : RadialToXyAngleSteps;

                var values = new Dictionary<string, string>()
                {
                    { "radialSteps", radialSteps},
                    { "angleSteps", angleSteps},
                };

                var queryParams = APIUtil.EncodeQueryParams(values);

                var response = await _httpClient.PostAsync($"radToXy?{queryParams}", form);

                if (response.IsSuccessStatusCode)
                {
                    var filePath = await _fileUtil.HandleDownload(response);

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
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
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
                XyToRadialFileName = fileName;
            }
            catch (Exception ex)
            {
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task XyToRadial()
        {
            try
            {
                if (XyToRadialFileResult == null)
                {
                    throw new Exception("No file selected");
                }

                using var stream = await XyToRadialFileResult.OpenReadAsync();
                var fileName = XyToRadialFileResult.FileName;

                var form = new MultipartFormDataContent();

                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                form.Add(fileContent, "file", fileName);

                var maxDistance = string.IsNullOrEmpty(XyToRadialMaxDistance) ? MaxDistanceDefault : XyToRadialMaxDistance;
                var radialSteps = string.IsNullOrEmpty(XyToRadialRadialSteps) ? RadialStepsDefault : XyToRadialRadialSteps;
                var angleSteps = string.IsNullOrEmpty(XyToRadialAngleSteps) ? AngleStepsDefault : XyToRadialAngleSteps;

                var values = new Dictionary<string, string>()
                {
                    { "maxDistance", maxDistance},
                    { "radialSteps", radialSteps},
                    { "angleSteps", angleSteps},
                };

                var queryParams = APIUtil.EncodeQueryParams(values);

                var response = await _httpClient.PostAsync($"xyToRad?{queryParams}", form);

                if (response.IsSuccessStatusCode)
                {
                    var filePath = await _fileUtil.HandleDownload(response);

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
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task FetchLatest()
        {
            try
            {
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
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task GCodePreviewButton()
        {
            try
            {
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
                Status = "Error...";

                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }

            GCodePreviewButtonText = "Upload";
        }
    }
}
