using SMGPA;
using SMGPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Administrator: User
    {
        public Guid idRole { get; set; }
        [ForeignKey("idRole")]      
        public virtual Role Rol { get; set; }
    }
}