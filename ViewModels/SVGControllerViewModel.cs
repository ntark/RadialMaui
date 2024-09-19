using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RadialMaui.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.ViewModels
{
    public partial class SVGControllerViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;

        public SVGControllerViewModel(HttpClient httpClient, IFileService fileService)
        {
            _httpClient = httpClient;
            _fileService = fileService;
        }

        [RelayCommand]
        async Task SVGToGCode()
        {
            await _fileService.ConvertFile("toGCode", _httpClient);
        }
    }
}
