namespace SMGPA.Migrations
{
    using Models;
    using SMGPA;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SMGPA.Models.SMGPAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SMGPA.Models.SMGPAContext context)
        {
           context.Role.AddOrUpdate(r=> r.idRole,
               new Role{ Nombre= "Usuario ROOT", Descripcion="Usuario con acceso global al sistema" });
            context.User.AddOrUpdate(a => a.idUser,
               new Administrator() { Rut = "18775929-3", Nombre = "Emilio", Apellido = "Valdivia", MailInstitucional = "e.valdiviaiglesias@uandresbello.edu", Contrasena = "rockinvokxd666", Rol = context.Role.FirstOrDefault() });
        }
    }
}
