using AutoMapper;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class EmployeeService:IEmployeeService
    {
        private readonly IClient _client;
        private readonly IMapper _mapper;

        public EmployeeService(IClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<List<EmployeeVm>> GetEmployees()
        {
            var employees= await _client.EmployeeAllAsync();
            return _mapper.Map<List<EmployeeVm>>(employees);
        }

        public Task<EmployeeVm> GetEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeVm> GetEmployeeByUserId(string userId)
        {
            var employee = await _client.EmployeeGETAsync(userId,0);
            return _mapper.Map<EmployeeVm>(employee);
        }

        public Task CreateEmployee(CreateEmployeeVm customer)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEmployee(int id, CreateEmployeeVm customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }
    }
}
