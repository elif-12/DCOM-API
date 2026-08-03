namespace DCOM_API.Entities
{
    public enum UserRole
    {
        SuperAdmin = 0,
        Doctor = 1
    }

    public class User : BaseEntity
    {
       
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Doctor;
        public bool IsActive { get; set; } = true;
    }
}
