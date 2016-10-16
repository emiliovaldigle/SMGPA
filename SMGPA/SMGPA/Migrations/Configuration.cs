namespace SMGPA.Migrations
{
    using SGMPA.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SGMPA.Models.SGMPAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SGMPA.Models.SGMPAContext context)
        {
            context.Functionary.AddOrUpdate(a => a.UserId,
               new Functionary() { Rut = "18775929-3", Nombre = "Emilio", Apellido = "Valdivia", MailInstitucional = "e.valdiviaiglesias@uandresbello.edu", Contrasena = "rockinvokxd666", NumeroTelefono = "88063940", CorreoPersonal = "emiliovaldigle@gmail.com" });
        }
    }
}
