namespace HolookorBackend.Core.Domain.Entities
{
    public class Location : AuditableEntities
    {
        public string Country { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string? State { get; private set; }

        private Location() { }

        public Location(string country, string city, string? state = null)
        {
            Country = country;
            City = city;
            State = state;
        }
    }

}
