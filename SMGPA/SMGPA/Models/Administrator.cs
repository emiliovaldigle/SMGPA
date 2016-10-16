using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMPA.Models
{
    public class Administrator: User
    {
        [Required(ErrorMessage = "El campo Carrera es Obligatorio")]
        public string Carrera { get; set; }
    
    }
   
}