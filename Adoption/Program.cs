using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Adoption.Data;
using Adoption.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AdoptionContextConnection") ?? throw new InvalidOperationException("Connection string 'AdoptionContextConnection' not found.");

builder.Services.AddDbContext<Adoption.Data.AdoptionContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<Adoption.Data.AdoptionContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
   //pattern: "{controller=Home}/{action=Index}/{id?}");
   pattern: "{controller=Dogs}/{action=Index}/{id?}");
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    string firstName = "Olti";
    string lastName = "Berisha";
    string email = "oadmin@gmail.com";
    string password = "Olti1919@@";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.UserName = email;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}


app.Run();
