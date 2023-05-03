using System.Security.Claims;

namespace Tutorial2TareasMVC.Services
{
    public interface IUserService
    {
        string ObtenerUsuarioId();
    }
    public class UserService:IUserService
    {
        private HttpContext context;

        public UserService(IHttpContextAccessor httpContext)
        {
            context = httpContext.HttpContext;
        }
        public string ObtenerUsuarioId()
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var idClaim=context.User.Claims
                    .Where(x=>x.Type==ClaimTypes.NameIdentifier).FirstOrDefault();
                return idClaim.Value;
            }
            else
            {
                throw new Exception("El usuario no esta autenticado");
            }
        }
    }
}
