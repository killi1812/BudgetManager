using Data.Models;

namespace Data.Dto;

public class BudgetDto
{
    public long Idbudget { get; set; }

    public decimal? Sum { get; set; }

    public long? UserId { get; set; }

    public long? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}