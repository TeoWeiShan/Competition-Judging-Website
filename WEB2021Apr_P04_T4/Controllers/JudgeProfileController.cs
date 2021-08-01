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
        private AreaInterestDAL areaInterestContext = new AreaInterestDAL();
        public ActionResult Index()
        {
            string loginID = HttpContext.Session.GetString("LoginID");
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            Judge judge = judgeContext.GetDetails(Convert.ToInt32(loginID));
            JudgeViewModel judgeVM = MapToJudgeVM(judge);
            return View(judgeVM);
        }     

        public JudgeViewModel MapToJudgeVM(Judge judge)
        {
            string interestName = "";
            if (judge.JudgeID != null)
            {
                List<AreaInterest> interestList = areaInterestContext.GetAllAreaInterest();
                foreach (AreaInterest interest in interestList)
                {
                    if (interest.AreaInterestID == judge.AreaInterestID)
                    {
                        interestName = interest.Name;
                        //Exit the foreach loop once the name is found
                        break;
                    }
                }
            }
            JudgeViewModel judgeVM = new JudgeViewModel
            {
                JudgeID = judge.JudgeID,
                JudgeName = judge.JudgeName,
                Salutation = judge.Salutation,
                AreaInterestID = interestName,
                EmailAddr = judge.EmailAddr,
                Password = judge.Password,
            };
            return judgeVM;        
    }

        private List<AreaInterest> GetAllAreaInterests()
        {
            // Get a list of branches from database
            List<AreaInterest> areaInterestList = areaInterestContext.GetAllAreaInterest();
            Console.WriteLine(areaInterestList);
            // Adding a select prompt at the first row of the branch list
            areaInterestList.Insert(0, new AreaInterest
            {
                AreaInterestID = 0,
                Name = "--Select--"
            });
            Console.WriteLine(areaInterestList);
            return areaInterestList;
        }

        // GET: JudgeProfileController/Create
        public ActionResult Create()
        {
            ViewData["InterestList"] = GetAllAreaInterests();
            return View();
        }

        // POST: JudgeProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Judge judge)
        {
            ViewData["InterestList"] = GetAllAreaInterests();
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
        public ActionResult Edit(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Judge" role

            if ((HttpContext.Session.GetString("LoginID") != id.ToString()) ||
            (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index", "Home");
            }
            ViewData["InterestList"] = GetAllAreaInterests();
            Judge judge = judgeContext.GetDetails(id.Value);
            if (judge == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index", "Home");
            }
            return View(judge);
        }

        // POST: JudgeProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Judge judge)
        {
            ViewData["InterestList"] = GetAllAreaInterests();

            if (ModelState.IsValid) {
                //Update judge record to database
                judgeContext.Update(judge);
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(judge);
            }
        }

        /*
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
        } */
    }
}
