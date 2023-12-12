using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MandoRango.Courier.Models.Bson
{
    public class CourierBson : Courier
    {
        public CourierBson(): base()
        {
            
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }        
    }
}
