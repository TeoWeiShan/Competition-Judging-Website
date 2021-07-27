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
    public class CompetitionScoreController : Controller
    {
        private CompetitionScoreDAL scoreContext = new CompetitionScoreDAL();

        // GET: CompetitionScore
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<CompetitionScore> scoreList = scoreContext.GetAllScore();
            return View(scoreList);
        }

        // GET: CompetitionScore/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetitionScore/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetitionScore/Create
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

        // GET: CompetitionScore/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompetitionScore/Edit/5
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

        // GET: CompetitionScore/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompetitionScore/Delete/5
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
