using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SanRafael.Data;
using SanRafael.Models;
using SanRafael.Models.InsumoModels;

namespace SanRafael.Controllers
{
    [Authorize]
    public class RecetasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecetasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ActualizarRecetasVendidas(int? id)
        {
            return View();
        }

        // GET: Recetas
        public async Task<IActionResult> Index(string filter, string searchQuery)
        {
            ViewData["currentFilter"] = filter;
            ViewData["currentSearchQuery"] = searchQuery;

            List<Receta> recetas = new List<Receta>();
            if (!String.IsNullOrEmpty(searchQuery) && !String.IsNullOrEmpty(filter))
            {
                switch (filter)
                {
                    case "nombre":
                        recetas = await _context.Receta.Where(x => x.Activo == true && x.Nombre.Contains(searchQuery)).Include(x => x.AreaProduccion).ToListAsync();
                        recetas.OrderByDescending(x => x.Nombre);
                        break;
                    case "tipo_receta":
                        recetas = await _context.Receta.Where(x => x.Activo == true && x.TipoReceta.Contains(searchQuery)).Include(x => x.AreaProduccion).ToListAsync();
                        break;
                    case "clasificacion_receta":
                        recetas = await _context.Receta.Where(x => x.Activo == true && x.Clasificacion.Contains(searchQuery)).Include(x => x.AreaProduccion).ToListAsync();
                        break;
                    case "area_produccion":
                        recetas = await _context.Receta.Where(x => x.Activo == true && x.AreaProduccion.Nombre.Contains(searchQuery)).Include(x => x.AreaProduccion).ToListAsync();
                        break;
                    default:
                        recetas = await _context.Receta.Where(x => x.Activo == true).Include(x => x.AreaProduccion).ToListAsync();
                        break;
                }

            } else
            {
                recetas = await _context.Receta.Where(x => x.Activo == true).Include(x => x.AreaProduccion).ToListAsync();
            }



            return View(recetas);
        }

        public async Task<IActionResult> IndexVariables()
        {
            return View(await _context.Receta.ToListAsync());
        }

        // GET: Recetas/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var receta = await _context.Receta.Include()
            //   .SingleOrDefaultAsync(m => m.Id == id);

            /*var receta = await _context.Receta
                .Where(x => x.Id == id)
                .Include(recetax => recetax.AreaProduccion)
                .Include(recetax => recetax.InsumosReceta)
                    .ThenInclude(insumosRecetasy => insumosRecetasy.Insumo)
                .Include(recetax => recetax.InsumosReceta)
                    .ThenInclude(insumosRecetasy => insumosRecetasy.Unidad)
                    .SingleOrDefaultAsync();

            for(int i = 0; i < receta.InsumosReceta.Count; i++)
            {
                receta.InsumosReceta.ElementAt(i).Insumo = _context.Insumo.Where(x => x.Id == receta.InsumosReceta.ElementAt(i).IdInsumo)
                    .SingleOrDefault();
            }*/


