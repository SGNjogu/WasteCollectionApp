using ttnm.ViewModels;

namespace ttnm.Views.Dashboard;

public partial class PickupSchedulePage : ContentPage
{
	public PickupSchedulePage(PickupScheduleViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;	
	}
}