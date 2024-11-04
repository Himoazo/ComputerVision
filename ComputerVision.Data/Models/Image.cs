using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerVision.Data.Models;


// Image tabellen 
[Table("Image")]
public class Image
{
    public int Id { get; set; } //Pimary key

    [Required]
    [MaxLength(30)]
    public string? ImageName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? ImageMaterial { get; set; } //bildfil uniktnamn på server

    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    [ForeignKey(nameof(User))] //Foreign key
    public string? UserId { get; set; }

    public virtual AppUser? User { get; set; }
}
