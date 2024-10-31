using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerVision.Data.Models;

[Table("Image")]
public class Image
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string? ImageName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? ImageMaterial { get; set; }

    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }

    public virtual AppUser? User { get; set; }
}
