using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using Api.Models.Identity;
using Api.Models.QueryParameter;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Api.Responses;
using X.PagedList;
using Api.Models.Validators.Book;
using Api.Models.Validators.BorrowAllocation;
using Api.ConstantParameters;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Marvin.Cache.Headers;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = UserRoles.Employee)]
    public class BorrowAllocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthManager _userService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public BorrowAllocationController(IUnitOfWork unitOfWork, IMapper mapper, IAuthManager userService, ICustomerRepository customerRepository, IBookRepository bookRepository, IEmployeeRepository employeeRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("GetAllAllocations")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<IList<BorrowAllocationDto>>> GetAll(
           [FromQuery] QueryParameter parameter)
        {
            var allocations = await _unitOfWork.BorrowAllocations.GetAll(parameter.RequestParameters, null, parameter.includes);
            var allocationsDto = new List<BorrowAllocationDto>();
            allocationsDto = _mapper.Map<List<BorrowAllocationDto>>(allocations);
            if (parameter.includes == null)
                return Ok(allocationsDto);
            if (parameter.includes.Contains("Customer"))
            {
                foreach (var allocation in allocationsDto)
                {
                    allocation.Customer.User = _mapper.Map<UserDto>(_userService.GetUserByUserId(allocation.Customer.UserId));
                }
            }
            if (parameter.includes.Contains("Employee"))
            {
                foreach (var allocation in allocationsDto)
                {
                    allocation.Employee.User = _mapper.Map<UserDto>(_userService.GetUserByUserId(allocation.Employee.UserId));
                }
            }
            return Ok(allocationsDto);
        }



        // GET: api/<BorrowAllocationsController>
        [HttpGet]
        [Route("GetAllFilteredAllocations")]
        //[Authorize(Roles = $"{UserRoles.Customer}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<IList<BorrowAllocationDto>>> GetAll([FromQuery] BorrowAllocationQueryParameter? parameter)
        {
            //parameter.includes = new List<string> { "Book", "Customer", "Employee" };

            var filteredAllocation = _unitOfWork.BorrowAllocations.GetFiltered(parameter.includes);

            if (parameter.CreatedBy != "")
            {
                filteredAllocation= filteredAllocation.Where(b =>
                    b.CreatedBy == parameter.CreatedBy || string.IsNullOrWhiteSpace(parameter.CreatedBy));
            }

            if (parameter.UpdatedBy != "")
            {
                filteredAllocation = filteredAllocation.Where(b =>
                    b.UpdatedBy == parameter.UpdatedBy || string.IsNullOrWhiteSpace(parameter.UpdatedBy));
            }

            if (parameter.CustomerId != null)
            {
                filteredAllocation = filteredAllocation.Where(b =>
                    b.CustomerId == parameter.CustomerId || string.IsNullOrWhiteSpace(parameter.CustomerId.ToString()));
            }

            if (parameter.EmployeeId != null)
            {
                filteredAllocation = filteredAllocation.Where(b =>
                    b.EmployeeId == parameter.EmployeeId || string.IsNullOrWhiteSpace(parameter.EmployeeId.ToString()));
            }

            if (parameter.BookId != null)
            {
                filteredAllocation = filteredAllocation.Where(b =>
                    b.BookId == parameter.BookId || string.IsNullOrWhiteSpace(parameter.BookId.ToString()));
            }

            var date = new DateTime(2000, 1, 1);
            if (parameter.BorrowStartDate != date)
            {
                filteredAllocation = filteredAllocation.Where(b => b.BorrowStartDate.Date >= parameter.BorrowStartDate);
            }

            if (parameter.DateApproved != date)
            {
                filteredAllocation = filteredAllocation.Where(b =>
                    b.DateApproved.AddDays(1) >= parameter.DateApproved &&
                    b.DateApproved.AddDays(-1) <= parameter.DateApproved);
            }

            if (parameter.BorrowEndDate != date)
            {
                filteredAllocation = filteredAllocation.Where(b => b.BorrowEndDate <= parameter.BorrowEndDate);
            }

            if (parameter.IsReturned != null)
            {
                filteredAllocation = filteredAllocation.Where(b => b.IsReturned == parameter.IsReturned);
            }

            if (parameter.DateReturned != date)
            {
                filteredAllocation = filteredAllocation.Where(b => b.DateReturned >= parameter.DateReturned);
            }
            filteredAllocation.ToPagedList(parameter.RequestParameters.PageNumber,
                parameter.RequestParameters.PageSize);
            var allocationsDto = new List<BorrowAllocationDto>();
            allocationsDto = _mapper.Map<List<BorrowAllocationDto>>(filteredAllocation);
            if (parameter.includes == null)
                return Ok(allocationsDto);
            if (parameter.includes.Contains("Customer"))
            {
                foreach (var allocation in allocationsDto)
                {
                    allocation.Customer.User =
                        _mapper.Map<UserDto>(_userService.GetUserByUserId(allocation.Customer.UserId));
                }
            }

            if (parameter.includes.Contains("Employee"))
            {
                foreach (var allocation in allocationsDto)
                {
                    allocation.Employee.User =
                        _mapper.Map<UserDto>(_userService.GetUserByUserId(allocation.Employee.UserId));
                }
            }

            return Ok(allocationsDto);

            //var books = await _unitOfWork.Books.GetAll(parameter.RequestParameters, null, parameter.includes);
            //return Ok(_mapper.Map<IList<BookDto>>(books));
        }



        // GET api/<BorrowAllocationsController>/5
        [HttpGet]
        [Authorize(Roles = UserRoles.Customer)]
        public async Task<ActionResult<BorrowAllocationDto>> Get(GetBorrowAllocationDto allocation)
        {
            var borrowAllocation = await _unitOfWork.BorrowAllocations.Get(b => b.Id == allocation.id, allocation.includes);
            if (borrowAllocation == null)
                return NotFound();
            var allocationDto = _mapper.Map<BorrowAllocationDto>(borrowAllocation);
            if (allocation.includes.Contains("Customer"))
            {

                allocationDto.Customer.User = _mapper.Map<UserDto>(_userService.GetUserByUserId(borrowAllocation.Customer.UserId));
            }
            if (allocation.includes.Contains("Employee"))
            {

                allocationDto.Employee.User = _mapper.Map<UserDto>(_userService.GetUserByUserId(borrowAllocation.Employee.UserId));
            }
            return Ok(allocationDto);
        }



        // POST api/<BorrowAllocationsController>
        [HttpPost]
        public async Task<CreateBorrowAllocationResponse> Post([FromBody] List<CreateBorrowAllocationDto> borrowAllocationDto)
        {
            var response = new CreateBorrowAllocationResponse();
            var validator = new CreateBorrowAllocationValidator(_customerRepository, _employeeRepository, _bookRepository);
            var invalidRequests = new List<CreateBorrowAllocationValidatorDto>();
            var invalidResult = new CreateBorrowAllocationValidatorDto();
            foreach (var allocation in borrowAllocationDto)
            {
                var validationResult = await validator.ValidateAsync(allocation);
                if (validationResult.IsValid) continue;
                invalidResult.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                invalidResult.RequestId = allocation.RequestId;
                invalidRequests.Add(invalidResult);
                borrowAllocationDto.Remove(allocation);
            }

            if (invalidRequests.Count != 0)
            {
                response.Success = false;
                response.InvalidRequests = invalidRequests;
                response.Message = "Creation for some requests Failed";
                return response;
            }
            foreach(var al in borrowAllocationDto)
            {
           
               var borrowAllocation= _mapper.Map<BorrowAllocation>(al);
                try
                {
                    await _unitOfWork.BorrowAllocations.Add(borrowAllocation);
                    var book = await _unitOfWork.Books.Get(b => b.Id == borrowAllocation.BookId);
                    var bookForUpdate = _mapper.Map<Book>(book);
                    bookForUpdate.DateBackToLibrary = borrowAllocation.BorrowEndDate;
                    bookForUpdate.IsInLibrary = false;
                    _unitOfWork.Books.Update(bookForUpdate);
                    var request = await _unitOfWork.BorrowRequests.Get(r => r.Id == borrowAllocation.RequestId);
                    request.Customer = null;
                    request.Book = null;
                    request.Approved = true;
                    _unitOfWork.BorrowRequests.Update(request);
                    await _unitOfWork.Save();
                }
                catch (BadRequestException e)
                {
                    invalidResult.Errors.Add(e.Message);
                    invalidResult.RequestId = borrowAllocation.RequestId;
                    invalidRequests.Add(invalidResult);
                }
            }
            if (invalidRequests.Count == 0) return response;
            response.Success = false;
            response.InvalidRequests = invalidRequests;
            response.Message = "Creation for some requests Failed";
            return response;
        }

        // PUT api/<BorrowAllocationsController>/5
        [HttpPut("{id}")]
        public async Task<BaseCommandResponse> Put(int id, [FromBody] CreateBorrowAllocationDto borrowAllocationDto)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateBorrowAllocationValidator(_customerRepository, _employeeRepository, _bookRepository);
            var validationResult = await validator.ValidateAsync(borrowAllocationDto);
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
                var allocate = await _unitOfWork.BorrowAllocations.Get(b => b.Id == id);
                if (allocate == null)
                    throw new NotFoundException("BorrowAllocation not found", id);
                _mapper.Map(borrowAllocationDto, allocate);
                _unitOfWork.BorrowAllocations.Update(allocate);
                var book = await _unitOfWork.Books.Get(b => b.Id == allocate.BookId);
                var bookForUpdate = _mapper.Map<Book>(book);
                if (allocate.IsReturned)
                {
                    bookForUpdate.DateBackToLibrary = allocate.DateReturned;
                    bookForUpdate.IsInLibrary = true;
                }
                else
                {
                    bookForUpdate.DateBackToLibrary = allocate.BorrowEndDate;
                }

                _unitOfWork.Books.Update(bookForUpdate);
                await _unitOfWork.Save();
            }

            return response;
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

