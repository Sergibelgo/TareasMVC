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
        // GET: api/<PasosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PasosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PasosController>
        [HttpPost("{tareaId:int}")]
        public async Task<IActionResult> Post(int tareaId, [FromBody] PasoCrearDTO model)
        {
            var usuarioId = _userService.ObtenerUsuarioId();
            var tarea = await _contextDB.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId);
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

        // PUT api/<PasosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PasosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
