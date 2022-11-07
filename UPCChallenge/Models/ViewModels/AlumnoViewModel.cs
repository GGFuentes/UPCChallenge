using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPCChallenge.Models
{
    public class AlumnoViewModel
    {
        public int IdAlumno { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto { get; set; }
    }
}