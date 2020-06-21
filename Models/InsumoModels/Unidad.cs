using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Models.InsumoModels
{
    public class Unidad
    {
        public int Id { get; set; }

        [Required]
        public UnidadDeMedida Nombre { get; set; }
        public string Simbolo { get; set; }
        public UnidadDeMedida Grupo { get; set; }
        public int UnidadBase { get; set; }
    }

    public enum UnidadDeMedida {
        Kilogramo,
        Gramo,
        Litro,
        Mililitro,
        Rollo,
        Pieza
    }
}
