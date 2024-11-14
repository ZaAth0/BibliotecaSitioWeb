using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bibliotecaaa.Data;
using Bibliotecaaa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Bibliotecaaa.Controllers
{
    public class LibroesController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public LibroesController(BibliotecaContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Libroes
        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros
                                       .Include(l => l.GeneroNavigation)  // Cargar relación con Genero
                                       .Include(l => l.AutorNavigation)  // Cargar relación con Autor
                                       .ToListAsync();
            return View(libros);
        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.GeneroNavigation)  // Incluir género al consultar detalle
                .Include(l => l.AutorNavigation)  // Incluir autor al consultar detalle
                .FirstOrDefaultAsync(m => m.IdLibro == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libroes/Create
        public async Task<IActionResult> Create()
        {
            // Traemos todos los géneros para la lista desplegable
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "id_genero", "nombre");
            // Traemos todos los autores para la lista desplegable
            ViewBag.Autores = new SelectList(await _context.Autores.ToListAsync(), "IdAutor", "AutorNombre");
            return View();
        }

        // POST: Libroes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLibro,Titulo,Genero,Descripcion,Portada,FechaPublicacion,Disponible,FechaAgregado")] Libro libro, string AutorNombre)
        {
            if (ModelState.IsValid)
            {
                // Verificar si se proporcionó un nombre de autor
                if (!string.IsNullOrEmpty(AutorNombre))
                {
                    // Buscar si el autor ya existe en la base de datos
                    var autorExistente = await _context.Autores
                        .FirstOrDefaultAsync(a => a.AutorNombre == AutorNombre);

                    if (autorExistente == null)
                    {
                        // Si el autor no existe, crear uno nuevo
                        var nuevoAutor = new Autor { AutorNombre = AutorNombre };
                        _context.Add(nuevoAutor);
                        await _context.SaveChangesAsync(); // Guardar el nuevo autor

                        // Asignar el Id del nuevo autor al libro
                        libro.AutorRel = nuevoAutor.IdAutor;
                    }
                    else
                    {
                        // Si el autor ya existe, asignar el Id del autor existente al libro
                        libro.AutorRel = autorExistente.IdAutor;
                    }
                }

                // Agregar el libro a la base de datos
                _context.Add(libro);
                await _context.SaveChangesAsync();  // Guardar libro

                return RedirectToAction(nameof(Index)); // Redirigir a la página de lista de libros
            }

            // Si la validación falla, cargar la lista de géneros y autores
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "id_genero", "nombre");
            ViewBag.Autores = new SelectList(await _context.Autores.ToListAsync(), "IdAutor", "AutorNombre");
            return View(libro); // Volver al formulario con los errores de validación
        }

        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cargar el libro junto con su autor y género
            var libro = await _context.Libros
                .Include(l => l.AutorNavigation)  // Cargar el autor asociado al libro
                .Include(l => l.GeneroNavigation) // Cargar el género asociado al libro
                .FirstOrDefaultAsync(m => m.IdLibro == id);

            if (libro == null)
            {
                return NotFound();
            }

            // Pasa la lista de géneros al ViewBag para el DropDownList
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "IdGenero", "GeneroNombre", libro.Genero);

            // Pasa la lista de autores al ViewBag y el autor del libro ya seleccionado
            ViewBag.Autores = new SelectList(await _context.Autores.ToListAsync(), "IdAutor", "AutorNombre", libro.AutorRel);

            return View(libro);
        }

        // POST: Libroes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLibro,Titulo,AutorRel,Genero,Descripcion,Portada,ISBN,FechaPublicacion,Disponible,FechaAgregado")] Libro libro)
        {
            if (id != libro.IdLibro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Aquí manejamos las relaciones de autor y género.
                    // Si el autor es nulo o no existe, se debe manejar la lógica para agregar o vincular uno existente.

                    // Actualizar el libro en la base de datos
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.IdLibro))
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

            // Si hay errores de validación, recargar los selectList para géneros y autores
            ViewBag.Generos = new SelectList(await _context.Generos.ToListAsync(), "IdGenero", "GeneroNombre", libro.Genero);
            ViewBag.Autores = new SelectList(await _context.Autores.ToListAsync(), "IdAutor", "AutorNombre", libro.AutorRel);
            return View(libro);
        }

        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.GeneroNavigation)  // Incluir género al consultar para eliminar
                .Include(l => l.AutorNavigation)  // Incluir autor al consultar para eliminar
                .FirstOrDefaultAsync(m => m.IdLibro == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                // Eliminar la imagen de la portada si existe
                if (!string.IsNullOrEmpty(libro.Portada))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, libro.Portada);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath); // Eliminar el archivo
                    }
                }

                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.IdLibro == id);
        }
    }
}
