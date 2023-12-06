using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandoRango.Courier.Data
{
    public class CourierPositionRepository
    {
        private readonly IMongoCollection<CourierPositionBson> _courierPositionCollection;
        //private readonly IMongoDatabase _database;
        public CourierPositionRepository(string connectionString, string dbName)
        {
            var mongoClient = new MongoClient(connectionString);

            var database = mongoClient.GetDatabase(dbName);

            _courierPositionCollection = database.GetCollection<CourierPositionBson>("CourierPosition");
        }

        public async Task CreateAsync(CourierPositionBson position)
        {
            var filter = Builders<CourierPositionBson>.Filter
                .Eq(pos => pos.CourierId, position.CourierId);

            var update = Builders<CourierPositionBson>.Update
                .Set(pos => pos.Latitude, position.Latitude)
                .Set(pos => pos.Longitude, position.Longitude)
                .Set(pos => pos.PositionTimeStamp, DateTime.Now)
                ;

            await _courierPositionCollection.UpdateOneAsync(filter, update, new UpdateOptions()
            {
                IsUpsert=true
            });
        }
    }

    public class CourierPositionBson
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public Guid CourierId {get;set;}
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime PositionTimeStamp { get; set; }
    }
}
