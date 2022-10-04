using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private SchoolContext db = new SchoolContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(string sortOrder, string searchDate, string searchCount, int? page, string currentFilter, string currentFilter2)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.StudentSortParm = sortOrder == "count" ? "count_desc" : "count";

            IQueryable<EnrollmentDateGroup> data = from student in db.Students
                                                   group student by student.EnrollmentDate into dateGroup
                                                   select new EnrollmentDateGroup()
                                                   {
                                                       EnrollmentDate = dateGroup.Key,
                                                       StudentCount = dateGroup.Count()
                                                   };
            if (searchDate != null)
            {
                page = 1;
            }
            else
            {
                searchDate = currentFilter;
            }

            if (searchCount != null)
            {
                page = 1;
            }
            else
            {
                searchCount = currentFilter2;
            }

            ViewBag.CurrentFilter = searchDate;
            ViewBag.CurrentFilter2 = searchCount;

            if (!String.IsNullOrEmpty(searchDate))
            {
                data = data.Where(s => s.EnrollmentDate.ToString().Contains(searchDate));
            }

            if (!String.IsNullOrEmpty(searchCount))
            {
                data = data.Where(s => s.StudentCount.ToString().Contains(searchCount));
            }

            switch (sortOrder)
            {
                case "count_desc":
                    data = data.OrderByDescending(s => s.StudentCount);
                    break;
                case "count":
                    data = data.OrderBy(s => s.StudentCount);
                    break;
                case "date_desc":
                    data = data.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    data = data.OrderBy(s => s.EnrollmentDate);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(data.ToPagedList(pageNumber, pageSize));
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}