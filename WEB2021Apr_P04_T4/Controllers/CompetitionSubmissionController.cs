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

        // GET: CompetitionSubmissionController
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Criteria" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<CompetitionSubmission> submissionList = competitionSubmissionContext.GetAllSubmission();
            return View(submissionList);
        }

        //private List<CompetitionSubmission> GetAllSubmission()
        //{
        //    // Get a list of branches from database
        //    List<CompetitionSubmission> submissionList = competitionSubmissionContext.GetAllSubmission();
        //    Console.WriteLine(submissionList);
        //    return submissionList;
        //}

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

            // CompetitionSubmission competitionSubmission = competitionSubmissionContext.GetDetails(id);
            // CompetitionSubmissionViewModel submissionVM = MapToSubmissionVM(competitionSubmission);
            // return View(submissionVM);
            return View();
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
        public ActionResult Edit(int id)
        {
            return View();
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
