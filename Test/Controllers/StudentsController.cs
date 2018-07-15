using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    [Authorize(Roles = "Principal,Teacher,Student")]
    public class StudentsController : Controller
    {
        private DbMigrationExampleEntities1 db = new DbMigrationExampleEntities1();
        private ApplicationDbContext db2 = new ApplicationDbContext();

        // GET: Students
        [Authorize(Roles = "Principal,Teacher")]
        public ActionResult Index()
        {
           
            return View(db.Students.ToList());
        }
        //Öğrencilerin Bilgilerini Göstermek. UserId kullanarak
        [Authorize(Roles = "Principal,Teacher,Student")]
        public ActionResult DetailsbyUserId(String id)
        {
            var studentId = db.Students.Where(i => i.UserId == id).First();
            return RedirectToAction("Details","Students",new { id=studentId.Id });
        }


        // GET: Students/Details/5
        [Authorize(Roles = "Principal,Teacher,Student")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            ViewBag.userInfo = db2.Users.Find(student.UserId);
            var reports = student.StudentsReports.ToList();
            ViewBag.reports = reports;
            var curricula = db.Curricula.Where(i => i.TermId == student.TermId).ToList();
            int curriculumLength = curricula.Count();
            //Toplam devamsızlık ve karne basma uygunluk hesabı
            int TotalAbsent = 0;
            Boolean ready = true;
            int TotalLessons = 0;
            if (reports != null)
            {
                foreach (var i in reports)
                {
                    if(!i.Absent.HasValue)
                    {
                        i.Absent = 0;
                    }
                    TotalAbsent = TotalAbsent + (int)i.Absent;
                    //Eğer bitane bile false var sa bütün kayıtlar hazır değil demektir.
                    if(i.Ready == false)
                    {
                        ready = false;
                    }
                    TotalLessons = TotalLessons + 1;
                }

            }
            //Öğrencinin bütün dersleri girilmişmi
            if (!(TotalLessons == curriculumLength))
            {
                ready = false;
            }
            ViewBag.totalAbsent = TotalAbsent;
            ViewBag.ready = ready;

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Principal,Teacher")]
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db2.Users.Where(i => i.UserRole == "Student"), "Id", "Email");
            ViewBag.TermId = new SelectList(db.Terms, "Id", "AcademicTerm");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Principal,Teacher")]
        public ActionResult Create([Bind(Include = "Id,UserId,StudentNo,TermId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Principal,Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            ViewBag.UserId = new SelectList(db2.Users, "Id", "Email",student.UserId);
            ViewBag.TermId = new SelectList(db.Terms, "Id", "AcademicTerm",student.TermId);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Principal,Teacher")]
        public ActionResult Edit([Bind(Include = "Id,UserId,StudentNo,TermId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Principal,Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Principal,Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //Export html element to pdf
        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
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
