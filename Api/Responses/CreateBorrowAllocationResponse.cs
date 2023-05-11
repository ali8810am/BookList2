using Api.Models;

namespace Api.Responses
{
    public class CreateBorrowAllocationResponse
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public List<CreateBorrowAllocationValidatorDto>? InvalidRequests { get; set; }
    }
}
