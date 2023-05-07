using SQLite;
using System.Diagnostics;
using ttnm.Domain.Data.Entities;

namespace ttnm.Domain.Data.DataService
{
    public class DataService : IDataService
    {
        private SQLiteAsyncConnection _database => LazyInitializer.Value;

        private bool _initialized = false;

        private IEnumerable<TableMapping> _tableMappings = default!;

        /// <summary>
        /// Tries to initialize database lazily
        /// </summary>
        readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        /// <summary>
        /// Method to Initialize Database
        /// </summary>
        private async Task InitializeAsync()
        {
            try
            {
                // Initialization
                if (!_initialized)
                {
                    await _database.CreateTableAsync<ToDo>(CreateFlags.None);
                    await _database.CreateTableAsync<CollectedRequests>(CreateFlags.None);
                    await _database.CreateTableAsync<AggregatorHistory>(CreateFlags.None);
                    await _database.CreateTableAsync<AcceptedRequests>(CreateFlags.None);
                    await _database.CreateTableAsync<PendingRequests>(CreateFlags.None);
                    await _database.CreateTableAsync<Aggregator>(CreateFlags.None);

                    _tableMappings = _database.TableMappings;

                    _initialized = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method to add an item to database
        /// </summary>
        /// <returns>Created Item</returns>
        public async Task<T> InsertItemAsync<T>(object obj)
        {
            await InitializeAsync();

            var item = (T)Convert.ChangeType(obj, typeof(T));

            await _database.InsertAsync(item);

            return item;
        }

        /// <summary>
        /// Method to add an item to database
        /// </summary>
        /// <returns>Created Item</returns>
        public async Task<List<T>> InsertAllItemsAsync<T>(List<T> objects)
        {
            await InitializeAsync();

            var items = (List<T>)Convert.ChangeType(objects, typeof(List<T>));

            await _database.InsertAllAsync(items);

            return items;
        }

        /// <summary>
        /// Method to get all items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>All items in the table</returns>
        public async Task<List<T>?> GetAllItemsAsync<T>()
        {
            await InitializeAsync();

            var tableName = typeof(T).Name;

            var tableMapping = _tableMappings.FirstOrDefault(s => s.TableName == tableName);

            if (tableMapping == null)
                throw new Exception($"The table {tableName} does not exist");

            var items = await _database.QueryAsync(tableMapping, $"SELECT * FROM {tableName}");

            if (items == null)
                return default;

            return items.OfType<T>().ToList();
        }

        /// <summary>
        /// Method to get an item by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns>Returns the item if exists</returns>
        public async Task<T?> GetItemById<T>(int id)
        {
            await InitializeAsync();

            var tableName = typeof(T).Name;

            var tableMapping = _tableMappings.FirstOrDefault(s => s.TableName == tableName);

            if (tableMapping == null)
                throw new Exception($"The table {tableName} does not exist");

            var item = await _database.QueryAsync(tableMapping, $"SELECT * FROM {tableName} WHERE ID = {id}");

            if (item == null)
                return default;

            return item.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Method to update an Item in Database
        /// </summary>
        /// <returns>Updated Item</returns>
        public async Task<T> UpdateItemAsync<T>(object obj)
        {
            await InitializeAsync();

            var item = (T)Convert.ChangeType(obj, typeof(T));

            await _database.UpdateAsync(item);

            return item;
        }

        /// <summary>
        /// Method to delete an Item in Database
        /// </summary>
        /// <returns>Deleted Item</returns>
        public async Task<T> DeleteItemAsync<T>(object obj)
        {
            await InitializeAsync();

            var item = (T)Convert.ChangeType(obj, typeof(T));

            await _database.DeleteAsync(item);

            return item;
        }

        /// <summary>
        /// Deletes All Items
        /// </summary>
        public async Task DeleteAllItemsAsync<T>()
        {
            await InitializeAsync();

            await _database.DeleteAllAsync<T>();
        }
    }
}
