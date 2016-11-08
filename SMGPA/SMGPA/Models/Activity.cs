using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Activity
    {
        public Activity(){
            Tareas = new HashSet<Tasks>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idActivity { get; set; }
        public States state { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        [StringLength(50, ErrorMessage = "Nombre muy Largo")]
        [Required(ErrorMessage = "Nombre es Requerido.")]
        public string Nombre { get; set; }
        public Guid idProcess { get; set; }
        [ForeignKey("idProcess")]
        public virtual Process Proceso { get; set; }
        public virtual ICollection<Tasks> Tareas { get; set; }

        
    }
    public enum States 
        {Activa, Inactiva, Completada};
}