using AutoMapper;
using System.ComponentModel.DataAnnotations;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class BorrowRequestService : IBorrowRequestService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IClient _client;

        public BorrowRequestService(ILocalStorageService storageService, IMapper mapper, IClient client)
        {
            _storageService = storageService;
            _mapper = mapper;
            _client = client;
        }
        public async Task<List<BorrowRequestVm>> GetBorrowRequests()
        {
            var borrowRequests = await _client.GetAllRequestsAsync( null, null, new List<string>{ "Book", "Customer" });
            return _mapper.Map<List<BorrowRequestVm>>(borrowRequests);
        }

        public async Task<BorrowRequestVm> GetBorrowRequest(int id,List<string>? include)
        {
            var borrowRequest = await _client.BorrowRequestGETAsync(id,include);
            return _mapper.Map<BorrowRequestVm>(borrowRequest);
        }
        public async Task<BorrowRequestVm> GetBorrowRequest(int id)
        {
            List<string> include = new List<string>();
            var borrowRequest = await _client.BorrowRequestGETAsync(id,include);
            return _mapper.Map<BorrowRequestVm>(borrowRequest);
        }

        public async Task<Response<int>> CreateBorrowRequest(CreateBorrowRequestVm borrowRequest)
        {
            var borrowRequestDto = _mapper.Map<CreateBorrowRequestDto>(borrowRequest);
           var apiResponse= await _client.BorrowRequestPOSTAsync(borrowRequestDto);
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

        public async Task<Response<int>> UpdateBorrowRequest(BorrowRequestVm borrowRequest)
        {
            var borrowRequestDto = _mapper.Map<CreateBorrowRequestDto>(borrowRequest);
            var apiResponse= await _client.BorrowRequestPUTAsync(borrowRequest.RequestId, borrowRequestDto);
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

        public async Task DeleteBorrowRequest(int id)
        {
            await _client.BorrowRequestDELETEAsync(id);
        }
    }
}
