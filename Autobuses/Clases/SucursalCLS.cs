using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class SucursalCLS
    {
        [Display(Name = "Id")]
        public int IIDSUCURSAL { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100, ErrorMessage = "Máx. 100 caracteres..")]
        public string NOMBRE { get; set; }
        [Display(Name = "Dirección")]
        [Required]
        [StringLength(200, ErrorMessage = "Máx. 200 caracteres..")]
        public string DIRECCION { get; set; }
        [Display(Name = "Teléfono")]
        [Required]
        [StringLength(10, ErrorMessage = "Máx. 10 caracteres..")]
        public string TELEFONO { get; set; }
        [Display(Name = "Email")]
        [Required]
        [StringLength(100, ErrorMessage = "Máx. 100 caracteres..")]
        [EmailAddress(ErrorMessage = "Debe escribir un mail válido..")]
        public string EMAIL { get; set; }
        [Display(Name = "Fecha de apertura")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FECHAAPERTURA { get; set; }
        public int? BHABILITADO { get; set; }

        //ADICIONAMOS
        public string MensajeError { get; set; }
    }
}