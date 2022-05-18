using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API9.Authenticate
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser> 
    {
       public DbSet<Course_Details> course_details { get;set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }

        public ApplicationDbContext()
        {
        }
    }
}
