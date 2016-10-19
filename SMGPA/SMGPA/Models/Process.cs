using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Process
    {
        public Process()
        {
            Operations = new HashSet<Operation>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idProcess { get; set; }
        [Required(ErrorMessage = "Criterio es Requerido.")]
        public string Criterio { get; set; }
        [Required(ErrorMessage = "Descripción es Requerida.")]
        public string Descripcion { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}