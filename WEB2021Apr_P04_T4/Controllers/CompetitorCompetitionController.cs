using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.DAL;
using WEB2021Apr_P04_T4.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WEB2021Apr_P04_T4.Controllers
{
    public class CompetitorCompetitionController : Controller
    {
        private CompetitorCompetitionDAL competitorcompetitionContext = new CompetitorCompetitionDAL();
        private CompetitionDAL competitionContext = new CompetitionDAL();
        private AreaInterestDAL areaInterestContext = new AreaInterestDAL();
        private CompetitionSubmissionDAL competitionSubmissionContext = new CompetitionSubmissionDAL();
        private CompetitionSubmissionDAL submissionContext = new CompetitionSubmissionDAL();
        private IHostingEnvironment _env;
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
        public ActionResult Create(CompetitorCompetition competitorcompetition)
        {
            if (ModelState.IsValid)
            {
                //competitorcompetition.CompetitionID = competitorcompetitionContext.Add(competitorcompetition);
                return RedirectToAction("Index");
            }
            else
                return View(competitorcompetition);
        }

        public ActionResult Join(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Competitor"))
            {
                return RedirectToAction("Index", "Home");
            }
            Competition competition = competitionContext.GetDetails(id);
            return View(competition);
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
            ModelState.Remove("AreaInterestID");
            ModelState.Remove("CompetitionID");
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

        //public CompetitionSubmissionViewModel MapToSubmissionVM(CompetitionSubmission competitionSubmission)
        //{
        //    string CompetitorID = "";
        //    if (competitionSubmission.CompetitorID != null)
        //    {
        //        List<CompetitionSubmission> submissionList = submissionContext.GetDetails();
        //        foreach (Competitor competitor in submissionList)
        //        {
        //            if (competitor.CompetitorID == competitionSubmission.CompetitorID.Value)
        //            {
        //                CompetitorID = competitor.CompetitorID;
        //                //Exit the foreach loop once the name is found
        //                break;
        //            }
        //        }
        //    }
        //    CompetitionSubmissionViewModel submissionVM = new CompetitionSubmissionViewModel
        //    {
        //        CompetitionID = competitionSubmission.CompetitionID,
        //        CompetitorID = competitionSubmission.CompetitorID,
        //        FileSubmitted = competitionSubmission.FileSubmitted,
        //        //fileToUpload = competitionSubmission.,
        //        DateTimeFileUpload = competitionSubmission.DateTimeFileUpload,
        //        Appeal = competitionSubmission.Appeal,
        //        VoteCount = competitionSubmission.VoteCount,
        //        Ranking = competitionSubmission.Ranking,
        //        //CriteriaID = competitionSubmission.,
        //        //Score = competitionSubmission.,
        //    };
        //    return submissionVM;
        //}

        //public CompetitionSubmissionViewModel MapToSubmissionVM(CompetitionSubmission competitionSubmission)
        //{
        //    string interestName = "";

        //    List<AreaInterest> interestList = areaInterestContext.GetAllAreaInterest();
        //    foreach (AreaInterest interest in interestList)
        //    {
        //        if (interest.AreaInterestID == competition.AreaInterestID)
        //        {
        //            interestName = interest.Name;
        //            //Exit the foreach loop once the name is found
        //            break;
        //        }
        //    }

        //    CompetitionViewModel competitionVM = new CompetitionViewModel
        //    {
        //        CompetitionID = competition.CompetitionID,
        //        Name = interestName,
        //        CompetitionName = competition.CompetitionName,
        //        StartDate = competition.StartDate,
        //        EndDate = competition.EndDate,
        //        ResultReleasedDate = competition.ResultReleasedDate
        //    };
        //    return competitionVM;
        //}

        // GET: CompetitorCompetitionController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    // Stop accessing the action if not logged in
        //    // or account not in the "Staff" role
        //    if ((HttpContext.Session.GetString("Role") == null) ||
        //    (HttpContext.Session.GetString("Role") != "Competitor"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    Competitor competitior = competitorContext.GetDetails(id);
        //    CompetitionSubmissionViewModel submissionVM = MapToSubmissionVM(competitionSubmission);
        //    return View(submissionVM);
        //}

        //// POST: CompetitorCompetitionController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(CompetitionSubmissionViewModel submissionVM)
        //{
        //    if (submissionVM.fileToUpload != null && submissionVM.fileToUpload.Length > 0)
        //    {
        //        try
        //        {
        //            // Find the filename extension of the file to be uploaded.
        //            string fileExt = Path.GetExtension(
        //             submissionVM.fileToUpload.FileName);
        //            // Rename the uploaded file with the staff’s name.
        //            string uploadedFile = submissionVM.Name + fileExt;
        //            // Get the complete path to the images folder in server
        //            string savePath = Path.Combine(
        //             Directory.GetCurrentDirectory(),
        //             "wwwroot\\images", uploadedFile);
        //            // Upload the file to server
        //            using (var fileSteam = new FileStream(
        //             savePath, FileMode.Create))
        //            {
        //                await submissionVM.fileToUpload.CopyToAsync(fileSteam);
        //            }
        //            submissionVM.fileToUpload = uploadedFile;
        //            ViewData["Message"] = "File uploaded successfully.";
        //        }
        //        catch (IOException)
        //        {
        //            //File IO error, could be due to access rights denied
        //            ViewData["Message"] = "File uploading fail!";
        //        }
        //        catch (Exception ex) //Other type of error
        //        {
        //            ViewData["Message"] = ex.Message;
        //        }
        //    }
        //    return View(submissionVM);
        //}

        //File upload part
        public CompetitorCompetitionController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Edit(IEnumerable<IFormFile> files)
        {
            int i = 0;
            foreach (var file in files)
            {
                var dir = _env.ContentRootPath;
                
                using (var fileStream = new FileStream(Path.Combine(dir, $"file(i++).pdf"), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
            }
            return RedirectToAction("Index");
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
