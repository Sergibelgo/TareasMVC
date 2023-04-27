using System.ComponentModel.DataAnnotations;

namespace Tutorial2TareasMVC.Models
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Recuérdame")]
        public bool Recuerdame { get; set; }
    }
}
