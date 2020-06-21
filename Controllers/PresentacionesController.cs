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
    public class PresentacionesController : Controller
    {
        private static int idInsumo = 0;
        private readonly ApplicationDbContext _context;

        public PresentacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Presentaciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Presentacion.ToListAsync());
        }

        // GET: Presentaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentacion = await _context.Presentacion
                .SingleOrDefaultAsync(m => m.idPresentacion == id);
            if (presentacion == null)
            {
                return NotFound();
            }

            return View(presentacion);
        }

        // GET: Presentaciones/Create
        public IActionResult Create(int id)
        {
            idInsumo = id;
            ViewBag.Categorias = _context.Categoria.ToList();
            var grupos = _context.Unidad.Select(u => u.Grupo).Distinct().ToList();
            var Unidades = new List<Unidad>();
            foreach (var g in grupos)
            {
                Unidades.Add(
                    _context.Unidad.Where(u => u.Nombre.Equals(g)).FirstOrDefault()
                    );
            }
            ViewBag.Unidades = Unidades;
            return View();
        }

        // POST: Presentaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idPresentacion,nombre,cantidadUnidades,precioPresentacion,precioUnitario,fechaCaducidad,UnidadId")] Presentacion presentacion)
        {
            if (ModelState.IsValid)
            {                
                _context.Add(presentacion);             
                await _context.SaveChangesAsync();
                InsumosPresentaciones insumoPresentacion = new InsumosPresentaciones();
                insumoPresentacion.InsumoId = idInsumo;
                insumoPresentacion.PresentacionId = presentacion.idPresentacion;
                _context.Add(insumoPresentacion);
                _context.SaveChanges();
                return RedirectToAction(nameof(VerInsumos));
            }
            return View(presentacion);
        }
        
        // GET: Presentaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentacion = await _context.Presentacion.SingleOrDefaultAsync(m => m.idPresentacion == id);
            if (presentacion == null)
            {
                return NotFound();
            }
            ViewBag.Categorias = _context.Categoria.ToList();
            var grupos = _context.Unidad.Select(u => u.Grupo).Distinct().ToList();
            var Unidades = new List<Unidad>();
            foreach (var g in grupos)
            {
                Unidades.Add(
                    _context.Unidad.Where(u => u.Nombre.Equals(g)).FirstOrDefault()
                    );
            }
            ViewBag.Unidades = Unidades;
            return View(presentacion);
        }

        // POST: Presentaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idPresentacion,nombre,cantidadUnidades,precioPresentacion,precioUnitario,fechaCaducidad,UnidadId")] Presentacion presentacion)
        {
            if (id != presentacion.idPresentacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presentacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresentacionExists(presentacion.idPresentacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VerInsumos));
            }
            return View(presentacion);
        }

        // GET: Presentaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentacion = await _context.Presentacion
                .SingleOrDefaultAsync(m => m.idPresentacion == id);
            if (presentacion == null)
            {
                return NotFound();
            }

            return View(presentacion);
        }

        // POST: Presentaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presentacion = await _context.Presentacion.SingleOrDefaultAsync(m => m.idPresentacion == id);
            InsumosPresentaciones insumoPresentacion = new InsumosPresentaciones();
            insumoPresentacion = await _context.InsumosPresentaciones.SingleOrDefaultAsync(m => m.PresentacionId == id);

            _context.InsumosPresentaciones.Remove(insumoPresentacion);
            _context.SaveChanges();
            _context.Presentacion.Remove(presentacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VerInsumos));
        }

        private bool PresentacionExists(int id)
        {
            return _context.Presentacion.Any(e => e.idPresentacion == id);
        }

        public IActionResult VerInsumos()
        {
            IQueryable<Categoria> IQCategorias = _context.Insumo.Select(x => x.Categoria).Distinct();
            ViewBag.categorias = IQCategorias.ToList();
            IQueryable<Insumo> insumosIQ = from s in _context.Insumo.Take(25) select s;
            insumosIQ = insumosIQ.Include(x => x.Unidad);
            insumosIQ = insumosIQ.Include(x => x.Categoria);
            return View(insumosIQ);
        }

        [HttpGet]
        public JsonResult ObtenerPresentaciones(String idInsumo)
        {
            List<InsumosPresentaciones> presentaciones = _context.InsumosPresentaciones.Where(x=>x.InsumoId.Equals(int.Parse(idInsumo))).ToList();
            List<Presentacion> listaPresentaciones = new List<Presentacion>();
            foreach (var presentacion in presentaciones)
            {
                IQueryable<Presentacion> IQpresentacion = _context.Presentacion.Where(x => x.idPresentacion.Equals(presentacion.PresentacionId));
                IQpresentacion = IQpresentacion.Include(x => x.unidad);
                listaPresentaciones.Add(IQpresentacion.FirstOrDefault());
            }
            
            return Json(listaPresentaciones);
        }
    }
}