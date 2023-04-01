using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookList.Infrastructure.Data;

namespace BookList.Domain.IRepository
{
    public interface IBookRepository:IGenericRepository<Book>
    {

    }
}
