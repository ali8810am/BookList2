using View.Model;
using View.Services.Base;

namespace View.Contracts
{
    public interface IEmployeeService
    {
        Task<List<EmployeeVm>> GetEmployees();
        Task<EmployeeVm> GetEmployee(int id);
        Task<EmployeeVm> GetEmployeeByUserId(string userId);
        Task<Response<int>> CreateEmployee(CreateEmployeeVm customer);
        Task<Response<int>> UpdateEmployee(int id, CreateEmployeeVm customer);
        Task DeleteEmployee(int id);
    }
}
