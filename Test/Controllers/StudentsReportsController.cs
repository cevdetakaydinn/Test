    using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class StudentsReportsController : Controller
    {
        private DbMigrationExampleEntities1 db = new DbMigrationExampleEntities1();

        // GET: StudentsReports Eğer parametre varsa öğrenciye ait dersnotlarını listele
        public ActionResult Index(int? i)
        {   
            if (i.HasValue)
            {
                var studentReports = db.StudentsReports.Where(x => x.StudentId == i);
                return View(studentReports.ToList());
            }
            else
            {
                var studentsReports = db.StudentsReports.Include(s => s.Curriculum).Include(s => s.Student);
                return View(studentsReports.ToList());
            }

        }

        // GET: StudentsReports/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsReport studentsReport = db.StudentsReports.Find(id);
            if (studentsReport == null)
            {
                return HttpNotFound();
            }
            return View(studentsReport);
        }

        // GET: StudentsReports/Create
        public ActionResult Create()
        {
            ViewBag.CirruculumId = new SelectList(db.Curricula, "Id", "Id");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "UserId");
            return View();
        }

        // POST: StudentsReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,StudentId,CirruculumId,FirstExam,SecondExam,Ready,Absent")] StudentsReport studentsReport)
        {
            if (ModelState.IsValid)
            {
                db.StudentsReports.Add(studentsReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CirruculumId = new SelectList(db.Curricula, "Id", "Id", studentsReport.CirruculumId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "UserId", studentsReport.StudentId);
            return View(studentsReport);
        }

        // GET: StudentsReports/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsReport studentsReport = db.StudentsReports.Find(id);
            if (studentsReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.CirruculumId = new SelectList(db.Curricula, "Id", "Id", studentsReport.CirruculumId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "UserId", studentsReport.StudentId);
            return View(studentsReport);
        }

        // POST: StudentsReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,StudentId,CirruculumId,FirstExam,SecondExam,Ready,Absent")] StudentsReport studentsReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentsReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CirruculumId = new SelectList(db.Curricula, "Id", "Id", studentsReport.CirruculumId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "UserId", studentsReport.StudentId);
            return View(studentsReport);
        }

        // GET: StudentsReports/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsReport studentsReport = db.StudentsReports.Find(id);
            if (studentsReport == null)
            {
                return HttpNotFound();
            }
            return View(studentsReport);
        }

        // POST: StudentsReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentsReport studentsReport = db.StudentsReports.Find(id);
            db.StudentsReports.Remove(studentsReport);
            db.SaveChanges();
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
