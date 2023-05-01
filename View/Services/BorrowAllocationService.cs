using AutoMapper;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class BorrowAllocationService : BaseHttpService, IBorrowAllocationService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IClient _client;

        public BorrowAllocationService(ILocalStorageService localStorageService, IClient client, ILocalStorageService storageService, IMapper mapper) : base(localStorageService, client)
        {
            _storageService = storageService;
            _mapper = mapper;
            _client = client;
        }
        public async Task<List<BorrowAllocationVm>> GetBorrowAllocations()
        {
            var borrowAllocations = await _client.GetAllAllocationsAsync(null, null, new List<string>{"Book","Customer","Employee"});
            return _mapper.Map<List<BorrowAllocationVm>>(borrowAllocations);
        }

        public async Task<BorrowAllocationVm> GetBorrowAllocation(int id, List<string>? include)
        {
            include ??= new List<string>();
            var borrowAllocation = await _client.BorrowAllocationGETAsync(id,include);
            return _mapper.Map<BorrowAllocationVm>(borrowAllocation);
        }



        public async Task<Response<int>> CreateBorrowAllocation(CreateBorrowAllocationVm borrowAllocation)
        {
            var borrowAllocationDto = _mapper.Map<CreateBorrowAllocationDto>(borrowAllocation);
            AddBearerToken();
            await _client.BorrowAllocationPOSTAsync(borrowAllocationDto);
            return new Response<int>
            {
            };
        }

        public async Task<Response<int>> UpdateBorrowAllocation(int id,CreateBorrowAllocationVm borrowAllocation)
        {
            var borrowAllocationDto = _mapper.Map<CreateBorrowAllocationDto>(borrowAllocation);
            AddBearerToken();
            await _client.BorrowAllocationPUTAsync(id, borrowAllocationDto);
            return new Response<int> {Success = true};
        }

        public async Task<Response<int>> DeleteBorrowAllocation(int id)
        {
            await _client.BorrowAllocationDELETEAsync(id);
            AddBearerToken();
            return new Response<int> { };
        }
    }
}

