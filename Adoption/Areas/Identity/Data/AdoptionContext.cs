using Adoption.Areas.Identity.Data; 
using Adoption.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Adoption.Data;

public class AdoptionContext : IdentityDbContext<ApplicationUser>
{
    public AdoptionContext(DbContextOptions<AdoptionContext> options)
        : base(options)
    {
    }

    public DbSet<WishList> WishLists { get; set; }
    public DbSet<Dog>Dogs { get; set; }
    public DbSet<Breed>Breeds { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
