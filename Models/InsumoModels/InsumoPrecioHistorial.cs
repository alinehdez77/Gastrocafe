using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Models.InsumoModels
{
    public class InsumoPrecioHistorial
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public float Precio { get; set; }        

        public int InsumoId { get; set; }

        [ForeignKey("InsumoId")]
        public Insumo Insumo { get; set; }
    }
}
