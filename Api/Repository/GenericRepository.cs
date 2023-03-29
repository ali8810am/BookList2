﻿using System.Linq.Expressions;
using Api.Data;
using Api.IRepository;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Api.Repository
{
    public class GenericRepository<T>:IGenericRepository<T> where T :class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }


        public async Task Add(T entity)
        {
           await _context.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            var entity =await _db.FindAsync(id);
             _context.Remove(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes!=null)
            {
                query = includes.Aggregate(query, (current, parameter) => current.Include(parameter));
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool OrderByDescending = false, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, parameter) => current.Include(parameter));
            }
            if (orderBy != null)
            {
                if (OrderByDescending = false)
                {
                    query = query.OrderBy(expression);
                }
                else
                {
                    query = query.OrderByDescending(expression);

                }
            }

            if (expression == null)
            {
                return await query.AsNoTracking().ToListAsync();
            }
            else
            {
                return await query.AsNoTracking().Where(expression).ToListAsync();

            }
        }

        public async Task<IPagedList<T>> GetAll(RequestParameters parameters, Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool OrderByDescending = false, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, parameter) => current.Include(parameter));
            }
            if (orderBy != null)
            {
                if (OrderByDescending = false)
                {
                    query = query.OrderBy(expression);
                }
                else
                {
                    query = query.OrderByDescending(expression);

                }
            }

            if (expression == null)
            {
                return await query.AsNoTracking().ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
            }
            else
            {
                return await query.AsNoTracking().Where(expression).ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            }
        }
    }
}
