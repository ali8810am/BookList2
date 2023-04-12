﻿using AutoMapper;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class BorrowAllocationService : IBorrowAllocationService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IClient _client;

        public BorrowAllocationService(ILocalStorageService storageService, IMapper mapper, IClient client)
        {
            _storageService = storageService;
            _mapper = mapper;
            _client = client;
        }
        public async Task<List<BorrowAllocationVm>> GetBorrowAllocations()
        {
            var borrowAllocations = await _client.BorrowAllocationAllAsync();
            return _mapper.Map<List<BorrowAllocationVm>>(borrowAllocations);
        }

        public async Task<BorrowAllocationVm> GetBorrowAllocation(int id)
        {
            var borrowAllocation = await _client.BorrowAllocationGETAsync(id);
            return _mapper.Map<BorrowAllocationVm>(borrowAllocation);
        }


        public async Task<Response<int>> CreateBorrowAllocation(CreateBorrowAllocationVm borrowAllocation)
        {
            var borrowAllocationDto = _mapper.Map<CreateBorrowAllocationDto>(borrowAllocation);
            await _client.BorrowAllocationPOSTAsync(borrowAllocationDto);
            return new Response<int>
            {
            };
        }

        public async Task<Response<int>> UpdateBorrowAllocation(BorrowAllocationVm borrowAllocation)
        {
            var borrowAllocationDto = _mapper.Map<CreateBorrowAllocationDto>(borrowAllocation);
            await _client.BorrowAllocationPUTAsync(borrowAllocation.AllocationId, borrowAllocationDto);
            return new Response<int> { };
        }

        public async Task<Response<int>> DeleteBorrowAllocation(int id)
        {
            await _client.BorrowAllocationDELETEAsync(id);
            return new Response<int> { };
        }
    }
}

