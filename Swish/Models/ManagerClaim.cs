using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swish.Models
{
    public class ManagerClaim
    {
        [Key]
        public int Id { get; set; }
        
        public int ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public VerifManager Manager { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public VerifUser User { get; set; }
        
        //Specific claim abilities for manager
        //Each will require some baseline ability {e.g. request removal, mutual ownerships (kyc),
        //                                            update(can update mistakes and should they reflect for others)}
        
    }
}