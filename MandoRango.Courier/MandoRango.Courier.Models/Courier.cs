
namespace MandoRango.Courier.Models
{
    public class Courier
    {
        public Courier()
        {
            Position = new Position();
            ActuationCities = new List<int>();
        }
        public Guid CourierId { get; set; }
        public IEnumerable<int> ActuationCities { get; set; }
        public bool IsBusy { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastPositionDate { get; set; }
        public Position Position { get; set; }
    }
}
