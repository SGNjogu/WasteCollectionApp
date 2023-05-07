namespace ttnm.Domain.Data.DataService
{
    public interface IDataService
    {
        Task DeleteAllItemsAsync<T>();
        Task<T> DeleteItemAsync<T>(object obj);
        Task<List<T>?> GetAllItemsAsync<T>();
        Task<T?> GetItemById<T>(int id);
        Task<List<T>> InsertAllItemsAsync<T>(List<T> objects);
        Task<T> InsertItemAsync<T>(object obj);
        Task<T> UpdateItemAsync<T>(object obj);
    }
}