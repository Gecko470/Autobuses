
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Autobuses.Clases
{
    public class ReservaCLS
    {
        [Display(Name = "Id")]
        public int iidViaje { get; set; }
        [Display(Name = "Foto")]
        public string nombreFoto { get; set; }
        public byte[] foto { get; set; }
        [Display(Name = "Lugar Origen")]
        public string lugarOrigen { get; set; }
        [Display(Name = "Lugar Destino")]
        public string lugarDestino { get; set; }
        [Display(Name = "Precio")]
        public decimal precio { get; set; }
        [Display(Name = "Fecha Viaje")]
        public DateTime fechaViaje { get; set; }
        [Display(Name = "Matrícula")]
        public string matricula { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
        [Display(Name = "Asientos disponibles")]
        public int asientosDisponibles { get; set; }
        [Display(Name = "Asientos totales")]
        public int totalAsientos { get; set; }
        public int iidBus { get; set; }
        public int cantidad { get; set; }
    }
}