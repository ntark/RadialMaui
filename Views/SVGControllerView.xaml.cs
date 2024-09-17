using RadialMaui.ViewModels;

namespace RadialMaui.Views;

public partial class SVGControllerView : ContentPage
{
    public SVGControllerView(SVGControllerViewModel sVGControllerViewModel)
    {
        InitializeComponent();

        BindingContext = sVGControllerViewModel;
    }
}