using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanRafael.Models
{
    public class Marca
    {
        [Key]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "Debe de contener mínimo {2} y máximo {10} caracteres.")]
        public string Nombre { get; set; }
    }
}
