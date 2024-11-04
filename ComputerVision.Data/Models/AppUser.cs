using Microsoft.AspNetCore.Identity;


namespace ComputerVision.Data.Models;

// Formar applikationens användare, förlänger (extend) ASP.NET Core Identity klass 
public class AppUser : IdentityUser
{

    // One to many relation, en användare kan ha flera bilder. Implementeras av EF Core.
    public virtual ICollection<Image>? Images { get; set; }
}
