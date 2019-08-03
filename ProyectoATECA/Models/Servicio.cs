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

    public partial class Servicio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Servicio()
        {
            this.BitacoraServicios = new HashSet<BitacoraServicio>();
            this.Fichas = new HashSet<Ficha>();
            this.HistorialAtendidos = new HashSet<HistorialAtendido>();
            this.Sucursales = new HashSet<Sucursale>();
        }
        private const string Letras =
        "^[a-zA-Z ÁÉÍÓÚáéíóú'0-9]+$";

        [Display(Name = "Identificador")]
        public int ID_servicio { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        [StringLength(45, MinimumLength = 5,
        ErrorMessage = "El nombre del servicio debe contener entre 5 y 45 caractéres de longitud")]
        [DataType(DataType.Text)]
        [RegularExpression(Letras, ErrorMessage = "Solo se admiten letras y números")]
        public string nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BitacoraServicio> BitacoraServicios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ficha> Fichas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistorialAtendido> HistorialAtendidos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sucursale> Sucursales { get; set; }
    }
}
