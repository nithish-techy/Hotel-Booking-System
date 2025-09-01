using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Booking_System.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public int RoomID { get; set; }
        public string Name { get; set; }  // Ensure Name exists
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }

    }
}
