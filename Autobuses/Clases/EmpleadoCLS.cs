using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class EmpleadoCLS
    {
        [Display(Name = "Id")]
        public int IIDEMPLEADO { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres..")]
        public string NOMBRE { get; set; }
        [Display(Name = "Ap. Paterno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(200, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres..")]
        public string APPATERNO { get; set; }
        [Display(Name = "Ap. Materno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(200, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres..")]
        public string APMATERNO { get; set; }
        [Display(Name = "Fecha Contrato")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FECHACONTRATO { get; set; }
        [Display(Name = "Sueldo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Range(0, 5000, ErrorMessage = "El campo {0} debe estar en el rango entre {1} y {2}")]
        public decimal SUELDO { get; set; }
        [Display(Name = "Tipo Usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int IIDTIPOUSUARIO { get; set; }
        [Display(Name = "Tipo Contrato")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int IIDTIPOCONTRATO { get; set; }
        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int IIDSEXO { get; set; }
        public int BHABILITADO { get; set; }

        //ADICIONAMOS
        [Display(Name = "Tipo Usuario")]
        public string TipoUsuario{ get; set; }
        [Display(Name = "Tipo Contrato")]
        public string TipoContrato{ get; set; }
           [Display(Name = "Sexo")]
        public string Sexo{ get; set; }

        //ADICIONAMOS
        public string MensajeError { get; set; }
    }
}