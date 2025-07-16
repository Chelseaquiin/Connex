namespace Connex.Models.Entities
{
    public class Invites
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string ReasonForApplying { get; set; }
        public string Event { get; set; } = "API Conference 2025";
        public InviteStatus Status { get; set; } = InviteStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
