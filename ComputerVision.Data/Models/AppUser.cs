using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerVision.Data.Models;

public class AppUser : IdentityUser
{
    public virtual ICollection<Image>? Images { get; set; }
}
