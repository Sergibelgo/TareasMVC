using Tutorial2TareasMVC.Models;

namespace Tutorial2TareasMVC.Services
{
    public interface IStoreService
    {
        Task Borrar(string ruta, string contenedor);
        Task<AlmacenarArchivoResultado[]> Almacenar(string contenedor, IEnumerable<IFormFile> archivos);

    }
    public class StoreService
    {
    }
}
