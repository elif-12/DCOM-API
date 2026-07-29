namespace DCOM_API.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}