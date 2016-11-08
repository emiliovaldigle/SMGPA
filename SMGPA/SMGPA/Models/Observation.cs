using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Observation
    {
        public Observation()
        {
            FechaComentario = DateTime.Now;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idObservation { get; set; }
        public DateTime FechaComentario { get; set; }
        [Required(ErrorMessage ="Se requiere un comentario")]
        [StringLength(200, ErrorMessage = "Comentario muy extenso")]
        public string Comentario { get; set; }
        [Required(ErrorMessage ="Se requiere especificar el estado de validación")]
        public Validate ValidacionEstatus { get; set; }
        public virtual Tasks Tarea { get; set; }
        public virtual Functionary Funcionario { get; set; }
    }
    public enum Validate { APROBADO, RECHAZADO };
}