namespace WebApp.ViewModels;

public class BudgetVM
{
    public string Guid { get; set; }

    public decimal Sum { get; set; }

    public string UserGuid { get; set; }

    public string CategoryGuid { get; set; }

    public string CategoryName { get; set; }
    public string UserName { get; set; }
}