using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Career
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idCarrera { get; set; }
        [Required(ErrorMessage = "Debe especificar el nombre de carrera")]
        public string Nombre { get; set; }
    }
}