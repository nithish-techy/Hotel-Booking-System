using System.Data.Entity;

namespace Hotel_Booking_System.Models
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}
