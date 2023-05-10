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
            AddBearerToken();
            var borrowAllocations = await _client.GetAllAllocationsAsync(null, null, new List<string> { "Book", "Customer", "Employee" });
            return _mapper.Map<List<BorrowAllocationVm>>(borrowAllocations);
        }

        public async Task<List<BorrowAllocationVm>> GetBorrowAllocations(bool isReturned)
        {
            AddBearerToken();
            var borrowAllocations = await _client.GetAllFilteredAllocationsAsync(null, null, null, null, null, null, isReturned, null, null, null, null, null, new List<string> { "Book", "Customer", "Employee" });
            return _mapper.Map<List<BorrowAllocationVm>>(borrowAllocations);
        }

        public async Task<BorrowAllocationVm> GetBorrowAllocation(int id, List<string>? include)
        {
            include ??= new List<string>();
            var dto = new GetBorrowAllocationDto()
            {
                Id = id,
                Includes = include
            };
            AddBearerToken();
            var borrowAllocation = await _client.BorrowAllocationGETAsync(dto);
            return _mapper.Map<BorrowAllocationVm>(borrowAllocation);
        }



        public async Task<Response<int>> CreateBorrowAllocation(List<CreateBorrowAllocationVm> borrowAllocation)
        {
            var borrowAllocationDto = _mapper.Map<List<CreateBorrowAllocationDto>>(borrowAllocation);
            AddBearerToken();
            var apiResponse = await _client.BorrowAllocationPOSTAsync(borrowAllocationDto);
            var response = new Response<int>();
            if (apiResponse.Success)
                response.Success = true;
            else
            {
                foreach (var error in apiResponse.InvalidRequests)
                {
                    response.ValidationErrors +=error.RequestId.ToString() + error.Errors + Environment.NewLine;
                }
            }
            return response;
        }

        public async Task<Response<int>> CreateBorrowAllocation(CreateBorrowAllocationVm borrowAllocation)
        {
            var list = new List<CreateBorrowAllocationVm>();
            list.Add(borrowAllocation);
            var borrowAllocationDto = _mapper.Map<List<CreateBorrowAllocationDto>>(list);
            AddBearerToken();
            var apiResponse = await _client.BorrowAllocationPOSTAsync(borrowAllocationDto);
            var response = new Response<int>();
            if (apiResponse.Success)
                response.Success = true;
            else
            {
                foreach (var error in apiResponse.InvalidRequests)
                {
                    response.ValidationErrors += error.RequestId.ToString() + error.Errors + Environment.NewLine;
                }
            }
            return response;
        }

        public async Task<Response<int>> UpdateBorrowAllocation(int id, CreateBorrowAllocationVm borrowAllocation)
        {
            var borrowAllocationDto = _mapper.Map<CreateBorrowAllocationDto>(borrowAllocation);
            AddBearerToken();
            var apiResponse = await _client.BorrowAllocationPUTAsync(id, borrowAllocationDto);
            var response = new Response<int>();
            if (apiResponse.Success)
                response.Success = true;
            else
            {
                foreach (var error in apiResponse.Errors)
                {
                    response.ValidationErrors += error + Environment.NewLine;
                }
            }
            return response;
        }

        public async Task DeleteBorrowAllocation(int id)
        {
            AddBearerToken();
            await _client.BorrowAllocationDELETEAsync(id);
        }
    }
}

