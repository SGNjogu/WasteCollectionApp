using CommunityToolkit.Mvvm.Messaging;
using ttnm.Domain.Data.DataService;
using ttnm.Infrastructure.Services.Collector;
using ttnm.Models;
using ttnm.Services.Dialogs;
using ttnm.Services.Maps;
using ttnm.ViewModels;
using ttnm.Views.CollectionPickup;

namespace ttnm.Views.Dashboard;

public partial class CollectionHistoryPage : ContentPage
{
    private readonly IMapService _mapService;
    private readonly IDialogService _dialogService;
    private readonly IDataService _dataService;
    private readonly ICollectionRequestService _collectionRequestService;

    public CollectionHistoryPage(CollectionHistoryViewModel viewModel, IMapService mapService, IDialogService dialogService, IDataService dataService, ICollectionRequestService collectionRequestService)
    {
        InitializeComponent();

        _mapService = mapService;
        _dialogService = dialogService;
        _dataService = dataService;
        _collectionRequestService = collectionRequestService;

        BindingContext = viewModel;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var selectCollection = e.Item as CollectedCollectionRequest;
        await Application.Current.MainPage.Navigation.PushAsync(new CollectedCollectionDetailsPage(_mapService, _dialogService, _dataService, _collectionRequestService));
        StrongReferenceMessenger.Default.Send(selectCollection);
    }
}