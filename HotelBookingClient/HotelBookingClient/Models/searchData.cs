
using System.ComponentModel.DataAnnotations;

namespace HotelBookingClient.Models
{
    public class searchData
    {
        [Required]
        public DateTime startDate { get; set; }
        [Required]
        public DateTime endDate { get; set; }
        [Required]
        public int branch_id { get; set; }               

    }
}
