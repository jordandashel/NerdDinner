using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using CodeFirstStoreFunctions;

namespace NerdDinner.Models
{
    public class DinnersDbContext : DbContext
    {
        public DbSet<Dinner> Dinners { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Add(new FunctionsConvention<DinnersDbContext>("dbo"));
        }

        [DbFunction("CodeFirstDatabaseSchema", "DistanceBetween")]
        public static double DistanceBetween(double lat1, double long1, double lat2, double long2)
        {
            throw new NotImplementedException("Only call through LINQ expression");
        }

        public IQueryable<Dinner> NearestDinners(double latitude, double longitude)
        {
            return from d in Dinners
                   where DistanceBetween(latitude, longitude, d.Latitude, d.Longitude) < 100
                   select d;
        }
        
        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            var upcomingDinners = from dinner in Dinners
                //  where dinner.EventDate > DateTime.Now
                orderby dinner.EventDate
                select dinner;

            var dinners = from dinner in upcomingDinners
                join i in NearestDinners(latitude, longitude) on dinner.DinnerId equals i.DinnerId
                select dinner;

            return dinners;
        } 
    }
}