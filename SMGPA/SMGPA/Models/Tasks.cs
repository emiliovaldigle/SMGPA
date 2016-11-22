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
            Documentos = new HashSet<Document>();
            Estado = StatusEnum.INACTIVA;
            Reprogramaciones = 0;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idTask { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public StatusEnum Estado { get; set; }
        public Guid? idFunctionary { get; set; }
        public Guid? idEntities { get; set; }
        public Guid? idOperation { get; set; }
        public Guid? idPredecesora { get; set; }
        [ForeignKey("idPredecesora")]
        public virtual Tasks Predecesora { get; set; }
        public virtual ICollection<Document> Documentos { get; set; }
        [ForeignKey("idFunctionary")]
        public virtual Functionary Responsable { get; set; }
        [ForeignKey("idEntities")]
        public virtual Entities Participantes { get; set; }
        [ForeignKey("idOperation")]
        public virtual Operation Operacion { get; set; }
        public virtual ICollection<Observation> Observaciones { get; set; }
        public int? Reprogramaciones { get; set; }
    }
    public enum StatusEnum { INACTIVA, ACTIVA, EN_PROGRESO, COMPLETADA, PENDIENTE }
    
}