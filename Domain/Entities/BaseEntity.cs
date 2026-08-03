namespace DCOM_API.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatorUserId { get; set; }

        /*Soft Delete Kısmı*/
        public bool IsDeleted { get; set; }
        public Guid? DeleterUserId
        {
            get; set;  // kayıt silinmediyse null olarak tutar. silindiyse silen kullanıcının Id'sini tutar.

        }
    }
}
