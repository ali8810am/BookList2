using View.Model;
using View.Services.Base;

namespace View.Contracts
{
    public interface ICustomerService
    {
        Task<List<CustomerVm>> GetCustomers();
        Task<CustomerVm> GetCustomer(int id);
        Task<CustomerVm> GetCustomerByUserId(string userId);
        Task<Response<int>> CreateCustomer(CreateCustomerVm customer);
        Task<Response<int>> UpdateCustomer(int id, CreateCustomerVm customer);
        Task<Response<int>> DeleteCustomer(int id);
    }
}
