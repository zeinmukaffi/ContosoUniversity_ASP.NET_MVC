using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class EnrollmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Enrollment
        public ActionResult Index(string sortOrder, string searchTitle, string searchName,  string searchGrade, string currentFilter, string currentFilter2, string currentFilter3, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.GradeSortParm = String.IsNullOrEmpty(sortOrder) ? "grade_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.TitleSortParm = sortOrder == "title" ? "title_desc" : "title";

            
            if (searchTitle != null)
            {
                page = 1;
            }
            else
            {
                searchTitle = currentFilter;
            }

            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFilter2;
            }

            if (searchGrade != null)
            {
                page = 1;
            }
            else
            {
                searchGrade = currentFilter3;
            }

            ViewBag.CurrentFilter = searchTitle;
            ViewBag.CurrentFilter2 = searchName;
            ViewBag.CurrentFilter3 = searchGrade;

            var enroll = from s in db.Enrollments
                              select s;
            if (!String.IsNullOrEmpty(searchName))
            {
                enroll = enroll.Where(s => s.Student.LastName.Contains(searchName)
                                       || s.Student.FirstMidName.Contains(searchName));
            }
            
            if (!String.IsNullOrEmpty(searchTitle))
            {
                enroll = enroll.Where(s => s.Course.Title.Contains(searchTitle));
            }
            
            if (!String.IsNullOrEmpty(searchGrade))
            {
                enroll = enroll.Where(s => s.Grade.ToString().Contains(searchGrade));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    enroll = enroll.OrderByDescending(s => s.Student.FirstMidName);
                    break;
                case "name":
                    enroll = enroll.OrderBy(s => s.Student.FirstMidName);
                    break;
                case "title_desc":
                    enroll = enroll.OrderByDescending(s => s.Course.Title);
                    break;
                case "title":
                    enroll = enroll.OrderBy(s => s.Course.Title);
                    break;
                case "grade_desc":
                    enroll = enroll.OrderByDescending(s => s.Grade);
                    break;
                default:
                    enroll = enroll.OrderBy(s => s.Grade);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var enrollments = db.Enrollments.Include(s => s.Course).Include(s => s.Student);
            return View(enroll.ToPagedList(pageNumber, pageSize));
        }

        // GET: Enrollment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollment/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FirstMidName", "LastName");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            try
            {
            if (ModelState.IsValid)
                        {
                            db.Enrollments.Add(enrollment);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
            }
            catch(DataException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FirstMidName", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            try
            {
                if (ModelState.IsValid)
                            {
                                db.Entry(enrollment).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
            }
            catch
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Enrollment enrollment = db.Enrollments.Find(id);
                        db.Enrollments.Remove(enrollment);
                        db.SaveChanges();
            }
            catch
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
