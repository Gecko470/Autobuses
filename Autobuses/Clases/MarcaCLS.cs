using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class MarcaCLS
    {
        [Display(Name = "Id")]
        public int IIDMARCA { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100, ErrorMessage = "Max. 100 caracteres..")]
        public string NOMBRE { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        [StringLength(100, ErrorMessage = "Max. 200 caracteres..")]
        [DataType(DataType.MultilineText)]
        public string DESCRIPCION { get; set; }
        public int? BHABILITADO { get; set; }

        //ADICIONAMOS
        public string MensajeError { get; set; }
    }
}