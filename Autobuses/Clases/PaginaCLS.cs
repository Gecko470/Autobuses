using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class PaginaCLS
    {
        [Display(Name = "Id")]
        public int IIDPAGINA { get; set; }
        [Display(Name = "Mensaje")]
        [Required]
        public string MENSAJE { get; set; }
        [Display(Name = "Acción")]
        [Required]
        public string ACCION { get; set; }
        [Display(Name = "Controlador")]
        [Required]
        public string CONTROLADOR { get; set; }
        public int BHABILITADO { get; set; }
    }
}