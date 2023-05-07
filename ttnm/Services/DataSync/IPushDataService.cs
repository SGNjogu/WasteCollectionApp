namespace ttnm.Services.DataSync
{
    public interface IPushDataService
    {
        Task<bool> SyncAcceptedRequests(int collectitonId);

        Task<bool> SyncCollectedRequests(int collectitonId);

        Task<bool> SyncCanceledRequests(int collectitonId);
    }
}