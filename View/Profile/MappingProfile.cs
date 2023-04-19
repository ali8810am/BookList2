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

            CreateMap<BookVm, BookDto>().ReverseMap();
            CreateMap<CreateBookDto, CreateBookVm>().ReverseMap();

            CreateMap<BorrowAllocationVm, BorrowAllocationDto>();
            CreateMap<CreateBorrowAllocationVm, CreateBorrowAllocationDto>().ReverseMap();
            CreateMap<BorrowAllocationVm, CreateBorrowAllocationDto>().ReverseMap();

            CreateMap<BorrowRequestVm, BorrowRequestDto>().ReverseMap();
            CreateMap<CreateBorrowRequestVm, CreateBorrowRequestDto>().ReverseMap();
            CreateMap<BorrowRequestVm, CreateBorrowRequestDto>().ReverseMap();


            CreateMap<CustomerDto, CustomerVm>()
                .ForMember(dest => dest.CustomerId, option => option.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
