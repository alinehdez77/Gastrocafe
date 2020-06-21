using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Models
{
    public class Costo
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "Debe de contener mínimo {2} y máximo {1} caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public string Descripcion { get; set; }

        [Display(Name = "Periodicidad")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public string Periodicidad { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public string Tipo { get; set; }

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser mayor a {1}")]
        public  decimal Monto { get; set; }

        [Display(Name = "Fecha de registro")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        [DefaultValue(false)]
        public bool Deshabilitado { get; set; }
    }
}
