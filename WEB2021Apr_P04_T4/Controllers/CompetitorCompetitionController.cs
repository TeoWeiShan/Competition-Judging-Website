using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P04_T4.Controllers
{
    public class CompetitorCompetitionController : Controller
    {
        // GET: CompetitorCompetitionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CompetitorCompetitionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetitorCompetitionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetitorCompetitionController/Create
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

        // GET: CompetitorCompetitionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompetitorCompetitionController/Edit/5
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

        // GET: CompetitorCompetitionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompetitorCompetitionController/Delete/5
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
