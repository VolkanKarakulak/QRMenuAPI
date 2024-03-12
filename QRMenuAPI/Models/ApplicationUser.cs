using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace QRMenuAPI.Models
{
	public class ApplicationUser : IdentityUser
	{
        [StringLength(100, MinimumLength = 1)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = "";

        [StringLength(100, MinimumLength = 1)]
        [Column(TypeName = "nvarchar(100)")]
        public override string UserName { get; set; } = "";

        [EmailAddress]
        [Column(TypeName = "varchar(100)")]
        public override string Email { get; set; } = "";

        [Phone]
        [StringLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public override string? PhoneNumber { get; set; }

        public DateTime RegisterDate { get; set; }

        public byte StateId { get; set; }

        public int CompanyId { get; set; }

        [ForeignKey("StateId")]
        public virtual State? State { get; set; }

        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }




    }
}

