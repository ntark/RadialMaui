using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Models
{
    public partial class ImageToRadialFillFields : BaseConvertibleModel
    {
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

        public string MinThresholdDefault => "1";

        public string MaxThresholdDefault => "250";

        public string ConversionAngleStepsDefault => "1000";

        public string ConversionRadialStepsDefault => "50";

        public string PrinterAngleStepsDefault => "-3500";

        public string PrinterRadialStepsDefault => "27800";


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
        string? angleSteps;

        [ObservableProperty]
        string? radialSteps;

        [ObservableProperty]
        string? fileName;

        [ObservableProperty]
        ImageSource? previewImageSource;
    }
}
