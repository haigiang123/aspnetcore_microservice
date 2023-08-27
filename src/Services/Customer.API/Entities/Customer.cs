using Contracts.Domains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.API.Entities
{
    public class Customer : EntityBase<int>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(170)")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddess { get; set; }
    }
}
