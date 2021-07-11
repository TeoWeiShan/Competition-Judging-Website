using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P04_T4.Controllers
{
    public class CompetitionScore : Controller
    {
        // GET: CompetitionScore
        public ActionResult Index()
        {
            return View();
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