            var result = _context.Receta
                .Where(m => m.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.Nombre,
                    e.MetodoPreparacion,
                    e.Porciones,
                    e.TipoReceta,
                    e.Clasificacion,
                    e.CostoUnitario,
                    e.CostoOtrosConUtilidad,
                    e.PrecioSugerido,
                    e.PrecioDefinidoPorUsuario,
                    e.PrecioVentaConIva,
                    e.Activo,
                    e.AreaProduccionId,
                    e.AreaProduccion,
                    e.InsumosReceta,
                    e.RecetasIntegradoras
                })
                .SingleOrDefault();

            for (int i = 0; i < result.InsumosReceta.Count; i++)
            {
                result.InsumosReceta.ElementAt(i).Insumo = _context.Insumo.Where(x => x.Id == result.InsumosReceta.ElementAt(i).IdInsumo)
                    .SingleOrDefault();
            }
            for (int i = 0; i < result.RecetasIntegradoras.Count; i++)
            {
                result.RecetasIntegradoras.ElementAt(i).RecetaHijo = _context.Receta.Where(x => x.Id == result.RecetasIntegradoras.ElementAt(i).IdRecetaHijo)
                    .SingleOrDefault();
            }

            //Mas informacion sobre querying https://docs.microsoft.com/en-us/ef/core/querying/related-data


            Receta final = new Receta
            {
                Id = result.Id,
                Nombre = result.Nombre,
                MetodoPreparacion = result.MetodoPreparacion,
                Porciones = result.Porciones,
                TipoReceta = result.TipoReceta,
                Clasificacion = result.Clasificacion,
                CostoUnitario = result.CostoUnitario,
                CostoOtrosConUtilidad = result.CostoOtrosConUtilidad,
                PrecioSugerido = result.PrecioSugerido,
                PrecioDefinidoPorUsuario = result.PrecioDefinidoPorUsuario,
                PrecioVentaConIva = result.PrecioVentaConIva,
                Activo = result.Activo,
                AreaProduccionId = result.AreaProduccionId,
                AreaProduccion = result.AreaProduccion,
                InsumosReceta = result.InsumosReceta,
                RecetasIntegradoras = result.RecetasIntegradoras
            };

            if (result == null)
            {
                return NotFound();
            }

            return View(final);
        }

        // GET: Recetas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recetas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,MetodoPreparacion,Porciones,TipoReceta,Clasificacion,CostoUnitario,CostoOtrosConUtilidad,PrecioSugerido,PrecioDefinidoPorUsuario,PrecioVentaConIva,Activo,IdAreaProduccion,RecetasVendidas")] Receta receta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receta);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public string CreateAjax([Bind("Nombre,MetodoPreparacion,Porciones,TipoReceta,Clasificacion,CostoUnitario,CostoOtrosConUtilidad,PrecioSugerido,PrecioDefinidoPorUsuario,PrecioVentaConIva,Activo,AreaProduccionId,RecetasVendidas")] Receta receta, [Bind("insumos")] List<InsumosRecetas> insumos, [Bind("recetas")] List<RecetaAReceta> recetas)
        {
            //if (ModelState.IsValid)
            //{
            var newReceta = receta;
            newReceta.InsumosReceta = new List<InsumosRecetas>();

            _context.Receta.Add(newReceta);
            _context.SaveChanges();

            foreach (InsumosRecetas insumo in insumos)
            {
                var oldInsumo = _context.Insumo.FirstOrDefault(x => x.Id == insumo.IdInsumo);
                var recetaOld = _context.Receta.FirstOrDefault(x => x.Id == newReceta.Id);
                var unidad = _context.Unidad.FirstOrDefault(x => x.Id == oldInsumo.UnidadId);
                var insumoReceta = new InsumosRecetas();
                insumoReceta.Insumo = oldInsumo;
                insumoReceta.IdInsumo = oldInsumo.Id;
                insumoReceta.Receta = recetaOld;
                insumoReceta.Unidad = unidad;
                insumoReceta.IdUnidad = oldInsumo.UnidadId;
                insumoReceta.PesoNeto = insumo.PesoNeto;
                _context.InsumosRecetas.Add(insumoReceta);
            }

            foreach (RecetaAReceta recetaHija in recetas)
            {

                var newRecetaHija = new RecetaAReceta
                {
                    IdRecetaHijo = recetaHija.IdRecetaHijo,
                    IdRecetaPadre = newReceta.Id,
                    Porciones = recetaHija.Porciones
                };

                _context.RecetaAReceta.Add(newRecetaHija);

            }

            _context.SaveChanges();

            return "complete";
            //}
            //return "Error";
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public string EditAjax([Bind("Id,Nombre,MetodoPreparacion,Porciones,TipoReceta,Clasificacion,CostoUnitario,CostoOtrosConUtilidad,PrecioSugerido,PrecioDefinidoPorUsuario,PrecioVentaConIva,Activo,AreaProduccionId,RecetasVendidas")] Receta receta, [Bind("insumos")] List<InsumosRecetas> insumos, [Bind("recetas")] List<RecetaAReceta> recetas)
        {
            _context.Update<Receta>(receta);
            _context.InsumosRecetas.RemoveRange(_context.InsumosRecetas.Where(x => x.IdReceta == receta.Id));
            _context.RecetaAReceta.RemoveRange(_context.RecetaAReceta.Where(x => x.IdRecetaPadre == receta.Id));
            _context.SaveChanges();


            foreach (InsumosRecetas insumo in insumos)
            {
                var oldInsumo = _context.Insumo.FirstOrDefault(x => x.Id == insumo.IdInsumo);
                var insumoReceta = new InsumosRecetas
                {
                    IdInsumo = oldInsumo.Id,
                    IdReceta = receta.Id,
                    IdUnidad = oldInsumo.UnidadId,
                    PesoNeto = insumo.PesoNeto,
                };

                _context.InsumosRecetas.Add(insumoReceta);

            }

            foreach (RecetaAReceta recetaHija in recetas)
            {

                var newRecetaHija = new RecetaAReceta
                {
                    IdRecetaHijo = recetaHija.IdRecetaHijo,
                    IdRecetaPadre = receta.Id,
                    Porciones = recetaHija.Porciones
                };

                _context.RecetaAReceta.Add(newRecetaHija);

            }

            _context.SaveChanges();

            var recetaEditada = _context.Receta.Where(x => x.Id == receta.Id).Include(x => x.InsumosReceta).Include(x => x.RecetasIntegradoras).SingleOrDefault();

            return "complete";
            //}
            //return "Error";
        }


        // GET: Recetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta.SingleOrDefaultAsync(m => m.Id == id);
            if (receta == null)
            {
                return NotFound();
            }
            return View(receta);
        }

        // POST: Recetas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,MetodoPreparacion,Porciones,TipoReceta,Clasificacion,CostoUnitario,CostoOtrosConUtilidad,PrecioSugerido,PrecioDefinidoPorUsuario,PrecioVentaConIva,Activo,AreaProduccionId,RecetasVendidas")] Receta receta)
        {
            if (id != receta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(receta);
        }

        // GET: Recetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Receta
                .SingleOrDefaultAsync(m => m.Id == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receta = await _context.Receta.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Receta.Remove(receta);
            receta.Activo = false;
            _context.Update(receta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetaExists(int id)
        {
            return _context.Receta.Any(e => e.Id == id);
        }


        //======================Funciones MXP

        // GET: Recetas/ListAreasProduccion
        public List<AreaProduccion> ListAreasProduccion()
        {
            return _context.AreaProduccion.ToList();
        }
        // GET: Recetas/ListTiposReceta
        public List<String> ListTiposReceta()
        {
            var lista = _context.Receta.ToList();

            var data1 =
            from p in _context.Receta
            group p by p.TipoReceta.ToLower() into g
            select new { tipo = g.Key, apariciones = g.Count() };

            data1.ToList();

            var result = new List<String>();
            foreach (var v in data1)
            {
                result.Add(v.tipo);
            }
            return result;
        }
        // GET: Recetas/ListClasificaciones
        public List<String> ListClasificaciones()
        {
            var lista = _context.Receta.ToList();

            var data1 =
            from p in _context.Receta
            group p by p.Clasificacion.ToLower() into g
            select new { clasificacion = g.Key, apariciones = g.Count() };

            data1.ToList();

            var result = new List<String>();
            foreach (var v in data1)
            {
                result.Add(v.clasificacion);
            }
            return result;
        }

        // GET: Insumos
        public List<Insumo> ListInsumos(string queryString)
        {
            if (String.IsNullOrEmpty(queryString))
            {
                return _context.Insumo.Include(x => x.Unidad).ToList();
            }
            return _context.Insumo.Where(x => x.Nombre.Contains(queryString)).Include(x => x.Unidad).ToList();

        }

        // GET: Insumos Id
        public List<Object> GetDataFromReceta (int Id)
        {
            //return _context.Insumo.Where(x => x.Id == Id).Include(x => x.UnidadBase).ToList();
            var result = _context.Receta
                .Where(m => m.Id == Id)
                .Select(e => new
                {
                    e.Id,
                    e.Nombre,
                    e.MetodoPreparacion,
                    e.Porciones,
                    e.TipoReceta,
                    e.Clasificacion,
                    e.CostoUnitario,
                    e.CostoOtrosConUtilidad,
                    e.PrecioSugerido,
                    e.PrecioDefinidoPorUsuario,
                    e.PrecioVentaConIva,
                    e.Activo,
                    e.AreaProduccionId,
                    e.AreaProduccion,
                    e.InsumosReceta,
                    e.RecetasIntegradoras
                })
                .SingleOrDefault();

            var PesoNeto = _context.InsumosRecetas.Where(m => m.IdReceta == Id).ToList();
            var InsumoPresentacion = new InsumosPresentaciones();
            var Presentacion = new Presentacion();
            var Presentaciones = new List<Presentacion>();
            for (int i = 0; i < result.InsumosReceta.Count; i++)
            {
                result.InsumosReceta.ElementAt(i).Insumo = _context.Insumo.Where(x => x.Id == result.InsumosReceta.ElementAt(i).IdInsumo).Include(x=>x.Unidad)
                    .SingleOrDefault();

                InsumoPresentacion = _context.InsumosPresentaciones.Where(m => m.InsumoId == result.InsumosReceta.ElementAt(i).IdInsumo).SingleOrDefault();
                Presentacion = _context.Presentacion.Where(m => m.idPresentacion == InsumoPresentacion.PresentacionId).SingleOrDefault();
                Presentaciones.Add(Presentacion);
            }            
            for (int i = 0; i < result.RecetasIntegradoras.Count; i++)
            {
                result.RecetasIntegradoras.ElementAt(i).RecetaHijo = _context.Receta.Where(x => x.Id == result.RecetasIntegradoras.ElementAt(i).IdRecetaHijo)
                    .SingleOrDefault();
            }

            
            Receta receta = new Receta{
                Id = result.Id,
                Nombre = result.Nombre,
                MetodoPreparacion = result.MetodoPreparacion,
                Porciones = result.Porciones,
                TipoReceta = result.TipoReceta,
                Clasificacion = result.Clasificacion,
                CostoUnitario = result.CostoUnitario,
                CostoOtrosConUtilidad = result.CostoOtrosConUtilidad,
                PrecioSugerido = result.PrecioSugerido,
                PrecioDefinidoPorUsuario = result.PrecioDefinidoPorUsuario,
                PrecioVentaConIva = result.PrecioVentaConIva,
                Activo = result.Activo,
                AreaProduccionId = result.AreaProduccionId,
                AreaProduccion = result.AreaProduccion,
                InsumosReceta = result.InsumosReceta,                
                RecetasIntegradoras = result.RecetasIntegradoras
            };

            List<Object> final = new List<Object>();
            final.Add(receta);
            final.Add(Presentaciones);

            return final;
        }

        public List<Receta> ListRecetas(string queryString)
        {
            if (String.IsNullOrEmpty(queryString))
            {
                return _context.Receta.ToList();
            }
            return _context.Receta.Where(x => x.Nombre.Contains(queryString)) .ToList();

        }

        //Función para calcular, con los costos unitarios de cada receta, el costo total Variable 
        [HttpGet]
        public decimal CalcularTotalVariable()
        {
            decimal TotalVariable = 0;
            List<Receta> Recetas = _context.Receta.ToList();
            
            foreach (var receta in Recetas)
            {
                TotalVariable = receta.CostoOperacion + TotalVariable;
            }

            return TotalVariable;
        }

        //Función para calcular el total de ingresos, sumando el IngresoProducto de todas las recetas activas 
        [HttpGet]
        public decimal CalcularTotalIngresos()
        {
            decimal TotalIngresos = 0;
            List<Receta> Recetas = _context.Receta.ToList();

            foreach (var receta in Recetas)
            {
                TotalIngresos = receta.IngresoProducto + TotalIngresos;
            }

            return TotalIngresos;
        }

        public async Task<IActionResult> ActualizarPlatillosVendidos(int id, [Bind("RecetasVendidas,CostoOperacion,IngresoProducto")]Receta receta)
        {
            var recetaUpdate = await _context.Receta.SingleOrDefaultAsync(m => m.Id == id);
            recetaUpdate.RecetasVendidas = receta.RecetasVendidas;
            recetaUpdate.CostoOperacion = recetaUpdate.CostoUnitario * receta.RecetasVendidas;
            recetaUpdate.IngresoProducto = recetaUpdate.PrecioDefinidoPorUsuario * receta.RecetasVendidas;
            _context.Update(recetaUpdate);
            await _context.SaveChangesAsync();            
            return RedirectToAction(nameof(IndexVariables));
        }
    }
}
