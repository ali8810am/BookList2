using AutoMapper;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class CustomerService: BaseHttpService, ICustomerService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IClient _client;

        public CustomerService(ILocalStorageService localStorageService, IClient client, ILocalStorageService storageService, IMapper mapper) : base(localStorageService, client)
        {
            _storageService = storageService;
            _mapper = mapper;
            _client = client;
        }
        public async Task<List<CustomerVm>> GetCustomers()
        {
            var customers = await _client.CustomerAllAsync();
            return _mapper.Map<List<CustomerVm>>(customers);
        }

        public async Task<CustomerVm> GetCustomer(int id)
        {
            var book = await _client.CustomerGETAsync(null,id);
            return _mapper.Map<CustomerVm>(book);
        }
        public async Task<CustomerVm> GetCustomerByUserId(string userId)
        {
            var book = await _client.CustomerGETAsync(userId,0);
            return _mapper.Map<CustomerVm>(book);
        }

        public async Task<Response<int>> CreateCustomer(CreateCustomerVm customer)
        {
            var customerDto = _mapper.Map<CreateCustomerDto>(customer);
            AddBearerToken();
            await _client.CustomerPOSTAsync(customerDto);
            return new Response<int>
            {
            };
        }

        public async Task<Response<int>> UpdateCustomer(int id, CreateCustomerVm customer)
        {
            var customerDto = _mapper.Map<CreateCustomerDto>(customer);
            AddBearerToken();
            await _client.CustomerPUTAsync(id, customerDto);
            return new Response<int> { };
        }

        public async Task<Response<int>> DeleteCustomer(int id)
        {
            await _client.CustomerDELETEAsync(id);
            AddBearerToken();
            return new Response<int> { };
        }
    }
}
