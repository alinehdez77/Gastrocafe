using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanRafael.Models.InsumoModels
{
    public class Insumo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "Debe de contener mínimo {2} y máximo {1} caracteres.")]
        public string Nombre { get; set; }

        [DefaultValue(false)]
        public bool Deshabilitado { get; set; }

        [DefaultValue(0)]
        public int Cantidad { get; set; }

        public string RutaImagen { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser mayor a {1}")]
        [Display(Name = "Stock mínimo:")]
        public int StockMinimo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "Debe de contener mínimo {2} y máximo {1} caracteres.")]
        public string Tienda { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Compra")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser mayor a {1}")]
        public float Precio { get; set; }
        #region Navegación
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }


        [Display(Name = "Unidad")]
        [Required(ErrorMessage = "Por favor selecciona una {0}")]
        public int UnidadId { get; set; }
        [ForeignKey("UnidadId")]
        public Unidad Unidad { get; set; }

        public List<InsumosRecetas> Recetas { get; set; }
        #endregion
    }
}
