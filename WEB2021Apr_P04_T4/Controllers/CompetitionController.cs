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
            //If not admin, cannot access
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            //Get all competition from DB
            List<Competition> competitionList = competitionContext.GetAllCompetition();
            return View(competitionList);
        }

        // GET: CompetitionController/Details/5
        public ActionResult Details(int id)
        {
            //If not admin, cannot access
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            //Get details of competition
            Competition competition = competitionContext.GetDetails(id);
            //If query id not in db, redirect out
            if (competition.CompetitionName == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            CompetitionViewModel competitionVM = MapToCompetitionVM(competition);
            return View(competitionVM);
        }

        public CompetitionViewModel MapToCompetitionVM(Competition competition)
        {
            string interestName = "";
            
            List<AreaInterest> interestList = areaInterestContext.GetAllAreaInterest();
            foreach (AreaInterest interest in interestList)
            {
                if (interest.AreaInterestID == competition.AreaInterestID)
                {
                    interestName = interest.Name;
                    //Exit the foreach loop once the name is found
                    break;
                }
            }
            
            CompetitionViewModel competitionVM = new CompetitionViewModel
            {
                CompetitionID = competition.CompetitionID,
                Name = interestName,
                CompetitionName = competition.CompetitionName,
                StartDate = competition.StartDate,
                EndDate = competition.EndDate,
                ResultReleasedDate = competition.ResultReleasedDate
            };
            return competitionVM;
        }

        private List<AreaInterest> GetAllAreaInterests()
        {
            // Get a list of interest from database
            List<AreaInterest> areaInterestList = areaInterestContext.GetAllAreaInterest();
            Console.WriteLine(areaInterestList);
            // Adding a select prompt at the first row of the list
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
            //If not admin, cannot access
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            //Populate dropdown list
            ViewData["InterestList"] = GetAllAreaInterests();
            return View();
        }

        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competition competition)
        {
            //Populate list for drop-down list
            //in case of the need to return to Edit.cshtml view
            ViewData["InterestList"] = GetAllAreaInterests();
            if (ModelState.IsValid)
            {
                //Update record to database
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
        public ActionResult Edit(int id)
        {
            //If not admin, cannot access
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            ViewData["InterestList"] = GetAllAreaInterests();
            Competition competition = competitionContext.GetDetails(id);
            //Check against functional requirement criteria and redirect to error if needed
            if (competitionContext.IsModifiable(id))
            {
                TempData["ErrorMsg"] = "Sorry! A competitor has joined and cannot be modified";
                if (competition.EndDate < DateTime.Today)
                {
                   
                    TempData["ErrorMsg"] = "Sorry! The competition has ended and cannot be modified";
                    return RedirectToAction("DisplayError", "Home" );
                }
               
                return RedirectToAction("DisplayError", "Home");
            }
            if (competition.CompetitionName == null)
            {
                //Invalid query string parameter 
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(competition);
        }

        // POST: CompetitionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Competition competition)
        {
            ViewData["InterestList"] = GetAllAreaInterests();

            if (ModelState.IsValid)
            {
                //Update record to database
                competitionContext.Update(competition);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(competition);
            }
        }

        // GET: CompetitionController/Delete/5
        public ActionResult Delete(int id)
        {
            // Stop accessing the action if not logged in or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Query string parameter not provided
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Competition competition= competitionContext.GetDetails(id);
            //Check against functional requirement criteria and redirect to error if needed
            if (competitionContext.IsModifiable(id))
            {
                TempData["ErrorMsg"] = "Sorry! A competitor has joined and cannot be modified";
                if (competition.EndDate < DateTime.Today)
                {

                    TempData["ErrorMsg"] = "Sorry! The competition has ended and cannot be modified";
                    return RedirectToAction("DisplayError", "Home");
                }

                return RedirectToAction("DisplayError", "Home");
            }
            if (competition.CompetitionName == null)
            {
                //Invalid query string parameter 
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
            ModelState.Remove("AreaInterestID");
            if (ModelState.IsValid)
            {
                // Delete the record from database
                competitionContext.Delete(competition.CompetitionID);
                //Redirect user 
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Delete view
                //to display error message
                return View(competition);
            }
        }
    }
}
