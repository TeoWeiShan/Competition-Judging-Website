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
    public class CriteriaController : Controller
    {
        private CriteriaDAL criteriaContext = new CriteriaDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private CompetitionJudgeDAL competitionjudgecontext = new CompetitionJudgeDAL();

        //GET: CriteriaController
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            string loginID = HttpContext.Session.GetString("LoginID");
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Criteria> criteriaList = criteriaContext.GetAllCriteria(Convert.ToInt32(loginID));
            return View(criteriaList);
        }

        private List<Competition> GetAllCompetition()
        {
            // Get a list of branches from database
            List<Competition> competitionList = competitionContext.GetAllCompetition();
            Console.WriteLine(competitionList);
            // Adding a select prompt at the first row of the branch list
            competitionList.Insert(0, new Competition
            {
                CompetitionID = 0,
                CompetitionName = "--Select--"
            });
            Console.WriteLine(competitionList);
            return competitionList;
        }

        private List<Competition> GetAvailableCompetition(int JudgeID)
        {
            // Get a list of branches from database
            List<Competition> competitionList = criteriaContext.GetAvailableCompetition(JudgeID);
            Console.WriteLine(competitionList);
            // Adding a select prompt at the first row of the branch list
            competitionList.Insert(0, new Competition
            {
                CompetitionID = 0,
                CompetitionName = "--Select--"
            });
            Console.WriteLine(competitionList);
            return competitionList;
        }

        // GET: CriteriaController/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Criteria" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            string loginID = HttpContext.Session.GetString("LoginID");
            ViewData["competitionList"] = GetAvailableCompetition(Convert.ToInt32(loginID));
            return View();
        }

        // POST: CriteriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Criteria criteria)
        {
            //string loginID = HttpContext.Session.GetString("LoginID");
            //ViewData["CompetitionList"] = GetAvailableCompetition(Convert.ToInt32(loginID));
            if (ModelState.IsValid)
            {
                //Add criteria record to database
                criteria.CriteriaID = criteriaContext.Add(criteria);
                //Redirect user to Criteria/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(criteria);
            }
        }

        // GET: CriteriaController/Edit/5
        public ActionResult Edit(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            
            Criteria criteria = criteriaContext.GetDetails(id.Value);
            Competition competition = competitionContext.GetDetails(criteria.CompetitionID);
            ViewData["CompetitionName"] = competition.CompetitionName;

            if (criteria == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(criteria);
        }

        // POST: CriteriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Criteria criteria)
        {
            ViewData["CompetitionList"] = GetAllCompetition();
            if (ModelState.IsValid)
            {
                //Update criteria record to database
                criteriaContext.Update(criteria);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(criteria);
            }
        }

        // GET: CriteriaController/Delete/5
        public ActionResult Delete(int? id)
        {
            // Stop accessing the action if not logged in or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Criteria criteria = criteriaContext.GetDetails(id.Value);
            if (criteria == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(criteria);
        }

        // POST: CriteriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Criteria criteria)
        {
            // Delete the interest record from database
            criteriaContext.Delete(criteria.CriteriaID);
            return RedirectToAction("Index");
        }
    }
}
