using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class JournalEntry
{
    public int Id { get; set; }
    
    [Required]
    public int CompanyId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string EntryNumber { get; set; } = string.Empty;
    
    [Required]
    public DateTime EntryDate { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TransactionType { get; set; } = string.Empty; // Sales, Purchase, Payment, Production, etc.
    
    public int? ReferenceId { get; set; }
    
    [MaxLength(100)]
    public string ReferenceNumber { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDebit { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCredit { get; set; }
    
    [MaxLength(20)]
    public string Status { get; set; } = "Posted"; // Draft, Posted, Reversed
    
    public bool IsReversed { get; set; } = false;
    
    public int? ReversedByEntryId { get; set; }
    
    public DateTime? ReversedDate { get; set; }
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    
    [MaxLength(100)]
    public string CreatedBy { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string UpdatedBy { get; set; } = string.Empty;
    
    // Navigation properties
    public Company? Company { get; set; }
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}

public class JournalEntryLine
{
    public int Id { get; set; }
    
    [Required]
    public int JournalEntryId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string AccountCode { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string AccountName { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DebitAmount { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CreditAmount { get; set; }
    
    public int LineNumber { get; set; }
    
    // Navigation properties
    public JournalEntry? JournalEntry { get; set; }
}
