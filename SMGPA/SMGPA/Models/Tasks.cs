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
            Estado = StatusEnum.INACTIVA;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idTask { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public double TiempoInactividad { get; set; }
        public double DesplazamientoHoras { get; set; }
        public double DesplazamientoDias { get; set; }
        public StatusEnum Estado { get; set; }
        public Guid idDocument { get; set; }
        public Guid idFunctionary { get; set; }
        public Guid idEntities { get; set; }
        [ForeignKey("idDocument")]
        public Document Documento { get; set; }
        [ForeignKey("idFunctionary")]
        public virtual Functionary Responsable { get; set; }
        [ForeignKey("idEntities")]
        public virtual Entities Participantes { get; set; }
        public virtual ICollection<Observation> Observaciones { get; set; }
    }
    public enum StatusEnum { INACTIVA, ACTIVA, EN_PROGRESO, EN_REVISION, PENDIENTE }
}