using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RadialMaui.Interfaces;
using RadialMaui.Models;
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
    public partial class ImageControllerViewModel : BaseViewModel
    {
        HttpClient _httpClient;
        IFileService _fileService;

        [ObservableProperty]
        ImageToRadialFillFields imageToRadialFill = new ImageToRadialFillFields();

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
                ImageToRadialFill.FileName = fileName;
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
                IsBusy = true;
                if (PickedFileForRadialFillResult == null)
                {
                    throw new Exception("No file selected");
                }

                var fileName = PickedFileForRadialFillResult.FileName;
                using var stream = await PickedFileForRadialFillResult.OpenReadAsync();

                var form = APIUtil.MultipartFileForm(fileName, stream);

                var values = new Dictionary<string, string?>()
                {
                    { "fileType", ImageToRadialFill.FileTypeSelectedItem},
                    { "minThreshold", ImageToRadialFill.MinThreshold},
                    { "maxThreshold", ImageToRadialFill.MaxThreshold},
                    { "invert", ImageToRadialFill.InvertPickerSelectedItem},
                    { "angle_steps", ImageToRadialFill.ConversionAngleSteps},
                    { "radius_steps", ImageToRadialFill.ConversionRadialSteps},
                    { "RADIUS_STEPPER_STEPS", ImageToRadialFill.AngleSteps},
                    { "ANGLE_STEPPER_STEPS", ImageToRadialFill.RadialSteps},
                };

                var queryParams = APIUtil.EncodeQueryParams(values); 

                var response = await _httpClient.PostAsync($"toRadialFill?{queryParams}", form);

                if (response.IsSuccessStatusCode)
                {
                    if(ImageToRadialFill.FileTypeSelectedItem == "Instructions")
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
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task ImageToGCode()
        {
            try
            {
                IsBusy = true;
                await _fileService.ConvertFile("toGCode", _httpClient);
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
        async Task ImageToEdgesGCode()
        {
            try
            {
                IsBusy = true;
                await _fileService.ConvertFile("toEdgesGCode", _httpClient);
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
            try
            {
                IsBusy = true;
                await _fileService.ConvertFile("toSvg", _httpClient);
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
        async Task ImageToEdgesSVG()
        {
            try
            {
                IsBusy = true;
                await _fileService.ConvertFile("toEdgesSvg", _httpClient);
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
    }
}
