using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMGPA.Models
{
    public class Operation
    {
        public Operation()
        {
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idOperation { get; set; }
        [Required(ErrorMessage = "Nombre es Requerido.")]
        [StringLength(100, ErrorMessage = "Nombre muy Largo")]
        public string Nombre { get; set; }
        [StringLength(200, ErrorMessage = "Descripción muy extensa")]
        [Required(ErrorMessage = "Descripción es Requerida.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Debe indicar si el responsable es un Funcionario o una  Entidad")]
        public OperationType Type { get; set; }
        [Required(ErrorMessage = "Debe especificar si la Operación es validada por una Entidad")]
        public bool Validable { get; set; }
        public Guid idProcess { get; set; }
        [ForeignKey("idProcess")]
        public virtual Process Process { get; set; }
        public Guid? idPredecesora { get; set; }
        [ForeignKey("idPredecesora")]
        public virtual Operation Predecesora { get; set; }
        [Required(ErrorMessage ="Especifíque el tipo de Operación")]
        public OperationClass Clase {get; set;}
        [Required(ErrorMessage ="Debe indicar la cantidad de iteraciones permitidas para esta Operación")]
        public int IteracionesPermitidas { get; set; }
	}
    public enum OperationType { ENTIDAD, FUNCIONARIO }
    public enum OperationClass { SUBIR_DOCUMENTO}
}