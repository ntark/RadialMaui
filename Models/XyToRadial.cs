using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Models
{
    public partial class XyToRadialFields : BaseConvertibleModel
    {
        public string MaxDistanceDefault => "0.1";
        public string RadialStepsDefault => "-3500";
        public string AngleStepsDefault => "27800";

        [ObservableProperty]
        string? fileName;

        [ObservableProperty]
        string? maxDistance;

        [ObservableProperty]
        string? radialSteps;

        [ObservableProperty]
        string? angleSteps;
    }
}
