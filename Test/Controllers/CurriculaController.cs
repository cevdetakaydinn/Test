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
    public class CurriculaController : Controller
    {
        private DbMigrationExampleEntities1 db = new DbMigrationExampleEntities1();

        // GET: Curricula
        public ActionResult Index()
        {
            var curricula = db.Curricula.Include(c => c.Lesson).Include(c => c.Teacher).Include(c => c.Term);
            return View(curricula.ToList());
        }

        // GET: Curricula/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Find(id);
            if (curriculum == null)
            {
                return HttpNotFound();
            }
            return View(curriculum);
        }

        // GET: Curricula/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "UserId");
            ViewBag.TermId = new SelectList(db.Terms, "Id", "Term1");
            return View();
        }

        // POST: Curricula/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LessonId,TeacherId,TermId")] Curriculum curriculum)
        {
            if (ModelState.IsValid)
            {
                db.Curricula.Add(curriculum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Name", curriculum.LessonId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "UserId", curriculum.TeacherId);
            ViewBag.TermId = new SelectList(db.Terms, "Id", "Term1", curriculum.TermId);
            return View(curriculum);
        }

        // GET: Curricula/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Find(id);
            if (curriculum == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Name", curriculum.LessonId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "UserId", curriculum.TeacherId);
            ViewBag.TermId = new SelectList(db.Terms, "Id", "Term1", curriculum.TermId);
            return View(curriculum);
        }

        // POST: Curricula/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LessonId,TeacherId,TermId")] Curriculum curriculum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curriculum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonId = new SelectList(db.Lessons, "Id", "Name", curriculum.LessonId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "UserId", curriculum.TeacherId);
            ViewBag.TermId = new SelectList(db.Terms, "Id", "Term1", curriculum.TermId);
            return View(curriculum);
        }

        // GET: Curricula/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Find(id);
            if (curriculum == null)
            {
                return HttpNotFound();
            }
            return View(curriculum);
        }

        // POST: Curricula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curriculum curriculum = db.Curricula.Find(id);
            db.Curricula.Remove(curriculum);
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
