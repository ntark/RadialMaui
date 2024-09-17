using RadialMaui.ViewModels;

namespace RadialMaui.Views;

public partial class ImageControllerView : ContentPage
{
	public ImageControllerView(ImageControllerViewModel imageControllerViewModel)
	{
		InitializeComponent();

		BindingContext = imageControllerViewModel;
    }
}