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
            var customers = await _unitOfWork.Customers.GetAll();

            return Ok(_mapper.Map<IList<CustomerDto>>(customers));
        }

        // GET api/<CustomerController>/5
        [HttpGet("{customerId}")]
        public async Task<ActionResult<CustomerDto>> Get(string? userId, int? customerId = 0)
        {
            var customer = new Customer();
            if (customerId != null && customerId != 0)
            {
                customer = await _unitOfWork.Customers.Get(c => c.Id == customerId, includes: new List<string> { "User" });
                if (customer == null)
                    return NotFound();

                return Ok(_mapper.Map<CustomerDto>(customer));
            }

            if (userId == null)
                return BadRequest();

            customer = await _unitOfWork.Customers.Get(c => c.UserId == userId, includes: new List<string> { "User" });
            if (customer == null)
                return NotFound();
            return Ok(_mapper.Map<CustomerDto>(customer));



        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task Post([FromBody] CreateCustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.DateMembered=DateTime.Now;
           await _unitOfWork.Customers.Add(customer);
           await _unitOfWork.Save();
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CreateCustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.Save();
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var customer = await _unitOfWork.Customers.Get(c => c.Id == id);
            if (customer == null)
                throw new NotFoundException("Customer not found", customer.Id);
            await _unitOfWork.Customers.Delete(id);
            await _unitOfWork.Save();

        }
    }
}
