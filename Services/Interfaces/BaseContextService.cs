using Microsoft.EntityFrameworkCore;
using PHILOBM.Database;
using PHILOBM.Services.Interfaces;

namespace PHILOBM.Services;

public abstract class BaseContextService<T> : IBaseContextService<T> where T : class
{
    protected readonly PhiloBMContext _context;

    protected BaseContextService(PhiloBMContext context)
    {
        _context = context;
    }

    public async Task Add(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
