using Adoption.Areas.Identity.Data;
using Adoption.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Adoption.Controllers
{
    public class CommentsController : Controller
    {
        private readonly Data.AdoptionContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(Data.AdoptionContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action to add a comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int dogId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                // Handle invalid input here (e.g., return an error message or redirect back with a validation message)
                return RedirectToAction("Details", "Dogs", new { id = dogId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handle the case where the user is not authenticated
                return RedirectToAction("Login", "Account");
            }

            var comment = new Comment
            {
                DogId = dogId,
                UserId = user.Id,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            TempData["success"] = "Comment added Successfully";
            return RedirectToAction("Details", "Dogs", new { id = dogId });
        }

        // Optional: Action to delete a comment (only by the comment owner or an admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (comment.UserId != user.Id && !User.IsInRole("Admin")))
            {
                // Handle unauthorized accessz
                return Forbid();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            TempData["Success"] = "The Comment has been deleted";
            return RedirectToAction("Details", "Dogs", new { id = comment.DogId });
        }
    }
}
