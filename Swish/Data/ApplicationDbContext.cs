using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Swish.Models;

namespace Swish.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<VerificationProfile> VerificationProfiles { get; set; }
        public DbSet<IdClaimManager> IdClaimManagers { get; set; }
        public DbSet<ManagerIds> ManagerIds { get; set; }
    }
}