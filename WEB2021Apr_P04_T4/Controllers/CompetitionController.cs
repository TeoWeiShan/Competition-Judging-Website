using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;
using WEB2021Apr_P04_T4.Models;

namespace WEB2021Apr_P04_T4.Controllers
{
    public class CompetitionController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private AreaInterestDAL areaInterestContext = new AreaInterestDAL();

        // GET: CompetitionController
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Competition> competitionList = competitionContext.GetAllCompetition();
            return View(competitionList);
        }

        // GET: CompetitionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        private List<AreaInterest> GetAllAreaInterests()
        {
            // Get a list of branches from database
            List<AreaInterest> areaInterestList = areaInterestContext.GetAllAreaInterest();
            Console.WriteLine(areaInterestList);
            // Adding a select prompt at the first row of the branch list
            areaInterestList.Insert(0, new AreaInterest
            {
                AreaInterestID = 0,
                Name = "--Select--"
            });
            Console.WriteLine(areaInterestList);
            return areaInterestList;
        }

        // GET: CompetitionController/Create
        public ActionResult Create()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["InterestList"] = GetAllAreaInterests();
            return View();
        }

        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competition competition)
        {
            //Get branch list for drop-down list
            //in case of the need to return to Edit.cshtml view
            ViewData["InterestList"] = GetAllAreaInterests();
            if (ModelState.IsValid)
            {
                //Update staff record to database
                competition.CompetitionID= competitionContext.Add(competition);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(competition);
            }
        }

        
        // GET: CompetitionController/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            ViewData["AreaInterestList"] = GetAllAreaInterests();
            Competition competition = competitionContext.GetDetails(id.Value);
            if (competition == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // POST: CompetitionController/Edit/5
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

        // GET: CompetitionController/Delete/5
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
            Competition competition= competitionContext.GetDetails(id.Value);
            if (competition == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // POST: CompetitionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Competition competition)
        {
            if (ModelState.IsValid)
            {
                // Delete the interest record from database
                competitionContext.Delete(competition.CompetitionID);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(competition);
            }
        }
    }
}
