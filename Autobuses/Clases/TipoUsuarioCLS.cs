using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class TipoUsuarioCLS
    {
        [Display(Name = "Id")]
        public int IIDTIPOUSUARIO { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Nombre")]
        public string NOMBRE { get; set; }
        public int BHABILITADO { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Descripción")]
        public string DESCRIPCION { get; set; }
    }
}