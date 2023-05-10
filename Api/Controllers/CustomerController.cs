using Api.ConstantParameters;
using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Api.Services;
using Marvin.Cache.Headers;
using Api.Models.Validators.UserValidator;
using Api.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = $"{UserRoles.Employee},{UserRoles.Admin}")]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public CustomerController(IUnitOfWork unitOfWork, IMapper mapper, IAuthManager authManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authManager = authManager;
        }

        // GET: api/<CustomerController>
        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<IList<CustomerDto>>> Get()
        {
            var customers = await _unitOfWork.Customers.GetAll();

            return Ok(_mapper.Map<IList<CustomerDto>>(customers));
        }

        // GET api/<CustomerController>/5
        [HttpGet("{customerId}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
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
        public async Task<BaseCommandResponse> Post([FromBody] CreateCustomerDto customerDto)
        {
            var response = new BaseCommandResponse();
            var validator = new CustomerValidator(_authManager);
            var validationResult = await validator.ValidateAsync(customerDto);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }
            var customer = _mapper.Map<Customer>(customerDto);
            customer.DateMembered=DateTime.Now;
           await _unitOfWork.Customers.Add(customer);
           await _unitOfWork.Save();
           return response;
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public async Task<BaseCommandResponse> Put(int id, [FromBody] CreateCustomerDto customerDto)
        {
            var response = new BaseCommandResponse();
            var validator = new CustomerValidator(_authManager);
            var validationResult = await validator.ValidateAsync(customerDto);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }
            if (id == 0)
                response.Errors.Add("Id must greater than 0");
            var customer = _mapper.Map<Customer>(customerDto);
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.Save();
            return response;
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
