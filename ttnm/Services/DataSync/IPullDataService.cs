namespace ttnm.Services.DataSync
{
    public interface IPullDataService
    {
        Task UpdateCollectedRequests();
        Task UpdateAggregatorHistory();

        Task UpdateAcceptedCollections();

        Task UpdatePendingCollections();
        
        void CancelDataSync();
        Task BeginDataSync();
        Task UpdateCollectorsList();
    }
}