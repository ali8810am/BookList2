using Api.Data;
using Api.Models;
using Api.Models.Identity;

namespace Api.Profile
{
    public class MappingProfile:AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, CreateBookDto>().ReverseMap();


            CreateMap<BorrowRequest, BorrowRequestDto>()
                .ForMember(dest => dest.CustomerId, option => option.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.RequestId, option => option.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<BorrowRequest, CreateBorrowRequestDto>().ReverseMap();
            //CreateMap<BorrowRequest, UpdateBorrowRequestDto>().ReverseMap();


            CreateMap<BorrowAllocation, BorrowAllocationDto>()
                .ForMember(dest => dest.AllocationId, option => option.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<BorrowAllocation, CreateBorrowAllocationDto>()
                .ForMember(dest => dest.RequestId, option => option.MapFrom(src => src.RequestId))
                .ForMember(dest => dest.CustomerId, option => option.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.BookId, option => option.MapFrom(src => src.BookId))
                .ForMember(dest => dest.EmployeeId, option => option.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.DateApproved, option => option.MapFrom(src => src.DateApproved))
                .ForMember(dest => dest.DateReturned, option => option.MapFrom(src => src.DateReturned))
                .ForMember(dest => dest.BorrowEndDate, option => option.MapFrom(src => src.BorrowEndDate))
                .ForMember(dest => dest.BorrowStartDate, option => option.MapFrom(src => src.BorrowStartDate))
                .ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();

            CreateMap<Employee,CreateEmployeeDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();

            CreateMap<ApiUser, UserDto>()
                .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhoneNumber, option => option.MapFrom(src => src.PhoneNumber))
                .ReverseMap();


            //[
            //{
            //    "borrowStartDate": "2023-05-07T22:49:28.762Z",
            //    "borrowEndDate": "2023-05-08T22:49:28.762Z",
            //    "bookId": 15,
            //    "dateApproved": "2023-05-07T22:49:28.762Z",
            //    "employeeId": 6,
            //    "customerId": 19,
            //    "isReturned": false,
            //    "dateReturned": "2023-05-07T22:49:28.762Z",
            //    "requestId": 22
            //}
            //]


        }
    }
}
