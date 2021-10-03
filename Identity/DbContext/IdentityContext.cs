using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.DbContext
{
    // Class ============= || DBContext-Identity ||==========================================
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {

        // Constructor - Options
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {    
        }




        // ==== || OnModelCreating || =============================================================================  
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ignore these so they dont come in to the db
            //builder.Entity<ApplicationUser>().Ignore(c => c.UserName);
            //builder.Entity<ApplicationUser>().Ignore(c => c.NormalizedUserName);
            //builder.Entity<ApplicationUser>().Ignore(c => c.NormalizedEmail);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);



            // ==== [Age] =============================================================================  
            builder.Entity<ApplicationUser>().Property(c => c.Age)
                .HasMaxLength(3)
                .HasDefaultValue(0);




            // ==== [Firstname] =======================================================================  
            builder.Entity<ApplicationUser>().Property(c => c.FirstName)
                .HasMaxLength(25)
                .IsRequired(false);





            // ==== [Lastname] ========================================================================  
            builder.Entity<ApplicationUser>().Property(c => c.LastName)
                .HasMaxLength(25)
                .IsRequired(false);

        }

    }
}
