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
    public class TareasController : ControllerBase
    {
        private readonly ContextDB _contextDB;
        private readonly IUserService userService;

        public TareasController(ContextDB contextDB, IUserService userService)
        {
            this._contextDB = contextDB;
            this.userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string titulo)
        {
            var usuarioId = userService.ObtenerUsuarioId();
            var existenTareas = await _contextDB.Tareas.Include(t => t.UsuarioCreacion).AnyAsync(t => t.UsuarioCreacion.Id == usuarioId);
            var ordenMayor = 0;
            if (existenTareas)
            {
                ordenMayor = await _contextDB.Tareas.Include(t => t.UsuarioCreacion).Where(t => t.UsuarioCreacion.Id == usuarioId).Select(t => t.Orden).MaxAsync();
            }
            var tarea = new Tarea
            {
                Titulo = titulo,
                UsuarioCreacion = await _contextDB.Users.Where(u => u.Id == usuarioId).FirstOrDefaultAsync(),
                FechaCreacion = DateTime.Now,
                Orden = ordenMayor + 1
            };
            _contextDB.Add(tarea);
            await _contextDB.SaveChangesAsync();
            return Ok(tarea);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarioId = userService.ObtenerUsuarioId();
            var tareas = await _contextDB.Tareas.Where(t=>t.UsuarioCreacion.Id==usuarioId).ToListAsync();
            return Ok(tareas);
        }
    }
}
