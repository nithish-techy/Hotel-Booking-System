using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Hotel_Booking_System.Models;
using System.Collections.Generic;

public class BookingController : Controller
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["HotelDbContext"].ConnectionString;

    // ----------------- BOOK ------------------
    public ActionResult Book(int id)
    {
        ViewBag.RoomID = id;
        return View();
    }

    [HttpPost]
    public ActionResult Book(Booking booking)
    {
        if (ModelState.IsValid)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string query = @"INSERT INTO Bookings 
                (RoomID, Name, Email, Phone, CheckInDate, CheckOutDate, Status) 
                VALUES 
                (@RoomID, @Name, @Email, @Phone, @CheckInDate, @CheckOutDate, @Status)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@RoomID", booking.RoomID);
                cmd.Parameters.AddWithValue("@Name", booking.Name);
                cmd.Parameters.AddWithValue("@Email", booking.Email);
                cmd.Parameters.AddWithValue("@Phone", booking.Phone);
                cmd.Parameters.AddWithValue("@CheckInDate", booking.CheckInDate);
                cmd.Parameters.AddWithValue("@CheckOutDate", booking.CheckOutDate);
                cmd.Parameters.AddWithValue("@Status", "Confirmed");

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();


                if (rowsAffected > 0)
                {
                    TempData["BookingMessage"] = "Booking successful!";
                    return RedirectToAction("MyBookings");
                }
                else
                {
                    TempData["BookingMessage"] = "Error: Booking failed.";
                }
            }
        }

        TempData["BookingMessage"] = "There was an issue with your booking.";
        return View(booking);
    }

    // ----------------- MY BOOKINGS ------------------
    public ActionResult MyBookings()
    {
        List<Booking> bookings = new List<Booking>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM Bookings";
            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bookings.Add(new Booking
                {
                    BookingID = Convert.ToInt32(reader["BookingID"]),
                    RoomID = Convert.ToInt32(reader["RoomID"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    CheckInDate = Convert.ToDateTime(reader["CheckInDate"]),
                    CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]),
                    Status = reader["Status"].ToString()
                });
            }
        }

        return View(bookings);
    }

    // ----------------- EDIT ------------------
    public ActionResult Edit(int id)
    {
        Booking booking = null;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM Bookings WHERE BookingID=@BookingID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@BookingID", id);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                booking = new Booking
                {
                    BookingID = Convert.ToInt32(reader["BookingID"]),
                    RoomID = Convert.ToInt32(reader["RoomID"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    CheckInDate = Convert.ToDateTime(reader["CheckInDate"]),
                    CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]),
                    Status = reader["Status"].ToString()
                };
            }
        }

        return View(booking);
    }

    [HttpPost]
    public ActionResult Edit(Booking booking)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"UPDATE Bookings 
                             SET Name=@Name, Email=@Email, Phone=@Phone, CheckInDate=@CheckInDate, CheckOutDate=@CheckOutDate, Status=@Status
                             WHERE BookingID=@BookingID";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Name", booking.Name);
            cmd.Parameters.AddWithValue("@Email", booking.Email);
            cmd.Parameters.AddWithValue("@Phone", booking.Phone);
            cmd.Parameters.AddWithValue("@CheckInDate", booking.CheckInDate);
            cmd.Parameters.AddWithValue("@CheckOutDate", booking.CheckOutDate);
            cmd.Parameters.AddWithValue("@Status", booking.Status);
            cmd.Parameters.AddWithValue("@BookingID", booking.BookingID);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("MyBookings");
    }

    // ----------------- DELETE ------------------
    public ActionResult Delete(int id)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Bookings WHERE BookingID=@BookingID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@BookingID", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("MyBookings");
    }
}
