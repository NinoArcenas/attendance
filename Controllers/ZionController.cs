using iTextSharp.text.pdf.qrcode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AttendanceManagementSystem.Filters;
using AttendanceManagementSystem.Models;
using WebMatrix.WebData;

namespace AttendanceManagementSystem.Controllers
{
    public class ZionController : Controller
    {
        // GET: Zion
        private UsersContext db = new UsersContext();
        public ActionResult ZionMaster()
        {
            var goal = from a in db.Ziongoals
                         where a.status == 1
                         select a;
            ViewBag.Goal = goal;

            MemberList();
            ListofPositionHolder();

            return View();
        }

        public ActionResult Attendance()
        {
            var bro = from a in db.Zionbrothers
                      where a.status == 1
                      select a;
            ViewBag.Brothers = bro;

            ListofPositionHolder();
            ListofGroupName();

            return View();
        }

        public static List<Tuple<string, DateTime?, int>> GetAttendanceCounts()
        {
            using (var db = new UsersContext())
            {
                // 1) Let EF build the SQL using an anonymous type
                var rows = db.ZionAttendances
                    .Where(x => x.status == 1)
                    .GroupBy(x => new { x.grouped_gencode, Date = DbFunctions.TruncateTime(x.date) })
                    .Select(g => new
                    {
                        grouped_gencode = g.Key.grouped_gencode,
                        date = g.Key.Date,          // DateTime? because of TruncateTime
                        count = g.Count()
                    })
                    .ToList(); // 2) MATERIALIZE IN MEMORY

                // 3) Now convert to Tuple in memory (EF no longer involved)
                return rows
                    .Select(r => Tuple.Create(r.grouped_gencode, r.date, r.count))
                    .OrderBy(t => t.Item2)
                    .ToList();
            }
        }

        //get Attendance list.. used this in view Attendance
        public static List<ZionAttendance> AttendanceListUsinggencode(string gencode)
        {
            UsersContext db = new UsersContext();
            var emp = from e in db.ZionAttendances
                      where e.status == 1 && e.generated_code == gencode
                      select e;
            return emp.ToList();
        }

        public static List<ZionAttendance> GetAllAttendance()
        {
            UsersContext db = new UsersContext();
            var emp = from e in db.ZionAttendances
                      where e.status == 1
                      select e;
            return emp.ToList();
        }

        public ActionResult ViewAttendance(string gencode)
        {
            ViewBag.viewattendance = gencode;

            return View();
        }

