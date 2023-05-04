using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial2TareasMVC.DBContext;
using Tutorial2TareasMVC.Entitys;
using Tutorial2TareasMVC.Models;
using Tutorial2TareasMVC.Services;

namespace Tutorial2TareasMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ContextDB _contextDB;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public TareasController(ContextDB contextDB, IUserService userService,IMapper mapper)
        {
            this._contextDB = contextDB;
            this.userService = userService;
            this.mapper = mapper;
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
            var tareas = await _contextDB.Tareas
                .Where(t => t.UsuarioCreacion.Id == usuarioId)
                .OrderBy(t => t.Orden)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(tareas);
        }
        [HttpPost("ordenar")]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = userService.ObtenerUsuarioId();
            var tareas = await _contextDB.Tareas.Where(t => t.UsuarioCreacion.Id == usuarioId).ToListAsync();
            var tareasId = tareas.Select(t => t.Id);
            var isPropetary=ids.Except(tareasId).ToList();
            if (isPropetary.Any())
            {
                return Forbid();
            }
            var tareasDiccionario = tareas.ToDictionary(x => x.Id);
            for(int i=0; i<ids.Length; i++)
            {
                var id = ids[i];
                var tarea = tareasDiccionario[id];
                tarea.Orden = i + 1;
            }
            await _contextDB.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var usuarioId = userService.ObtenerUsuarioId();
            var tarea = await _contextDB.Tareas.Where(t => t.UsuarioCreacion.Id == usuarioId && t.Id==id).FirstOrDefaultAsync();
            if (tarea is null)
            {
                return Forbid();
            }
            return Ok(tarea);
        }
    }
}
