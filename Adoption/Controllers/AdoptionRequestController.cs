using Adoption.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Adoption.Models;
using Adoption.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AdoptionRequest = Adoption.Models.AdoptionRequest;

namespace Adoption.Controllers
{
    public class AdoptionRequestController : Controller
    {
        private readonly Data.AdoptionContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdoptionRequestController(Data.AdoptionContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> UserRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var adoptionRequests = await _context.AdoptionRequests
                .Include(ar => ar.Dog)
                .Where(ar => ar.UserId == user.Id)
                .ToListAsync();

            var viewModel = new UserAdoptionRequestViewModel
            {
                AdoptionRequests = adoptionRequests
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(int dogId)
        {
            // You might want to fetch the dog to ensure the dogId is valid
            var dog = _context.Dogs.FirstOrDefault(d => d.Id == dogId);
            if (dog == null)
            {
                return NotFound(); // Return 404 if the dog doesn't exist
            }

            var viewModel = new AdoptionRequestViewModel
            {
                DogId = dogId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdoptionRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Challenge(); // Redirect to login
                }

                var adoptionRequest = new AdoptionRequest
                {
                    DogId = model.DogId,
                    UserId = user.Id,
                    FullName = model.FullName,
                    Email = model.Email,
                    Address = model.Address,
                    Phone = model.Phone,
                    Status = "Pending"
                };

                _context.AdoptionRequests.Add(adoptionRequest);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Dogs", new { id = model.DogId });
            }

            return View(model);
        }

        // GET: AdoptionRequests/Approve/1
        public async Task<IActionResult> Approve(int id)
        {
            var adoptionRequest = await _context.AdoptionRequests.FindAsync(id);
            if (adoptionRequest == null)
            {
                return NotFound();
            }

            adoptionRequest.Status = "Approved";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index)); 
        }

        // GET: AdoptionRequests/Deny/1
        public async Task<IActionResult> Deny(int id)
        {
            var adoptionRequest = await _context.AdoptionRequests.FindAsync(id);
            if (adoptionRequest == null)
            {
                return NotFound();
            }

            adoptionRequest.Status = "Denied";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        /*[HttpGet]
        public IActionResult Create(int dogId)
        {
            var dog = _context.Dogs.Find(dogId);
            if (dog == null)
            {
                return NotFound();
            }

            var viewModel = new AdoptionRequestViewModel
            {
                DogId = dogId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdoptionRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var request = new AdoptionRequest
            {
                DogId = model.DogId,
                UserId = user.Id,
                FullName = model.FullName,
                Email = model.Email,
                Address = model.Address,
                Phone = model.Phone,
                Status = "Pending"
            };

            _context.AdoptionRequests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Dogs");
        }*/


    }

}


