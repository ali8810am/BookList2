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

            CreateMap<BookVm, BookDto>().ReverseMap();
            CreateMap<CreateBookDto, CreateBookVm>().ReverseMap();

            CreateMap<BorrowAllocationVm, BorrowAllocationDto>();
            CreateMap<CreateBorrowAllocationVm, CreateBorrowAllocationDto>().ReverseMap();
            CreateMap<BorrowAllocationVm, CreateBorrowAllocationDto>().ReverseMap();
        }
    }
}
