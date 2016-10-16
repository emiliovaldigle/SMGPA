using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGMPA.Models
{
    public class Tasks : Operation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public Status status { get; set; }


    }
    public enum Status {Inactiva,EnProgreso,Activa,EnRevision,Completada};
}