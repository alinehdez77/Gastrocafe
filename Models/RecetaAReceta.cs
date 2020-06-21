using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Models
{
    public class RecetaAReceta
    {
        public int? IdRecetaPadre { get; set; }
        public int? IdRecetaHijo { get; set; }

        [ForeignKey("IdRecetaPadre")]
        public Receta RecetaPadre { get; set; }
        [ForeignKey("IdRecetaHijo")]
        public Receta RecetaHijo { get; set; }

        public int Porciones { get; set; }
    }
}
