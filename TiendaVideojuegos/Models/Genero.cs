//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TiendaVideojuegos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Genero
    {
        public Genero()
        {
            this.Juego = new List<Juego>();
        }
    
        public byte IdGenero { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Nombre Génerp")]
        public string NombreGenero { get; set; }
    
        public virtual List<Juego> Juego { get; set; }
    }
}
