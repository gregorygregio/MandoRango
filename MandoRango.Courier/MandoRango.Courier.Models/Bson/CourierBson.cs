using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandoRango.Courier.Models.Bson
{
    public class CourierBson
    {
        public CourierBson()
        {
            Position = new Position();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public Guid CourierId { get; set; }
        public IEnumerable<int> ActuationCities { get; set; }
        public bool IsBusy { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastPositionDate { get; set; }
        public Position Position { get; set; }
    }
}
