using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRMenuAPI.Models
{
	public class State
    {
		//[Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // veritabanında ıd otomatik olarak artmasın
        public byte Id { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string Name { get; set; } = "";
    }
}

