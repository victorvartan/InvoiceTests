using System.ComponentModel.DataAnnotations;

namespace InvoiceTest.Models
{
    public class User : BaseEntity
    {
        [StringLength(400)]
        public string ApiKey { get; set; }

        public UserRole Role { get; set; }
    }
}
