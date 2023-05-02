﻿using System.ComponentModel.DataAnnotations;

namespace Tutorial2TareasMVC.Models
{
    public class RegistroDTO
    {
        [Required(ErrorMessage ="Error.Requerido")]
        [EmailAddress(ErrorMessage ="Error.Email")]
        public string Email { get; set; }
        [Required (ErrorMessage ="Error.Requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
