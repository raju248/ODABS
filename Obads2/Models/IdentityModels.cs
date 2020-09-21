using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Obads2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime dateOfBirth { get; set; }

        public string Gender { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [JsonIgnore]
        public virtual Admin admin { get; set; }
        [JsonIgnore]
        public virtual Doctor doctor { get; set; }
        [JsonIgnore]
        public virtual Patient patient { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasOptional(ad => ad.admin).WithRequired(u => u.User);
            modelBuilder.Entity<ApplicationUser>().HasOptional(ad => ad.doctor).WithRequired(u => u.User);
            modelBuilder.Entity<ApplicationUser>().HasOptional(ad => ad.patient).WithRequired(u => u.User);

            modelBuilder.Entity<Doctor>().HasRequired(d => d.room).WithRequiredPrincipal(r => r.doctor);
            modelBuilder.Entity<Appointment>().HasRequired(a => a.payment).WithRequiredPrincipal(p => p.appointment);
            modelBuilder.Entity<Appointment>().HasRequired(a => a.prescription).WithRequiredPrincipal(p => p.appointment);
            modelBuilder.Entity<Room>().HasKey(t => t.Id);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

    }
}