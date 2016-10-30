using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMGPA.Models
{
    public class Functionary: User
    {
        [Required(ErrorMessage = "Número Telefónico es requerido.")]
        public string NumeroTelefono { get; set; }
        [Required(ErrorMessage = "Correo Personal es requerido.")]
        public string CorreoPersonal { get; set; }
        public Guid idCareer { get; set; }
        [ForeignKey("idCareer")]
        public virtual Career Carrera { get; set; }
        public virtual ICollection<Entities> Entidades { get; set; }

    }
}