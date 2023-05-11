using Api.Data;
using Api.IRepository;

namespace Api.Repository
{
    public class BorrowAllocationRepository:GenericRepository<BorrowAllocation>,IBorrowAllocationRepository
    {
        public BorrowAllocationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
