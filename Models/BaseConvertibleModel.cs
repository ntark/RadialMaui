using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Models
{
    public partial class BaseConvertibleModel : ObservableObject
    {
        [ObservableProperty]
        bool convertButtonEnabled = false;
    }
}
