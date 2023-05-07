using CommunityToolkit.Mvvm.Messaging;
using ttnm.Models;
using ttnm.Services.DataSync;
using ttnm.Services.Dialogs;
using ttnm.Services.Maps;
using ttnm.ViewModels;
using ttnm.Views.CollectionPickup;

namespace ttnm.Views.Dashboard;

public partial class CollectionPickupPage : ContentPage
{
    private readonly IMapService _mapService;
    private readonly IDialogService _dialogService;
    private readonly IPushDataService _pushDataService;
    public CollectionPickupPage(CollectionPickupViewModel viewModel, IMapService mapService, IDialogService dialogService, IPushDataService pushDataService)
    {
        InitializeComponent();
        _mapService = mapService;
        _dialogService = dialogService;
        _pushDataService = pushDataService;
        BindingContext = viewModel;
    }

    private async void AcceptedListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var selectCollection = e.Item as AcceptedCollectionRequest;
        await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new AcceptedCollectionDetailsPage(_mapService, _dialogService, _pushDataService)));
        StrongReferenceMessenger.Default.Send(selectCollection);
    }

    private async void PendingListview_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var selectCollection = e.Item as PendingCollectionRequest;
        await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new PendingCollectionDetailsPage(_mapService, _dialogService, _pushDataService)));
        StrongReferenceMessenger.Default.Send(selectCollection);
    }
}