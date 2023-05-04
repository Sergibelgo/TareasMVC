using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial2TareasMVC.DBContext;
using Tutorial2TareasMVC.Entitys;
using Tutorial2TareasMVC.Models;
using Tutorial2TareasMVC.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tutorial2TareasMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasosController : ControllerBase
    {
        private readonly ContextDB _contextDB;
        private readonly IUserService _userService;

        public PasosController(ContextDB contextDB, IUserService userService)
        {
            this._contextDB = contextDB;
            this._userService = userService;
        }


        // POST api/<PasosController>
        [HttpPost("{tareaId:int}")]
        public async Task<IActionResult> Post(int tareaId, [FromBody] PasoCrearDTO model)
        {
            var usuarioId = _userService.ObtenerUsuarioId();
            var tarea = await _contextDB.Tareas.Include(t=>t.UsuarioCreacion).FirstOrDefaultAsync(t => t.Id == tareaId);
            if (tarea is null)
            {
                return NotFound();
            }
            if (tarea.UsuarioCreacion.Id!=usuarioId)
            {
                return Forbid();
            }
            var existenPasos = await _contextDB.Pasos.AnyAsync(p=>p.TareaId==tareaId);
            var ordenMayor = 0;
            if (existenPasos)
            {
                ordenMayor = (await _contextDB.Pasos.Where(p => p.TareaId == tareaId).Select(p => p.Orden).MaxAsync())+1;
            }
            Paso paso = new()
            {
                TareaId = tareaId,
                Orden = ordenMayor,
                Descripcion = model.Descripcion,
                Realizado = model.Realizado
            };
            _contextDB.Add(paso);
            await _contextDB.SaveChangesAsync();
            return Ok(paso);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PasoCrearDTO model)
        {
            var usuarioId = _userService.ObtenerUsuarioId();
            var paso = await _contextDB.Pasos.Include(p=>p.Tarea).ThenInclude(p=>p.UsuarioCreacion).FirstOrDefaultAsync(p=> p.Id==id);
            if(paso is null)
            {
                return NotFound();
            }
            if (paso.Tarea.UsuarioCreacion.Id!=usuarioId)
            {
                return Forbid();
            }
            paso.Descripcion = model.Descripcion;
            paso.Realizado = model.Realizado;
            await _contextDB.SaveChangesAsync();
            return Ok(paso);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var usuarioId = _userService.ObtenerUsuarioId();
            var paso = await _contextDB.Pasos.Include(p => p.Tarea).ThenInclude(p => p.UsuarioCreacion).FirstOrDefaultAsync(p => p.Id == id);
            if (paso is null)
            {
                return NotFound();
            }
            if (paso.Tarea.UsuarioCreacion.Id!=usuarioId)
            {
                return Forbid();
            }
            _contextDB.Remove(paso);
            await _contextDB.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("ordenar/{tareaId:int}")]
        public async Task<IActionResult> Ordenar(int tareaId, [FromBody] Guid[] ids)
        {
            var usuarioId = _userService.ObtenerUsuarioId();
            var tarea = await _contextDB.Tareas.FirstOrDefaultAsync(t=>t.Id==tareaId&&t.UsuarioCreacion.Id==usuarioId);
            if (tarea is null)
            {
                return NotFound();
            }
            var pasos = await _contextDB.Pasos.Where(x => x.Tarea.Id == tareaId).ToListAsync();
            var pasosIds = pasos.Select(x => x.Id);
            var isPert = ids.Except(pasosIds).ToList();
            if (isPert.Any())
            {
                return BadRequest("No todos los pasos");
            }
            var pasosDiccionario = pasos.ToDictionary(p => p.Id);
            for (int i = 0; i < ids.Length; i++)
            {
                var pasoId = ids[i];
                var paso = pasosDiccionario[pasoId];
                paso.Orden = i + 1;
            }
            await _contextDB.SaveChangesAsync();
            return Ok();
        }

    }
}
