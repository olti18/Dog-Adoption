using Adoption.Areas.Identity.Data;
using AdoptionContext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Adoption.Areas.Identity.Data;
using Adoption.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AdoptionContext.Controllers
{
   [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Adoption.Data.AdoptionContext _context;
        private readonly UserManager<ApplicationUser> userManager;// Error:I dont have IdentityUser But application User

        public AdminController(Adoption.Data.AdoptionContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //IEnumerable<AdoptionRequest> adoptionRequests = _context.AdoptionRequests.ToList();
            var index = await _context.AdoptionRequests.ToListAsync();
            return View(index);
        }
        public async Task<IActionResult> ListUsers()
        {
            IEnumerable<AdoptionRequest> adoptionRequests = _context.AdoptionRequests.ToList();
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {   
            var user = await userManager.FindByIdAsync(id);// In async method varialbles should have await Because it thores error
            if (user == null)
            {
                return NotFound();
            }
            await userManager.DeleteAsync(user);
            TempData["success"] = "User has been deleted";

            return RedirectToAction("ListUsers");//ig we type return View("ListUsers"); Error While Deleting
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string UserId)
        {
            var user  = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }
            //await userManager.UpdateAsync(user);
            //return RedirectToAction("ListUsers");

            var userClamis = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                //UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
               Claims = userClamis.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user  = await userManager.FindByIdAsync(model.Id);
            if(user == null) 
            {
                return NotFound();
            }
            else
            {
             
                user.Email = model.Email;
                user.UserName = model.Email; // type Firsname if you want to Display FirstName
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                
                var result = await userManager.UpdateAsync(user);
                TempData["success"] = "User has been Edited";


                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                
                return View(model);


            }
        }

      /* public async Task<IActionResult> Index()
        {
            var PendingRequests = await _context.AdoptionRequests.
                Include(r => r.Dog)
                .Include(r => r.User)
                .Where(r => r.Status == "Pending")
                .ToListAsync();
            return View(PendingRequests);
        }*/


    }   
}
