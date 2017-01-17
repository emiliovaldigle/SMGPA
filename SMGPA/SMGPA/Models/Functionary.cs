using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMGPA.Models
{
    public class Functionary : User
    {
        public Functionary()
        {
            Entidades = new HashSet<Entities>();
            Observaciones = new HashSet<Observation>();
            Notificaciones = new HashSet<Notificacion>();
            FuncionarioEntidad = new HashSet<FunctionaryEntity>();
            Carrera = null;
        }
        [Required(ErrorMessage = "Número Telefónico es requerido.")]
        [StringLength(8, ErrorMessage = "Numero muy Largo")]
        public string NumeroTelefono { get; set; }
        [Required(ErrorMessage = "Correo Personal es requerido.")]
        [StringLength(50, ErrorMessage = "Correo muy Largo")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Correo Personal inválido.")]
        public string CorreoPersonal { get; set; }
        public Guid? idCareer { get; set; }
        [ForeignKey("idCareer")]
        public virtual Career Carrera { get; set; }
        public ICollection<Entities> Entidades { get; set; }
        public virtual ICollection<Observation> Observaciones { get; set; }
        public virtual ICollection<Notificacion> Notificaciones { get; set; }
        public ICollection<FunctionaryEntity> FuncionarioEntidad { get; set; }

    }
}