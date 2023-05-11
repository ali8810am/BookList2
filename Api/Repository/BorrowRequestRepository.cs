using Api.Data;
using Api.IRepository;

namespace Api.Repository
{
    public class BorrowRequestRepository:GenericRepository<BorrowRequest>,IBorrowRequestRepository
    {
        public BorrowRequestRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
