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

    public partial class HistorialAtendido
    {
        [Display(Name = "Identificador")]
        public int ID_historialAtendido { get; set; }
        [Display(Name = "Servicio")]
        public int ID_servicio { get; set; }
        [Display(Name = "Identificador de ficha")]
        public int ID_ficha { get; set; }
        [Display(Name = "Inicio")]
        public System.DateTime horaInicio { get; set; }
        [Display(Name = "Fin")]
        public System.DateTime horaFin { get; set; }
        [Display(Name = "Duración en segundos")]
        public int duracion { get; set; }
        [Display(Name = "Fecha")]
        public System.DateTime fecha { get; set; }
        [Display(Name = "Identificador de usuario")]
        public int ID_usuario { get; set; }
    
        public virtual Ficha Ficha { get; set; }
        public virtual Servicio Servicio { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
