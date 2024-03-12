using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRMenuAPI.Models
{
	public class RestaurantUser
	{

		public string UserId { get; set; } = "";

		public int RestaurantId { get; set; }

		
		[ForeignKey("RestaurantId")]
		public Restaurant? Restaurant { get; set; }

        [Column(TypeName = "nvarchar(400)")]
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUsers { get; set; }
    }
}

