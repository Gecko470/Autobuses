using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class RolPaginaCLS
    {
        [Display(Name = "Id")]
        public int IIDROLPAGINA { get; set; }
        [Display(Name = "Id Rol")]
        [Required]
        public int? IIDROL { get; set; }
        [Display(Name = "Id Página")]
        [Required]
        public int? IIDPAGINA { get; set; }
        public int BHABILITADO { get; set; }

        //ADICIONAMOS
        [Display(Name = "Rol")]
        public string Rol { get; set; }
        [Display(Name = "Página")]
        public string Pagina { get; set; }
    }
}