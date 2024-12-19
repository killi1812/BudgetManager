using System.ComponentModel.DataAnnotations;
using Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class IncomeVM
{
    [Required] public string Guid { get; set; }

    [Display(Name = "Sum", Description = "Amount of money for income")]
    [Required(ErrorMessage = "Sum for income is required")]
    public decimal Sum { get; set; }

    [Display(Name = "Source", Description = "The source of income")]
    public string? Source { get; set; }

    [Display(Name = "Date", Description = "Date when income is coming")]
    [Required(ErrorMessage = "Date for income is required")]
    public DateTime Date { get; set; }

    [Display(Name = "Frequency", Description = "Amount of money for income")]
    [Required(ErrorMessage = "Frequency for income is required")]
    public Frequency Frequency { get; set; }

    public List<SelectListItem> Frequencies { get; set; } = Enum.GetValues(typeof(Frequency))
        .Cast<Frequency>()
        .Select(f => new SelectListItem
        {
            Value = f.ToString(),
            Text = f.ToString()
        }).ToList();
}