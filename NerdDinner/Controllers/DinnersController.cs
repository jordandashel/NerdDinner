using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using NerdDinner.Helpers;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class DinnersController : Controller
    {
        //
        // GET: /Dinners/

        private DinnersDbContext db = new DinnersDbContext();


        public ActionResult Index(int? page)
        {
            const int pageSize = 10;

            IQueryable<Dinner> upcomingDinners = from dinner in db.Dinners
                                            //  where dinner.EventDate > DateTime.Now
                                                orderby dinner.EventDate
                                                select dinner;

/*
            var paginatedDinners = upcomingDinners.Skip((page ?? 0) * pageSize)
                .Take(20)
                .ToList();
*/

            var paginatedDinners = new PaginatedList<Dinner>(upcomingDinners, page ?? 0, pageSize);

            return View(paginatedDinners);
        }


        public ActionResult Details(int id)
        {
            Dinner dinner = db.Dinners.Find(id);
            
            return dinner == null ? View("NotFound") : View("Details", dinner);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Dinner dinner = db.Dinners.Find(id);


            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");

            var countries = new List<string>();
            countries.Add("America");
            countries.Add("Australia");
            countries.Add("Austria");

            ViewData["Countries"] = new SelectList(countries);

            return View(dinner);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection formValues)
        {
            // Retrieve existing dinner
            Dinner dinner = db.Dinners.Find(id);

            if (!dinner.IsHostedBy(User.Identity.Name))
            {
                return View("InvalidOwner");
            }
            UpdateModel(dinner);

            // Persist changes back to database
            db.SaveChanges();

            // Perform HTTP redirect to details page for the saved Dinner
            return RedirectToAction("Details", new { id = dinner.DinnerId });
        }

        [Authorize]
        public ActionResult Create()
        {

            var countries = new List<string>();
            countries.Add("America");
            countries.Add("Australia");
            countries.Add("Austria");

            ViewData["Countries"] = new SelectList(countries);

            Dinner dinner = new Dinner()
            {
                EventDate = DateTime.Now.AddDays(7)
            };

            return View(dinner);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        public ActionResult Create(Dinner dinner)
        {
            if (ModelState.IsValid)
            {
                
                dinner.HostedBy = User.Identity.Name;

                Rsvp rsvp = new Rsvp();
                rsvp.AttendeeName = User.Identity.Name;
                dinner.Rsvps.Add(rsvp);

                db.Dinners.Add(dinner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dinner);
        }

        public ActionResult Delete(int id)
        {
            Dinner dinner = db.Dinners.Find(id);

            if (dinner == null)
                return View("NotFound");

            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");
            

            return View(dinner);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, string confirmButton)
        {
            Dinner dinner = db.Dinners.Find(id);
   
            if (dinner == null)
                return View("NotFound");

            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");
            
            db.Dinners.Remove(dinner);
            db.SaveChanges();

            return View("Deleted");
        }
    }
}
