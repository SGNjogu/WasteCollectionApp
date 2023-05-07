using ttnm.ViewModels;

namespace ttnm.Views.Settings;

public partial class MapPage : ContentPage
{
	public MapPage(MapViewModel mapViewModel)
	{
		InitializeComponent();
		BindingContext = mapViewModel;
	}
}
