using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class MataKuliahController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Course
        public ActionResult Index()
        {
            ViewBag.courses = db.Courses.ToList();
            CourseSearch model = new CourseSearch();
            return View(model);
        }

        public ActionResult IndexProses(CourseSearch model, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.CreditSortParm = sortOrder == "credit" ? "credit_desc" : "credit";
            ViewBag.CourseIDSortParm = sortOrder == "id" ? "id_desc" : "id";

            IEnumerable<Course> courses = from s in db.Courses
            select s;

            switch (sortOrder)
            {
                case "title_desc":
                    courses = courses.OrderByDescending(s => s.Title);
                    break;
                case "credit":
                    courses = courses.OrderBy(s => s.Credits);
                    break;
                case "credit_desc":
                    courses = courses.OrderByDescending(s => s.Credits);
                    break;
                case "id_desc":
                    courses = courses.OrderByDescending(s => s.CourseID);
                    break;
                case "id":
                    courses = courses.OrderBy(s => s.CourseID);
                    break;
                default:
                    courses = courses.OrderBy(s => s.Title);
                    break;
            }

            //if (ModelState.IsValid)
            //{
                if (model.CourseID != null)
                {
                    courses = courses.Where(s => s.CourseID.ToString().Contains(model.CourseID));
                }
                if (!String.IsNullOrEmpty(model.Title))
                {
                    courses = courses.Where(s => s.Title.Contains(model.Title));
                }
                if (model.Credit != null)
                {
                    courses = courses.Where(s => s.Credits.ToString().Contains(model.Credit));
                }
            //}

            ViewBag.courses = courses.ToList();
            return View("Index", model);
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = db.Courses.Find(id);
            if (TryUpdateModel(courseToUpdate, "",
               new string[] { "CourseID", "Title", "Credits" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(courseToUpdate);
        }

        // GET: Course/Delete/5
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
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Course course = db.Courses.Find(id);
                db.Courses.Remove(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (DataException/* dex */)
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
