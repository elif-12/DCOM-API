namespace DCOM_API.Entities
{
    public class Patient :BaseEntity, IOwnedEntity
    {
      
        public string PatientId { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public ICollection<Study> Studies { get; set; } = new List<Study>();
    }
}