using System.Linq.Expressions;
<<<<<<< HEAD:BookList.Domain/IRepository/IGenericRepository.cs
=======
using Api.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using X.PagedList;
>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00:Api/IRepository/IGenericRepository.cs


namespace BookList.Domain.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        void Update(T entity);
        Task Delete(int id);
        Task<bool> Exist(Expression<Func<T, bool>> expression = null);
        Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null);
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>>? expression = null,
            List<string> includes = null);

<<<<<<< HEAD:BookList.Domain/IRepository/IGenericRepository.cs
        //Task<IPagedList<T>> GetAll(
        //    RequestParameters parameters,
        //    Expression<Func<T, bool>>? expression = null,
        //    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //    bool OrderByDescending = false,
        //    List<string> includes = null);
=======
        Task<IPagedList<T>> GetAll(
            RequestParameters parameters,
            Expression<Func<T, bool>>? expression = null,
            List<string> includes = null
        );
        IQueryable<T> GetFiltered(
            List<string> includes = null
        );

>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00:Api/IRepository/IGenericRepository.cs
    }
}
