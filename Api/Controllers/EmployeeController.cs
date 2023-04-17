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
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> Get(int id)
        {
            var employee = await _unitOfWork.Employee.Get(c => c.EmployeeId == id);
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] CreateEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            _unitOfWork.Employee.Add(employee);
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CreateEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            _unitOfWork.Employee.Update(employee);
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var employee = await _unitOfWork.Employee.Get(c => c.EmployeeId == id);
            if (employee == null)
                throw new NotFoundException("Employee not found", employee.EmployeeId);
            await _unitOfWork.Employee.Delete(id);

        }
    }
}
