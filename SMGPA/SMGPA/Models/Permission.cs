using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace SMGPA.Models
{
    public class Permission
    {
        public Permission()
        {
            Roles = new HashSet<Role>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid idPermission { get; set; }
        [Required(ErrorMessage = "El campo Texto es Obligatorio")]
        public string TextLink { get; set; }
        [Required(ErrorMessage = "El campo Controlador es Obligatorio")]
        public string Controller { get; set; }
        [Required(ErrorMessage = "El campo Acción es Obligatorio")]
        public string ActionResult { get; set; }
        [Required(ErrorMessage = "El campo Menu Activo es Obligatorio")]
        public bool ActiveMenu { get; set; }
        public Guid idRole { get; set; }
        [ForeignKey("idRole")]
        public virtual ICollection<Role> Roles { get; set; }

    }
}