using ttnm.ViewModels;

namespace ttnm.Views.NewTransaction;

public partial class ConfirmPaymentPage : ContentPage
{
    public ConfirmPaymentPage(ConfirmPaymentDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}