using ttnm.ViewModels;

namespace ttnm.Views.NewTransaction;

public partial class EnterPaymentDetailsPage : ContentPage
{
	public EnterPaymentDetailsPage(EnterPaymentDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}