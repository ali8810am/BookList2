using AutoMapper;
using View.Model;
using View.Services.Base;

namespace View.Profile
{
    public class MappingProfile:AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterVm, RegisterRequestDto>().ReverseMap();
            CreateMap<UserRegisterVm, UserDto>().ReverseMap();

            CreateMap<BookDto, BookVm>()
                .ForMember(dest => dest.DateBackToLibrary, option => option.MapFrom(src => src.DateBackToLibrary.GetValueOrDefault().DateTime))
                .ReverseMap();
            CreateMap<CreateBookDto, CreateBookVm>().ReverseMap();

            CreateMap<BorrowAllocationVm, BorrowAllocationDto>().ReverseMap();
            CreateMap<CreateBorrowAllocationVm, CreateBorrowAllocationDto>().ReverseMap();
            CreateMap<BorrowAllocationVm, CreateBorrowAllocationDto>().ReverseMap();

            CreateMap<BorrowRequestDto, BorrowRequestVm>()
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.StartDate.DateTime))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate.DateTime))
                .ForMember(dest => dest.DateRequested, option => option.MapFrom(src => src.DateRequested.DateTime))
                .ReverseMap();
            CreateMap<BorrowRequestVm, BorrowRequestForAllocationListVm>().ReverseMap();
            CreateMap<CreateBorrowRequestDto, BorrowRequestVm>()
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.StartDate.DateTime))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate.DateTime))
                .ForMember(dest => dest.DateRequested, option => option.MapFrom(src => src.DateRequested.DateTime))
                .ReverseMap();
            CreateMap<UpdateBorrowRequestDto, BorrowRequestVm>()
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.StartDate.DateTime))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate.DateTime))
                .ForMember(dest => dest.DateRequested, option => option.MapFrom(src => src.DateRequested.DateTime))
                .ReverseMap();
            CreateMap<CreateBorrowRequestDto, CreateBorrowRequestVm>()
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.StartDate.DateTime))
                .ForMember(dest => dest.EndDate, option => option.MapFrom(src => src.EndDate.DateTime))
                .ForMember(dest => dest.DateRequested, option => option.MapFrom(src => src.DateRequested.DateTime))
                .ReverseMap();


            CreateMap<CustomerDto, CustomerVm>()
                .ForMember(dest => dest.CustomerId, option => option.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<EmployeeDto, EmployeeVm>()
                .ForMember(dest => dest.EmployeeId, option => option.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateHired, option => option.MapFrom(src => src.DateHired.DateTime))
                .ReverseMap();
        }
    }
}
