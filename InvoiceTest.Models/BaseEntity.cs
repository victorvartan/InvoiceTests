using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceTest.Models
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateLastUpdated { get; set; }

        [JsonIgnore]
        public Guid CreatedByUserId { get; set; }

        [JsonIgnore]
        public Guid? LastUpdatedByUserId { get; set; }
    }
}
