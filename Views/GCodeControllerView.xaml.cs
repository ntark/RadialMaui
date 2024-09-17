using RadialMaui.ViewModels;

namespace RadialMaui.Views;

public partial class GCodeControllerView : ContentPage
{
	public GCodeControllerView(GCodeControllerViewModel gCodeControllerViewModel)
	{
		InitializeComponent();

		BindingContext = gCodeControllerViewModel;
	}
}