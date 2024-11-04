using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ComputerVision.Data.Models;

//IDdbContext inkluderar både Identity tabeller och Image tabell 
public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    // constructor initierar klassen med appens databasinställningarna
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    // Skapar Images tabellen i db
    public DbSet<Image> Images { get; set; }

}
