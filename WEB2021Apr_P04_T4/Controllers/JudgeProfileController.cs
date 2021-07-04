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
    public class JudgeProfileController : Controller
    {
       private JudgeProfileDAL judgeContext = new JudgeProfileDAL();

        // GET: JudgeProfileController
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Judge> judgeList = judgeContext.GetAllJudge();
            return View(judgeList);
        }

        // GET: JudgeProfileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: JudgeProfileController/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: JudgeProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Judge judge)
        {
            if (ModelState.IsValid)
            {
                //Add judge record to database
                judge.JudgeID = judgeContext.Add(judge);
                //Redirect user to Judge/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(judge);
            }
        }

        // GET: JudgeProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: JudgeProfileController/Edit/5
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

        // GET: JudgeProfileController/Delete/5
        public ActionResult Delete(int? id)
        {
            // Stop accessing the action if not logged in or account not in the "Admin" role
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
            Judge judge = judgeContext.GetDetails(id.Value);
            if (judge == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(judge);
        }

        // POST: JudgeProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Judge judge)
        {
            // Delete the interest record from database
            judgeContext.Delete(judge.JudgeID);
            return RedirectToAction("Index");
        }
    }
}
