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
    
    public partial class Ficha
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ficha()
        {
            this.HistorialAtendidos = new HashSet<HistorialAtendido>();
        }
    
        public int ID_ficha { get; set; }
        public int ID_servicio { get; set; }
        public string codigoFicha { get; set; }
        public int numeroFicha { get; set; }
        public System.DateTime fecha { get; set; }
    
        public virtual Servicio Servicio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistorialAtendido> HistorialAtendidos { get; set; }
    }
}