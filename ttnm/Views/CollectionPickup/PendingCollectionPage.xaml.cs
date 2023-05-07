using ttnm.ViewModels;

namespace ttnm.Views.CollectionPickup;

public partial class PendingCollectionPage : ContentPage
{
	private readonly PendingCollectionViewModel viewModel;

    public PendingCollectionPage()
	{
		InitializeComponent();

		viewModel= new PendingCollectionViewModel();
		BindingContext= viewModel;
	}
}