using RadialMaui.ViewModels;

namespace RadialMaui;

public partial class GCodeControllerView : ContentPage
{
	public GCodeControllerView(GCodeControllerViewModel gCodeControllerViewModel)
	{
		InitializeComponent();

		BindingContext = gCodeControllerViewModel;
	}
}