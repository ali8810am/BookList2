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
            var borrowRequests = await _client.BorrowRequestAllAsync(null, null, null, null, null, null, null, null, null, null, new List<string>{ "Book" }, null);
            return _mapper.Map<List<BorrowRequestVm>>(borrowRequests);
        }

        public async Task<BorrowRequestVm> GetBorrowRequest(int id)
        {
            var borrowRequest = await _client.BorrowRequestGETAsync(id);
            return _mapper.Map<BorrowRequestVm>(borrowRequest);
        }

        public async Task<Response<int>> CreateBorrowRequest(CreateBorrowRequestVm borrowRequest)
        {
            var borrowRequestDto = _mapper.Map<CreateBorrowRequestDto>(borrowRequest);
            await _client.BorrowRequestPOSTAsync(borrowRequestDto);
            return new Response<int>
            {
            };
        }

        public async Task<Response<int>> UpdateBorrowRequest(BorrowRequestVm borrowRequest)
        {
            var borrowRequestDto = _mapper.Map<CreateBorrowRequestDto>(borrowRequest);
            await _client.BorrowRequestPUTAsync(borrowRequest.RequestId, borrowRequestDto);
            return new Response<int> { };
        }

        public async Task<Response<int>> DeleteBorrowRequest(int id)
        {
            await _client.BorrowRequestDELETEAsync(id);
            return new Response<int> { };
        }
    }
}
