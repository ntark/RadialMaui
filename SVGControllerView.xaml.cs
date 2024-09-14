using RadialMaui.ViewModels;

namespace RadialMaui;

public partial class SVGControllerView : ContentPage
{
    public SVGControllerView(SVGControllerViewModel sVGControllerViewModel)
    {
        InitializeComponent();

        BindingContext = sVGControllerViewModel;
    }
}