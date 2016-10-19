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
        [Required(ErrorMessage = "Rut es Requerido.")]
        public string Rut { get; set; }
        [Required(ErrorMessage = "Nombre es Requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Apellido es Requerido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Correo Institucional es Requerido.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Correo institucional inválido.")]
        public string MailInstitucional { get; set; }
        [Required(ErrorMessage = "Contraseña es Requerida.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
        public bool Activo { get; set; }
    }
}