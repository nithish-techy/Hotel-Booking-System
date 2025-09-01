using Hotel_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Hotel_Booking_System.Data_Access_Layer
{

   
    public class DatabaseHelper
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HotelDbContext"].ConnectionString;

        public List<Room> GetAvailableRooms()
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT RoomID, RoomType, Price, IsAvailable FROM Rooms WHERE IsAvailable = 1";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rooms.Add(new Room
                    {
                        RoomID = reader.GetInt32(0),
                        RoomType = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        IsAvailable = reader.GetBoolean(3) // Ensure IsAvailable is a BIT column
                    });
                }

                reader.Close();
            }

            return rooms;
        }

    }
}
