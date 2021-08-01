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
    public class CompetitionSubmissionController : Controller
    {
        private CompetitionSubmissionDAL competitionSubmissionContext = new CompetitionSubmissionDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private CompetitionScoreDAL scoreContext = new CompetitionScoreDAL();
        private CompetitionJudgeDAL competitionjudgecontext = new CompetitionJudgeDAL();

        // GET: CompetitionSubmissionController
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
            List<CompetitionSubmission> submissionList = competitionSubmissionContext.GetAllSubmission(Convert.ToInt32(loginID));
            return View(submissionList);
        }

        //Calculate Total Score
        //public ActionResult Calculate(CompetitionSubmission totalscore)
        //{
        //    return View();
        //}

        //private List<CompetitionSubmission> GetAvailableSubmissions(int CompetitorID)
        //{
        //    // Get a list of branches from database
        //    List<CompetitionSubmission> submissionList = competitionSubmissionContext.GetAvailableSubmissions(CompetitorID);
        //    Console.WriteLine(submissionList);
        //    // Adding a select prompt at the first row of the branch list
        //    submissionList.Insert(0, new CompetitionSubmission
        //    {
        //        CompetitorID = 0
        //    });
        //    Console.WriteLine(submissionList);
        //    return submissionList;
        //}

        // GET: CompetitionSubmissionController/Details/5
        public ActionResult Details(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }

            CompetitionSubmission submission = competitionSubmissionContext.GetDetails(id);
            CompetitionSubmissionViewModel submissionVM = MapToSubmissionVM(submission);
            return View(submissionVM);
        }

        public CompetitionSubmissionViewModel MapToSubmissionVM(CompetitionSubmission submission)
        {
            string loginID = HttpContext.Session.GetString("LoginID");
            int criteriaID = 0;
            int criteriaScore = 0;
            if (submission.CompetitionID != null)
            {
                List<CompetitionScore> scoreList = scoreContext.GetAllScore(Convert.ToInt32(loginID));
                foreach (CompetitionScore score in scoreList)
                {
                    if (score.CompetitionID == submission.CompetitionID)
                    {
                        criteriaID = score.CriteriaID;
                        criteriaScore = score.Score;
                        break;
                    }
                }
            }

            CompetitionSubmissionViewModel submissionVM = new CompetitionSubmissionViewModel
            {
                CompetitionID = submission.CompetitionID,
                CompetitorID = submission.CompetitorID,
                FileSubmitted = submission.FileSubmitted,
                DateTimeFileUpload = submission.DateTimeFileUpload,
                Appeal = submission.Appeal,
                Ranking = submission.Ranking,
                TotalScore = criteriaScore,
            };

            return submissionVM;
        }

        // GET: CompetitionSubmissionController/Edit/5
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
            {
                //Query string parameter not provided
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            CompetitionSubmission submission = competitionSubmissionContext.GetDetails(id.Value);
            if (submission == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(submission);
        }

        // POST: CompetitionSubmissionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompetitionSubmission submission)
        {
            if (ModelState.IsValid)
            {
                //Update submission ranking record to database
                competitionSubmissionContext.Update(submission);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(submission);
            }
        }
    }
}
