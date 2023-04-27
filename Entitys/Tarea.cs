using System.ComponentModel.DataAnnotations;

namespace Tutorial2TareasMVC.Entitys
{
    public class Tarea
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<Paso> Pasos { get; set; }
        public List<ArchivoAdjunto> ArchivoAdjuntos { get; set; }
    }
}
