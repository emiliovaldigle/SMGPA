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
        public Career(){
            Activa = true;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idCareer { get; set; }
        [Required(ErrorMessage = "Debe especificar el nombre de carrera")]
        [StringLength(80, ErrorMessage = "Nombre muy Largo")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe especificar una descripción para la carrera")]
        public string Descripcion { get; set; }
        public bool Activa { get; set; }
        public Guid? idFaculty { get; set; }
        [ForeignKey("idFaculty")]
        public virtual Faculty Facultad { get; set; }
    }
}