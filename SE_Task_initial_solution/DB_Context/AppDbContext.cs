using Microsoft.EntityFrameworkCore;
using SE_Task_initial_solution.Models;
using System.Collections.Generic;

namespace SE_Task_initial_solution.DB_Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
