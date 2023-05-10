using AutoMapper;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class EmployeeService : BaseHttpService, IEmployeeService
    {
        private readonly IClient _client;
        private readonly IMapper _mapper;
        private readonly ILocalStorageService _storageService;

        public EmployeeService(ILocalStorageService localStorageService, IClient client, IMapper mapper, ILocalStorageService storageService) : base(localStorageService, client)
        {
            _client = client;
            _mapper = mapper;
            _storageService = storageService;
        }
        public async Task<List<EmployeeVm>> GetEmployees()
        {
            AddBearerToken();
            var employees = await _client.EmployeeAllAsync();
            return _mapper.Map<List<EmployeeVm>>(employees);
        }

        public async Task<EmployeeVm> GetEmployee(int id)
        {
            AddBearerToken();
            var employee = await _client.EmployeeGETAsync("", id);
            return _mapper.Map<EmployeeVm>(employee);
        }

        public async Task<EmployeeVm> GetEmployeeByUserId(string userId)
        {
            AddBearerToken();
            var employee = await _client.EmployeeGETAsync(userId, 0);
            return _mapper.Map<EmployeeVm>(employee);
        }

        public async Task<Response<int>> CreateEmployee(CreateEmployeeVm customer)
        {
            AddBearerToken();
            var apiResponse = await _client.EmployeePOSTAsync(_mapper.Map<CreateEmployeeDto>(customer));
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

        public async Task<Response<int>> UpdateEmployee(int id, CreateEmployeeVm customer)
        {
            AddBearerToken();
            var apiResponse = await _client.EmployeePUTAsync(id, _mapper.Map<CreateEmployeeDto>(customer));
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

        public async Task DeleteEmployee(int id)
        {
            AddBearerToken();
            await _client.EmployeeDELETEAsync(id);
        }
    }
}
