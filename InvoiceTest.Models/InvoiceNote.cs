using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTest.Models
{
    public class InvoiceNote : BaseEntity
    {
        public Guid InvoiceId { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(InvoiceId))]
        public Invoice Invoice { get; set; }

        public void CopyFrom(InvoiceNote item)
        {
            Text = item.Text;
        }
    }
}
