using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace Swish.Models
{
    
    // MId to use
    //Row#V  --MId->
    //Find ColumnName(MId) select all (UIds)
    //Insert at bottom of column
    
    
    
    public class IdClaimManager
    {
        public IdClaimManager() => MClaims = new HashSet<MClaim>();

        
        [ForeignKey("MId")]
        public virtual ManagerIds M { get; set; } 
        public HashSet<MClaim> MClaims { get; set; }
        //public Guid Id { get; set; }
    }

    public class MClaim
    {
        public MClaim() => UIDs = new HashSet<string>();
        
        [ForeignKey("MId")]
        public virtual ManagerIds M { get; set; } 
        public HashSet<string> UIDs { get; set; }
        //public string UId { get; set; }
        //public int MId { get; set; }
        //Add MId specific properties {View ID, Name, etc}

    }
    
}