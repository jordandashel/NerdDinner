using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class DinnersController : Controller
    {
        //
        // GET: /Dinners/

        private DinnersDbContext db = new DinnersDbContext();


        public ActionResult Index()
        {
            var dinners = db.Dinners.ToList();
            return View(dinners);
        }


        public ActionResult Details(int id)
        {
            Dinner dinner = db.Dinners.Find(id);
            //Dinner dinner = dinnersDb.Dinners.Find(id);
            return dinner == null ? View("NotFound") : View("Details", dinner);
        }

        public ActionResult Edit(int id)
        {
            Dinner dinner = db.Dinners.Find(id);

            var countries = new List<string>();
            countries.Add("America");
            countries.Add("Australia");
            countries.Add("Austria");

            ViewData["Countries"] = new SelectList(countries);

            return View(dinner);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection formValues)
        {
            // Retrieve existing dinner
            Dinner dinner = db.Dinners.Find(id);

            UpdateModel(dinner);

            // Persist changes back to database
            db.SaveChanges();

            // Perform HTTP redirect to details page for the saved Dinner
            return RedirectToAction("Details", new { id = dinner.DinnerId });
        }

        public ActionResult Create()
        {
            Dinner dinner = new Dinner()
            {
                EventDate = DateTime.Now.AddDays(7)
            };

            return View(dinner);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Dinner dinner)
        {
            dinner.HostedBy = "some guy";
            if (ModelState.IsValid)
            {
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
            {
                return View("NotFound");
            }
            return View(dinner);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, string confirmButton)
        {
            Dinner dinner = db.Dinners.Find(id);

            if (dinner == null)
                return View("NotFound");

            db.Dinners.Remove(dinner);
            db.SaveChanges();

            return View("Deleted");
        }
    }
}
