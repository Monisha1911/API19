using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API9.Authenticate
{
    public class ApplicationUser:IdentityUser 
    {
        public string RoleType { get; internal set; }
    }
}
