using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class CustomerPaymentInvoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CustomerPaymentId { get; set; }

    [ForeignKey("CustomerPaymentId")]
    public CustomerPayment? CustomerPayment { get; set; }

    [Required]
    public int SalesInvoiceId { get; set; }

    [ForeignKey("SalesInvoiceId")]
    public SalesInvoice? SalesInvoice { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal InvoiceAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountDue { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PaymentApplied { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RemainingBalance { get; set; }
}
