using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;
using WEB2021Apr_P04_T4.Models;

namespace WEB2021Apr_P04_T4.Controllers
{
    public class AreaInterestController : Controller
    {
        private AreaInterestDAL areaInterestContext = new AreaInterestDAL();
        // GET: AreaInterestController
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<AreaInterest> areaInterestList = areaInterestContext.GetAllAreaInterest();
            return View(areaInterestList);
        }

        // GET: AreaInterestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AreaInterestController/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: AreaInterestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaInterest areaInterest)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                areaInterest.AreaInterestID = areaInterestContext.Add(areaInterest);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(areaInterest);
            }
        }

        // GET: AreaInterestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AreaInterestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AreaInterestController/Delete/5
        public ActionResult Delete(int? id)
        {
            // Stop accessing the action if not logged in or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            AreaInterest areaInterest = areaInterestContext.GetDetails(id.Value);
            if (areaInterest == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(areaInterest);
        }

        // POST: AreaInterestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AreaInterest areaInterest)
        {
            // Delete the interest record from database
            areaInterestContext.Delete(areaInterest.AreaInterestID);
            return RedirectToAction("Index");
        }
    }
}
