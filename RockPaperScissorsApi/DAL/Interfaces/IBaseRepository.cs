namespace RockPaperScissorsApi.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task CreateAsync(T entity);

        IQueryable<T> GetAll();

        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);
    }
}
