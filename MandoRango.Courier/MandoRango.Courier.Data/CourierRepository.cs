using MongoDB.Driver;
using MandoRango.Courier.Models;
using MandoRango.Courier.Models.Bson;

namespace MandoRango.Courier.Data
{
    public class CourierRepository
    {
        private readonly IMongoCollection<CourierBson> _courierCollection;

        public CourierRepository(string connectionString, string dbName)
        {
            var mongoClient = new MongoClient(connectionString);

            var database = mongoClient.GetDatabase(dbName);

            _courierCollection = database.GetCollection<CourierBson>("Courier");
        }

        public async Task UpdateCourierPositionAsync(CourierPosition position)
        {
            var filter = Builders<CourierBson>.Filter
                .Eq(pos => pos.CourierId, position.CourierId);

            var update = Builders<CourierBson>.Update
                .Set(pos => pos.Position.Latitude, position.Latitude)
                .Set(pos => pos.Position.Longitude, position.Longitude)
                .Set(pos => pos.LastPositionDate, DateTime.Now)
                ;

            await _courierCollection.UpdateOneAsync(filter, update, new UpdateOptions()
            {
                IsUpsert = true
            });
        }

        public async Task<List<MandoRango.Courier.Models.Courier>> GetAvailableCouriersByCity(int cityId)
        {

            var filter = Builders<CourierBson>.Filter
                .ElemMatch(pos => pos.ActuationCities, cId => cId == cityId);

            var search = await _courierCollection.FindAsync(filter);
            var result = await search.ToListAsync();

            return result.Select(x => (MandoRango.Courier.Models.Courier)x).ToList();

        }
    }
}
