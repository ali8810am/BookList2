using View.Model;

namespace View.Contracts
{
    public interface IEmployeeService
    {
        Task<List<EmployeeVm>> GetEmployees();
        Task<EmployeeVm> GetEmployee(int id);
        Task<EmployeeVm> GetEmployeeByUserId(string userId);
        Task CreateEmployee(CreateEmployeeVm customer);
        Task UpdateEmployee(int id, CreateEmployeeVm customer);
        Task DeleteEmployee(int id);
    }
}
