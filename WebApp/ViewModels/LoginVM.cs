using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class LoginVM
{
    [Display(Name = "Username")]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public string ReturnUrl { get; set; } = "/Home/Index";
}