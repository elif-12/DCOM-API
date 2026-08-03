namespace DCOM_API.Entities
{
    public class Study :BaseEntity, IOwnedEntity
    {
       
        public string StudyInstanceUid { get; set; } = string.Empty;
        public DateTime? StudyDate { get; set; }
        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public ICollection<Series> Series { get; set; } = new List<Series>();
    }
}