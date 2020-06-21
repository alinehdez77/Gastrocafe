using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanRafael.Models
{
    public class InsumosPresentaciones
    {
        [ForeignKey("Id")]
        public int InsumoId { get; set; }
        [ForeignKey("idPresentacion")]
        public int PresentacionId { get; set; }
        public Presentacion Presentacion { get; set; }
        public InsumoModels.Insumo Insumo { get; set; }
    }
}
