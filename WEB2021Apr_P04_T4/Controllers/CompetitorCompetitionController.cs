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
    public class CompetitorCompetitionController : Controller
    {
        private CompetitorCompetitionDAL competitorcompetitionContext = new CompetitorCompetitionDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private AreaInterestDAL areaInterestContext = new AreaInterestDAL();
        // GET: CompetitorCompetitionController
        public ActionResult Index()
        {
            string loginID = HttpContext.Session.GetString("LoginID");
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Competition> competitionList = competitorcompetitionContext.GetCompetitorCompetition(Convert.ToInt32(loginID));
            return View(competitionList);
        }

        // GET: CompetitorCompetitionController/Details/5
        public ActionResult Details(int id)
        {/*
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            CompetitorCompetition competitorcompetition = competitorcompetitionContext.GetDetails(id);
            //CompetitionJudgeViewModel competitionVM = MapToCompetitionJudgeVM(competition);*/
            return View();
        }

        // GET: CompetitorCompetitionController/Create
        public ActionResult Create()
        {
            string loginID = HttpContext.Session.GetString("LoginID");
            List<Competition> competitionList = competitorcompetitionContext.GetAllCompetitorCompetition(Convert.ToInt32(loginID));
            return View(competitionList);
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
        //public ActionResult Create(CompetitorCompetition competitorcompetition)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        competitorcompetition.CompetitionID = competitorcompetitionContext.Add(competitorcompetition);
        //        return RedirectToAction("Index");
        //    }
        //    else
        //        return View(competitorcompetition);
        //}

        public ActionResult Join(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            Competition competition = competitionContext.GetDetails(id);
            CompetitionViewModel competitionVM = MapToCompetitionVM(competition);
            return View(competitionVM);
        }
        public CompetitionViewModel MapToCompetitionVM(Competition competition)
        {
            string interestName = "";
            if (competition.CompetitionID != null)
            {
                List<AreaInterest> interestList = areaInterestContext.GetAllAreaInterest();
                foreach (AreaInterest interest in interestList)
                {
                    if (interest.AreaInterestID == competition.AreaInterestID)
                    {
                        interestName = interest.Name;
                        //Exit the foreach loop once the name is found
                        break;
                    }
                }
            }
            CompetitionViewModel competitionVM = new CompetitionViewModel
            {
                CompetitionID = competition.CompetitionID,
                Name = interestName,
                CompetitionName = competition.CompetitionName,
                StartDate = competition.StartDate,
                EndDate = competition.EndDate,
                ResultReleasedDate = competition.ResultReleasedDate
            };
            return competitionVM;
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


        // POST: CompetitorCompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(Competition competition)
        {
            ViewData["InterestList"] = GetAllAreaInterests();
            string loginID = HttpContext.Session.GetString("LoginID");
            if (ModelState.IsValid)
            {
                //Update staff record to database
                competitorcompetitionContext.Join(competition.CompetitionID,Convert.ToInt32(loginID));
                return RedirectToAction("Index");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(competition);
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
