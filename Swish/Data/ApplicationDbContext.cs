using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Swish.Models;

namespace Swish.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<VerifUser> VerifUsers { get; set; }
        public DbSet<VerifManager> VerifManagers { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<ManagerClaim> ManagerClaims { get; set; }

    }
}