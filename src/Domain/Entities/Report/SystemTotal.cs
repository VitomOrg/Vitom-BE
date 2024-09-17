using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Report;

public class SystemTotal
{
    [Key]
    public int Id { get; set; } = 1;

    public decimal TotalIncome { get; set; }

    public int TotalTransactions { get; set; }

    public int TotalProducts { get; set; }

    public int TotalUsers { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}
