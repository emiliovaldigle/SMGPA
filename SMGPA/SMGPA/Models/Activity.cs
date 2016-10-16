using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGMPA.Models
{
    public class Activity: Process
    {
        public Activity(){
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public States state { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }
    public enum States 
        {Activa, Inactiva, Completada};
}