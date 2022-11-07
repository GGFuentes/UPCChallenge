using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPCChallenge.Models;
using UPCChallenge.Models.ViewModels;

namespace UPCChallenge.Controllers
{
    public class SolicitudsController : Controller
    {
        private DBMatriculaEntities db = new DBMatriculaEntities();

        // GET: Solicituds
        public ActionResult Index()
        {
            var solicitud = db.Solicitud.Include(s => s.Alumno).Include(s => s.DetalleSolicitud);
            return View(solicitud.ToList());
        }

        // GET: Solicituds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }

        // GET: Solicituds/Create
        public ActionResult Create()
        {
            CargarDropDownList();

            return View();
        }

        // POST: Solicituds/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SolicitudViewModel solicitudViewModel)
        {
            if (ModelState.IsValid)
            {
                Solicitud solicitud = new Solicitud();
                solicitud.IdAlumno = solicitudViewModel.IdAlumno;
                solicitud.FechaSolicitud = solicitudViewModel.FechaSolicitud;
                solicitud.CodRegistrante = solicitudViewModel.CodRegistrante;
                solicitud.Carrera = solicitudViewModel.Carrera;
                solicitud.Periodo = solicitudViewModel.Periodo;
                db.Solicitud.Add(solicitud);
                db.SaveChanges();

                foreach(var item in solicitudViewModel.detalleSolicituds)
                {
                    item.IdSolicitud = solicitud.IdSolicitud;
                    db.DetalleSolicitud.Add(item);
                }
                return RedirectToAction("Index");
            }

            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "Nombres", solicitudViewModel.IdAlumno);
            return View(solicitudViewModel);
        }

        // GET: Solicituds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "Nombres", solicitud.IdAlumno);
            return View(solicitud);
        }

        // POST: Solicituds/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSolicitud,IdAlumno,FechaSolicitud,CodRegistrante,Carrera,Periodo")] SolicitudViewModel solicitud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "Nombres", solicitud.IdAlumno);
            return View(solicitud);
        }

        // GET: Solicituds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }

        // POST: Solicituds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solicitud solicitud = db.Solicitud.Find(id);
            db.Solicitud.Remove(solicitud);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void CargarDropDownList()
        {
            List<AlumnoViewModel> alumnoViewModels = new List<AlumnoViewModel>();
            alumnoViewModels = (from a in db.Alumno
                                select new AlumnoViewModel
                                {
                                    IdAlumno = a.IdAlumno,
                                    NombreCompleto = a.Apellidos + " " + a.Nombres

                                }).ToList();
            ViewBag.IdAlumno = new SelectList(alumnoViewModels, "IdAlumno", "NombreCompleto");
            List<CursoViewModel> cursoViewModels = new List<CursoViewModel>();

            cursoViewModels = (from a in db.Cursos
                               select new CursoViewModel
                               {
                                   IdCurso = a.IdCurso,
                                   Nombre = a.Nombre

                               }).ToList();
            ViewBag.IdCurso = new SelectList(cursoViewModels, "IdCurso", "Nombre");
        }

        public ActionResult ListarSolicitudes(string Periodo, DateTime fechaSolicitud, int IdAlumno, int IdCurso)
        {
            //1era Forma
            /*bool isEmptyPeriodo = string.IsNullOrEmpty(Periodo);
            bool isEmptyFechaSolicitud = string.IsNullOrEmpty(Periodo);
            bool isEmptyIdAlumno = IdAlumno == 0;
            var result = db.Solicitud
                .Where(
                s => 
                (isEmptyIdAlumno || s.IdAlumno==IdAlumno) &&
                (isEmptyPeriodo || s.Periodo == Periodo) &&
                (isEmptyFechaSolicitud || s.FechaSolicitud == fechaSolicitud)
                ).ToList();*/

            var result2 = from sol in db.Solicitud
                          join detalle in db.DetalleSolicitud on sol.IdSolicitud equals detalle.IdSolicitud
                          join curso in db.Cursos on detalle.IdCurso equals curso.IdCurso
                          where sol.Periodo == Periodo &&
                          sol.FechaSolicitud == fechaSolicitud &&
                          sol.IdAlumno == IdAlumno &&
                          curso.IdCurso == IdCurso
                          select new SolicitudViewModel()
                          {
                              IdSolicitud = sol.IdSolicitud,
                              IdAlumno = sol.IdAlumno,
                              Carrera = sol.Carrera,
                              Periodo = sol.Periodo,
                              CodRegistrante = sol.CodRegistrante                              
                          };
            return Json(JsonConvert.SerializeObject(result2));
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
