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
using Api.Models.Identity;
using Api.Services;
using Marvin.Cache.Headers;
using X.PagedList;
using Azure.Core;
using System.Reflection.Metadata;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = UserRoles.Customer)]
    //[Authorize]
    public class BorrowRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthManager _authManager;

        public BorrowRequestController(IUnitOfWork unitOfWork, IMapper mapper, ICustomerRepository customerRepository, IBookRepository bookRepository, IAuthManager authManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
            _authManager = authManager;
        }

        [HttpGet]
        [Route("GetAllRequests")]
        //[Authorize(Roles = $"{UserRoles.Employee}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<IList<BorrowRequestDto>>> Get([FromQuery] QueryParameter? parameter)
        {
            var requests = await _unitOfWork.BorrowRequests.GetAll(parameter.RequestParameters, null, parameter.includes);
            var requestsDto = new List<BorrowRequestDto>();
            requestsDto = _mapper.Map<List<BorrowRequestDto>>(requests);
            if (parameter.includes == null)
                return Ok(requestsDto);
            if (!parameter.includes.Contains("Customer"))
                return Ok(requestsDto);
            foreach (var allocation in requestsDto)
            {
                allocation.Customer.User = _mapper.Map<UserDto>(_authManager.GetUserByUserId(allocation.Customer.UserId));
            }
            return Ok(requestsDto);
        }



        // GET: api/<BorrowRequestsController>
        [HttpGet]
        [Route("GetFilteredRequests")]
        [Authorize(Roles = UserRoles.Employee)]
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        //[HttpCacheValidation(MustRevalidate = false)]
        public ActionResult<IList<BorrowRequestDto>> Get([FromQuery] BorrowRequestQueryParameter? parameter)
        {
            var filteredRequest = _unitOfWork.BorrowRequests.GetFiltered(parameter.includes);

            if (parameter.CustomerId != null)
            {
                filteredRequest = filteredRequest.Where(b =>
                    b.CustomerId == parameter.CustomerId || string.IsNullOrWhiteSpace(parameter.CustomerId.ToString()));
            }

            if (parameter.BookId != null)
            {
                filteredRequest = filteredRequest.Where(b =>
                    b.BookId == parameter.BookId || string.IsNullOrWhiteSpace(parameter.BookId.ToString()));
            }

            var date = new DateTime(2000, 1, 1);
            if (parameter.StartDate != null)
            {
                filteredRequest = filteredRequest.Where(b => b.StartDate.Date >= parameter.StartDate);
            }

            if (parameter.EndDate != null)
            {
                filteredRequest = filteredRequest.Where(b => b.EndDate <= parameter.EndDate);
            }
            if (parameter.Approved != null)
            {
                filteredRequest = filteredRequest.Where(b => b.Approved == parameter.Approved);
            }

            filteredRequest.ToPagedList(parameter.RequestParameters.PageNumber,
                 parameter.RequestParameters.PageSize);
            var requests = new List<BorrowRequestDto>();
            requests = _mapper.Map<List<BorrowRequestDto>>(filteredRequest);
            if (parameter.includes == null)
                return Ok(requests);

            if (parameter.includes.Contains("Customer"))
            {
                foreach (var allocation in requests)
                {
                    allocation.Customer.User = _mapper.Map<UserDto>(_authManager.GetUserByUserId(allocation.Customer.UserId));
                }
            }
            return Ok(requests);
        }



        // GET api/<BorrowRequestsController>/5
        [HttpGet]
        [Authorize(Roles = UserRoles.Employee)]
        public async Task<ActionResult<BorrowRequestDto>> Get(GetBorrowRequestDto requestDto)
        {
            var borrowRequest = await _unitOfWork.BorrowRequests.Get(b => b.Id == requestDto.id, requestDto.includes);
            if (borrowRequest == null)
                return NotFound();
            var request = _mapper.Map<BorrowRequestDto>(borrowRequest);
            if (requestDto.includes != null)
            {
                if (!requestDto.includes.Contains(UserRoles.Customer))
                    return Ok(borrowRequest);
            }
            request.Customer.User = _mapper.Map<UserDto>(_authManager.GetUserByUserId(request.Customer.UserId));
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
        [Authorize(Roles = UserRoles.Employee)]
        public async Task Delete(int id)
        {
            if (!await _unitOfWork.BorrowRequests.Exist(b => b.Id == id))
                throw new NotFoundException("borrowRequest not found", id);
            await _unitOfWork.BorrowRequests.Delete(id);
            await _unitOfWork.Save();
        }
    }
}
