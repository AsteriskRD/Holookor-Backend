namespace HolookorBackend.Core.Domain.Entities
{
    public class Parent : AuditableEntities
    {
        public virtual ICollection<Student> Children { get; set; } = new List<Student>();

        public string UserProfileId { get; set; } = default!;
        public virtual UserProfile Profile { get; set; } = default!;


    }
}
