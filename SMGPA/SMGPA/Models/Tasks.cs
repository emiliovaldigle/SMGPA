using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Tasks
    {
        public Tasks()
        {
            Observaciones = new HashSet<Observation>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idTask { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public double TiempoInactividad { get; set; }
        public double Desplazamiento { get; set; }

        public string Documento { get; set; }
        public StatusEnum Estado { get; set; }
        public Guid idFunctionary { get; set; }
        public Guid idEntities { get; set; }
        public Guid idObservation { get; set; }
        [ForeignKey("idFunctionary")]
        public virtual Functionary Responsable { get; set; }
        [ForeignKey("idEntities")]
        public virtual Entities Participantes { get; set; }
        [ForeignKey("idObservation")]
        public virtual ICollection<Observation> Observaciones { get; set; }
    }
    public enum StatusEnum { INACTIVA, ACTIVA, EN_PROGRESO, EN_REVISION, PENDIENTE }
}