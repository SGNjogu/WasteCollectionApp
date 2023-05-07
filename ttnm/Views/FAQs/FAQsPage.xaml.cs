using ttnm.ViewModels;

namespace ttnm.Views.FAQs;

public partial class FAQsPage : ContentPage
{
	public FAQsPage(FAQsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;	
	}
}