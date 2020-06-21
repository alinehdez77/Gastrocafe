using System;    
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanRafael.Data;
using SanRafael.Models.InsumoModels;
using SanRafael.Models;

namespace SanRafael.Controllers.InsumoControllers
{
    [Authorize]
    public class InsumosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int DIFERENCIA_STOCK_MINIMO = 2;

        public InsumosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Insumo
        public async Task<IActionResult> Index(string busqueda, string orden, int pagina = 1, int estado = 0)
        {
            ViewBag.BusquedaActual = busqueda;
            ViewBag.OrdenActual = orden;
            ViewBag.EstadoActual = estado;
            IQueryable<Insumo> insumosIQ = from s in _context.Insumo select s;
            insumosIQ = insumosIQ.Include(x => x.Categoria);
            insumosIQ = insumosIQ.Include(x => x.Unidad);
            if (!String.IsNullOrEmpty(busqueda))
            {
                insumosIQ = insumosIQ.Where(i => i.Nombre.Contains(busqueda));
                ViewBag.EstadoActual = 2;
            }
            switch (orden)
            {
                case "Nombre":
                    insumosIQ = insumosIQ.OrderBy(x => x.Nombre);
                    break;
                case "Categoria":
                    insumosIQ = insumosIQ.OrderBy(x => x.Categoria.Nombre);
                    break;
            }
            switch (estado)
            {
                case 0:
                    insumosIQ = insumosIQ.Where(i => i.Deshabilitado == false);
                    break;
                case 1:
                    insumosIQ = insumosIQ.Where(i => i.Deshabilitado == true);
                    break;
            }
            int tamañoDePagina = 20;
            var total = await insumosIQ.CountAsync();
            var insumos = await insumosIQ.Skip(
               (pagina - 1) * tamañoDePagina)
               .Take(tamañoDePagina).ToListAsync();
            ViewBag.TotalDeRegistros = total;
            ViewBag.TamañoPagina = tamañoDePagina;
            ViewBag.PaginaActual = pagina;
            return View(insumos);
        }

        // GET: Insumo/Details/n
        public async Task<IActionResult> Details(int? id, string mensaje)
        {
            if (id == null)
            {
                return NotFound();
            }
            var insumo = await _context.Insumo.Include(i => i.Categoria).Include(i => i.Unidad)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (insumo == null)
            {
                return NotFound();
            }
            ViewBag.Mensaje = mensaje;
            return View(insumo);
        }


