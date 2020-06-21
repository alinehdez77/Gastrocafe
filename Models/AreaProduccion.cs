using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Models
{
    public class AreaProduccion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        List<Receta> Recetas { get; set; }
    }
}
