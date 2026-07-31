namespace DCOM_API.Application.Interfaces
{
    public interface ITokenStore
    {
        Task<bool> ExistsAsync(string tokenId);        // kayıt var mı (idle aşılmamış mı)
        Task SetAsync(string tokenId, TimeSpan ttl);   // kayıt oluştur/yenile (sliding)
        Task RemoveAsync(string tokenId);
    }
}
