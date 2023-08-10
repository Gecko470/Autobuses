using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class ClienteCLS
    {
        [Display(Name = "Id")]
        public int IIDCLIENTE { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100, ErrorMessage = "Máx. 100 caracteres..")]
        public string NOMBRE { get; set; }
        [Display(Name = "Ap. Paterno")]
        [Required]
        [StringLength(150, ErrorMessage = "Máx. 150 caracteres..")]
        public string APPATERNO { get; set; }
        [Display(Name = "Ap. Materno")]
        [Required]
        [StringLength(150, ErrorMessage = "Máx. 150 caracteres..")]
        public string APMATERNO { get; set; }
        [Display(Name = "Email")]
        [Required]
        [StringLength(200, ErrorMessage = "Máx. 200 caracteres..")]
        [EmailAddress(ErrorMessage = "Introduce un mail válido..")]
        public string EMAIL { get; set; }
        [Display(Name = "Tf. Fijo")]
        [Required]
        [StringLength(10, ErrorMessage = "Máx. 10 caracteres..")]
        public string TELEFONOFIJO { get; set; }
        [Display(Name = "Tf. Móvil")]
        [Required]
        [StringLength(10, ErrorMessage = "Máx. 10 caracteres..")]
        public string TELEFONOCELULAR { get; set; }
        [Display(Name = "Dirección")]
        [Required]
        [StringLength(200, ErrorMessage = "Máx. 200 caracteres..")]
        [DataType(DataType.MultilineText)]
        public string DIRECCION { get; set; }
        [Display(Name = "Sexo")]
        [Required]
        public int IIDSEXO  { get; set; }
        public int BHABILITADO { get; set; }
        [Display(Name = "Sexo")]
        public string SEXO => IIDSEXO == 1 ? "M" : "F";

        //ADICIONAMOS
        public string MensajeError { get; set; }
    }
}