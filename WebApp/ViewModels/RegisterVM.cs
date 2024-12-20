using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class RegisterVM
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Display(Name = "First name")]
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }
    
    [Display(Name = "Last name")]
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }
    
    [Display(Name = "JMBAG")]
    [Required(ErrorMessage = "JMBAG is required")]
    public string Jmbag { get; set; }
    
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    
}