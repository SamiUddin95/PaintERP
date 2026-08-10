using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class VendorPaymentBill
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int VendorPaymentId { get; set; }

    [ForeignKey("VendorPaymentId")]
    public VendorPayment? VendorPayment { get; set; }

    [Required]
    public int BillId { get; set; }

    [ForeignKey("BillId")]
    public Bill? Bill { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BillAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountDue { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PaymentAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RemainingBalance { get; set; }
}
