using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class BusCLS
    {
        [Display(Name = "Id")]
        public int IIDBUS { get; set; }
        [Required]
        [Display(Name = "Sucursal")]
        public int IIDSUCURSAL { get; set; }
        [Required]
        [Display(Name = "Tipo Bus")]
        public int IIDTIPOBUS { get; set; }
        [Display(Name = "Matrícula")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(15, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres...")]
        public string PLACA { get; set; }
        [Display(Name = "Fecha Compra")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FECHACOMPRA { get; set; }
        [Required]
        [Display(Name = "Modelo")]
        public int IIDMODELO { get; set; }
        [Display(Name = "Filas")]
        [Required]
        public int NUMEROFILAS { get; set; }
        [Display(Name = "Columnas")]
        [Required]
        public int NUMEROCOLUMNAS { get; set; }
        [Display(Name = "Desc.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(200, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres...")]
        public string DESCRIPCION { get; set; }
        [Display(Name = "Observ.")]
        [StringLength(200, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres...")]
        public string OBSERVACION { get; set; }
        [Required]
        [Display(Name = "Marca")]
        public int IIDMARCA { get; set; }
        public int BHABILITADO { get; set; }

        //ADICIONAMOS
        [Display(Name = "Sucursal")]
        public string Sucursal { get; set; }
        [Display(Name = "Tipo")]
        public string TipoBus { get; set; }
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        //ADICIONAMOS
        public string MensajeError { get; set; }
    }
}