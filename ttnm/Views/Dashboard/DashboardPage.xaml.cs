using ttnm.ViewModels;

namespace ttnm.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;	
	}
}