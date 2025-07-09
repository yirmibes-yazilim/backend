using backend.Application.DTOs.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IService<T>
        where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
    }
}
