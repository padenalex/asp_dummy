namespace Swish.Models
{
    public class VerificationProfile
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FakeImgStr { get; set; }
        public VerificationProfileStatus Status { get; set; }
    }

    public enum VerificationProfileStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}