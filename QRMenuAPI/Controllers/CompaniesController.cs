using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRMenuAPI.Data;
using QRMenuAPI.Models;

namespace QRMenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Claim claim;
        }


        // GET: api/Companies
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
          if (_context.Companies == null)
          {
              return NotFound();
          }
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
          if (_context.Companies == null)
          {
              return NotFound();
          }
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "CompanyAdminstrator")]
        [Authorize(Policy = "CompAdmin")]
        public async Task<IActionResult> PutCompany(Company company)
        {
            if (User.HasClaim("CompanyId", company.Id.ToString()) == false)
            {
                return Unauthorized();
            }

            _context.Entry(company).State = EntityState.Modified;


            _context.SaveChanges();
            return Ok();
            
        }
        [Authorize(Roles = "Administrator")]
        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public int PostCompany(Company company)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            Claim claim;

            _context.Companies.Add(company);
            _context.SaveChanges();
            applicationUser.CompanyId = company.Id;
            applicationUser.Email = "abc@def.com";
            applicationUser.PhoneNumber = "1112223344";
            applicationUser.RegisterDate = DateTime.Today;
            applicationUser.StateId = 1;
            applicationUser.UserName = "Administrator" + company.Id.ToString();
            _userManager.CreateAsync(applicationUser, "Admin123!").Wait();
            _userManager.AddToRoleAsync(applicationUser, "CompanyAdministrator").Wait();
            claim = new Claim("CompanyId", company.Id.ToString());

            _userManager.AddClaimAsync(applicationUser, claim).Wait();

            return company.Id;
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "CompanyAdministrator, Administrator")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (_context.Companies == null)
            {
                return NotFound();
            }
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return (_context.Companies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
