using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPCChallenge.Models
{
    public class CursoViewModel
    {
        public int IdCurso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NroCreditos { get; set; }
        public bool Activo { get; set; }
    }
}