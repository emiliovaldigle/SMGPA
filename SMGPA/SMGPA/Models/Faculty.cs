using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class Faculty: Entities
    {
        public Faculty()
        {
            Carreras = new HashSet<Career>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual ICollection<Career> Carreras { get; set; }
    }
}