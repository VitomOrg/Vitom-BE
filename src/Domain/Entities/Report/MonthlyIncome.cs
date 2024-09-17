using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Report;

public class MonthlyIncome
{
    [Range(2024, 3000)]
    public int Year { get; set; } = DateTime.Now.Year;

    [Range(1, 12)]
    public int Month { get; set; } = DateTime.Now.Month;

    [Range(0, 9999999999)]
    [RegularExpression(@"^\d+(\.\d{1,2})?$")]
    public decimal TotalIncome { get; set; } = 0;

    [Range(0, int.MaxValue)]
    public decimal TotalTransaction { get; set; } = 0;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    public void Update(decimal totalIncome, decimal totalTransaction)
    {
        TotalIncome = totalIncome;
        TotalTransaction = totalTransaction;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
