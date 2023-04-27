using System.ComponentModel.DataAnnotations;

namespace Tutorial2TareasMVC.Models
{
    public class RegistroDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
