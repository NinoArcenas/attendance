using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceManagementSystem.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;

namespace AttendanceManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private UsersContext db = new UsersContext();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var account = from ab in db.Account
                          select ab;
            ViewBag.Account = account;

            var birlocation = from ab in db.location
                          where ab.status == 1
                          select ab;
            ViewBag.Birlocation = birlocation;

            var rodlocation = from ab in db.rdlocation
                              where ab.status == 1
                              select ab;
            ViewBag.Rodlocation = rodlocation;

            var asslocation = from ab in db.asseslocation
                              where ab.status == 1
                              select ab;
            ViewBag.Asslocation = asslocation;

            var total = from ab in db.totaldayscounts
                              select ab;
            ViewBag.TotalDaysCount = total;

            var agingReports = (from pb in db.agingreports
                                select pb).ToList();
            var models = new List<AgingTable>();
            // Loop through each record and apply the status logic
            foreach (var agingReport in agingReports)            {
                //PRE-BIR
                DateTime? projected = agingReport?.notice_of_fullpayment_date; // Use notice_of_fullpayment_date
                DateTime? actual = agingReport?.fullpayment_date_prebir; // Assume this is the actual completion date
                //BIR SUBMISSION
                DateTime? projectedbirsub = agingReport?.submissiondateprojected30; // Use notice_of_fullpayment_date
                DateTime? actualbirsub = agingReport?.submission_date_bir; // Assume this is the actual completion date
                //BIR RELEASED
                DateTime? projectedbirrel = agingReport?.birrealeasedprojected90; // Use notice_of_fullpayment_date
                DateTime? actualbirrel = agingReport?.released_date_bir; // Assume this is the actual completion date
                //PRE-ROD SUBMISSION
                DateTime? projectedprerodsub = agingReport?.prerodsubprojected45; // Use notice_of_fullpayment_date
                DateTime? actualprerodsub = agingReport?.date_submitted_rod; // Assume this is the actual completion date
                //ROD RELEASED
                DateTime? projectedrodrel = agingReport?.rodsubprojected60; // Use notice_of_fullpayment_date
                DateTime? actualrodrel = agingReport?.date_released_rod; // Assume this is the actual completion date
                //PRE- ASSESSOR SUBMISSION
                DateTime? projectedpreasssub = agingReport?.preasssubprojected15; // Use notice_of_fullpayment_date
                DateTime? actualpreasssub = agingReport?.date_submitted_preassessors; // Assume this is the actual completion date
                //ASSESSOR RELEASED
                DateTime? projectedassrel = agingReport?.assrelprojected60; // Use notice_of_fullpayment_date
                DateTime? actualassrel = agingReport?.date_released_assessors; // Assume this is the actual completion date
                var model = new AgingTable
                {
                    //PRE-BIR
                    PRE_BIR_PROJECTED = projected,
                    PRE_BIR_ACTUAL = actual,
                    //BIR SUBMISSION
                    BIR_SUBMISSION_PRJECTED = projectedbirsub,
                    BIR_SUBMISSION_ACTUAL = actualbirsub,
                    //BIR RELEASED
                    BIR_RELEASE_PROJECTED = projectedbirrel,
                    BIR_RELEASE_ACTUAL = actualbirrel,
                    //PRE-ROD SUBMISSION
                    PRE_ROD_SUBMISION_PROJECTED = projectedprerodsub,
                    PRE_ROD_SUBMISION_ACTUAL = actualprerodsub,
                    //ROD RELEASED
                    ROD_RELEASE_PROJECTED = projectedrodrel,
                    ROD_RELEASE_ACTUAL = actualrodrel,
                    //PRE- ASSESSOR SUBMISSION
                    PRE_ASSESSOR_SUBMISSION_PROJECTED = projectedpreasssub,
                    PRE_ASSESSOR_SUBMISSION_ACTUAL = actualpreasssub,
                    //PRE- ASSESSOR SUBMISSION
                    ASSESSOR_RELEASE_PROJECTED = projectedassrel,
                    ASSESSOR_RELEASE_ACTUAL = actualassrel
                };
                //PRE-BIR
                if (!actual.HasValue)
                {
                    model.PRE_BIR_STATUS = "ongoing";
                }
                else if (actual > projected)
                {
                    model.PRE_BIR_STATUS = "done delay";
                }
                else if (actual <= projected)
                {
                    model.PRE_BIR_STATUS = "done on time";
                }
                else
                {
                    model.PRE_BIR_STATUS = "delay";
                }

                //BIR SUBMISSION
                if (!actualbirsub.HasValue)
                {
                    if (model.PRE_BIR_STATUS == "")
                    {
                        model.BIR_SUBMISSION_STATUS = ""; // Set release status to empty if submission status is ongoing
                    }
                    else if (model.PRE_BIR_STATUS == "ongoing")
                    {
                        model.BIR_SUBMISSION_STATUS = "";
                    }
                    else
                    {
                        model.BIR_SUBMISSION_STATUS = "ongoing";
                    }
                }
                else if (actualbirsub > projectedbirsub)
                {
                    model.BIR_SUBMISSION_STATUS = "done delay";
                }
                else if (actualbirsub <= projectedbirsub)
                {
                    model.BIR_SUBMISSION_STATUS = "done on time";
                }
                else
                {
                    model.BIR_SUBMISSION_STATUS = "delay";
                }

                //BIR RELEASED
                if (!actualbirrel.HasValue)
                {
                    if (model.BIR_SUBMISSION_STATUS == "")
                    {
                        model.BIR_RELEASE_STATUS = ""; // Set release status to empty if submission status is ongoing
                    }
                    else if (model.BIR_SUBMISSION_STATUS == "ongoing")
                    {
                        model.BIR_RELEASE_STATUS = "";
                    }
                    else
                    {
                        model.BIR_RELEASE_STATUS = "ongoing";
                    }                    
                }
                else if (actualbirrel > projectedbirrel)
                {
                    model.BIR_RELEASE_STATUS = "done delay";
                }
                else if (actualbirrel <= projectedbirrel)
                {
                    model.BIR_RELEASE_STATUS = "done on time";
                }
                else
                {
                    model.BIR_RELEASE_STATUS = "delay";
                }

                //PRE-ROD SUBMISSION
                if (!actualprerodsub.HasValue)
                {
                    if (model.BIR_RELEASE_STATUS == "")
                    {
                        model.PRE_ROD_SUBMISSION_STATUS = ""; // Set release status to empty if submission status is ongoing
                    }
                    else if (model.BIR_RELEASE_STATUS == "ongoing")
                    {
                        model.PRE_ROD_SUBMISSION_STATUS = "";
                    }
                    else
                    {
                        model.PRE_ROD_SUBMISSION_STATUS = "ongoing";
                    }
                }
                else if (actualprerodsub > projectedprerodsub)
                {
                    model.PRE_ROD_SUBMISSION_STATUS = "done delay";
                }
                else if (actualprerodsub <= projectedprerodsub)
                {
                    model.PRE_ROD_SUBMISSION_STATUS = "done on time";
                }
                else
                {
                    model.PRE_ROD_SUBMISSION_STATUS = "delay";
                }

                //ROD RELEASED
                if (!actualrodrel.HasValue)
                {
                    if (model.PRE_ROD_SUBMISSION_STATUS == "")
                    {
                        model.ROD_RELEASE_STATUS = ""; // Set release status to empty if submission status is ongoing
                    }
                    else if (model.PRE_ROD_SUBMISSION_STATUS == "ongoing")
                    {
                        model.ROD_RELEASE_STATUS = "";
                    }
                    else
                    {
                        model.ROD_RELEASE_STATUS = "ongoing";
                    }
                }
                else if (actualrodrel > projectedrodrel)
                {
                    model.ROD_RELEASE_STATUS = "done delay";
                }
                else if (actualrodrel <= projectedrodrel)
                {
                    model.ROD_RELEASE_STATUS = "done on time";
                }
                else
                {
                    model.ROD_RELEASE_STATUS = "delay";
                }

                //PRE- ASSESSOR SUBMISSION
                if (!actualpreasssub.HasValue)
                {
                    if (model.ROD_RELEASE_STATUS == "")
                    {
                        model.PRE_ASSESSOR_SUBMISSION_STATUS = ""; // Set release status to empty if submission status is ongoing
                    }
                    else if (model.ROD_RELEASE_STATUS == "ongoing")
                    {
                        model.PRE_ASSESSOR_SUBMISSION_STATUS = "";
                    }
                    else
                    {
                        model.PRE_ASSESSOR_SUBMISSION_STATUS = "ongoing";
                    }
                }
                else if (actualpreasssub > projectedpreasssub)
                {
                    model.PRE_ASSESSOR_SUBMISSION_STATUS = "done delay";
                }
                else if (actualpreasssub <= projectedpreasssub)
                {
                    model.PRE_ASSESSOR_SUBMISSION_STATUS = "done on time";
                }
                else
                {
                    model.PRE_ASSESSOR_SUBMISSION_STATUS = "delay";
                }

                //ASSESSOR RELEASED
                if (!actualassrel.HasValue)
                {
                    if (model.PRE_ASSESSOR_SUBMISSION_STATUS == "")
                    {
                        model.ASSESSOR_RELEASE_STATUS = ""; // Set release status to empty if submission status is ongoing
                    }
                    else if (model.PRE_ASSESSOR_SUBMISSION_STATUS == "ongoing")
                    {
                        model.ASSESSOR_RELEASE_STATUS = "";
                    }
                    else
                    {
                        model.ASSESSOR_RELEASE_STATUS = "ongoing";
                    }
                }
                else if (actualassrel > projectedassrel)
                {
                    model.ASSESSOR_RELEASE_STATUS = "done delay";
                }
                else if (actualassrel <= projectedassrel)
                {
                    model.ASSESSOR_RELEASE_STATUS = "done on time";
                }
                else
                {
                    model.ASSESSOR_RELEASE_STATUS = "delay";
                }
                // Add the model to the list
                models.Add(model);
            }
            // Return the list of models to the view
            return View(models);
        }

        

        //Count Dashboard Client Account
        public static int ClientCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard Turn Over
        public static int TurnoverCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 8
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard Pull Out Account
        public static int PullOutCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.pullouts
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard Cancelled Account
        public static int CancelledCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.cancels
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard PRE-BIR Account
        public static int PreBIRCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 1
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard BIR Account
        public static int BIRCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 2
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard PRE-ROD Account
        public static int PreRODCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 3
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard ROD Account
        public static int RODCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 4
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard PRE-ASSESSOR Account
        public static int PreAssessorCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 5
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard ASSESSOR Account
        public static int AssessorCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 6
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard Completed Account
        public static int CompletedCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.status == 7
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard Bank Account
        public static int BankCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.financing == "BANK"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard HDMF Account
        public static int HDMFCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.financing == "HDMF"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard IHF Account
        public static int IHFCount()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.financing == "IHF"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard BREAKDOWN WITH BIR
        public static int BWBBohol()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.location == "BOHOL"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWBNorth()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.location == "NORTH"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWBSouth()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.location == "SOUTH"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWBMandaue()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.location == "MANDAUE"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWBTalisay()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.location == "TALISAY"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard BREAKDOWN WITH ROD
        public static int BWRBohol()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.rd_location == "BOHOL"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWRCebuCity()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.rd_location == "CEBU CITY"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWRCebuProvince()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.rd_location == "CEBU PROVINCE"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWRMandaue()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.rd_location == "MANDAUE"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWRLapulapu()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.rd_location == "LAPU-LAPU"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        //Count Dashboard BREAKDOWN WITH ASSESSOR
        public static int BWAMandaue()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.ass_location == "MANDAUE"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWACebuCity()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.ass_location == "CEBU CITY"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWALapulapu()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.ass_location == "LAPU-LAPU"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWABohol()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.ass_location == "BOHOL"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }
        public static int BWACebuProvince()
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Account
                         where emp.ass_location == "PROVINCE"
                         select emp).Count();
            if (admin > 0)
            {
                return admin;
            }
            else
            {
                return 0;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AgingReport()
        {
            var aging = from ab in db.agingreports
                        where ab.status == 1
                         select ab;
            ViewBag.AgingReport = aging;

            var year = from ab in db.years
                        select ab;
            ViewBag.Years = year;

            var export = from ab in db.agingreports
                         where ab.status == 1
                         select ab;
            ViewBag.EXPORT = export;

            return View();
        }

        [Authorize]
        public ActionResult Aging_Report()
        {
            return View();
        }

        //aging report piechart
        [HttpGet]
        public JsonResult AgingReportPieChart()
        {
            var query = @"SELECT c.notice_of_fullpayment_date AS 'PRE_BIR_PROJECTED', c.fullpayment_date_prebir AS 'PRE_BIR_ACTUAL', c.submissiondateprojected30 AS 'BIR_SUBMISSION_PRJECTED' , c.submission_date_bir as 'BIR_SUBMISSION_ACTUAL', 
                c.birrealeasedprojected90 AS 'BIR_RELEASE_PROJECTED', c.released_date_bir AS 'BIR_RELEASE_ACTUAL', c.prerodsubprojected45 as 'PRE_ROD_SUBMISION_PROJECTED', c.date_submitted_rod as 'PRE_ROD_SUBMISION_ACTUAL',
                c.rodsubprojected60 as 'ROD_RELEASE_PROJECTED', c.date_released_rod as 'ROD_RELEASE_ACTUAL', c.preasssubprojected15 as 'PRE_ASSESSOR_SUBMISSION_PROJECTED', c.date_submitted_preassessors as 'PRE_ASSESSOR_SUBMISSION_ACTUAL',
                c.assrelprojected60 as 'ASSESSOR_RELEASE_PROJECTED', c.date_released_assessors as 'ASSESSOR_RELEASE_ACTUAL'
            FROM Client_Status a
            INNER JOIN tbl_Aging_Report c on c.generatedcode = a.generatedcode;";

            var result = db.Database.SqlQuery<AgingTable>(query).ToList();

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //aging report
        [HttpGet]
        public JsonResult AgingTADConversion()
        {
            var query = @"SELECT 
                        a.account AS 'ACCOUNT_NAME', 
                        a.unit_no AS 'UNIT_NO', 
                        a.financing AS 'FINANCING', 

                        -- PRE_BIR
                        c.notice_of_fullpayment_date AS 'PRE_BIR_PROJECTED', 
                        c.fullpayment_date_prebir AS 'PRE_BIR_ACTUAL',
                        COALESCE(
                            CASE
                                WHEN c.notice_of_fullpayment_date IS NULL AND c.fullpayment_date_prebir IS NULL THEN NULL
                                WHEN c.fullpayment_date_prebir IS NULL THEN 'Ongoing'
                                WHEN c.fullpayment_date_prebir <= c.notice_of_fullpayment_date THEN 'Done on time'
                                WHEN c.fullpayment_date_prebir > c.notice_of_fullpayment_date THEN 'Done Delayed'
                                ELSE 'Delayed'
                            END, ''
                        ) AS 'PRE_BIR_STATUS',

                        -- BIR_SUBMISSION
                        c.submissiondateprojected30 AS 'BIR_SUBMISSION_PRJECTED', 
                        c.submission_date_bir AS 'BIR_SUBMISSION_ACTUAL',
                        COALESCE(
                            CASE
                                WHEN c.fullpayment_date_prebir IS NULL THEN ''
                                WHEN c.submission_date_bir IS NULL THEN 'Ongoing'
                                WHEN c.submission_date_bir <= c.submissiondateprojected30 THEN 'Done on time'
                                WHEN c.submission_date_bir > c.submissiondateprojected30 THEN 'Done Delayed'
                                ELSE 'Delayed'
                            END, ''
                        ) AS 'BIR_SUBMISSION_STATUS',

                        -- BIR_RELEASE
                        c.birrealeasedprojected90 AS 'BIR_RELEASE_PROJECTED', 
                        c.released_date_bir AS 'BIR_RELEASE_ACTUAL',
                        COALESCE(
                            CASE
                                WHEN c.submission_date_bir IS NULL THEN ''
                                WHEN c.released_date_bir IS NULL THEN 'Ongoing'
                                WHEN c.released_date_bir <= c.birrealeasedprojected90 THEN 'Done on time'
                                WHEN c.released_date_bir > c.birrealeasedprojected90 THEN 'Done Delayed'
                                ELSE 'Delayed'
                            END, ''
                        ) AS 'BIR_RELEASE_STATUS',

                        -- PRE_ROD_SUBMISSION
                        c.prerodsubprojected45 AS 'PRE_ROD_SUBMISION_PROJECTED', 
                        c.date_submitted_rod AS 'PRE_ROD_SUBMISION_ACTUAL',
                        COALESCE(
                            CASE
                                WHEN c.released_date_bir IS NULL THEN ''
                                WHEN c.date_submitted_rod IS NULL THEN 'Ongoing'
                                WHEN c.date_submitted_rod <= c.prerodsubprojected45 THEN 'Done on time'
                                WHEN c.date_submitted_rod > c.prerodsubprojected45 THEN 'Done Delayed'
                                ELSE 'Delayed'
                            END, ''
                        ) AS 'PRE_ROD_SUBMISSION_STATUS',

                        -- ROD_RELEASE
                        c.rodsubprojected60 AS 'ROD_RELEASE_PROJECTED', 
                        c.date_released_rod AS 'ROD_RELEASE_ACTUAL',
                        COALESCE(
                            CASE
                                WHEN c.date_submitted_rod IS NULL THEN ''
                                WHEN c.date_released_rod IS NULL THEN 'Ongoing'
                                WHEN c.date_released_rod <= c.rodsubprojected60 THEN 'Done on time'
                                WHEN c.date_released_rod > c.rodsubprojected60 THEN 'Done Delayed'
                                ELSE 'Delayed'
                            END, ''
                        ) AS 'ROD_RELEASE_STATUS',

                        -- PRE_ASSESSOR_SUBMISSION
                        c.preasssubprojected15 AS 'PRE_ASSESSOR_SUBMISSION_PROJECTED', 
                        c.date_submitted_preassessors AS 'PRE_ASSESSOR_SUBMISSION_ACTUAL',
                        COALESCE(
                            CASE
                                WHEN c.date_released_rod IS NULL THEN ''
                                WHEN c.date_submitted_preassessors IS NULL THEN 'Ongoing'
                                WHEN c.date_submitted_preassessors <= c.preasssubprojected15 THEN 'Done on time'
                                WHEN c.date_submitted_preassessors > c.preasssubprojected15 THEN 'Done Delayed'
                                ELSE 'Delayed'
                            END, ''
                        ) AS 'PRE_ASSESSOR_SUBMISSION_STATUS',

                        -- ASSESSOR_RELEASE
                        c.assrelprojected60 AS 'ASSESSOR_RELEASE_PROJECTED', 
                        c.date_released_assessors AS 'ASSESSOR_RELEASE_ACTUAL',
                        COALESCE(
                            CASE
                                WHEN c.date_submitted_preassessors IS NULL THEN ''
                                WHEN c.date_released_assessors IS NULL THEN 'Ongoing'
                                WHEN c.date_released_assessors <= c.assrelprojected60 THEN 'Done on time'
                                WHEN c.date_released_assessors > c.assrelprojected60 THEN 'Done Delayed'
                                ELSE 'Delayed'
                            END, ''
                        ) AS 'ASSESSOR_RELEASE_STATUS'

                    FROM 
                        Client_Status a
                    INNER JOIN 
                        tbl_Aging_Report c ON c.generatedcode = a.generatedcode;";   

            var result = db.Database.SqlQuery<AgingTable>(query).ToList();

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get Year Month
        [HttpGet]
        public JsonResult GetYearMonths()
        {
                var query = @"CREATE TABLE #YearMonthCounts (
        Year INT,
        Month INT,
        NoticeOfFullpaymentDate INT,
        FullpaymentDatePrebir INT,
        SubmissionDateProjected30 INT,
        SubmissionDateBir INT,
        BirReleasedProjected90 INT,
        ReleasedDateBir INT,
        PreRodSubProjected45 INT,
        DateSubmittedRod INT,
        RodSubProjected60 INT,
        DateReleasedRod INT,
        PreAssSubProjected15 INT,
        DateSubmittedPreAssessors INT,
        AssRelProjected60 INT,
        DateReleasedAssessors INT
    );

    -- Insert counts for each date column into the temporary table
    INSERT INTO #YearMonthCounts (Year, Month, NoticeOfFullpaymentDate, FullpaymentDatePrebir, SubmissionDateProjected30, SubmissionDateBir, BirReleasedProjected90, ReleasedDateBir, PreRodSubProjected45, DateSubmittedRod, RodSubProjected60, DateReleasedRod, PreAssSubProjected15, DateSubmittedPreAssessors, AssRelProjected60, DateReleasedAssessors)
    SELECT 
        YEAR(COALESCE(notice_of_fullpayment_date, fullpayment_date_prebir, submissiondateprojected30, submission_date_bir, birrealeasedprojected90, released_date_bir, prerodsubprojected45, date_submitted_rod, rodsubprojected60, date_released_rod, preasssubprojected15, date_submitted_preassessors, assrelprojected60, date_released_assessors)) AS Year,
        MONTH(COALESCE(notice_of_fullpayment_date, fullpayment_date_prebir, submissiondateprojected30, submission_date_bir, birrealeasedprojected90, released_date_bir, prerodsubprojected45, date_submitted_rod, rodsubprojected60, date_released_rod, preasssubprojected15, date_submitted_preassessors, assrelprojected60, date_released_assessors)) AS Month,
        COUNT(notice_of_fullpayment_date) AS NoticeOfFullpaymentDate,
        COUNT(fullpayment_date_prebir) AS FullpaymentDatePrebir,
        COUNT(submissiondateprojected30) AS SubmissionDateProjected30,
        COUNT(submission_date_bir) AS SubmissionDateBir,
        COUNT(birrealeasedprojected90) AS BirReleasedProjected90,
        COUNT(released_date_bir) AS ReleasedDateBir,
        COUNT(prerodsubprojected45) AS PreRodSubProjected45,
        COUNT(date_submitted_rod) AS DateSubmittedRod,
        COUNT(rodsubprojected60) AS RodSubProjected60,
        COUNT(date_released_rod) AS DateReleasedRod,
        COUNT(preasssubprojected15) AS PreAssSubProjected15,
        COUNT(date_submitted_preassessors) AS DateSubmittedPreAssessors,
        COUNT(assrelprojected60) AS AssRelProjected60,
        COUNT(date_released_assessors) AS DateReleasedAssessors
    FROM 
        tbl_Aging_Report
    GROUP BY 
        YEAR(COALESCE(notice_of_fullpayment_date, fullpayment_date_prebir, submissiondateprojected30, submission_date_bir, birrealeasedprojected90, released_date_bir, prerodsubprojected45, date_submitted_rod, rodsubprojected60, date_released_rod, preasssubprojected15, date_submitted_preassessors, assrelprojected60, date_released_assessors)),
        MONTH(COALESCE(notice_of_fullpayment_date, fullpayment_date_prebir, submissiondateprojected30, submission_date_bir, birrealeasedprojected90, released_date_bir, prerodsubprojected45, date_submitted_rod, rodsubprojected60, date_released_rod, preasssubprojected15, date_submitted_preassessors, assrelprojected60, date_released_assessors));

    -- Generate a list of all months from January to December
    WITH AllMonths AS (
        SELECT 
            Year,
            Month
        FROM (
            SELECT DISTINCT YEAR(notice_of_fullpayment_date) AS Year, MONTH(notice_of_fullpayment_date) AS Month FROM tbl_Aging_Report WHERE notice_of_fullpayment_date IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(fullpayment_date_prebir) AS Year, MONTH(fullpayment_date_prebir) AS Month FROM tbl_Aging_Report WHERE fullpayment_date_prebir IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(submissiondateprojected30) AS Year, MONTH(submissiondateprojected30) AS Month FROM tbl_Aging_Report WHERE submissiondateprojected30 IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(submission_date_bir) AS Year, MONTH(submission_date_bir) AS Month FROM tbl_Aging_Report WHERE submission_date_bir IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(birrealeasedprojected90) AS Year, MONTH(birrealeasedprojected90) AS Month FROM tbl_Aging_Report WHERE birrealeasedprojected90 IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(released_date_bir) AS Year, MONTH(released_date_bir) AS Month FROM tbl_Aging_Report WHERE released_date_bir IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(prerodsubprojected45) AS Year, MONTH(prerodsubprojected45) AS Month FROM tbl_Aging_Report WHERE prerodsubprojected45 IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(date_submitted_rod) AS Year, MONTH(date_submitted_rod) AS Month FROM tbl_Aging_Report WHERE date_submitted_rod IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(rodsubprojected60) AS Year, MONTH(rodsubprojected60) AS Month FROM tbl_Aging_Report WHERE rodsubprojected60 IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(date_released_rod) AS Year, MONTH(date_released_rod) AS Month FROM tbl_Aging_Report WHERE date_released_rod IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(preasssubprojected15) AS Year, MONTH(preasssubprojected15) AS Month FROM tbl_Aging_Report WHERE preasssubprojected15 IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(date_submitted_preassessors) AS Year, MONTH(date_submitted_preassessors) AS Month FROM tbl_Aging_Report WHERE date_submitted_preassessors IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(assrelprojected60) AS Year, MONTH(assrelprojected60) AS Month FROM tbl_Aging_Report WHERE assrelprojected60 IS NOT NULL
            UNION ALL
            SELECT DISTINCT YEAR(date_released_assessors) AS Year, MONTH(date_released_assessors) AS Month FROM tbl_Aging_Report WHERE date_released_assessors IS NOT NULL
        ) AS AllDates
    ),
    AllMonthsExpanded AS (
        -- Generate all months from January to December for each year
        SELECT 
            DISTINCT Year,
            Month
        FROM (
            SELECT Year, 1 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 2 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 3 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 4 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 5 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 6 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 7 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 8 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 9 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 10 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 11 AS Month FROM AllMonths
            UNION ALL
            SELECT Year, 12 AS Month FROM AllMonths
        ) AS AllMonthsExpanded
    )

    -- Select distinct years and months with proper ordering and month names
    SELECT 
        am.Year AS Year,
        DATENAME(MONTH, DATEADD(MONTH, am.Month - 1, '2000-01-01')) AS Months,
        ISNULL(SUM(ym.NoticeOfFullpaymentDate), 0) AS NoticeOfFullpaymentDate,
        ISNULL(SUM(ym.FullpaymentDatePrebir), 0) AS FullpaymentDatePrebir,
        ISNULL(SUM(ym.SubmissionDateProjected30), 0) AS SubmissionDateProjected30,
        ISNULL(SUM(ym.SubmissionDateBir), 0) AS SubmissionDateBir,
        ISNULL(SUM(ym.BirReleasedProjected90), 0) AS BirReleasedProjected90,
        ISNULL(SUM(ym.ReleasedDateBir), 0) AS ReleasedDateBir,
        ISNULL(SUM(ym.PreRodSubProjected45), 0) AS PreRodSubProjected45,
        ISNULL(SUM(ym.DateSubmittedRod), 0) AS DateSubmittedRod,
        ISNULL(SUM(ym.RodSubProjected60), 0) AS RodSubProjected60,
        ISNULL(SUM(ym.DateReleasedRod), 0) AS DateReleasedRod,
        ISNULL(SUM(ym.PreAssSubProjected15), 0) AS PreAssSubProjected15,
        ISNULL(SUM(ym.DateSubmittedPreAssessors), 0) AS DateSubmittedPreAssessors,
        ISNULL(SUM(ym.AssRelProjected60), 0) AS AssRelProjected60,
        ISNULL(SUM(ym.DateReleasedAssessors), 0) AS DateReleasedAssessors
    FROM 
        AllMonthsExpanded am
    LEFT JOIN 
        #YearMonthCounts ym 
        ON am.Year = ym.Year AND am.Month = ym.Month
    GROUP BY 
        am.Year, 
        am.Month
    ORDER BY 
        am.Year, 
        am.Month;  -- Ensure the order by uses the Month number to sort months correctly

    -- Drop the temporary table
    DROP TABLE #YearMonthCounts;

                ";

            var result = db.Database.SqlQuery<GetMonthAndYear>(query).ToList();

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        //Get the NoticeOfFullPayment Date 7-20-2024
        [HttpGet]
        public JsonResult GetFullPaymentDate()
        {
            var query = from notice in db.agingreports
                        where notice.status == 1
                        select notice.fullpayment_date_prebir;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the submissiondateprojected30 Date 7-20-2024
        [HttpGet]
        public JsonResult GetSubmissionDateProjected30()
        {
            var query = from birsubpro in db.agingreports
                        where birsubpro.status == 1
                        select birsubpro.submissiondateprojected30;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the submission_date_bir Date 7-20-2024
        [HttpGet]
        public JsonResult GetSubmissionDateActual()
        {
            var query = from birsubact in db.agingreports
                        where birsubact.status == 1
                        select birsubact.submission_date_bir;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the birrealeasedprojected90 Date 7-20-2024
        [HttpGet]
        public JsonResult GetBirRealeasedProjected90()
        {
            var query = from birrealpro in db.agingreports
                        where birrealpro.status == 1
                        select birrealpro.birrealeasedprojected90;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the released_date_bir Date 7-20-2024
        [HttpGet]
        public JsonResult GetReleasedDateBirActual()
        {
            var query = from birrealact in db.agingreports
                        where birrealact.status == 1
                        select birrealact.released_date_bir;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the prerodsubprojected45 Date 7-20-2024
        [HttpGet]
        public JsonResult GetPreRodSubProjected45()
        {
            var query = from prerodsubpro in db.agingreports
                        where prerodsubpro.status == 1
                        select prerodsubpro.prerodsubprojected45;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the date_submitted_rod Date 7-20-2024
        [HttpGet]
        public JsonResult GetDateSubmittedRod()
        {
            var query = from prerodsubact in db.agingreports
                        where prerodsubact.status == 1
                        select prerodsubact.date_submitted_rod;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the rodsubprojected60 Date 7-20-2024
        [HttpGet]
        public JsonResult GetRodSubProjected60()
        {
            var query = from rodrealpro in db.agingreports
                        where rodrealpro.status == 1
                        select rodrealpro.rodsubprojected60;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the date_released_rod Date 7-20-2024
        [HttpGet]
        public JsonResult GetDateReleasedRod()
        {
            var query = from rodrealact in db.agingreports
                        where rodrealact.status == 1
                        select rodrealact.date_released_rod;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the preasssubprojected15 Date 7-20-2024
        [HttpGet]
        public JsonResult GetPreAssSubProjected15()
        {
            var query = from preassubpro in db.agingreports
                        where preassubpro.status == 1
                        select preassubpro.preasssubprojected15;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the date_submitted_preassessors Date 7-20-2024
        [HttpGet]
        public JsonResult GetDateSubmittedPreassessors()
        {
            var query = from preassubact in db.agingreports
                        where preassubact.status == 1
                        select preassubact.date_submitted_preassessors;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the assrelprojected60 Date 7-20-2024
        [HttpGet]
        public JsonResult GetAssRelProjected60()
        {
            var query = from assrealpro in db.agingreports
                        where assrealpro.status == 1
                        select assrealpro.assrelprojected60;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Get the date_released_assessors Date 7-20-2024
        [HttpGet]
        public JsonResult GetDateReleasedAssessors()
        {
            var query = from assrealact in db.agingreports
                        where assrealact.status == 1
                        select assrealact.date_released_assessors;
            var jsonResult = Json(query, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //Save Developer
        [HttpPost]
        public string SaveYear(string year)
        {
            string stralert = "";
            int isexist = (from a in db.years
                           where a.year == year
                           select a).Count();
            if (isexist > 0)
            {
                stralert = "This Year was already exist.";
            }
            else
            {
                Year d = new Year();
                d.year = year;

                db.years.Add(d);
                db.SaveChanges();
                stralert = "saveyear";
            }
            return stralert;
        }
        //list of aging report
        public static List<AgingReport> GetListAgingPreBIRProjected(string yr, string month)//PRE-BIR Projected
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.year_notice_of_fullpayment_date == yr && c.month_notice_of_fullpayment_date == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAging(string yr, string month)//PRE-BIR Actual
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearsubpro == yr && c.monthsubpro == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAginBIRSUBProjected(string yr, string month)//BIR SUBMISSION Projected
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearsubmissiondateprojected30 == yr && c.monthsubmissiondateprojected30 == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingActual(string yr, string month)//BIR SUBMISSION Actual
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearsubactbir == yr && c.monthsubactbir == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingBirRelProjected90days(string yr, string month)//Projected +90 days
        {
            UsersContext db = new UsersContext();             
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearbirbirreaprojected90 == yr && c.monthbirreaprojected90 == month
                      select c).ToList();                
            return df.ToList();            
        }
        //list of aging report
        public static List<AgingReport> GetListAgingBirRelActual(string yr, string month)//Bir Release Actual
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearreabir == yr && c.monthreabir == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingPreRodProjected45days(string yr, string month)//PRE-ROD SUBMISSION Projected 45 days
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yr_prerodsubprojected45 == yr && c.mo_prerodsubprojected45 == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingPreRodActual(string yr, string month)//PRE-ROD SUBMISSION Actual
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearsubrod == yr && c.monthsubrod == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingRodProjected60days(string yr, string month)//ROD RELEASED Projected 60 days
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yr_rodsubprojected60 == yr && c.mo_rodsubprojected60 == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingRodActual(string yr, string month)//ROD RELEASED Actual
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearrearod == yr && c.monthrearod == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingPreAssProjected15days(string yr, string month)//ROD RELEASED Projected 60 days
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yr_preasssubprojected15 == yr && c.mo_preasssubprojected15 == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingPreAssActual(string yr, string month)//ROD RELEASED Actual
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearsubprea == yr && c.monthsubpreass == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingAssRelProjected60days(string yr, string month)//ROD RELEASED Projected 60 days
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yr_assrelprojected60 == yr && c.mo_assrelprojected60 == month
                      select c).ToList();
            return df.ToList();
        }
        //list of aging report
        public static List<AgingReport> GetListAgingAssRelActual(string yr, string month)//ROD RELEASED Projected 60 days
        {
            UsersContext db = new UsersContext();
            var df = (from c in db.agingreports
                      where c.status == 1 && c.yearreaass == yr && c.monthreaass == month
                      select c).ToList();
            return df.ToList();
        }
        ////Save TotalDaysCount
        [HttpPost]
        public string TotalDaysCount(int id, int NineTy, int OneEighty, int TreeSixfiven, int SevinTreeZiro, int SevinTreeOne)
        {
            TotalDaysCount d = db.totaldayscounts.Find(id);
            d.ninetydays = NineTy;
            d.oneeighty = OneEighty;
            d.treesixfive = TreeSixfiven;
            d.sevintreeziro = SevinTreeZiro;
            d.sevintreeone = SevinTreeOne;

            db.Entry(d).State = EntityState.Modified;
            db.SaveChanges();
            return "Savetotaldayscount";
        }
        // GET: /Account/ExternalLoginFailure
        public static string getyears()
        {
            UsersContext db = new UsersContext();
            var admin = from a in db.Account
                        where a.status == 1
                        select a;

            int stat = 0;
            var hdm = "";
            var gen = "";
            foreach (var c in admin)
            {
                stat = c.status;
                hdm = c.financing;
                gen = c.generatedcode;
            }
            if (stat == 1) //pre bir
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date BIR submission Projected
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual BIR submission Projected
                {
                    var prebir = (from pb in db.bir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().submission;
                }
            }
            else if (stat == 2) //bir
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date BIR submission Actual
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual BIR submission Actual
                {
                    var prebir = (from pb in db.bir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().submission;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else if (stat == 1) //pre bir
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date BIR RELEASED Projected + 90 days
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual BIR RELEASED Projected
                {
                    var prebir = (from pb in db.bir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().released;
                }
            }
            else if (stat == 2) //bir
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date BIR RELEASED Actual
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual BIR RELEASED Actual
                {
                    var prebir = (from pb in db.bir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().released;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else if (stat == 3) //PRE-ROD
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date PRE-ROD SUBMISSION	Projected + 45 days
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                    DateTime newdate = DateTime.Now.AddDays(45);
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual PRE-ROD SUBMISSION Projected
                {
                    var prebir = (from pb in db.rods
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_submitted;
                    DateTime newdate = DateTime.Now.AddDays(45);
                }
            }
            else if (stat == 3) //PRE-ROD
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date PRE-ROD RELEASED Actual
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual PRE-ROD RELEASED Actual
                {
                    var prebir = (from pb in db.rods
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_submitted;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else if (stat == 4) //ROD
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date ROD RELEASED Projected + 60 days
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                    DateTime newdate = DateTime.Now.AddDays(60);
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual ROD RELEASED Projected
                {
                    var prebir = (from pb in db.rods
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_released;
                    DateTime newdate = DateTime.Now.AddDays(60);
                }
            }
            else if (stat == 4) //ROD
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date ROD RELEASED Actual
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual ROD RELEASED Actual
                {
                    var prebir = (from pb in db.rods
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_released;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else if (stat == 5) //Pre-Assessor
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date Pre-Assessor SUBMISSION Projected + 60 days
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                    DateTime newdate = DateTime.Now.AddDays(15);
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual Pre-Assessor SUBMISSION Projected
                {
                    var prebir = (from pb in db.preassessors
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_completed;
                    DateTime newdate = DateTime.Now.AddDays(15);
                }
            }
            else if (stat == 5) //Pre-Assessor
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date Pre-Assessor SUBMISSION Actual
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual Pre-Assessor SUBMISSION Actual
                {
                    var prebir = (from pb in db.preassessors
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_completed;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else if (stat == 6) //Assessor
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date Assessor SUBMISSION Projected + 60 days
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                    DateTime newdate = DateTime.Now.AddDays(60);
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual Assessor SUBMISSION Projected
                {
                    var prebir = (from pb in db.assessors
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_released;
                    DateTime newdate = DateTime.Now.AddDays(60);
                }
            }
            else if (stat == 6) //Assessor
            {
                if (hdm == "HDMF")//HDMF - projected based on Fulll Payment Date Assessor RELEASED Actual
                {
                    var prebir = (from pb in db.Prebir
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().unit_no;
                }
                else if (hdm == "BANK" && hdm == "IHF")//IHF /BANK - projected based on actual Assessor RELEASED Actual
                {
                    var prebir = (from pb in db.assessors
                                  where pb.generatedcode == gen
                                  select pb).FirstOrDefault().date_released;
                }
            }

            return "";
        }
        private void PrebirDistinct()
        {
            var prebir = db.Prebir
           .Where(x => x.status == 2)
           .Select(x => new
           {
               Value = x.unit_no,
               Text = x.unit_no
           }).ToList().Distinct();

            ViewBag.PreBir = prebir;
        }
        //Get Aging Report
        public static string UnitNo_Financing(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.bir
                         where emp.generatedcode == generatedcode
                        select emp).FirstOrDefault().submission; 
            if (admin != null)
            {
                return admin.ToString();
            }
            else
            {
                return "";
            }
        }
        //Get Aging Report fullpayment
        public static string fullpayment(string generatedcode)
        {            
            UsersContext db = new UsersContext();
            var admin = (from emp in db.Prebir
                         where emp.generatedcode == generatedcode
                         select emp).FirstOrDefault().unit_no;
            if (admin != null)
            {
                return admin.ToString(); ;
            }
            else
            {
                return null;
            }
        }
        //Get Aging Report DateReleased
        public static string DateReleased(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.bir
                         where emp.generatedcode == generatedcode
                         select emp).FirstOrDefault().submission;
            if (admin != null)
            {
                return admin.ToString();
            }
            else
            {
                return "";
            }
        }
        //Get Aging Report DateSubmitted
        public static string DateSubmitted(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.bir
                         where emp.generatedcode == generatedcode
                         select emp).FirstOrDefault().submission;
            if (admin != null)
            {
                return admin.ToString();
            }
            else
            {
                return "";
            }
        }
        //Get Aging Report DateReleased
        public static string DateReleasedROD(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.bir
                         where emp.generatedcode == generatedcode
                         select emp).FirstOrDefault().submission;
            if (admin != null)
            {
                return admin.ToString();
            }
            else
            {
                return "";
            }
        }
        //Get Aging Report Date Submitted Pre-Assessor
        public static string DateSubmittedPreAssessor(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.bir
                         where emp.generatedcode == generatedcode
                         select emp).FirstOrDefault().submission;
            if (admin != null)
            {
                return admin.ToString();
            }
            else
            {
                return "";
            }
        }
        //Get Aging Report Date date_released Assessor
        public static string DateReleasedAssessor(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var admin = (from emp in db.assessors
                         where emp.generatedcode == generatedcode
                         select emp).FirstOrDefault().date_released;
            if (admin != null)
            {
                return admin.ToString();
            }
            else
            {
                return "";
            }
        }

    }
}
