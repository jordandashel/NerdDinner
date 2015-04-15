using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CodeFirstStoreFunctions;

namespace NerdDinner.Models
{
    public class DinnerRepository : IDinnerRepository
    {
        private DinnersDbContext db = new DinnersDbContext();

        //
        // Query Methods

        public IQueryable<Dinner> FindAllDinners()
        {
            return db.Dinners;
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            return from dinner in db.Dinners
                   //where dinner.EventDate > DateTime.Now
                   orderby dinner.EventDate
                   select dinner;
        }

        public Dinner GetDinner(int id)
        {
            return db.Dinners.SingleOrDefault(d => d.DinnerId == id);
        }

        //
        // Insert/Delete Methods

        public void Add(Dinner dinner)
        {
            db.Dinners.Add(dinner);
            Save();
        }

        public void Delete(Dinner dinner)
        {
            db.Dinners.Remove(dinner);
            Save();
        }

        //
        // Persistence

        public void Save()
        {
            db.SaveChanges();
        }

        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            var upcomingDinners = from dinner in db.Dinners
                                  //  where dinner.EventDate > DateTime.Now
                                  orderby dinner.EventDate
                                  select dinner;

            var dinners = from dinner in upcomingDinners
                          join i in db.NearestDinners(latitude, longitude) on dinner.DinnerId equals i.DinnerId
                          select dinner;

            return dinners;
        } 
    }
}