        [HttpGet]
        public JsonResult GetAttendanceById(int id)
        {
            using (var db = new UsersContext())
            {
                var data = db.ZionAttendances.FirstOrDefault(a => a.id == id);

                if (data == null)
                    return Json(null, JsonRequestBehavior.AllowGet);

                // Format date in C# after fetching from DB
                var result = new
                {
                    data.id,
                    data.generated_code,
                    data.grouped_gencode,
                    data.members_gencode,
                    date = data.date.HasValue ? data.date.Value.ToString("yyyy-MM-dd") : "",
                    data.total_members,
                    data.first_week,
                    data.second_week,
                    data.therd_week,
                    data.forth_week,
                    data.fifth_week,
                    data.status
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateAttendance(ZionAttendance model)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var existing = db.ZionAttendances.FirstOrDefault(a => a.id == model.id);
                    if (existing != null)
                    {
                        existing.total_members = model.total_members;
                        existing.date = model.date;
                        existing.first_week = model.first_week;
                        existing.second_week = model.second_week;
                        existing.therd_week = model.therd_week;
                        existing.forth_week = model.forth_week;
                        existing.fifth_week = model.fifth_week;
                        existing.status = model.status;
                        existing.date_update = DateTime.Now;

                        db.SaveChanges();

                        return Json(new { success = true, message = "Attendance updated successfully!" });
                    }
                    return Json(new { success = false, message = "Record not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //get list of Attendance used this in view Attendance using gencode
        public static List<ZionAttendance> GetListOfAttendance_Item(string gencode)
        {
            using (UsersContext db = new UsersContext())
            {
                return db.ZionAttendances
                         .Where(x => x.grouped_gencode == gencode && x.status == 1)
                         .GroupBy(x => x.members_gencode)   // 👈 pick unique field
                         .Select(g => g.FirstOrDefault()) // take first in each group
                         .OrderBy(x => x.id)
                         .ToList();
            }
        }

        //get list of Attendance using members_gencode .. used this in view Attendance
        public static List<ZionAttendance> GetListOfAttendancePresentUsingGencode(string gencode)
        {
            using (UsersContext db = new UsersContext())
            {
                return db.ZionAttendances
                     .Where(x => x.status == 1 && x.members_gencode == gencode)
                         .OrderBy(x => x.id)
                         .ToList();
            }
        }

        //get fullname using idno
        // Get fullname using generatedcode
        public static string GetMemberFullnameUsingGencode(string generatedcode)
        {
            using (var db = new UsersContext())
            {
                var member = db.Zionbrothers
                               .FirstOrDefault(e => e.generatedcode == generatedcode);

                return member?.full_name ?? string.Empty;
            }
        }

        //get fullname using idno
        public static string GetMemberLifeNumberUsingGencode(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var emp = from e in db.Zionbrothers
                      where e.generatedcode == generatedcode
                      select e;
            int ifexist = emp.Count();

            if (ifexist > 0)
            {
                var lifenumber = emp.FirstOrDefault().life_number.ToString();
                return lifenumber;
            }
            else
            {
                return "";
            }
        }

        //List of brothers name
        private void MemberList()
        {
            var member = db.Zionbrothers
        .Select(x => new {
            Value = x.id + "*" + x.life_number + "*" + x.full_name,
            Text = x.full_name
        }).ToList();
            ViewBag.members = member;
        }

        //List of Position Holder
        private void ListofPositionHolder()
        {
            var member = db.ZionP0sitionHolders
        .Select(x => new {
            Value = x.generated_code + "*" + x.life_number +"*"+ x.position,
            Text = x.full_name
        }).ToList();
            ViewBag.leader = member;
        }

        private void ListofGroupName() 
        { 
            var member = db.ZionGroupNames
                .Select(x => new { 
                    Value = x.gencode + "*" + x.total_members, 
                    Text = x.group_name }).ToList(); 
            ViewBag.groupname = member; 
        }

        [HttpGet]
        public JsonResult GetGroupMembers(string groupNameGencode)
        {
            using (UsersContext db = new UsersContext())
            {
                var data = (from m in db.Zionbrothers
                            join g in db.ZionGroupeds
                                on m.generatedcode equals g.group_name_gencode
                            where g.generatedcode == groupNameGencode
                                  && m.status == 1
                                  && g.status == 1
                            select new
                            {
                                FullName = m.full_name,
                                LifeNumber = m.life_number,
                                Generatedcode = m.generatedcode
                            }).ToList();

                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }


        //get list of Members
        public static List<Zionbrother> GetListOfMembers()
        {
               
            using (UsersContext db = new UsersContext())
            { return db.Zionbrothers
                     .Where(x => x.status == 1)
                         .OrderByDescending(x => x.full_name)
                         .ThenBy(x => x.id)
                         .ToList();
            }
        }

        //get list of Members
        public static List<ZionGroupName> GetListOfGroupName()
        {
            using (UsersContext db = new UsersContext())
            {
                return db.ZionGroupNames
                .Where(x => x.status == 1)
                .GroupBy(x => x.gencode)                   // group by id
                .Select(g => g.FirstOrDefault())      // take only the first from each group
                .OrderBy(x => x.id)
                .ToList();
            }
        }

        public static List<ZionGroupName> GetListOfGroupNameUsingGencode(string gencode)
        {
            using (UsersContext db = new UsersContext())
            {
                return db.ZionGroupNames
                .Where(x => x.status == 1 && x.gencode == gencode)
                .GroupBy(x => x.gencode)                   // group by id
                .Select(g => g.FirstOrDefault())      // take only the first from each group
                .OrderBy(x => x.id)
                .ToList();
            }
        }

        [HttpGet]
        public JsonResult GetGroupMembersByGroup(string gencode)
        {
            using (var db = new UsersContext()) // use your actual DbContext
            {
                var data = (from g in db.ZionGroupeds
                            join m in db.Zionbrothers on g.group_name_gencode equals m.generatedcode
                            where g.generatedcode == gencode && g.status == 1 && m.status == 1
                            select new
                            {
                                g.id,
                                g.generatedcode,
                                g.group_name_gencode,
                                m.full_name,
                                m.life_number,
                                m.gender,
                                m.status,
                                m.date_created,
                                m.created_by
                            }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RemoveGroupMember(int id)
        {
            using (var db = new UsersContext()) // replace with your actual DbContext
            {
                var record = db.ZionGroupeds.FirstOrDefault(x => x.id == id);
                if (record != null)
                {
                    // Step 1: Mark as removed
                    record.status = 2;
                    record.date_update = DateTime.Now;
                    record.updated_by = User.Identity.Name; // optional
                    db.SaveChanges();

                    // Step 2: Find related group_name entry (using g.generatedcode)
                    var groupCode = record.generatedcode;

                    // Recount active members for that group
                    var totalActive = db.ZionGroupeds
                        .Count(x => x.generatedcode == groupCode && x.status != 2);

                    // Step 3: Update total_members in zion_group_name
                    var groupNameRecord = db.ZionGroupNames.FirstOrDefault(g => g.gencode == groupCode);
                    if (groupNameRecord != null)
                    {
                        groupNameRecord.total_members = totalActive;
                        groupNameRecord.date_update = DateTime.Now;
                        groupNameRecord.update_by = User.Identity.Name;
                        db.SaveChanges();
                    }

                    return Json(new { success = true, message = "Member removed and total members updated." });
                }

                return Json(new { success = false, message = "Member not found." });
            }
        }

        [HttpPost]
        public JsonResult DeleteGroupName(int id)
        {
            try
            {
                using (var db = new UsersContext()) // replace with your actual DbContext
                {
                    var group = db.ZionGroupNames.FirstOrDefault(x => x.id == id);

                    if (group == null)
                        return Json(new { success = false, message = "Group not found." });

                    // Soft delete: update status = 2 and set delete date/user
                    group.status = 2;
                    group.date_delete = DateTime.Now;
                    group.deleted_by = User.Identity.Name; // or any identifier you want

                    db.SaveChanges();

                    return Json(new { success = true, message = "Group has been successfully deleted (status updated)." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting group: " + ex.Message });
            }
        }

        //get fullname using idno
        public static string GetFullnameUsingGencode(string generatedcode)
        {
            UsersContext db = new UsersContext();
            var emp = from e in db.ZionP0sitionHolders
                      where e.generated_code == generatedcode
                      select e;
            int ifexist = emp.Count();

            if (ifexist > 0)
            {
                var fullname = emp.FirstOrDefault().full_name.ToString();
                return fullname;
            }
            else
            {
                return "";
            }
        }

        //POST:GET /ZionMaster/Position Holder
        public ActionResult GetPositionHolder()
        {
            using (var db = new UsersContext())
            {
                var positionHolders = db.ZionP0sitionHolders.ToList();
                return Json(positionHolders, JsonRequestBehavior.AllowGet);
            }
        }

        //POST:UPDATE /ZionMaster/Position Holder
        [HttpPost]
        public JsonResult UpdatePositionHolder(ZionP0sitionHolder updatedpositionholder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing Position Holder
                    var positionH = db.ZionP0sitionHolders.SingleOrDefault(e => e.id == updatedpositionholder.id);

                    if (positionH == null)
                    {
                        return Json("Position Holder not found", JsonRequestBehavior.AllowGet);
                    }

                    // Update Position
                    positionH.position = updatedpositionholder.position;

                    // Save changes to the database
                    db.Entry(positionH).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json("Position updated successfully", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("Invalid input", JsonRequestBehavior.AllowGet);
        }

        //Save Brothers
        [HttpPost]
        public JsonResult Save_Brothers(string AddF, string AddNo, string AddGencode, string gender)
        {
            string message = "";
            bool success = false;
            int isexist = (from a in db.Zionbrothers
                           where a.full_name == AddF && a.life_number == AddNo
                           select a).Count();
            if (isexist > 0)
            {
                message = "This Brother already exists.";
            }
            else
            {
                try
                {
                    Zionbrother d = new Zionbrother();
                    d.full_name = AddF;
                    d.life_number = AddNo;
                    d.generatedcode = AddGencode;
                    d.gender = gender;
                    d.created_by = User.Identity.Name;
                    d.date_created = DateTime.Now;
                    d.status = 1;
                    db.Zionbrothers.Add(d);
                    db.SaveChanges();

                    success = true;
                    message = "Brother saved successfully.";
                }
                catch (Exception ex)
                {
                    message = "Error saving the brother: " + ex.Message;
                }
            }
            return Json(new { success = success, message = message });
        }

        //Save Attendance
        [HttpPost]
        public JsonResult Save_Attendance(string attendanceGencode, string groupNameGencode, decimal totalmembers, DateTime date)
        {
            string message = "";
            bool success = false;
            int isexist = (from a in db.ZionAttendances
                           where a.grouped_gencode == groupNameGencode && a.date == date
                           select a).Count();
            if (isexist > 0)
            {
                message = "This Attendance already exists.";
            }
            else
            {
                try
                {
                    ZionAttendance d = new ZionAttendance();
                    d.generated_code = attendanceGencode;
                    d.grouped_gencode = groupNameGencode;
                    d.date = date;
                    d.total_members = totalmembers;
                    d.created_by = User.Identity.Name;
                    d.date_created = DateTime.Now;
                    d.status = 1;
                    db.ZionAttendances.Add(d);
                    db.SaveChanges();

                    success = true;
                    message = "Attendance created successfully.";
                }
                catch (Exception ex)
                {
                    message = "Error saving the attendance: " + ex.Message;
                }
            }
            return Json(new { success = success, message = message });
        }

        //Save Position Holder
        [HttpPost]
        public JsonResult Save_PositionHolder(string addGeneratedCode, int addPositionHolderID, string addMemberfullname, string addPositionLifeNumber, string addPosition)
        {
            string message = "";
            bool success = false;
            int isexist = (from a in db.ZionP0sitionHolders
                           where a.member_id == addPositionHolderID && a.full_name == addMemberfullname && a.life_number == addPositionLifeNumber && a.position == addPosition
                           select a).Count();
            if (isexist > 0)
            {
                message = "This Position Holder already exists.";
            }
            else
            {
                try
                {
                    ZionP0sitionHolder d = new ZionP0sitionHolder();
                    d.generated_code = addGeneratedCode;
                    d.member_id = addPositionHolderID;
                    d.full_name = addMemberfullname;
                    d.life_number = addPositionLifeNumber;
                    d.position = addPosition;
                    d.created_by = User.Identity.Name;
                    d.date_created = DateTime.Now;
                    d.status = 1;
                    db.ZionP0sitionHolders.Add(d);
                    db.SaveChanges();

                    success = true;
                    message = "Position holder saved successfully.";
                }
                catch (Exception ex)
                {
                    message = "Error saving the position holder: " + ex.Message;
                }
            }
            return Json(new { success = success, message = message });
        }

        //Save Group Name
        [HttpPost]
        public JsonResult Save_GroupName(string addGeneratedCode, string addGroupName, string addLeaderID, int addTotalNumber)
        {
            string message = "";
            bool success = false;
            int isexist = (from a in db.ZionGroupNames
                           where a.gencode == addGeneratedCode && a.group_name == addGroupName && a.leader_gencode == addLeaderID
                           select a).Count();
            if (isexist > 0)
            {
                message = "This Group already exists.";
            }
            else
            {
                try
                {
                    ZionGroupName d = new ZionGroupName();
                    d.gencode = addGeneratedCode;
                    d.group_name = addGroupName;
                    d.leader_gencode = addLeaderID;
                    d.total_members = addTotalNumber;
                    d.created_by = User.Identity.Name;
                    d.date_create = DateTime.Now;
                    d.status = 1;
                    db.ZionGroupNames.Add(d);
                    db.SaveChanges();

                    success = true;
                    message = "Group name saved successfully.";
                }
                catch (Exception ex)
                {
                    message = "Error saving the group: " + ex.Message;
                }
            }
            return Json(new { success = success, message = message });
        }

        [HttpPost]
        public JsonResult SaveGroup(string groupgeneratedcode, string groupgencode)
        {
            try
            {
                ZionGrouped dp = new ZionGrouped
                {
                    generatedcode = groupgeneratedcode,
                    group_name_gencode = groupgencode,
                    date_create = DateTime.Now,
                    created_by = User?.Identity?.Name,
                    status = 1
                };

                db.ZionGroupeds.Add(dp);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public static string GetAccessType_usingUsername(string idno)
        {
            var msg = "";
            UsersContext db = new UsersContext();
            try
            {
                var admin = (from emp in db.Users
                             where emp.username == idno
                             select emp).FirstOrDefault().accesstype;
                return admin.ToString();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        //Get Fname Display sa Tible
        public static string GetFname(string idno)
        {
            var msg = "";
            UsersContext db = new UsersContext();
            try
            {
                var admin = (from emp in db.Users
                             where emp.username == idno
                             select emp).FirstOrDefault().fname;
                return admin.ToString();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        //Get Fname Display sa Tible
        public static string Accesstypes(string idno)
        {
            using (UsersContext db = new UsersContext())
            {
                var accesstype = db.Users
                    .Where(emp => emp.username == idno)
                    .Select(emp => (int?)emp.accesstype)
                    .FirstOrDefault();

                return accesstype.HasValue ? accesstype.Value.ToString() : string.Empty;
            }
        }

        // Replace this method:
        public static string Accesstype(string idno)
        {
            using (UsersContext db = new UsersContext())
            {
                var user = (from emp in db.Users
                            where emp.username == idno
                            select emp).FirstOrDefault();

                // Check if user is found and accesstype is not null
                if (user != null)
                {
                    return user.accesstype.ToString();
                }
                else
                {
                    return string.Empty; // or return "User" / "Unknown" if you prefer
                }
            }
        }

    }
}