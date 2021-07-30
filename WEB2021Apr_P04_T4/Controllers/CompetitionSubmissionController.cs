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

        // GET: CompetitionSubmissionController/Details/5
        public ActionResult Details(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Criteria" role
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
            int criteriaID = 0;
            int criteriaScore = 0;
            if (submission.CompetitionID != null)
            {
                List<CompetitionScore> scoreList = scoreContext.GetAllScore();
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
                //CriteriaName = criteriaName,
                CriteriaID = criteriaID,
                Score = criteriaScore,
            };

            return submissionVM;
    }

    // GET: CompetitionSubmissionController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: CompetitionSubmissionController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
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
            { //Query string parameter not provided
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

    // GET: CompetitionSubmissionController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: CompetitionSubmissionController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
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
}
}
