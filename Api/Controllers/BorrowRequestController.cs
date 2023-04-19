using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using Api.Models.QueryParameter;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BorrowRequestController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<BorrowRequestsController>
        [HttpGet]
        public async Task<ActionResult<IList<BorrowRequestDto>>> Get([FromQuery] BorrowRequestQueryParameter? parameter)
        {
            if (parameter.WantAll == true)
            {
                var allRequests = await _unitOfWork.BorrowRequests.GetAll(null, parameter.includes);
                return Ok(_mapper.Map<IList<BorrowAllocationDto>>(allRequests));
            }

            if (parameter.CreatedBy != null
                || parameter.BookId != null || parameter.EndDate != null || parameter.Approved != null ||
                parameter.StartDate != null
                || parameter.DateRequested != null || parameter.Cancelled != null)
            {
                var filteredBooks = _unitOfWork.BorrowRequests.GetFiltered(parameter.includes)
                    .Where(b => b.CreatedBy == parameter.CreatedBy || string.IsNullOrWhiteSpace(parameter.CreatedBy))
                    .Where(b => b.BookId.ToString() == parameter.BookId || string.IsNullOrWhiteSpace(parameter.BookId))
                    .Where(b => b.StartDate >= parameter.StartDate)
                    .Where(b => b.EndDate <= parameter.EndDate)
                    .Where(b => b.DateRequested.AddDays(1) >= parameter.DateRequested &&
                                b.DateRequested.AddDays(-1) <= parameter.DateRequested)
                    .Where(b => b.Cancelled == parameter.Cancelled)
                    .ToPagedList(parameter.RequestParameters.PageNumber,
                        parameter.RequestParameters.PageSize);
                return Ok(_mapper.Map<IList<BookDto>>(filteredBooks));
            }

            var requests = await _unitOfWork.BorrowRequests.GetAll(null, parameter.includes);

            return Ok(_mapper.Map<IList<BorrowRequestDto>>(requests));
        }
        // GET api/<BorrowRequestsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRequestDto>> Get(int id)
        {
            var borrowRequest = await _unitOfWork.BorrowRequests.Get(b => b.Id == id);
            return Ok(_mapper.Map<BorrowRequestDto>(borrowRequest));
        }

        // POST api/<BorrowRequestsController>
        [HttpPost]
        public async Task Post([FromBody] CreateBorrowRequestDto borrowRequestDto)
        {
            var borrowRequest = _mapper.Map<BorrowRequest>(borrowRequestDto);
            await _unitOfWork.BorrowRequests.Add(borrowRequest);
            await _unitOfWork.Save();
        }

        // PUT api/<BorrowRequestsController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CreateBorrowRequestDto borrowRequestDto)
        {
            var borrowRequest =await _unitOfWork.BorrowRequests.Get(r => r.Id == id);
            if (borrowRequest==null)
                throw new NotFoundException("borrowRequest not found", id); 
            _mapper.Map(borrowRequestDto,borrowRequest);
            _unitOfWork.BorrowRequests.Update(borrowRequest);
            await _unitOfWork.Save();
        }

        // DELETE api/<BorrowRequestsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (!await _unitOfWork.BorrowRequests.Exist(b => b.Id == id))
                throw new NotFoundException("borrowRequest not found", id);
            await _unitOfWork.BorrowRequests.Delete(id);
            await _unitOfWork.Save();
        }
    }
}
