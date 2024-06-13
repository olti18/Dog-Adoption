
using Adoption.Areas.Identity.Data;
using Adoption.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Adoption.Controllers
{
    public class WishListController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Data.AdoptionContext _context;
        
        public WishListController(Data.AdoptionContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AddToWishlist(int Id)
        {
            try
            {
                // Check if the dog exists in the Dogs table
                var dogAddToWishlist = await _context.Dogs.FirstOrDefaultAsync(u => u.Id == Id);
                if (dogAddToWishlist == null)
                {   
                    // Dog doesn't exist, return an error message
                    return NotFound("The dog with the specified ID does not exist.");
                }

                // Get the current user
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized("User not found.");
                }

                var userId = user.Id;
                var wishlistItems = await _context.WishLists.Where(u => u.userId == userId).ToListAsync();

                var existingWishListItem = wishlistItems.FirstOrDefault(p => p.DogId == Id);
                if (existingWishListItem != null)
                {
                    // Dog is already in wishlist, increase quantity
                    existingWishListItem.Quantity += 1;
                    _context.WishLists.Update(existingWishListItem);
                }
                else
                {
                    // Dog is not in wishlist, create a new entry
                    var newWishList = new WishList
                    {
                        DogId = Id,
                        userId = userId,
                        Quantity = 1
                    };
                    await _context.WishLists.AddAsync(newWishList);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Log the detailed error
                var errorMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                // Log the error message
                // _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }
            catch (Exception ex)
            {
                // Log the detailed error
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                // Log the error message
                // _logger.LogError(errorMessage);
                return StatusCode(500, errorMessage);
            }

            return RedirectToAction("Index", "Dogs");
        }

        public async Task<IActionResult> DeleteFromWishlist(int dogId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized();
            }

            var userId = user.Id;
            var wishlistItem = await _context.WishLists
                                             .FirstOrDefaultAsync(u => u.userId == userId && u.DogId == dogId);

            if (wishlistItem == null)
            {
                // Handle the case where the wishlist item is not found
                return NotFound();
            }

            _context.WishLists.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewWishList", "WishList");
        }
        public async Task<IActionResult> ViewWishlist()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized();
            }

            var userId = user.Id;
            var wishlistItems = await _context.WishLists
                                              .Include(w => w.Dog)
                                              .Where(w => w.userId == userId)
                                              .ToListAsync();

            return View(wishlistItems);
        }
    }
}
