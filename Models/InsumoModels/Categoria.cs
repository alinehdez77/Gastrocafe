using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Models.InsumoModels
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido.")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "Debe de contener mínimo {2} y máximo {1} caracteres.")]
        public string Nombre { get; set; }
    }
}
