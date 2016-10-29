using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Entities
    {
        public Entities()
        {
            Involucrados = new HashSet<Functionary>();
            Activo = true;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idEntities { get; set; }
        [Required(ErrorMessage = "Nombre es Requerido.")]
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public Guid idFunctionary { get; set; }
        [ForeignKey("idFunctionary")]
        public virtual ICollection<Functionary> Involucrados { get; set; }
        
    }
}