        // GET: Insumo/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = _context.Categoria.ToList();
            var grupos = _context.Unidad.Select(u => u.Grupo).Distinct().ToList();
            var Unidades = new List<Unidad>();
            foreach (var g in grupos) {
                Unidades.Add(
                    _context.Unidad.Where(u => u.Nombre.Equals(g)).FirstOrDefault()
                    );
            }
            ViewBag.Unidades = Unidades;
            return View();
        }
        public IActionResult AgregarCompras()
        {
            ViewBag.mensaje = "";
            try
            {
                IQueryable<Categoria> IQCategorias = _context.Insumo.Select(x => x.Categoria).Distinct();
                ViewBag.categorias = IQCategorias.ToList();
                IQueryable<InsumosPresentaciones> IQInsumosPresentacions = from s in _context.InsumosPresentaciones select s;
                List<InsumosPresentaciones> insumosPresentaciones = new List<InsumosPresentaciones>();
                foreach (var insumPresent in IQInsumosPresentacions)
                {
                    Insumo insumo = _context.Insumo.Where(x => x.Id.Equals(insumPresent.InsumoId)).FirstOrDefault();
                    Presentacion presentacion = (from s in _context.Presentacion.Where(x => x.idPresentacion.Equals(insumPresent.PresentacionId)) select s).FirstOrDefault();
                    InsumosPresentaciones insumPresentObj = new InsumosPresentaciones();
                    insumPresentObj.Insumo = insumo;
                    insumPresentObj.Presentacion = presentacion;
                    insumosPresentaciones.Add(insumPresentObj);
                }
                ViewBag.insumPresent = insumosPresentaciones;
            }catch(Exception ex)
            {
                ViewBag.mensaje = "No hay ningún insumo asociado a una presentación";
            }
            return View();
        }

        [HttpGet]
        public string ObtenerInsumosIds(String arrJSON)
        {
            List<String> idInsumosPresentaciones = new List<string>();
            String resultado = "";
            Boolean isFirstElement = true;
            if (arrJSON.Length != 3) {
                resultado = "[";
                foreach (var idInsumo in arrJSON.Split(","))
                {
                    idInsumosPresentaciones.Add(idInsumo);
                }
                
                foreach (var idInsumPresent in idInsumosPresentaciones)
                {
                    string[] arrIdInsumoPresentacion = idInsumPresent.Split("-");
                    Insumo insumo = _context.Insumo.Where(x => x.Id.Equals(int.Parse(arrIdInsumoPresentacion[0]))).FirstOrDefault();

                    IQueryable<Presentacion> presentacionIQ = _context.Presentacion.Where(x => x.idPresentacion.Equals(int.Parse(arrIdInsumoPresentacion[1])));
                    presentacionIQ = presentacionIQ.Include(x => x.unidad);
                    Presentacion presentacion = presentacionIQ.FirstOrDefault();

                    List<Marca> marcas = new List<Marca>();
                    foreach(var obj2 in (_context.InsumosMarcas.Where(x => x.InsumoId.Equals(int.Parse(arrIdInsumoPresentacion[0])))))
                    {
                        marcas.Add(_context.Marca.Where(x => x.MarcaId.Equals(obj2.MarcaId)).FirstOrDefault());
                    }
                    if (isFirstElement)
                    {
                        resultado += toStringifyInsumo(insumo, presentacion, marcas);
                        isFirstElement = false;
                    }
                    else
                    {
                        resultado += ", " + toStringifyInsumo(insumo, presentacion, marcas);
                    }
                }
                resultado += "]";
            }
            else
            {
                string[] arrIdInsumoPresentacion = arrJSON.Split("-");
                Insumo insumo = _context.Insumo.Where(x => x.Id.Equals(int.Parse(arrIdInsumoPresentacion[0]))).FirstOrDefault();

                IQueryable<Presentacion> presentacionIQ = _context.Presentacion.Where(x => x.idPresentacion.Equals(int.Parse(arrIdInsumoPresentacion[1])));
                presentacionIQ = presentacionIQ.Include(x => x.unidad);
                Presentacion presentacion = presentacionIQ.FirstOrDefault();

                List<Marca> marcas = new List<Marca>();
                foreach (var obj2 in (_context.InsumosMarcas.Where(x => x.InsumoId.Equals(int.Parse(arrIdInsumoPresentacion[0])))))
                {
                    marcas.Add(_context.Marca.Where(x => x.MarcaId.Equals(obj2.MarcaId)).FirstOrDefault());
                }
                resultado += toStringifyInsumo(insumo, presentacion, marcas);
            }
            return resultado;
        }
        [HttpPost]
        public JsonResult RegistrarCompras(string idInsumo, string cantidadStock)
        {
            string result = "Se modifico correctamente el inventario";

            try
            {
                Insumo insumoResult = _context.Insumo.Where(x => x.Id.Equals(int.Parse(idInsumo))).FirstOrDefault();
                insumoResult.Cantidad = insumoResult.Cantidad + int.Parse(cantidadStock);
                _context.Update(insumoResult);
                _context.Add(new InsumoPrecioHistorial()
                {
                    InsumoId = int.Parse(idInsumo),
                    Precio = float.Parse(cantidadStock),
                    //BajasMerma = 0,
                    Fecha = DateTime.Now
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                result = "Ocurrio un error en el servidor, intentalo más tarde.";
            }


            return Json(result);
        }
        private String toStringifyInsumo(Insumo insumo, Presentacion presentacion, List<Marca> marcas)
        {
            String result = "{";
            result += '"' + "id" + '"' + ": " + '"' + insumo.Id + '"' + ',';
            result += " " + '"' + "nombre" + '"' + ": " + '"' + insumo.Nombre + '"' + ','; 
            result += " " + '"' + "rutaImagen" + '"' + ": " + '"' + insumo.RutaImagen + '"' + ',';
            result += " " + '"' + "unidad" + '"' + ": " + '"' + presentacion.unidad.Nombre + '"' + ',';
            result += " " + '"' + "unidadPresentacion" + '"' + ": " + '"' + presentacion.cantidadUnidades + '"' + ',';
            result += " " + '"' + "precio" + '"' + ": " + '"' + presentacion.precioPresentacion + '"' + ',';
            result += " " + '"' + "presentacion" + '"' + ": " + '"' + presentacion.nombre + '"' + ',';
            result += " " + '"' + "marcas" + '"' + ": [";
            Boolean isFirstElement = true;
            foreach(var marca in marcas)
            {
                if (isFirstElement)
                {
                    result += '"' + marca.Nombre + '"';
                    isFirstElement = false;
                }
                else
                {
                    result += ", " + '"' + marca.Nombre + '"';
                }
            }
            result += "]}";
            return result;
        }


        ///<summary>
        ///Recupera del modelo los insumos cuya cantidad sea mayor a cero. También llena la ViewBag con 
        ///las categorías de dichos insumos. 
        ///</summary>
        ///<return>
        ///Devuelve la vista correspondiente al inventario con los datos obtenidos del modelo. 
        ///</return>
        public IActionResult ConsultarInventario()
        {
            IQueryable<Categoria> IQCategorias = _context.Insumo.Where(m => m.Cantidad > 0).Select(x => x.Categoria).Distinct();
            ViewBag.categorias = IQCategorias.ToList();

            IQueryable<Insumo> insumosIQ = from s in _context.Insumo where s.Cantidad > 0 select s;
            insumosIQ = insumosIQ.Include(x => x.Unidad);
            insumosIQ = insumosIQ.Include(x => x.Categoria);

            List<Insumo> insumosMenorMinimo = new List<Insumo>();
            List<Insumo> insumosCercaMinimo = new List<Insumo>();
            List<Insumo> insumos = new List<Insumo>();

            foreach (var insumo in insumosIQ)
            {
                if (insumo.Cantidad <= insumo.StockMinimo)
                {
                    insumosMenorMinimo.Add(insumo);
                }
                else if ((insumo.Cantidad - DIFERENCIA_STOCK_MINIMO) <= insumo.StockMinimo)
                {
                    insumosCercaMinimo.Add(insumo);
                }
                else
                {
                    insumos.Add(insumo);
                }
            }

            ViewBag.insumosMenoresMinimo = insumosMenorMinimo.Count;
            ViewBag.insumosCercaMinimo = insumosCercaMinimo.Count + insumosMenorMinimo.Count;

            insumosMenorMinimo.AddRange(insumosCercaMinimo);
            insumosMenorMinimo.AddRange(insumos);
            return View(insumosMenorMinimo);
        }

        // POST: Insumo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Tienda,Precio,UnidadId,StockMinimo")] Insumo insumo
            , [FromForm] string nueva_categoria, [FromForm] int select_categoria)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(nueva_categoria)) {
                    var categoria = new Categoria() { Nombre = nueva_categoria };
                    _context.Add(categoria);
                    await _context.SaveChangesAsync();
                    var id = _context.Categoria.Where(c => c.Nombre == nueva_categoria).Select(c => c.Id).FirstOrDefault();
                    insumo.CategoriaId = id;
                }
                else
                {
                    insumo.CategoriaId = select_categoria;
                }
                _context.Add(insumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insumo);
        }

        // GET: Insumo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumo.SingleOrDefaultAsync(m => m.Id == id);
            if (insumo == null)
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
            ViewBag.CategoriaActual = insumo.CategoriaId;
            ViewBag.precio = insumo.Precio;
            return View(insumo);
        }

        // POST: Insumo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Tienda,Precio,UnidadId,StockMinimo,Deshabilitado")] Insumo insumo
            , [FromForm] string nueva_categoria, [FromForm] int select_categoria, [FromForm] float precioAux, [FromForm]float mermaAux, [FromForm] float precioAjustadoAux)
        {
            if (id != insumo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!String.IsNullOrEmpty(nueva_categoria))
                    {
                        var categoria = new Categoria() { Nombre = nueva_categoria };
                        _context.Add(categoria);
                        await _context.SaveChangesAsync();
                        insumo.CategoriaId = _context.Categoria.Where(c => c.Nombre == nueva_categoria).Select(c => c.Id).FirstOrDefault();
                    }
                    else
                    {
                        insumo.CategoriaId = select_categoria;
                    }

                    _context.Add(new InsumoPrecioHistorial()
                    {
                        InsumoId = id,
                        Precio = precioAux
                    });

                    _context.Update(insumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsumoExists(insumo.Id))
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
            return View(insumo);
        }

        // GET: Insumo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumo
                .SingleOrDefaultAsync(m => m.Id == id);
            if (insumo == null)
            {
                return NotFound();
            }

            return View(insumo);
        }

        // POST: Insumo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insumo = await _context.Insumo.SingleOrDefaultAsync(m => m.Id == id);
            _context.Insumo.Remove(insumo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsumoExists(int id)
        {
            return _context.Insumo.Any(e => e.Id == id);
        }


        [HttpPost, ActionName("Deshabilitar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeshabilitarAsync(int id, bool estado) {
            var insumo = await _context.Insumo.SingleOrDefaultAsync(i => i.Id == id);
            if (insumo != null)
            {
                string Msg = "";
                if (estado)
                {
                    Msg = "El Insumo se ha Deshabilitado.";
                }
                else {
                    Msg = "El Insumo se ha Habilitado.";
                }
                insumo.Deshabilitado = estado;
                _context.Insumo.Update(insumo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {Id = id, mensaje = Msg });
            }
            else {
                return NotFound();
            }
        }

        public IActionResult HistorialPrecios(int id) {
           var lista = _context.InsumoPrecioHistorial.Where(i => i.InsumoId == id).ToList();
            return Ok(lista);
        }

        ///<summary>
        ///Obtiene del modelo los datos de un Insumo. 
        ///</summary>
        ///<return>
        ///Devuelve un objeto tipo Json con la información del Insumo. 
        ///</return>
        ///<param name="idInsumo">
        ///Identificador del Insumo que se quiere obtener del modelo. 
        ///</param>
        [HttpGet]
        public JsonResult ObtenerDatosInsumo(String idInsumo)
        {
            var ultimaFecha = _context.Insumo.Where(m => m.Cantidad > 0).Select(x => x.Categoria).Distinct();
            ViewBag.ultimaFecha = ultimaFecha;

            var consulta =
                from insumo in _context.Insumo
                where insumo.Id == int.Parse(idInsumo)
                join presentaciones in _context.InsumosPresentaciones on insumo.Id equals presentaciones.InsumoId
                join presentacion in _context.Presentacion on presentaciones.PresentacionId equals presentacion.idPresentacion into grupoPresentaciones
                join marcas in _context.InsumosMarcas on insumo.Id equals marcas.InsumoId
                join marca in _context.Marca on marcas.MarcaId equals marca.MarcaId into grupoMarcas
                join insumoHistorial in _context.InsumoPrecioHistorial on insumo.Id equals insumoHistorial.InsumoId orderby insumoHistorial.Fecha descending
                select new
                {
                    id = insumo.Id,
                    imagen = insumo.RutaImagen,
                    insumoNombre = insumo.Nombre,
                    unidad = insumo.Unidad,
                    stockMin = insumo.StockMinimo,
                    stock = insumo.Cantidad,
                    presentaciones = grupoPresentaciones,
                    marcas = grupoMarcas,
                    ultimaFecha = insumoHistorial.Fecha.ToShortDateString(),
                   //bajasMerma = insumoHistorial.BajasMerma
                };

            return Json(consulta);
        }

        ///<summary>
        ///Reduce el atributo cantidad de un Insumo en el modelo. 
        ///</summary>
        ///<param name="idInsumo">
        ///Identificador del Insumo del que se quiere reducir la cantidad. 
        ///</param>
        ///<param name="cantidad">
        ///Cantidad que se quiere descontar.
        ///</param>
        [HttpPost]
        public void DescontarInventario(String idInsumo, String cantidad)
        {
            Insumo insumo = (from p in _context.Insumo
                             where p.Id == int.Parse(idInsumo)
                             select p).SingleOrDefault();

            insumo.Cantidad = insumo.Cantidad - int.Parse(cantidad);

            InsumoPrecioHistorial insumoHistorial = (from p in _context.InsumoPrecioHistorial
                                                     where p.InsumoId == int.Parse(idInsumo)
                                                     select p).LastOrDefault();
            //insumoHistorial.BajasMerma = insumoHistorial.BajasMerma + int.Parse(cantidad);

            _context.SaveChanges();
        }
    }
}
