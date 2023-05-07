using ttnm.ViewModels;

namespace ttnm.Views.Login;

public partial class VerifyCollectorPage : ContentPage
{
	public VerifyCollectorPage(VerifyCollectorViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}