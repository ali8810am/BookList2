using System.Linq.Expressions;
using BookList.Domain.IRepository;
using BookList.Persistance.Data;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD:BookList.Persistance/Repository/GenericRepository.cs
=======
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using X.PagedList;
>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00:Api/Repository/GenericRepository.cs


namespace BookList.Persistance.Repository
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

        public async Task<bool> Exist(Expression<Func<T, bool>> expression = null)
        {
            return await _db.AnyAsync(expression);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes!=null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression = null,
            List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property) ;
                }
            }
            if (expression == null)
            {
                return await query.AsNoTracking().ToListAsync();
            }
            else
            {

                return await query.AsNoTracking().Where(expression).Where(expression).ToListAsync();

            }
        }

<<<<<<< HEAD:BookList.Persistance/Repository/GenericRepository.cs
        //public async Task<IPagedList<T>> GetAll(RequestParameters parameters, Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //    bool OrderByDescending = false, List<string> includes = null)
        //{
        //    IQueryable<T> query = _db;
        //    if (includes != null)
        //    {
        //        query = includes.Aggregate(query, (current, parameter) => current.Include(parameter));
        //    }
        //    if (orderBy != null)
        //    {
        //        if (OrderByDescending = false)
        //        {
        //            query = query.OrderBy(expression);
        //        }
        //        else
        //        {
        //            query = query.OrderByDescending(expression);

        //        }
        //    }
=======
        public async Task<IPagedList<T>> GetAll(RequestParameters parameters,
            Expression<Func<T, bool>>? expression = null,
            List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }
>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00:Api/Repository/GenericRepository.cs

        //    if (expression == null)
        //    {
        //        return await query.AsNoTracking().ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
        //    }
        //    else
        //    {
        //        return await query.AsNoTracking().Where(expression).ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

<<<<<<< HEAD:BookList.Persistance/Repository/GenericRepository.cs
        //    }
        //}
=======
            }
        }

        public  IQueryable<T> GetFiltered( List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }
            return  query.AsNoTracking();

            
        }
>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00:Api/Repository/GenericRepository.cs
    }
}
