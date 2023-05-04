using System.ComponentModel.DataAnnotations;

namespace Tutorial2TareasMVC.Models
{
    public class TareaEditarDTO
    {
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
