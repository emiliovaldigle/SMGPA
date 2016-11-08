using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMGPA.Models
{
    public class User
    {
        public User()
        {
            Activo = true;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid idUser { get; set; }
        [RegularExpression(@"^(\d{1,3}(\.?\d{3}){2})\-?([\dkK])$", ErrorMessage = "Rut inválido, probar formato 0.000.000-x o 0000000-x")]
        [StringLength(14, ErrorMessage = "Rut muy Largo")]
        [Required(ErrorMessage = "Rut es Requerido.")]
        public string Rut { get; set; }
        [StringLength(50, ErrorMessage = "Nombre muy Largo")]
        [Required(ErrorMessage = "Nombre es Requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Apellido es Requerido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Correo Institucional es Requerido.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Correo institucional inválido.")]
        [StringLength(50, ErrorMessage = "Correo muy Largo")]
        public string MailInstitucional { get; set; }
        [Required(ErrorMessage = "Contraseña es Requerida.")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Contraseña muy Larga")]
        public string Contrasena { get; set; }
        public bool Activo { get; set; }
    }
}