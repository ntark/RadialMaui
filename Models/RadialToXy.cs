using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Models
{
    public partial class RadialToXyFields : BaseConvertibleModel
    {
        public string RadialStepsDefault => "-3500";
        public string AngleStepsDefault => "27800";

        [ObservableProperty]
        string? fileName;

        [ObservableProperty]
        string? radialSteps;

        [ObservableProperty]
        string? angleSteps;
    }
}
