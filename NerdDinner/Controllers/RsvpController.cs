using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class RsvpController : Controller
    {
        DinnersDbContext db = new DinnersDbContext();

        [HttpPost]
        public void Register(int id)
        {
            Dinner dinner = db.Dinners.Find(id);

            if (!dinner.IsUserRegistered(User.Identity.Name))
            {
                Rsvp rsvp = new Rsvp();
                rsvp.AttendeeName = User.Identity.Name;

                dinner.Rsvps.Add(rsvp);
                db.SaveChanges();
            }
        }

    }
}
