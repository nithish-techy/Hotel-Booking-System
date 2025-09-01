using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Hotel_Booking_System.Models;

public class RoomsController : Controller
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["HotelDbContext"].ConnectionString;

    public ActionResult Index()
    {
        List<Room> Rooms = new List<Room>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT RoomID, RoomType, Price FROM Rooms";
            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Rooms.Add(new Room
                {
                    RoomID = (int)reader["RoomID"],
                    RoomType = reader["RoomType"].ToString(),
                    Price = (decimal)reader["Price"]
                });
            }
        }

        return View(Rooms);
    }
}
