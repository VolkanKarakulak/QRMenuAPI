using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRMenuAPI.Models
{
	public class Restaurant
	{
      
            public int Id { get; set; }

            [StringLength(50, MinimumLength = 1)]
            [Column(TypeName = "nvarchar(50)")]
            public string Name { get; set; } = "";

            [Phone]
            [StringLength(100)]
            [Column(TypeName = "nvarchar(30)")]
            public string Phone { get; set; } = "";

            [StringLength(5, MinimumLength = 5)]
            [Column(TypeName = "char(5)")]
            [DataType(DataType.PostalCode)]
            public string PostalCode { get; set; } = "";

            [StringLength(200, MinimumLength = 5)]
            [Column(TypeName = "nvarchar(200)")]
            public string AddressDetail { get; set; } = "";

            [Column(TypeName = "smalldatetime")]
            public DateTime DateTime { get; set; }

            public byte StateId { get; set; }

            [ForeignKey("StateId")]
            public State? State { get; set; }

            public int CompanyId { get; set; }

            [ForeignKey("CompanyId")]
            public Company? Company { get; set; }

    }
}

