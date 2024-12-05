using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class CategoryVM
{
    public string Guid { get; set; }
    
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Name for category is required")]
    public string Name { get; set; }
}