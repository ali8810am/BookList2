using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<ActionResult<IList<CustomerDto>>> Get()
        {
            var customers= await _unitOfWork.Customers.GetAll();

            return Ok(_mapper.Map<IList<CustomerDto>>(customers));
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            var customer =await _unitOfWork.Customers.Get(c=>c.CustomerId==id);
            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] CreateCustomerDto customerDto)
        {
            var customer=_mapper.Map<Customer>(customerDto);
            _unitOfWork.Customers.Add(customer);
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CreateCustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            _unitOfWork.Customers.Update(customer);
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var customer = await _unitOfWork.Customers.Get(c => c.CustomerId == id);
            if (customer == null)
                throw new NotFoundException("Customer not found", customer.CustomerId);
            await _unitOfWork.Customers.Delete(id);

        }
    }
}
