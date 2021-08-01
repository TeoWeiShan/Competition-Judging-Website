﻿using Microsoft.AspNetCore.Http;
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
    public class CompetitorController : Controller
    {
        private CompetitorDAL competitorContext = new CompetitorDAL();
        // GET: CompetitorController
        public ActionResult Index()
        {
            string loginID = HttpContext.Session.GetString("LoginID");
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            Competitor competitor = competitorContext.GetDetails(Convert.ToInt32(loginID));
            return View(competitor);
        }

        // GET: CompetitorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetitorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetitorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                competitor.CompetitorID = competitorContext.Add(competitor);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(competitor);
            }
        }

        // GET: CompetitorController/Edit/5
        public ActionResult Edit(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("LoginID") != id.ToString()) ||
            (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index", "Home");
            }
            Competitor competitor = competitorContext.GetDetails(id.Value);
            if (competitor == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index", "Home");
            }
            return View(competitor);
        }

        // POST: CompetitorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                //Update competitor record to database
                competitorContext.Update(competitor);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(competitor);
            }
        }

        // GET: CompetitorController/Delete/5
        public ActionResult Delete(int? id)
        {
            // Stop accessing the action if not logged in or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Competitor competitor = competitorContext.GetDetails(id.Value);
            if (competitor == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(competitor);
        }

        // POST: CompetitorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Competitor competitor)
        {
            // Delete the interest record from database
            competitorContext.Delete(competitor.CompetitorID);
            return RedirectToAction("Index");
        }
    }
}
