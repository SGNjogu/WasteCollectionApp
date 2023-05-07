using ttnm.ViewModels;

namespace ttnm.Views.Dashboard;

public partial class SupportPage : ContentPage
{
	public SupportPage(SupportViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;	
	}
}