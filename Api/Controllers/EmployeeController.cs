using Api.ConstantParameters;
using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Api.Responses;
using Marvin.Cache.Headers;
using Api.Models.Validators.BorrowRequest;
using Api.Models.Validators.UserValidator;
using Api.Repository;
using Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =UserRoles.Employee)]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper, IAuthManager authManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authManager = authManager;
        }
        // GET: api/<EmployeeController>

        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<IList<EmployeeDto>>> Get()
        {
            var employees = await _unitOfWork.Employee.GetAll();

            return Ok(_mapper.Map<IList<EmployeeDto>>(employees));
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{employeeId}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<EmployeeDto>> Get(string? userId, int employeeId)
        {
            var employee = new Employee();
            if (employeeId!=0)
            {
                employee = await _unitOfWork.Employee.Get(c => c.Id == employeeId, includes: new List<string> { "User" });
                if (employee == null)
                    return NotFound();

                return Ok(_mapper.Map<EmployeeDto>(employee));
            }

            if (userId == null)
                return BadRequest();

            employee = await _unitOfWork.Employee.Get(c => c.UserId == userId, includes: new List<string> { "User" });
            if (employee == null)
                return NotFound();
            return Ok(_mapper.Map<EmployeeDto>(employee));



            //var employee = await _unitOfWork.Employee.Get(c => c.EmployeeId == id);
            //return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<BaseCommandResponse> Post([FromBody] CreateEmployeeDto employeeDto)
        {
            var response = new BaseCommandResponse();
            var validator = new EmployeeValidator(_authManager);
            var validationResult = await validator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }
            var employee = _mapper.Map<Employee>(employeeDto);
            _unitOfWork.Employee.Add(employee);
            await _unitOfWork.Save();
            return response;
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task<BaseCommandResponse> Put(int id, [FromBody] CreateEmployeeDto employeeDto)
        {
            var response = new BaseCommandResponse();
            var validator = new EmployeeValidator(_authManager);
            var validationResult = await validator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }
            if (id == 0)
                response.Errors.Add("Id must greater than 0");
            var employee = _mapper.Map<Employee>(employeeDto);
            _unitOfWork.Employee.Update(employee);
            await _unitOfWork.Save();
            return response;
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var employee = await _unitOfWork.Employee.Get(c => c.Id == id);
            if (employee == null)
                throw new NotFoundException("Employee not found", employee.Id);
            await _unitOfWork.Employee.Delete(id);
            await _unitOfWork.Save();

        }
    }
}
