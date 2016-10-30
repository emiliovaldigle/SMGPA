using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMGPA.Models
{
    public class Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idOperation { get; set; }
        [Required(ErrorMessage = "Nombre es Requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Descripción es Requerida.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Tipo de operación es Requerida.")]
        public OperationType Type { get; set; }
        public Guid idProcess { get; set; }
        [ForeignKey("idProcess")]
        public virtual Process Process { get; set; }
		
	}
	public enum OperationType { SUBIR_DOCUMENTO, VALIDAR_DOCUMENTO, ENVIAR_DOCUMENTO, HACER_TEST, SUBIR_TEST}
}