using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using Api.Models.QueryParameter;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowAllocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BorrowAllocationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<BorrowAllocationsController>
        [HttpGet]
        public async Task<ActionResult<IList<BorrowAllocationDto>>> GetAll([FromQuery] BorrowAllocationQueryParameter? parameter)
        {
            if (parameter.WantAll == true)
            {
                var allAllocations = await _unitOfWork.BorrowAllocations.GetAll(null, parameter.includes);
                return Ok(_mapper.Map<IList<BorrowAllocationDto>>(allAllocations));
            }

            if (parameter.CreatedBy != null || parameter.UpdatedBy != null || parameter.CustomerId != null || parameter.EmployeeId != null
                || parameter.BookId != null || parameter.BorrowStartDate != null || parameter.DateApproved != null || parameter.BorrowEndDate != null
                || parameter.IsReturned != null || parameter.DateReturned != null)
            {
                var filteredBooks = _unitOfWork.BorrowAllocations.GetFiltered(parameter.includes)
                    .Where(b => b.CreatedBy==parameter.CreatedBy || string.IsNullOrWhiteSpace(parameter.CreatedBy))
                    .Where(b => b.UpdatedBy == parameter.UpdatedBy || string.IsNullOrWhiteSpace(parameter.UpdatedBy))
                    .Where(b => b.CustomerId.ToString() == parameter.CustomerId || string.IsNullOrWhiteSpace(parameter.CustomerId))
                    .Where(b => b.EmployeeId.ToString() == parameter.EmployeeId || string.IsNullOrWhiteSpace(parameter.EmployeeId))
                    .Where(b => b.BookId == parameter.BookId || string.IsNullOrWhiteSpace(parameter.BookId.ToString()))
                    .Where(b => b.BorrowStartDate >= parameter.BorrowStartDate )
                    .Where(b => b.BorrowEndDate <= parameter.BorrowEndDate)
                    .Where(b => b.DateApproved.AddDays(1) >= parameter.DateApproved&& b.DateApproved.AddDays(-1)<= parameter.DateApproved)
                    .Where(b => b.DateReturned<= parameter.DateReturned)
                    .Where(b => b.IsReturned == parameter.IsReturned)
                    .ToPagedList(parameter.RequestParameters.PageNumber,
                        parameter.RequestParameters.PageSize);
                return Ok(_mapper.Map<IList<BookDto>>(filteredBooks));
            }

            var books = await _unitOfWork.Books.GetAll(parameter.RequestParameters, null, parameter.includes);
            return Ok(_mapper.Map<IList<BookDto>>(books));
        }

        // GET api/<BorrowAllocationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowAllocationDto>> Get(int id)
        {
            var borrowAllocation = await _unitOfWork.BorrowAllocations.Get(b => b.Id == id);
            return Ok(_mapper.Map<BorrowAllocationDto>(borrowAllocation));
        }

        // POST api/<BorrowAllocationsController>
        [HttpPost]
        public async Task Post([FromBody] CreateBorrowAllocationDto borrowAllocationDto)
        {
            var borrowAllocation = _mapper.Map<BorrowAllocation>(borrowAllocationDto);
            await _unitOfWork.BorrowAllocations.Add(borrowAllocation);
            await _unitOfWork.Save();
        }

        // PUT api/<BorrowAllocationsController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CreateBorrowAllocationDto borrowAllocationDto)
        {
            var allocate = await _unitOfWork.BorrowAllocations.Get(b => b.Id == id);
            if (allocate == null)
                throw new NotFoundException("BorrowAllocation not found", id);
            _mapper.Map(borrowAllocationDto, allocate);
            _unitOfWork.BorrowAllocations.Update(allocate);
            await _unitOfWork.Save();
        }

        // DELETE api/<BorrowAllocationsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (!await _unitOfWork.BorrowAllocations.Exist(b => b.Id == id))
                throw new NotFoundException("borrowAllocation not found", id);
            await _unitOfWork.BorrowAllocations.Delete(id);
            await _unitOfWork.Save();
        }
    }
}

