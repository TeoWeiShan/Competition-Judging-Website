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
    public class CompetitionJudgeController : Controller
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();
        // GET: CompetitionJudgeController
        public ActionResult Index(int? id)
        {
            CompetitionJudgeViewModel competitionJudgeVM = new CompetitionJudgeViewModel();
            competitionJudgeVM.competitionList = competitionContext.GetAllCompetition();
            // Check if BranchNo (id) presents in the query string
            if (id != null)
            {
                ViewData["selectedCompetitionNo"] = id.Value;
                // Get list of staff working in the branch
                competitionJudgeVM.judgeList = competitionContext.GetCompetitionJudge(id.Value);
            }
            else
            {
                ViewData["selectedCompetitionNo"] = "";
            }
            return View(competitionJudgeVM);

        }

        // GET: CompetitionJudgeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetitionJudgeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetitionJudgeController/Create
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

        // GET: CompetitionJudgeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompetitionJudgeController/Edit/5
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

        // GET: CompetitionJudgeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompetitionJudgeController/Delete/5
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
