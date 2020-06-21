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

namespace SanRafael.Controllers
{
    [Authorize]
    public class AreaProduccionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AreaProduccionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AreaProduccions
        public async Task<IActionResult> Index()
        {
            return View(await _context.AreaProduccion.ToListAsync());
        }

        // GET: AreaProduccions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var areaProduccion = await _context.AreaProduccion
                .SingleOrDefaultAsync(m => m.Id == id);
            if (areaProduccion == null)
            {
                return NotFound();
            }

            return View(areaProduccion);
        }

        // GET: AreaProduccions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AreaProduccions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] AreaProduccion areaProduccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(areaProduccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(areaProduccion);
        }

        // GET: AreaProduccions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var areaProduccion = await _context.AreaProduccion.SingleOrDefaultAsync(m => m.Id == id);
            if (areaProduccion == null)
            {
                return NotFound();
            }
            return View(areaProduccion);
        }

        // POST: AreaProduccions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] AreaProduccion areaProduccion)
        {
            if (id != areaProduccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(areaProduccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaProduccionExists(areaProduccion.Id))
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
            return View(areaProduccion);
        }

        // GET: AreaProduccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var areaProduccion = await _context.AreaProduccion
                .SingleOrDefaultAsync(m => m.Id == id);
            if (areaProduccion == null)
            {
                return NotFound();
            }

            return View(areaProduccion);
        }

        // POST: AreaProduccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var areaProduccion = await _context.AreaProduccion.SingleOrDefaultAsync(m => m.Id == id);
            _context.AreaProduccion.Remove(areaProduccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaProduccionExists(int id)
        {
            return _context.AreaProduccion.Any(e => e.Id == id);
        }
    }
}
