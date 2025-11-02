using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assisment_Ahmed_Alaa.Models
{
    class AppoimentHandlingDB : DbContext
    {
        public AppoimentHandlingDB() {}
        public AppoimentHandlingDB(DbContextOptions<AppoimentHandlingDB> options) : base (options) {}


        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<User> User { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=COM184-LAB3\\SQLEXPRESS;Initial Catalog=AppoimentHandling;Integrated Security=True;Trust Server Certificate=True");
            }
        }

    }
}
