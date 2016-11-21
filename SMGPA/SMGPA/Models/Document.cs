using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idDocument { get; set; }
        [NotMapped]
        public HttpPostedFileBase Documento { get; set; }
        public string Path { get; set; }
        public Guid idTask { get; set; }
        [ForeignKey("idTask")]
        public Tasks Tarea { get; set; }
        public DateTime Fecha { get; set; }
    }
}