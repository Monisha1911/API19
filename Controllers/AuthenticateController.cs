using API9.Authenticate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;

       
        public AuthenticateController(UserManager<ApplicationUser> userManager, IConfiguration configuration,ApplicationDbContext context)
        {
            this.userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user.UserName,
                    email = user.Email,
                    rolename = user.RoleType
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);

            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                RoleType = model.RoleType
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        public List<Users> use = Users.GetAllUsers();

        [HttpGet]
        [Route("Get All Users")]
        public ActionResult GetUsers()
        {
            return Ok(use);
        }




        [HttpGet]
        [Route("get courses")]

        public ActionResult GetCourses()
        {
            var lists = _context.course_details.ToList();
            return Ok(lists);
        }

        [HttpGet]
        [Route("GetByID")]

        public ActionResult GetCoursesById(int Course_ID)
        {
           
            Course_Details courses = _context.course_details.Find(Course_ID);
            return Ok(courses);
        }





        [HttpPost]
        [Route("Add Courses")]

        public ActionResult InsertCourses(Course_Details C)
        {
            if (ModelState.IsValid)
            {
               
                _context.course_details.Add(C);
               _context.SaveChanges();
                return Ok();

            }
            else
            {
                return BadRequest();
            }
        }



        [HttpPut]
        [Route("update Course")]
        public IActionResult UpdateAdmin(Course_Details C )
        {
            
            if (ModelState.IsValid)
            {
                Course_Details course = _context.course_details.Find(C.Course_Id);
                course.Coursecategory = C.Coursecategory;
                course.Description = C.Description;
                course.Coursestartdate = C.Coursestartdate;
                course.Format = C.Format;
                course.Level = C.Level;
                course.Price = C.Price;
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        [Route("delete Courses")]

        public IActionResult DeleteCourses(int Id)
        {

            var db = new ApplicationDbContext();
            Course_Details courses = _context.course_details.Find(Id);
            if (courses == null)
            {
                return BadRequest();
            }
            else
            {
                _context.course_details.Remove(courses);
                _context.SaveChanges();
                return Ok();
            }

        }



    }
}
