namespace PHILOBM.Services.Interfaces;

public interface IBaseContextService<T>
{
    Task Add(T entity);
    Task<List<T>> GetAll();
    Task Update(T entity);
    Task Delete(int id);
}