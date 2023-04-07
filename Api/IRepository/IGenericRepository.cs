﻿using System.Linq.Expressions;
using Api.Models;
using X.PagedList;

namespace Api.IRepository
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
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool OrderByDescending = false,
            List<string> includes = null);

        Task<IPagedList<T>> GetAll(
            RequestParameters parameters,
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool OrderByDescending = false,
            List<string> includes = null);
    }
}
