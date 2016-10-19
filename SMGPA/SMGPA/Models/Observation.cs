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
        public string Comentario { get; set; }
        public bool ValidacionEstatus { get; set; }
        public Guid idUser { get; set; }
        public Guid idTask { get; set; }
        [ForeignKey("idUser")]
        public virtual Functionary Funcionario { get; set; }
        [ForeignKey("idTask")]
        public virtual Tasks Tarea { get; set; }
    }
}