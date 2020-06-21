using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SanRafael.Data;
using SanRafael.Models;

namespace SanRafael.Controllers
{
    [Authorize]
    public class CostoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CostoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Costo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Costo.ToListAsync());
        }

        // GET: Costo otro
        public async Task<IActionResult> IndexOtro()
        {
            return View(await _context.Costo.ToListAsync());
        }

        // GET: Costo/Details/5
        public async Task<IActionResult> Details(int? id, string mensaje)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costo = await _context.Costo
                .SingleOrDefaultAsync(m => m.Id == id);
            if (costo == null)
            {
                return NotFound();
            }
            ViewBag.Mensaje = mensaje;
            return View(costo);
        }

        // GET: Costo/Create
        public IActionResult Create()
        {
            return View();
        }

        //GET: Costo/CreateOtro
        public IActionResult CreateOtro()
        {
            return View();
        }

        //GET: Costo/PuntoEquilibrio
        public IActionResult PuntoEquilibrio()
        {
            return View();
        }

        // POST: Costo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Periodicidad,Tipo,Monto,FechaRegistro")] Costo costo)
        {            
            if (ModelState.IsValid)
            {
                _context.Add(costo);
                await _context.SaveChangesAsync();
                if (costo.Tipo.Equals("Fijo"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (costo.Tipo.Equals("Otro")){
                    return RedirectToAction(nameof(IndexOtro));
                }                
            }
            
            return View(costo);
        }

        // GET: Costo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costo = await _context.Costo.SingleOrDefaultAsync(m => m.Id == id);
            if (costo == null)
            {
                return NotFound();
            }
            return View(costo);
        }

        // GET: Costo/Edit/5 otro
        public async Task<IActionResult> EditOtro(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costo = await _context.Costo.SingleOrDefaultAsync(m => m.Id == id);
            if (costo == null)
            {
                return NotFound();
            }
            return View(costo);
        }

        // POST: Costo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Periodicidad,Tipo,Monto,FechaRegistro")] Costo costo)
        {
            if (id != costo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(costo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CostoExists(costo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (costo.Tipo.Equals("Fijo"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (costo.Tipo.Equals("Otro")){
                    return RedirectToAction(nameof(IndexOtro));
                }
            }
            return View(costo);
        }

        //Deshabilitar costos
        [HttpPost, ActionName("Deshabilitar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeshabilitarAsync(int id, bool estado)
        {
            var CostoFijo = await _context.Costo.SingleOrDefaultAsync(i => i.Id == id);
            if (CostoFijo != null)
            {
                string Msg = "";
                if (estado)
                {
                    Msg = "El costo se ha deshabilitado.";
                }
                else
                {
                    Msg = "El costo se ha habilitado.";
                }
                CostoFijo.Deshabilitado = estado;
                _context.Costo.Update(CostoFijo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { Id = id, mensaje = Msg });
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Costo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costo = await _context.Costo
                .SingleOrDefaultAsync(m => m.Id == id);
            if (costo == null)
            {
                return NotFound();
            }

            return View(costo);
        }

        // POST: Costo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var costo = await _context.Costo.SingleOrDefaultAsync(m => m.Id == id);
            _context.Costo.Remove(costo);
            await _context.SaveChangesAsync();
            if (costo.Tipo.Equals("Fijo"))
            {
                return RedirectToAction(nameof(Index));
            }
            else if (costo.Tipo.Equals("Otro"))
            {
                return RedirectToAction(nameof(IndexOtro));
            }
            return View(costo);
        }

        private bool CostoExists(int id)
        {
            return _context.Costo.Any(e => e.Id == id);
        }

        //Función para calcular el total de los costos fijos registrados 
        [HttpGet]
        public decimal CalcularTotalFijo()
        {
            decimal TotalFijo = 0;
            List<Costo> Costos = _context.Costo.ToList();
            List<decimal> CostosFijos = new List<decimal>();

            foreach (var costo in Costos)
            {
                if (costo.Tipo.Equals("Fijo") && costo.Deshabilitado == false)
                {
                    CostosFijos.Add(costo.Monto);
                }
            }

            TotalFijo = CostosFijos.Sum();

            return TotalFijo;
        }

        //Función para calcular el total de los costos otros registrados          
        [HttpGet]
        public decimal CalcularTotalOtro()
        {
            decimal TotalOtro = 0;
            List<Costo> Costos = _context.Costo.ToList();
            List<decimal> CostosOtros = new List<decimal>();

            foreach (var costo in Costos)
            {
                if (costo.Tipo.Equals("Otro") && costo.Deshabilitado == false)
                {
                    CostosOtros.Add(costo.Monto);
                }
            }

            TotalOtro = CostosOtros.Sum();

            return TotalOtro;
        }
        
        [HttpGet]
        public decimal RecuperarTotales()
        {
            decimal TotalOtro = 0;
            decimal TotalVariable = 0;
            decimal TotalFijo = 0;
            decimal TotalIngresos = 0;
            decimal PuntoEquilibrio = 0;

            List<Costo> Costos = _context.Costo.ToList();
            List<Receta> Recetas = _context.Receta.ToList();

            List<decimal> CostosFijos = new List<decimal>();
            List<decimal> CostosVariables = new List<decimal>();
            List<decimal> CostosOtros = new List<decimal>();                        
            List<decimal> Ingresos = new List<decimal>();            

            foreach (var costo in Costos)
            {
                if (costo.Tipo.Equals("Otro") && costo.Deshabilitado == false)
                {
                    CostosOtros.Add(costo.Monto);
                } else if (costo.Tipo.Equals("Fijo") && costo.Deshabilitado == false)
                {
                    CostosFijos.Add(costo.Monto);
                }
            }

            foreach(var receta in Recetas)
            {
                if (receta.Activo == true)
                {
                    CostosVariables.Add(receta.CostoOperacion);
                    Ingresos.Add(receta.IngresoProducto);
                }
            }

            TotalFijo = CostosFijos.Sum();
            TotalVariable = CostosVariables.Sum();
            TotalOtro = CostosOtros.Sum();                        
            TotalIngresos = Ingresos.Sum();
            PuntoEquilibrio = CalcularPuntoEquilibrio(TotalFijo, TotalVariable, TotalOtro, TotalIngresos);
            return PuntoEquilibrio;
        }
        
        public decimal CalcularPuntoEquilibrio(decimal TotalFijo, decimal TotalVariable, decimal TotalOtro, decimal Ingresos)
        {
            decimal PuntoEquilibrio = 0;
            decimal FijosVariables = 0;
            decimal IngresosFijosVariables = 0;            

            FijosVariables = TotalFijo + TotalVariable;
            IngresosFijosVariables = Ingresos - FijosVariables;
            PuntoEquilibrio = IngresosFijosVariables - TotalOtro;

            return PuntoEquilibrio;
        }
    }
}
