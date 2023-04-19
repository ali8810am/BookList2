﻿using Api.Data;
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


            CreateMap<BorrowAllocation, BorrowAllocationDto>()
                .ForMember(dest => dest.AllocationId, option => option.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<BorrowAllocation, CreateBorrowAllocationDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();

            CreateMap<ApiUser, UserDto>().ReverseMap();



        }
    }
}
