using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Adoption.Data;
using Adoption.Models;
using Microsoft.AspNetCore.Hosting;
using Adoption.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Adoption.Controllers
{
    public class DogsController : Controller
    {
        private readonly Data.AdoptionContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        public DogsController(Data.AdoptionContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            this.userManager = userManager;
        }

        // GET: Dogs
        public async Task<IActionResult> Index()
        {
            var adoptionContext = _context.Dogs.Include(d => d.Breeds);
            return View(await adoptionContext.ToListAsync());
        }

        // GET: Dogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var dog = await _context.Dogs
                .Include(d => d.Breeds) // Include the Breed information
                .Include(d => d.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dog == null)
            {
                return NotFound();
            }

            var viewModel = new DogDetailsViewModel
            {
                Dog = dog,
                Comments = dog.Comments.OrderByDescending(c => c.CreatedAt).ToList(),
                NewComment = new Comment {  DogId = (int)(int?)id }// Cast 'id' to int?
            };

            return View(viewModel);
            /*if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Breeds)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);*/
        }

        // GET: Dogs/Create
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Description");
            return View();
        }

        // POST: Dogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,Age,Color,Weight,Height,ImageUrl,BreedId")] Dog dog , IFormFile? file)
        {
            try
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(dog.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, dog.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    dog.ImageUrl = @"\images\product\" + fileName;

                    _context.Add(dog);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
            }
            catch
            {
                ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Description", dog.BreedId);
                return View(dog);
            }
            return View(dog);
        }

        // GET: Dogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs.FindAsync(id);
            if (dog == null)
            {
                return NotFound();
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Description", dog.BreedId);
            return View(dog);
        }

        // POST: Dogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,Age,Color,Weight,Height,ImageUrl,BreedId")] Dog dog)
        {
            if (id != dog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Description", dog.BreedId);
            return View(dog);
        }

        // GET: Dogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Breeds)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);
            //if (dog != null)
            //{
                _context.Dogs.Remove(dog);
            //}
            var wishLists = await _context.WishLists.Where(w => w.DogId == id).ToListAsync();
            _context.WishLists.RemoveRange(wishLists);

            _context.Dogs.Remove(dog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogExists(int id)
        {
            return _context.Dogs.Any(e => e.Id == id);
        }
    }
}
