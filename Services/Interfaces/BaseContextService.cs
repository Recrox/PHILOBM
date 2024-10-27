using Microsoft.EntityFrameworkCore;
using PHILOBM.Database;
using PHILOBM.Models.Base;
using PHILOBM.Services.Interfaces;

namespace PHILOBM.Services;

public abstract class BaseContextService<T> : IBaseContextService<T> where T : BaseEntity
{
    protected readonly PhiloBMContext _context;

    protected BaseContextService(PhiloBMContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FirstOrDefaultAsync(item => item.Id == id);

    public async Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
