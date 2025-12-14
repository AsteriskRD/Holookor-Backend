namespace HolookorBackend.Core.Domain
{
    public abstract class AuditableEntities
    {

        public string Id { get; protected set; } = Guid.NewGuid().ToString();
        public DateTime DateCreated { get; protected set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; protected set; }



    }
}
