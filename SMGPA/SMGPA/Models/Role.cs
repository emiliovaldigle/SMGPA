using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Role
    {
        public Role()
        {
            Permisos = new HashSet<Permission>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idRole { get; set; }
        [Required(ErrorMessage = "El campo Nombre es Obligatorio")]
        [StringLength(20, ErrorMessage ="Nombre muy largo")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Descripción es Obligatorio")]
        [StringLength(200, ErrorMessage = "Descripción muy extensa")]
        public string Descripcion { get; set; }
        public virtual ICollection<Permission> Permisos { get; set; }
    }
}