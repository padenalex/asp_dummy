using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Swish.Authorization;

namespace Swish.Models
{
    public class VerificationProfile
    {
        public VerificationProfile(){MIds = new HashSet<string>();}
        public int UId { get; set; }
        public virtual IdentityUser U { get; set; }
        //public int Id { get; set; }
        //public string OwnerId { get; set; }
        //[ForeignKey("MId")]
        //public virtual ManagerIds M { get; set; }
        public HashSet<string> MIds { get; set; }
        
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