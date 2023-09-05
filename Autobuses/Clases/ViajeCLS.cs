using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class ViajeCLS
    {
        [Display(Name = "Id")]
        public int IIDVIAJE { get; set; }
        [Required]
        [Display(Name = "Origen")]
        public int IIDLUGARORIGEN { get; set; }
        [Required]
        [Display(Name = "Destino")]
        public int IIDLUGARDESTINO { get; set; }
        [Required]
        [Display(Name = "Precio")]
        [Range(1, 100, ErrorMessage = "El campo {0} debe estar en el rango entre {1} y {2}")]
        public decimal PRECIO { get; set; }
        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FECHAVIAJE { get; set; }
        [Required]
        [Display(Name = "Bus")]
        public int IIDBUS { get; set; }
        [Required]
        [Display(Name = "Asientos disp.")]
        public int NUMEROASIENTOSDISPONIBLES { get; set; }
       
        public int BHABILITADO { get; set; }
        public byte[] FOTO { get; set; }
        [Display(Name = "Nombre Foto")]
        public string nombrefoto { get; set; }

        //ADICIONAMOS
        [Display(Name = "Origen")]
        public string  Origen { get; set; }
        [Display(Name = "Destino")]
        public string  Destino { get; set; }
        [Display(Name = "Matrícula Bus")]
        public string  Bus { get; set; }
        public string FechaViajeString { get; set; }
        public string extension { get; set; }
        public string FotoString { get; set; }
    }
}