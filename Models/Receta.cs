using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Models
{
    public class Receta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Método de preparación")]
        public string MetodoPreparacion { get; set; }
        public int Porciones { get; set; }
        [Display(Name = "Tipo de receta")]
        public string TipoReceta { get; set; }
        [Display(Name = "Clasificación")]
        public string Clasificacion { get; set; }

        [Display(Name = "Costo unitario")]
        public decimal CostoUnitario { get; set; }
        [Display(Name = "Costo + otros gasto con utilidad")]
        public decimal CostoOtrosConUtilidad { get; set; }
        [Display(Name = "Precio sugerido")]
        public decimal PrecioSugerido { get; set; }
        [Display(Name = "Precio definido por el usuario")]
        public decimal PrecioDefinidoPorUsuario { get; set; }
        [Display(Name = "Precio de venta con IVA")]
        public decimal PrecioVentaConIva { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        public int RecetasVendidas { get; set; }

        //El costo de operación es el total de multiplicar el costo unitario de cada platillo
        // por el número de platillos vendidos
        [Display(Name = "Costo de operación")]        
        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser mayor a {1}")]
        public decimal CostoOperacion { get; set; }

        //El ingreso de producto es el total de multiplicar el costo definido por el usuario
        // por el número de platillos vendidos
        [Display(Name = "Ingreso de producto")]
        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser mayor a {1}")]
        public decimal IngresoProducto { get; set; }        

        [Display(Name = "Identificador de area")]
        public int AreaProduccionId { get; set; }
        [Display(Name = "Area de producción")]
        public AreaProduccion AreaProduccion { get; set; }

        [Display(Name = "Insumos de la receta")]
        virtual public List<InsumosRecetas> InsumosReceta { get; set; }


        virtual public List<RecetaAReceta> RecetasPadres { get; set; }
        virtual public List<RecetaAReceta> RecetasIntegradoras { get; set; }

        public Receta()
        {
            //InsumosReceta = new List<InsumosRecetas>();
            //RecetasIntegradoras = new List<RecetaAReceta>();
        }
    }
}
