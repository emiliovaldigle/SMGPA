using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMPA.Models
{
    public class Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OperationId { get; set; }
        [Required(ErrorMessage = "Nombre es Requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Descripción es Requerida.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Indique si hay Observador activo")]
        public bool ActiveObserver { get; set; }
        public Guid ProcessId { get; set; }
        public OperationType Type { get; set; }
        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; }
		
	}
	public enum OperationType { Upload, Send, Validate}
}