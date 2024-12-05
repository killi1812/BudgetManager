using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class RegisterVM
{
    [Display(Name = "Username")]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Display(Name = "Repeat Password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password2 { get; set; }
}