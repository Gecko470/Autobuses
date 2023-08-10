using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class RolCLS
    {
        [Display(Name = "Id")]
        public int IIDROL { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string NOMBRE { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        public string DESCRIPCION { get; set; }
        public int BHABILITADO { get; set; }
    }
}