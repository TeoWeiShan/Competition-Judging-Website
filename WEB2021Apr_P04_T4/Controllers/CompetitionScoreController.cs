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
    public class CompetitionScoreController : Controller
    {
        private CompetitionScoreDAL scoreContext = new CompetitionScoreDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private CriteriaDAL criteriaContext = new CriteriaDAL();
        private CompetitionSubmissionDAL submissionContext = new CompetitionSubmissionDAL();

        // GET: CompetitionScore
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
            List<CompetitionScore> scoreList = scoreContext.GetAllScore(Convert.ToInt32(loginID));
            return View(scoreList);
        }

        private List<Competition> GetAvailableCompetition(int JudgeID)
        {
            // Get a list of competitions from database
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

        private List<Criteria> GetAvailableCriteria(int JudgeID)
        {
            // Get a list of criterias based on selected competitionID from database
            List<Criteria> criteriaList = criteriaContext.GetAvailableCriteria(JudgeID);
            Console.WriteLine(criteriaList);
            // Adding a select prompt at the first row of the criteria list
            criteriaList.Insert(0, new Criteria
            {
                CriteriaID = 0,
                CriteriaName = "--Select--"
            });
            Console.WriteLine(criteriaList);
            return criteriaList;
        }

        private List<CompetitionSubmission> GetAvailableSubmissions(int CompetitorID)
        {
            // Get a list of branches from database
            List<CompetitionSubmission> submissionList = submissionContext.GetAvailableSubmissions(CompetitorID);
            Console.WriteLine(submissionList);
            // Adding a select prompt at the first row of the branch list
            submissionList.Insert(0, new CompetitionSubmission
            {
                CompetitorID = 0
            });
            Console.WriteLine(submissionList);
            return submissionList;
        }

        // GET: CompetitionScore/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetitionScore/Create
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
            ViewData["scoreList"] = GetAllScore(Convert.ToInt32(loginID));
            ViewData["competitionList"] = GetAvailableCompetition(Convert.ToInt32(loginID));
            ViewData["CriteriaList"] = GetAvailableCriteria(Convert.ToInt32(loginID));
            ViewData["submissionList"] = GetAvailableSubmissions(Convert.ToInt32(loginID));
            return View();
        }

        private List<CompetitionScore> GetAllScore(int JudgeID)
        {
            // Get a list of branches from database
            List<CompetitionScore> scoreList = scoreContext.GetAllScore(JudgeID);
            Console.WriteLine(scoreList);
            return scoreList;
        }

        // POST: CompetitionScore/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompetitionScore Cscore)
        {
            if (ModelState.IsValid)
            {
                //Add criteria record to database
                Cscore.CompetitorID = scoreContext.Add(Cscore);
                //Redirect user to Criteria/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(Cscore);
            }
        }

        // GET: CompetitionScore/Edit/5
        public ActionResult Edit(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Criteria" role
            string loginID = HttpContext.Session.GetString("LoginID");
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            CompetitionScore cscore = scoreContext.GetDetails(id.Value);
            if (cscore == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(cscore);
        }

        // POST: CompetitionScore/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompetitionScore cscore)
        {
            if (ModelState.IsValid)
            {
                //Update submission score record to database
                scoreContext.Update(cscore);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(cscore);
            }
        }
    }
}
