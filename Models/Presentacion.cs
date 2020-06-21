using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanRafael.Models
{
    public class Presentacion
    {
        [Key]
        [Required]
        public int idPresentacion { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public String nombre { get; set; }

        [Display(Name = "Cantidad de unidades")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public int cantidadUnidades { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public double precioPresentacion { get; set; }

        [Display(Name = "Precio unitario")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public double precioUnitario { get; set; }

        [Display(Name = "Fecha de caducidad")]
        [Required(ErrorMessage = "El campo de {0} es requerido.")]
        public bool fechaCaducidad { get; set; }

        [Display(Name = "Unidad")]
        [Required(ErrorMessage = "Por favor selecciona una {0}")]
        public int UnidadId { get; set; }
        [ForeignKey("UnidadId")]
        public InsumoModels.Unidad unidad { get; set; }
    }
}
