using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial2TareasMVC.DBContext;
using Tutorial2TareasMVC.Services;

namespace Tutorial2TareasMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ContextDB _contextDB;
        private readonly IUserService userService;

        public TareasController(ContextDB contextDB,IUserService userService)
        {
            this._contextDB = contextDB;
            this.userService = userService;
        }
    }
}
