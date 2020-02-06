using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Swish.Models
{
    public class VerifUser
    {
        [Key]
        public int Id { get; set; }
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }
        
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