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
    public class JudgeProfileController : Controller
    {
       private JudgeProfileDAL judgeProfileContext = new JudgeProfileDAL();
        // GET: JudgeProfileController
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Judge> judgeProfileList = judgeProfileContext.GetAllJudgeProfile();
            return View(judgeProfileList);
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
            // or account not in the "Staff" role
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
        public ActionResult Create(Judge judgeProfile)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                judgeProfile.JudgeID = judgeProfileContext.Add(judgeProfile);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(judgeProfile);
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
            Judge judgeProfile = judgeProfileContext.GetDetails(id.Value);
            if (judgeProfile == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(judgeProfile);
        }

        // POST: JudgeProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Judge judgeProfile)
        {
            // Delete the interest record from database
            judgeProfileContext.Delete(judgeProfile.JudgeID);
            return RedirectToAction("Index");
        }
    }
}
