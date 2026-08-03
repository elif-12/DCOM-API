using DCOM_API.Application.Interfaces;
using DCOM_API.Data;

namespace DCOM_API.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}