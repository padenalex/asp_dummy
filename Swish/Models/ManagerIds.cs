using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Swish.Models
{
    public class ManagerIds
    {
        [ForeignKey("MId")]
        public virtual IdentityUser M { get; set; }
        [ForeignKey("UId")]
        public virtual IdentityUser U { get; set; }
    }
}