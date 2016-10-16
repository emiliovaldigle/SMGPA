using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGMPA.Models
{
    public class Process
    {
        public Process()
        {
            Operations = new HashSet<Operation>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProcessId { get; set; }
        [Required(ErrorMessage = "Nombre es Requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Descripción es Requerida.")]
        public string Description { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}