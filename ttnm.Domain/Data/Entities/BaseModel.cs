using SQLite;
using System.Text.Json.Serialization;

namespace ttnm.Domain.Data.Entities
{
    public class BaseModel
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int ID { get; set; }
    }
}
