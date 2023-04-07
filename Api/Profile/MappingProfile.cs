using Api.Data;
using Api.Models;

namespace Api.Profile
{
    public class MappingProfile:AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, CreateBookDto>().ReverseMap();


            CreateMap<BorrowRequest, BorrowRequestDto>().ReverseMap();
            CreateMap<BorrowRequest, CreateBorrowRequestDto>().ReverseMap();


            CreateMap<BorrowAllocation, BorrowAllocationDto>().ReverseMap();
            CreateMap<BorrowAllocation, CreateBorrowAllocationDto>().ReverseMap();


        }
    }
}
