using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobuses.Clases
{
    public class UsuarioCLS
    {
        [Display(Name = "Id")]
        public int IIDUSUARIO { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string NOMBREUSUARIO { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string CONTRA { get; set; }
        [Display(Name = "Tipo Usuario")]
        [Required]
        public string TIPOUSUARIO { get; set; }
        [Display(Name = "IID")]
        [Required]
        public int IID { get; set; }
        [Required]
        public int IIDROL { get; set; }
        public int bhabilitado { get; set; }

        //ADICIONAMOS
        [Display(Name = "Rol")]
        public string Rol { get; set; }
        [Display(Name = "Persona")]
        public string Persona { get; set; }
    }
}