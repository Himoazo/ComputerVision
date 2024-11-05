using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ComputerVision.Data.Models.DTO;

// Presenterar appens olika filter
public enum FilterType
{
    sobel,
    blur,
    reflect,
    grayscale
}

//Strukturerar bildfil info som användaren laddar upp
public class ImageDTO
{
    [Required]
    [MaxLength(30)]
    public string? ImageName { get; set; }

    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    public IFormFile? ImageFile { get; set; }

    [Required]
    public FilterType Filter {  get; set; }
}

/*public class ImageUpdateDTO
{

[Required]
public int Id { get; set; }

[Required]
[MaxLength(30)]
public string? ImageName { get; set; }

[Required]
[MaxLength(50)]
public string? ImageMaterial { get; set; }

[Required]
public string? ImageUrl { get; set; }

public IFormFile? ImageFile { get; set; }
}*/