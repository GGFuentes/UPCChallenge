using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPCChallenge.Models.ViewModels
{
    public class SolicitudViewModel
    {
        public int IdSolicitud { get; set; }
        public int IdAlumno { get; set; }
        public System.DateTime FechaSolicitud { get; set; }
        public string CodRegistrante { get; set; }
        public string Carrera { get; set; }
        public string Periodo { get; set; }
        public List<DetalleSolicitud> detalleSolicituds { get; set; }
    }
}