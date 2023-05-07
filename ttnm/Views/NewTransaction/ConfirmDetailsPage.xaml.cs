using ttnm.ViewModels;

namespace ttnm.Views.NewTransaction;

public partial class ConfirmDetailsPage : ContentPage
{
	public ConfirmDetailsPage(ConfirmDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}