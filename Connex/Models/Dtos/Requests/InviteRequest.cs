using System.ComponentModel.DataAnnotations;

namespace Connex.Models.Dtos.Requests
{
    public class InviteRequest
    {
        public required string FullName { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }
        public string ReasonForApplying { get; set; }
    }
}
