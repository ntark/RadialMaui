using RadialMaui.ViewModels;

namespace RadialMaui;

public partial class ImageControllerView : ContentPage
{
	public ImageControllerView(ImageControllerViewModel imageControllerViewModel)
	{
		InitializeComponent();

		BindingContext = imageControllerViewModel;
    }
}