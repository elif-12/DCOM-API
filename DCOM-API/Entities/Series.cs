namespace DCOM_API.Entities
{
    public class Series
    {
        public Guid Id { get; set; }
        public string SeriesInstanceUid { get; set; } = string.Empty;
        public string? Modality { get; set; }

        public Guid StudyId { get; set; }
        public Study Study { get; set; } = null!;

        public ICollection<DicomFile> DicomFiles { get; set; } = new List<DicomFile>();
    }
}
