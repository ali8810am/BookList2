using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IList<BorrowRequestDto>>> Get()
        {
            var requests = await _unitOfWork.BorrowRequests.GetAll();

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
