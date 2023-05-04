using Api.ConstantParameters;
using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Employee},{UserRoles.Admin}")]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<EmployeeController>

        [HttpGet]
        public async Task<ActionResult<IList<EmployeeDto>>> Get()
        {
            var employees = await _unitOfWork.Employee.GetAll();

            return Ok(_mapper.Map<IList<EmployeeDto>>(employees));
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{employeeId}")]
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
        public async Task Post([FromBody] CreateEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            _unitOfWork.Employee.Add(employee);
            await _unitOfWork.Save();
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CreateEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            _unitOfWork.Employee.Update(employee);
            await _unitOfWork.Save();
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
