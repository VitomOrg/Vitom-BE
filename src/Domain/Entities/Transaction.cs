using Domain.Enums;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Transaction : Entity
{
    public required Guid UserId { get; set; }
    [Range(0, 9999999999)]
    [RegularExpression(@"^\d+(\.\d{1,2})?$")]
    public decimal TotalAmount { get; set; } = 0;
    public required PaymentMethodEnum PaymentMethod { get; set; }
    public TransactionStatusEnum TransactionStatus { get; set; } = TransactionStatusEnum.Pending;
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
}