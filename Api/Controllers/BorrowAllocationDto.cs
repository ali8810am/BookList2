using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowAllocationDto : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BorrowAllocationDto(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<BorrowAllocationsController>
        [HttpGet]
        public async Task<ActionResult<IList<BorrowAllocationDto>>> Get()
        {
            var requests = await _unitOfWork.BorrowAllocations.GetAll();

            return Ok(_mapper.Map<IList<BorrowAllocationDto>>(requests));
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

