using ttnm.ViewModels;

namespace ttnm.Views.Dashboard;

public partial class RegisterCollectorPage : ContentPage
{
	public RegisterCollectorPage(RegisterCollectorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
