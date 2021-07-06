using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.Models;
using WEB2021Apr_P04_T4.DAL;

namespace WEB2021Apr_P04_T4.Controllers
{
    public class HomeController : Controller
    {
        private JudgeProfileDAL judgeContext = new JudgeProfileDAL();
        private CompetitorDAL competitorContext = new CompetitorDAL();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string loginID = formData["txtLoginID"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();
            //Check if login is Judge
            List<Judge> judgeList = judgeContext.GetAllJudge();
            foreach (Judge judge in judgeList)
            {
                if (judge.EmailAddr.ToLower() == loginID && judge.Password == password)
                {
                    // Store Login ID in session with the key “LoginID”
                    HttpContext.Session.SetString("LoginID", loginID);
                    // Store user role “Judge” as a string in session with the key “Role” 
                    HttpContext.Session.SetString("Role", "Judge");
                    return RedirectToAction("JudgeMain");
                }
            }
            //Check if login is Competitor
            List<Competitor> competitorList = competitorContext.GetAllCompetitor();
            foreach (Competitor competitor in competitorList)
            {
                if (competitor.EmailAddr.ToLower() == loginID && competitor.Password == password)
                {
                    // Store Login ID in session with the key “LoginID”
                    HttpContext.Session.SetString("LoginID", loginID);
                    // Store user role “Judge” as a string in session with the key “Role” 
                    HttpContext.Session.SetString("Role", "Competitor");
                    return RedirectToAction("CompetitorMain");

                }
            }
            //Check if login is Admin, else invalid credentials
            if (loginID == "admin1@lcu.edu.sg" && password == "p@55Admin")
            {
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", loginID);
                // Store user role “Staff” as a string in session with the key “Role” 
                HttpContext.Session.SetString("Role", "Admin");
                return RedirectToAction("AdminMain");
            }/*
            else if (loginID == "abc1@lcu.edu.sg" && password == "p@55Judge")
            {
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", loginID);
                // Store user role “Judge” as a string in session with the key “Role” 
                HttpContext.Session.SetString("Role", "Judge");
                return RedirectToAction("JudgeMain");
            }
            else if (loginID == "competitor1@lcu.edu.sg" && password == "p@55Competitor")
            {
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", loginID);
                // Store user role “Judge” as a string in session with the key “Role” 
                HttpContext.Session.SetString("Role", "Competitor");
                return RedirectToAction("CompetitorMain");
            }*/
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";

                // Redirect user back to the index view through an action
                return RedirectToAction("Login");
            }
        }

        public ActionResult AdminMain()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult JudgeMain()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Judge"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult CompetitorMain()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
