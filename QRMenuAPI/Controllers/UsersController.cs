using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRMenuAPI.Data;
using QRMenuAPI.Models;
using Microsoft.AspNetCore.Identity;


namespace QRMenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(ApplicationContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _userManager.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string id)
        {
            var applicationUser = await _signInManager.UserManager.FindByIdAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return applicationUser;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutApplicationUser(string id, ApplicationUser applicationUser)/*, string? passWord = null, string? currentPassWord=null*/
        {

            var existingApplicationUser = _signInManager.UserManager.FindByIdAsync(id).Result;

            existingApplicationUser.Email = applicationUser.Email;
            existingApplicationUser.Name = applicationUser.Name;
            existingApplicationUser.PhoneNumber = applicationUser.PhoneNumber;
            existingApplicationUser.StateId = applicationUser.StateId;

            _userManager.UpdateAsync(existingApplicationUser).Wait();

            /*if(passWord!= null)
            {

                IdentityResult identityResult = _userManager.ChangePasswordAsync(existingApplicationUser, currentPassWord, passWord).Result;
            }*/
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser, string passWord)
        {
            await _userManager.CreateAsync(applicationUser, passWord);

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(string id)
        {

            var applicationUser = await _signInManager.UserManager.FindByIdAsync(id);

         
            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.StateId = 0; //passive alıyoruz
            _context.Users.Remove(applicationUser);
            await _userManager.UpdateAsync(applicationUser);

            return NoContent();
        }

        private bool ApplicationUserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpPost("LogIn")]
        public bool LogIn(string userName, string passWord)
        {


            Microsoft.AspNetCore.Identity.SignInResult signInResult;
     

            ApplicationUser? applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (applicationUser == null)
            {
                return false;
            }
            signInResult = _signInManager.PasswordSignInAsync(applicationUser, passWord, false, false).Result;
                
            return signInResult.Succeeded;
            

        }

        [HttpPost("ResetPassWord")]
        public void ResetPassWord(string userName, string passWord)
        {
            ApplicationUser applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;

            if(applicationUser == null)
            {
                return;
            }
            _signInManager.UserManager.RemovePasswordAsync(applicationUser).Wait();
            _signInManager.UserManager.AddPasswordAsync(applicationUser, passWord);
        }

        [HttpPost("PassWordReset")]
        public string? PasswordReset(string userName)
        {
            ApplicationUser applicationUser = _signInManager.UserManager.FindByNameAsync(userName).Result;
            if (User == null)
            {
                return null;//Kullanıcı Yok
            }
            return _signInManager.UserManager.GeneratePasswordResetTokenAsync(applicationUser).Result;
        }


        [HttpPost("ValideToken")]
        public ActionResult<string> ValidateResetPassword(string UserName, string token, string newPassword)
        {
            ApplicationUser applicationUser = _signInManager.UserManager.FindByNameAsync(UserName).Result;
            if (User == null)
            {
                return NotFound();//Kullanıcı Yok
            }
            IdentityResult identityResult = _signInManager.UserManager.ResetPasswordAsync(applicationUser, token, newPassword).Result;

            if (!identityResult.Succeeded)
            {
                return identityResult.Errors.First().Description;
            }
            return Ok("Password Reset Successfull");
        }
    }

}