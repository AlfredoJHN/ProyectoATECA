using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoATECA.Models
{
    public class LoginUsuario
    {
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ser una dirección de correo válida")]
        [StringLength(45, MinimumLength = 7,
        ErrorMessage = "El debe estar entre 7 y 45 caractéres de longitud")]
        public string correo { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string contraseña { get; set; }

        [Display(Name = "Recordar usuario")]
        public bool RememberMe { get; set; }
        
    }
}