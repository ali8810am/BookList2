using Api.ConstantParameters;
using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using Api.Models.QueryParameter;
using Api.Models.Validators.BorrowAllocation;
using Api.Models.Validators.BorrowRequest;
using Api.Repository;
using Api.Responses;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using X.PagedList;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Customer}")]
    public class BorrowRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowRequestController(IUnitOfWork unitOfWork, IMapper mapper, ICustomerRepository customerRepository, IBookRepository bookRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [Route("GetAllRequests")]
        [Authorize(Roles = $"{UserRoles.Employee}")]
        public async Task<ActionResult<IList<BorrowRequestDto>>> Get([FromQuery] QueryParameter? parameter)
        {
            var requests = await _unitOfWork.BorrowRequests.GetAll(parameter.RequestParameters, null, parameter.includes);
            return Ok(_mapper.Map<IList<BorrowRequestDto>>(requests));
        }



        // GET: api/<BorrowRequestsController>
        [HttpGet]
        [Route("GetFilteredRequests")]
        [Authorize(Roles = $"{UserRoles.Employee}")]
        public async Task<ActionResult<IList<BorrowRequestDto>>> Get([FromQuery] BorrowRequestQueryParameter? parameter)
        {
            var filteredRequest = _unitOfWork.BorrowRequests.GetFiltered(parameter.includes);

            if (parameter.CustomerId != null)
            {
                filteredRequest.Where(b =>
                    b.CustomerId == parameter.CustomerId || string.IsNullOrWhiteSpace(parameter.CustomerId.ToString()));
            }

            if (parameter.BookId != null)
            {
                filteredRequest.Where(b =>
                    b.BookId == parameter.BookId || string.IsNullOrWhiteSpace(parameter.BookId.ToString()));
            }

            var date = new DateTime(2000, 1, 1);
            if (parameter.StartDate != null)
            {
                filteredRequest.Where(b => b.StartDate.Date >= parameter.StartDate);
            }

            if (parameter.EndDate != null)
            {
                filteredRequest.Where(b => b.EndDate <= parameter.EndDate);
            }

            if (parameter.Approved != null)
            {
                filteredRequest.Where(b => b.Approved == parameter.Approved);
            }

            filteredRequest.ToPagedList(parameter.RequestParameters.PageNumber,
                parameter.RequestParameters.PageSize);
            //if (parameter.includes != null)
            //{
            //    foreach (var request in requests)
            //    {
            //        request.Customer = includeCustomer;
            //        request.Book = includeBook;
            //        request.Customer.User = includeUser;
            //    }
            //}
            return Ok(_mapper.Map<IList<BorrowRequestDto>>(filteredRequest));
        }



        // GET api/<BorrowRequestsController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Employee}")]
        public async Task<ActionResult<BorrowRequestDto>> Get(int id, List<string>? includes)
        {
            var borrowRequest = await _unitOfWork.BorrowRequests.Get(b => b.Id == id, includes);
            if (borrowRequest == null)
                return NotFound();
            return Ok(_mapper.Map<BorrowRequestDto>(borrowRequest));
        }




        // POST api/<BorrowRequestsController>
        [HttpPost]
        public async Task<BaseCommandResponse> Post([FromBody] CreateBorrowRequestDto borrowRequestDto)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateBorrowRequestValidator(_bookRepository, _customerRepository);
            var validationResult = await validator.ValidateAsync(borrowRequestDto);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }

            else
            {
                var borrowRequest = _mapper.Map<BorrowRequest>(borrowRequestDto);
                borrowRequest.Book = null;
                borrowRequest.Customer = null;
                await _unitOfWork.BorrowRequests.Add(borrowRequest);
                await _unitOfWork.Save();
            }

            return response;
        }

        // PUT api/<BorrowRequestsController>/5
        [HttpPut("{id}")]
        public async Task<BaseCommandResponse> Put(int id, [FromBody] CreateBorrowRequestDto borrowRequestDto)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateBorrowRequestValidator(_bookRepository, _customerRepository);
            var validationResult = await validator.ValidateAsync(borrowRequestDto);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }
            if (id == 0)
                response.Errors.Add("Id must greater than 0");
            else
            {
                var borrowRequest = await _unitOfWork.BorrowRequests.Get(r => r.Id == id);
                borrowRequest.Book = null;
                borrowRequest.Customer = null;
                if (borrowRequest == null)
                    throw new NotFoundException("borrowRequest not found", id);
                _mapper.Map(borrowRequestDto, borrowRequest);
                _unitOfWork.BorrowRequests.Update(borrowRequest);
                await _unitOfWork.Save();
            }

            return response;
        }

        // DELETE api/<BorrowRequestsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.Employee}")]
        public async Task Delete(int id)
        {
            if (!await _unitOfWork.BorrowRequests.Exist(b => b.Id == id))
                throw new NotFoundException("borrowRequest not found", id);
            await _unitOfWork.BorrowRequests.Delete(id);
            await _unitOfWork.Save();
        }
    }
}
