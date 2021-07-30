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
            // Stop accessing the action if not logged in or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }//Retrieve all available areaInterest from DB
            List<AreaInterest> areaInterestList = areaInterestContext.GetAllAreaInterest();
            //Pass all available areaInterest to Views
            return View(areaInterestList);
        }

        // GET: AreaInterestController/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
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
                //Add areaInterest record to database
                areaInterest.AreaInterestID = areaInterestContext.Add(areaInterest);
                //Redirect user to AreaInterest/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view to display error message
                return View(areaInterest);
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
            // Stop accessing the action if requesting id is not valid
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            AreaInterest areaInterest = areaInterestContext.GetDetails(id.Value);
            //If interest has competition record, redirect to error and prevent deletion
            if (areaInterestContext.IsDeletable(id.Value))
            {
                TempData["ErrorMsg"] = "Sorry! A competition record has been made!";
                return RedirectToAction("DisplayError", "Home");
            }
            // Stop accessing the action if requesting id is not in database
            if (areaInterest.Name == null)
            {
                return RedirectToAction("Index");
            }
            return View(areaInterest);
        }

        // POST: AreaInterestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AreaInterest areaInterest)
        {
            if (ModelState.IsValid)
            {
                // Delete the interest record from database
                areaInterestContext.Delete(areaInterest.AreaInterestID);
                //Redirect user to AreaInterest/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Delete view to display error message
                return View(areaInterest);
            }
            
        }
    }
}
