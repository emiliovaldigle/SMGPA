using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Notificacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idNotification { get; set; }
        public string Cuerpo { get; set; }
        public DateTime? Fecha { get; set; }
        public bool Vista { get; set; }
        public string UrlAction { get; set; }
        public Guid idUser { get; set; }
        [ForeignKey("idUser")]
        public virtual Functionary Funcionario{get;set;}
    }
}