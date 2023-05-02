using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tutorial2TareasMVC.Services
{
    public class Constantes
    {
        public const string RolAdmin = "admin";
        public static readonly SelectListItem[] culturasUISoportadas = new SelectListItem[]
        {
            new SelectListItem{Value="es",Text="Español" },
            new SelectListItem{Value="en",Text="English"}
        };
    }
}
