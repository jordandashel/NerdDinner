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

        private IDinnerRepository dinnerRepository;

        public DinnersController()
        {
            dinnerRepository = new DinnerRepository();
        }

        public DinnersController(IDinnerRepository repository)
        {
            dinnerRepository = repository;
        }

        public ActionResult Index(int? page)
        {
            const int pageSize = 10;

            IQueryable<Dinner> upcomingDinners = dinnerRepository.FindUpcomingDinners();

            var paginatedDinners = new PaginatedList<Dinner>(upcomingDinners, page ?? 0, pageSize);

            return View(paginatedDinners);
        }


        public ActionResult Details(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
            
            return dinner == null ? View("NotFound") : View("Details", dinner);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);


            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");

            var countries = new List<string>();
            countries.Add("America");
            countries.Add("Australia");
            countries.Add("Austria");

            ViewData["Countries"] = new SelectList(countries);

            return View(new DinnerFormViewModel(dinner));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection formValues)
        {
            // Retrieve existing dinner
            Dinner dinner = dinnerRepository.GetDinner(id);

            if (!dinner.IsHostedBy(User.Identity.Name))
            {
                return View("InvalidOwner");
            }
            UpdateModel(dinner);

            // Persist changes back to database
            dinnerRepository.Save();

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

            return View(new DinnerFormViewModel(dinner));
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

                dinnerRepository.Add(dinner);
                dinnerRepository.Save();
                return RedirectToAction("Index");
            }

            return View(new DinnerFormViewModel(dinner));
        }

        public ActionResult Delete(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);

            if (dinner == null)
                return View("NotFound");

            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");
            

            return View(dinner);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, string confirmButton)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
   
            if (dinner == null)
                return View("NotFound");

            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");
            
            dinnerRepository.Delete(dinner);
            dinnerRepository.Save();

            return View("Deleted");
        }
    }
}
