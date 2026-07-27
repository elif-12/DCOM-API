namespace DCOM_API.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string PatientId { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;

        public ICollection<Study> Studies { get; set; } = new List<Study>();
    }
}

