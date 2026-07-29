namespace DCOM_API.Entities
{
    public class DicomFile : BaseEntity, IOwnedEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }

        public Guid UserId { get; set; }

        public Guid SeriesId { get; set; }
        public Series Series { get; set; } = null!;
    }
}