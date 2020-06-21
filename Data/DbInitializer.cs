using SanRafael.Models.InsumoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanRafael.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context) {

            context.Database.EnsureCreated();

            if (!context.Unidad.Any())
            {
                var Unidades = new Unidad[] {
                    new Unidad{ Nombre = UnidadDeMedida.Kilogramo, Simbolo = "Kg", Grupo = UnidadDeMedida.Kilogramo, UnidadBase = 1000 },
                    new Unidad{ Nombre = UnidadDeMedida.Gramo, Simbolo = "g", Grupo = UnidadDeMedida.Kilogramo, UnidadBase = 1},
                    new Unidad{ Nombre = UnidadDeMedida.Litro, Simbolo = "L", Grupo = UnidadDeMedida.Litro, UnidadBase = 1000},
                    new Unidad{ Nombre = UnidadDeMedida.Mililitro, Simbolo = "ml", Grupo = UnidadDeMedida.Litro, UnidadBase = 1},
                    new Unidad{ Nombre = UnidadDeMedida.Rollo, Grupo = UnidadDeMedida.Rollo, UnidadBase = 1},
                    new Unidad{ Nombre = UnidadDeMedida.Pieza, Grupo = UnidadDeMedida.Pieza, UnidadBase = 1}
                };

                foreach (Unidad unidad in Unidades)
                {
                    context.Unidad.Add(unidad);
                }
            }

            if (!context.Categoria.Any())
            {
                var categoria = new Categoria()
                {
                    Nombre = "Sin Categoria"
                };
                context.Categoria.Add(categoria);
            }

            context.SaveChanges();
        }
    }
}
