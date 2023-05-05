using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial2TareasMVC.DBContext;
using Tutorial2TareasMVC.Entitys;
using Tutorial2TareasMVC.Services;

namespace Tutorial2TareasMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private readonly ContextDB _contextDB;
        private readonly IStoreService _storeService;
        private readonly IUserService _userService;
        private readonly string contenedor = "archivosAdjuntos";

        public ArchivosController(ContextDB contextDB, IStoreService storeService, IUserService userService)
        {
            this._contextDB = contextDB;
            this._storeService = storeService;
            this._userService = userService;
        }
        [HttpPost("{tareaId}")]
        public async Task<IActionResult> Post(int tareaId, [FromForm] IEnumerable<IFormFile> archivos)
        {
            var usuarioId = _userService.ObtenerUsuarioId();
            var tarea = await _contextDB.Tareas.Include(t => t.UsuarioCreacion).FirstOrDefaultAsync(t => t.Id == tareaId);
            if (tarea is null)
            {
                return NotFound();
            }
            if (tarea.UsuarioCreacion.Id != usuarioId)
            {
                return Forbid();
            }
            var existenArchivosAdjuntos = await _contextDB.ArchivoAdjuntos.AnyAsync(a => a.TareaId == tareaId);
            var ordenMayor = 0;
            if (existenArchivosAdjuntos)
            {
                ordenMayor = await _contextDB.ArchivoAdjuntos.Where(a => a.TareaId == tareaId).Select(a => a.Orden).MaxAsync();
            }
            var resultados = await _storeService.Almacenar(contenedor, archivos);
            var archivosAdjuntos = resultados.Select((resultado, indice) => new ArchivoAdjunto
            {
                TareaId = tareaId,
                FechaCreacion = DateTime.UtcNow,
                Url = resultado.URL,
                Titulo = resultado.Titulo,
                Orden = ordenMayor + indice + 1
            }).ToList();
            _contextDB.AddRange(archivosAdjuntos);
            await _contextDB.SaveChangesAsync();
            return Ok(archivosAdjuntos);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] string titulo)
        {
            var usuarioId = _userService.ObtenerUsuarioId();
            var archivoAdjunto = await _contextDB.ArchivoAdjuntos.Include(a => a.Tarea).ThenInclude(a => a.UsuarioCreacion).FirstOrDefaultAsync(a => a.Id == id);
            if (archivoAdjunto is null)
            {
                return NotFound();
            }
            if (archivoAdjunto.Tarea.UsuarioCreacion.Id != usuarioId)
            {
                return Forbid();
            }
            archivoAdjunto.Titulo = titulo;
            await _contextDB.SaveChangesAsync();
            return Ok();
        }
    }
}
