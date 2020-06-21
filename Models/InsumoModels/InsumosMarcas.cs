using SanRafael.Models.InsumoModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanRafael.Models
{
    public class InsumosMarcas
    {
        public int InsumoId { get; set; }
        public int  MarcaId { get; set; }
        public Marca Marca { get; set; }
        public Insumo Insumo { get; set; }
    }
}
