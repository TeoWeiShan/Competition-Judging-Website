﻿using Microsoft.AspNetCore.Http;
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
        private JudgeProfileDAL judgeContext = new JudgeProfileDAL();
        private CompetitionJudgeDAL competitionJudgeContext = new CompetitionJudgeDAL();
        // GET: CompetitionJudgeController
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Competition> competitionList = competitionContext.GetAllCompetition();
            return View(competitionList);
        }

        // GET: CompetitionJudgeController/Details/5
        public ActionResult Details(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            
            ViewData["CompetitionID"] = id;
            
            Competition competition = competitionContext.GetDetails(id);
            if(id == 0 || competition.CompetitionName  ==null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index", "Competition");
            }
            ViewData["CompetitionName"] = competition.CompetitionName;
            ViewData["EndDate"] = competition.EndDate;
            List<Judge> judgeList = judgeContext.GetCompetitionJudgeDetails(id);
            //CompetitionJudgeViewModel competitionVM = MapToCompetitionJudgeVM(competition);
            return View(judgeList);
        }

        private List<Judge> GetAvailableJudge(int competitionId)
        {
            // Get a list of branches from database
            List<Judge> judgeList = judgeContext.GetAvailableJudge(competitionId);
            Console.WriteLine(judgeList);
            // Adding a select prompt at the first row of the branch list
            judgeList.Insert(0, new Judge
            {
                JudgeID = 0,
                JudgeName = "--Select--",
            });
            Console.WriteLine(judgeList);
            return judgeList;
        }

        public CompetitionJudgeViewModel MapToCompetitionJudgeVM(Competition comp)
        {
            string competitionName = "";
            if (comp.CompetitionID != null)
            {
                List<Competition> competitionList = competitionContext.GetAllCompetition();
                foreach (Competition competition in competitionList)
                {
                    if (competition.CompetitionID == comp.CompetitionID)
                    {
                        competitionName = competition.CompetitionName;
                        //Exit the foreach loop once the name is found
                        break;
                    }
                }
            }

            CompetitionJudgeViewModel competitionJudgeVM = new CompetitionJudgeViewModel
            {
                CompetitionID = comp.CompetitionID,
                CompetitionName = competitionName,
                JudgeID = 0,
                JudgeName ="-",
            };
            return competitionJudgeVM;
        }

        // GET: CompetitionJudgeController/Create
        public ActionResult Create(int id)
        {

            // Stop accessing the action if not logged in or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            Competition competition = competitionContext.GetDetails(id);
            if (competition.EndDate < DateTime.Today)
            {
                TempData["ErrorMsg"] = "Sorry! The competition has already ended and no judges can be assigned!";
                return RedirectToAction("DisplayError", "Home");
            }
            if (competition.EndDate == null)
            {
                TempData["ErrorMsg"] = "Sorry! The competition has no confirmed dates and no judges can be assigned";
                return RedirectToAction("DisplayError", "Home");
            }
            CompetitionJudgeViewModel competitionJudgeVM = MapToCompetitionJudgeVM(competition);
            ViewData["JudgeList"] = GetAvailableJudge(id);
            return View(competitionJudgeVM);
        }

        // POST: CompetitionJudgeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompetitionJudgeViewModel competitionJudge)
        {
            ViewData["JudgeList"] = GetAvailableJudge(competitionJudge.CompetitionID);
            if (ModelState.IsValid)
            {
                //Update staff record to database
                competitionJudgeContext.AddCompJudge(competitionJudge);
                return RedirectToAction("Index", "Competition");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(competitionJudge);
            }
        }

        private List<Judge> GetCompetitionJudge(int competitionId)
        {
            // Get a list of branches from database
            List<Judge> judgeList = judgeContext.GetCompetitionJudgeDetails(competitionId);
            Console.WriteLine(judgeList);
            // Adding a select prompt at the first row of the branch list
            judgeList.Insert(0, new Judge
            {
                JudgeID = 0,
                JudgeName = "--Select--",
            });
            Console.WriteLine(judgeList);
            return judgeList;
        }

        // GET: CompetitionJudgeController/Delete/5
        public ActionResult Delete(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            Competition competition = competitionContext.GetDetails(id);
            if (competition.EndDate < DateTime.Today)
            {
                TempData["ErrorMsg"] = "Sorry! The competition has already ended and no judges can be assigned!";
                return RedirectToAction("DisplayError", "Home");
            }
            if (competition.EndDate == null)
            {
                TempData["ErrorMsg"] = "Sorry! The competition has no confirmed dates and no judges can be assigned";
                return RedirectToAction("Index", "Competition");
            }
            CompetitionJudgeViewModel competitionJudgeVM = MapToCompetitionJudgeVM(competition);
            ViewData["JudgeList"] = GetCompetitionJudge(id);
            return View(competitionJudgeVM);
        }

        // POST: CompetitionJudgeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CompetitionJudgeViewModel competitionJudge)
        {
            ViewData["JudgeList"] = GetCompetitionJudge(competitionJudge.CompetitionID);
            if (ModelState.IsValid)
            {
                //Update staff record to database
                competitionJudgeContext.RemoveCompJudge(competitionJudge);
                return RedirectToAction("Index", "Competition");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(competitionJudge);
            }
        }
    }
}
