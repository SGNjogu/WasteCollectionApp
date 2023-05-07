using ttnm.Views.CollectionPickup;
using ttnm.Views.CollectorHistory;
using ttnm.Views.Dashboard;
using ttnm.Views.FAQs;
using ttnm.Views.Login;
using ttnm.Views.NewTransaction;
using ttnm.Views.Settings;

namespace ttnm;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterPageRoutes();
    }

    private void RegisterPageRoutes()
    {
        // TabBar Pages
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(FAQsPage), typeof(FAQsPage));

        // Other pages
        Routing.RegisterRoute(nameof(CollectionPickupPage), typeof(CollectionPickupPage));
        Routing.RegisterRoute(nameof(PickupSchedulePage), typeof(PickupSchedulePage));
        Routing.RegisterRoute(nameof(CollectionHistoryPage), typeof(CollectionHistoryPage));
        Routing.RegisterRoute(nameof(SupportPage), typeof(SupportPage));

        Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        Routing.RegisterRoute(nameof(PendingCollectionPage), typeof(PendingCollectionPage));
        Routing.RegisterRoute(nameof(PendingCollectionDetailsPage), typeof(PendingCollectionDetailsPage));
        Routing.RegisterRoute(nameof(AcceptedCollectionPage), typeof(AcceptedCollectionPage));
        Routing.RegisterRoute(nameof(AcceptedCollectionDetailsPage), typeof(AcceptedCollectionDetailsPage));
        Routing.RegisterRoute(nameof(CollectedCollectionDetailsPage), typeof(CollectedCollectionDetailsPage));
        Routing.RegisterRoute(nameof(RegisterCollectorPage), typeof(RegisterCollectorPage));
        Routing.RegisterRoute(nameof(VerifyCollectorPage), typeof(VerifyCollectorPage));
        Routing.RegisterRoute(nameof(NewTransactionPage), typeof(NewTransactionPage));
        Routing.RegisterRoute(nameof(ConfirmDetailsPage), typeof(ConfirmDetailsPage));
        Routing.RegisterRoute(nameof(EnterPaymentDetailsPage), typeof(EnterPaymentDetailsPage));
        Routing.RegisterRoute(nameof(ConfirmPaymentPage), typeof(ConfirmPaymentPage));
        Routing.RegisterRoute(nameof(NewTransactionSuccessPage), typeof(NewTransactionSuccessPage));
        Routing.RegisterRoute(nameof(CollectionDeliveryPage), typeof(CollectionDeliveryPage));
    }
}
