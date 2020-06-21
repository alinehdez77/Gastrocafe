using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanRafael.Data;
using SanRafael.Models.InsumoModels;

namespace SanRafael.Controllers.InsumoControllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categoria
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categoria.ToListAsync());
        }

        // GET: Categoria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .SingleOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                var i = categoria.Id;

                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.SingleOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var categoria = await _context.Categoria
                .SingleOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            ViewBag.Insumos = await _context.Insumo.Where(i => i.CategoriaId == id).ToListAsync();
            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var insumos = _context.Insumo.Where(i => i.CategoriaId == id).ToList();
            var SinCantegoria = _context.Categoria.Where(c => c.Id == 1 || c.Nombre.Equals("Sin Categoría")).First();
            if (SinCantegoria == null) {
                return NotFound();
            }
            foreach (var insumo in insumos) {
                insumo.CategoriaId = SinCantegoria.Id;
                _context.Insumo.Update(insumo);
            }
            var categoria = await _context.Categoria.SingleOrDefaultAsync(m => m.Id == id);
            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }
    }
}
