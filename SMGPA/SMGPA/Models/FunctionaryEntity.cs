using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMGPA.Models
{
    public class FunctionaryEntity
    {
        [Key, Column(Order = 0)]
        public Guid idUser { get; set; }
        [Key, Column(Order = 1)]
        public Guid idEntities { get; set; }
        public Entities Entidad { get; set; }
        public Functionary Funcionario { get; set; }
        public string Cargo { get; set; }
    }
}