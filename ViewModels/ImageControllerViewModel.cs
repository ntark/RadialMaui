using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RadialMaui.Interfaces;
using RadialMaui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.ViewModels
{
    public partial class ImageControllerViewModel : ObservableObject
    {
        HttpClient _httpClient;
        IFileService _fileService;

        [ObservableProperty]
        public List<string> fileTypePickerItems = new List<string>()
        {
            "Preview",
            "Instructions"
        };

        [ObservableProperty]
        public List<string> invertPickerItems = new List<string>()
        {
            "true",
            "false"
        };

        [ObservableProperty]
        string? fileTypeSelectedItem;

        [ObservableProperty]
        string? minThreshold;

        [ObservableProperty]
        string? maxThreshold;

        [ObservableProperty]
        string? invertPickerSelectedItem;

        [ObservableProperty]
        string? conversionAngleSteps;

        [ObservableProperty]
        string? conversionRadialSteps;

        [ObservableProperty]
        string? printerAngleSteps;

        [ObservableProperty] 
        string? printerRadialSteps;

        [ObservableProperty]
        string? radialFillFileName;

        [ObservableProperty]
        string minThresholdDefault = "1";

        [ObservableProperty]
        string maxThresholdDefault = "250";

        [ObservableProperty]
        string conversionAngleStepsDefault = "1000";

        [ObservableProperty]
        string conversionRadialStepsDefault = "50";

        [ObservableProperty]
        string printerAngleStepsDefault = "-3500";

        [ObservableProperty]
        string printerRadialStepsDefault = "27800";

        [ObservableProperty]
        ImageSource? previewImageSource;

        [ObservableProperty]
        ImageSource? edgesPreviewImageSource;

        public ImageControllerViewModel(HttpClient httpClient, IFileService fileService)
        {
            _httpClient = httpClient;
            _fileService = fileService;
        }

        FileResult? PickedFileForRadialFillResult = null;

        [RelayCommand]
        async Task PickFileRadialFill()
        {
            try
            {
                var result = await FilePicker.PickAsync();

                if (result == null)
                {
                    return;
                }

                var fileName = result.FileName;

                PickedFileForRadialFillResult = result;
                RadialFillFileName = fileName;
            }
            catch (Exception ex)
            {
                UIUtil.DisplayPopup("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task RadialFill()
        {
            try
            {
                if (PickedFileForRadialFillResult == null)
                {
                    throw new Exception("No file selected");
                }

                var fileName = PickedFileForRadialFillResult.FileName;
                using var stream = await PickedFileForRadialFillResult.OpenReadAsync();

                var form = APIUtil.MultipartFileForm(fileName, stream);

                var values = new Dictionary<string, string?>()
                {
                    { "fileType", FileTypeSelectedItem},
                    { "minThreshold", MinThreshold},
                    { "maxThreshold", MaxThreshold},
                    { "invert", InvertPickerSelectedItem},
                    { "angle_steps", ConversionAngleSteps},
                    { "radius_steps", ConversionRadialSteps},
                    { "RADIUS_STEPPER_STEPS", PrinterAngleSteps},
                    { "ANGLE_STEPPER_STEPS", PrinterRadialSteps},
                };

                var queryParams = APIUtil.EncodeQueryParams(values); 

                var response = await _httpClient.PostAsync($"toRadialFill?{queryParams}", form);

                if (response.IsSuccessStatusCode)
                {
                    if(FileTypeSelectedItem == "Instructions")
                    {
                        var filePath = await _fileService.HandleDownload(response);
                        UIUtil.DisplayPopup("Success", $"file {Path.GetFileName(filePath)} {Environment.NewLine}was inserted into printer queue and was saved to {Environment.NewLine}{filePath}", "OK");
                    } 
                    else
                    {
                        var bytes = await response.Content.ReadAsByteArrayAsync();
                        PreviewImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }
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

        [RelayCommand]
        async Task ImageToGCode()
        {
            await _fileService.ConvertFile("toGCode", _httpClient);
        }

        [RelayCommand]
        async Task ImageToEdgesGCode()
        {
            await _fileService.ConvertFile("toEdgesGCode", _httpClient);
        }

        [RelayCommand]
        async Task ImageToEdges()
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

                var response = await _httpClient.PostAsync("toEdges", form);

                if (response.IsSuccessStatusCode)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    EdgesPreviewImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
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

        [RelayCommand]
        async Task ImageToSVG()
        {
            await _fileService.ConvertFile("toSvg", _httpClient);
        }

        [RelayCommand]
        async Task ImageToEdgesSVG()
        {
            await _fileService.ConvertFile("toEdgesSvg", _httpClient);
        }
    }
}
