namespace SE_Task_initial_solution.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SE_Task_initial_solution.DB_Context;
    using SE_Task_initial_solution.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("rooms/{branchId}")]
        public ActionResult<IEnumerable<Room>> GetAvailableRooms(int branchId, [FromQuery] DateTime checkInDate, [FromQuery] DateTime checkOutDate)
        {
            var availableRooms = _context.Rooms
                .Where(r => r.BranchId == branchId && r.IsAvailable)
                .Where(r => !_context.Bookings
                    .Any(b => b.RoomId == r.RoomId &&
                              (checkInDate >= b.CheckInDate && checkInDate < b.CheckOutDate ||
                               checkOutDate > b.CheckInDate && checkOutDate <= b.CheckOutDate ||
                               checkInDate <= b.CheckInDate && checkOutDate >= b.CheckOutDate)))
                .ToList();

            return Ok(availableRooms);
        }

        [HttpPost("bookings")]
        public ActionResult<int> CreateBooking([FromBody] Booking booking)
        {
            booking.TotalCost = CalculateTotalCost(booking);

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Update room availability
            var room = _context.Rooms.Find(booking.RoomId);
            room.IsAvailable = false;
            _context.SaveChanges();

            return Ok(booking.BookingId);
        }

        [HttpDelete("bookings/{bookingId}")]
        public ActionResult CancelBooking(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
                return NotFound();

            // Update room availability
            var room = _context.Rooms.Find(booking.RoomId);
            room.IsAvailable = true;

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return Ok(new { message = "Booking canceled successfully" });
        }

        [HttpPut("bookings/{bookingId}")]
        public ActionResult UpdateBooking(int bookingId, [FromBody] Booking updatedBooking)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
                return NotFound();

            updatedBooking.TotalCost = CalculateTotalCost(updatedBooking);

            // Update room availability
            var room = _context.Rooms.Find(updatedBooking.RoomId);
            room.IsAvailable = false;

            booking.CheckInDate = updatedBooking.CheckInDate;
            booking.CheckOutDate = updatedBooking.CheckOutDate;
            booking.TotalCost = updatedBooking.TotalCost;

            _context.SaveChanges();

            return Ok(new { message = "Booking updated successfully" });
        }

        [HttpGet("report")]
        public ActionResult<IEnumerable<object>> GetReport()
        {
            var report = _context.Rooms
                .Select(r => new
                {
                    RoomNumber = r.RoomNumber,
                    RoomType = r.RoomType,
                    Status = _context.Bookings.Any(b => b.RoomId == r.RoomId) ? "Booked" : "Available"
                })
                .ToList();

            return Ok(report);
        }

        private decimal CalculateTotalCost(Booking booking)
        {
            
            return 0;
        }
    }
}
