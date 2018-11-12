using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTest.Models
{
    public class Invoice : BaseEntity
    {
        [StringLength(400)] // Index annotation not available in EF Core yet, so it's done in the fluent config in DataContext.cs
        public string Identifier { get; set; }

        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        [InverseProperty(nameof(Invoice))]
        public virtual List<InvoiceNote> Notes { get; set; }

        public void CopyFrom(Invoice item)
        {
            Identifier = item.Identifier;
            Amount = item.Amount;
        }
    }
}
