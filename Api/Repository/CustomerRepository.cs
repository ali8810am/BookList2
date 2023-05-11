using System.Linq.Expressions;
using Api.Data;
using Api.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class CustomerRepository:GenericRepository<Customer>,ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
