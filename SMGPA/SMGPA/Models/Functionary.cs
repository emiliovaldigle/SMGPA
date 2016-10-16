using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGMPA.Models
{
    public class Functionary: User
    {
        [Required(ErrorMessage = "Número Telefónico es requerido.")]
        public string NumeroTelefono { get; set; }
        [Required(ErrorMessage = "Correo Personal es requerido.")]
        public string CorreoPersonal { get; set; }
    }
}