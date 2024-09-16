using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelLinkerModels.Models;

namespace TravelLinkerDataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Hotel db sets 
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room>  Rooms { get; set; }
        public DbSet<RoomOffer> RoomsOffers { get; set; }
        // Company db sets
        public DbSet<Company> Companies { get; set; }
        public DbSet<VehicleSchedule> VehicleSchedules { get; set; }    

        public DbSet<RoomSchedule> RoomSchedules { get; set; }      
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Trip>  Trips { get; set; }

        public DbSet<Comment> Comments { get; set; }    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>().ToTable("Users");

            
            builder.Entity<Hotel>().ToTable("Hotels", schema: "Hsc");
            builder.Entity<Room>().ToTable("Rooms", schema: "Hsc");
            builder.Entity<RoomOffer>().ToTable("RoomsOffers", schema: "Hsc");
            builder.Entity<RoomImage>().ToTable("RoomsImages", schema: "Hsc");
            builder.Entity<RoomFeature>().ToTable("RoosmFeatures", schema: "Hsc");
            builder.Entity<RoomSchedule>().ToTable("RoomSchedules", schema: "Hsc");
            builder.Entity<RoomTransaction>()
                .ToTable("RoomTransaction", schema: "Hsc").HasOne(rt => rt.Schedule)
                .WithOne();

            builder.Entity<RoomFeature>().HasKey(x => new { x.RoomId, x.FeatureName });
            builder.Entity<Room>().HasMany(r => r.RoomImages)
                .WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
            builder.Entity<Room>().HasMany(r => r.RoomFeatures)
                .WithOne(x => x.Room).HasForeignKey(x => x.RoomId);

            builder.Entity<Company>().ToTable("Companies", schema: "Csc");
     
            //Vehicles
            builder.Entity<Vehicle>().ToTable("Vehicles", schema: "Csc");
            builder.Entity<Vehicle>().HasIndex(v=>v.LicenseNumber).IsUnique();
            builder.Entity<VehicleFeature>().ToTable("VehicleFeatures", schema: "Csc");
            builder.Entity<VehicleFeature>().HasKey(x => new { x.VehicleId, x.FeatureName });

            builder.Entity<TripTransaction>().ToTable("TripTransaction", schema: "Csc");

            // trip Trips
            builder.Entity<Trip>().ToTable("Trips", schema: "Csc");
            builder.Entity<Trip>().HasIndex(t => t.Code).IsUnique();
            builder.Entity<Trip>().HasOne(T=>T.VehicleSchedule).WithOne(VS=>VS.Trip).OnDelete(DeleteBehavior.NoAction);  
            // Schedules and Vehicles  
            builder.Entity<VehicleSchedule>().ToTable("VehicleSchedules", schema: "Csc");

            builder.Entity<Comment>().HasOne(c=>c.From).WithMany().
                HasForeignKey(c=>c.FromId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Comment>().HasOne(c => c.To).WithMany().
                HasForeignKey(c => c.ToId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Rate>().ToTable("Rates");
        }
    }


}
