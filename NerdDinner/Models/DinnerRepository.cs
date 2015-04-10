using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class DinnerRepository
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
                   where dinner.EventDate > DateTime.Now
                   orderby dinner.EventDate
                   select dinner;
        }

        public Dinner GetDinner(int id)
        {
            var result = db.Dinners.SingleOrDefault(d => d.DinnerId == id);
           return result;
        }

        //
        // Insert/Delete Methods

        public void Add(Dinner dinner)
        {
            db.Dinners.Add(dinner);
            db.SaveChanges();
            //db.Dinners.InsertOnSubmit(dinner);
        }

        public void Delete(Dinner dinner)
        {           
            dinner.Rsvps.Clear();
            db.Dinners.Remove(dinner);
            db.SaveChanges();

//            db.Rsvps.DeleteAllOnSubmit(dinner.RSVPs);
//            db.Dinners.DeleteOnSubmit(dinner);
        }

        //
        // Persistence

        public void Save()
        {
            db.SaveChanges();
            //db.SubmitChanges();
        }
    }
}