//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoATECA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            this.BitacoraUsuarios = new HashSet<BitacoraUsuario>();
            this.HistorialAtendidos = new HashSet<HistorialAtendido>();
        }

        private const string Nombre =
              "^[a-zA-ZÁÉÍÓÚáéíóú']+$";
        private const string Apellidos =
        "^[a-zA-Z ÁÉÍÓÚáéíóú']+$";
        private const string Cedulas =
        "^[a-zA-Z ÁÉÍÓÚáéíóú0-9]+$";
        private const string Numeros =
        "^[0-9]*$";

        [Display(Name = "Identificador")]
        public int ID_usuario { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Nombre")]
        [StringLength(15, MinimumLength = 3,
        ErrorMessage = "El nombre debe contener entre 2 y 15 caractéres de longitud")]
        [DataType(DataType.Text)]
        [RegularExpression(Nombre, ErrorMessage = "Solo se admiten letras")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Cédula de identidad")]
        [StringLength(25, MinimumLength = 9,
        ErrorMessage = "La cédula debe ser de 9 dígitos mínimo y 25 máximo")]
        [DataType(DataType.Text)]
        [RegularExpression(Cedulas, ErrorMessage = "Solo se admiten números")]
        public string cedula { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Apellidos")]
        [StringLength(30, MinimumLength = 3,
        ErrorMessage = "Los apellidos deben contener entre 2 y 30 caractéres de longitud")]
        [DataType(DataType.Text)]
        [RegularExpression(Apellidos, ErrorMessage = "Solo se admiten letras")]
        public string apellidos { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime fechaNacimiento { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debe ser una dirección de correo válida")]
        [StringLength(45, MinimumLength = 7,
        ErrorMessage = "El debe estar entre 7 y 45 caractéres de longitud")]
        public string correo { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Compare("contraseña", ErrorMessage = "Las contraseñas no coinciden")]
        public string contraseña { get; set; }
        public Nullable<int> ID_rol { get; set; }

        public string estado { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        [Compare("contraseña", ErrorMessage = "Las contraseñas no coinciden")]
        public string confirmarcontrasena { get; set; }
        public Nullable<System.Guid> codigoActivacion { get; set; }
        public bool correoVerificado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BitacoraUsuario> BitacoraUsuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistorialAtendido> HistorialAtendidos { get; set; }
        public virtual Role Role { get; set; }
    }
}
