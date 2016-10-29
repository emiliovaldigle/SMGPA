namespace SMGPA.Migrations
{
    using Models;
    using SMGPA;
    using System;
    using System.Collections.Generic;
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
            Role rol = new Role{ Nombre = "Usuario ROOT", Descripcion = "Usuario con acceso global al sistema" };
            Permission p1 = new Permission { TextLink = "Usuarios", Controller = "Users", ActionResult = "Index", ActiveMenu = true};
            Permission p2 = new Permission { TextLink = "Crear Usuario", Controller = "Users", ActionResult = "Create", ActiveMenu = false };
            Permission p3 = new Permission { TextLink = "Editar Usuarios", Controller = "Users", ActionResult = "Edit", ActiveMenu = false };
            Permission p4 = new Permission { TextLink = "Eliminar Usuarios", Controller = "Users", ActionResult = "Delete", ActiveMenu = false };
            Permission p5 = new Permission { TextLink = "Detalles de Usuario", Controller = "Users", ActionResult = "Details", ActiveMenu = false };
            Permission p6 = new Permission { TextLink = "Carreras", Controller = "Careers", ActionResult = "Index", ActiveMenu = true };
            Permission p7= new Permission { TextLink = "Crear Carrera", Controller = "Careers", ActionResult = "Create", ActiveMenu = false };
            Permission p8 = new Permission { TextLink = "Editar Carreras", Controller = "Careers", ActionResult = "Edit", ActiveMenu = false };
            Permission p9= new Permission { TextLink = "Eliminar Carreras", Controller = "Careers", ActionResult = "Delete", ActiveMenu = false };
            Permission p10 = new Permission { TextLink = "Detalles de Carrera", Controller = "Careers", ActionResult = "Details", ActiveMenu = false };
            Permission p11 = new Permission { TextLink = "Roles", Controller = "Roles", ActionResult = "Index", ActiveMenu = true };
            Permission p12 = new Permission { TextLink = "Crear Roles", Controller = "Roles", ActionResult = "Create", ActiveMenu = false };
            Permission p13 = new Permission { TextLink = "Editar Roles", Controller = "Roles", ActionResult = "Edit", ActiveMenu = false };
            Permission p14 = new Permission { TextLink = "Eliminar Roles", Controller = "Roles", ActionResult = "Index", ActiveMenu = false };
            Permission p15 = new Permission { TextLink = "Detalles Rol", Controller = "Roles", ActionResult = "Index", ActiveMenu = false };
            Permission p16 = new Permission { TextLink = "Permisos", Controller = "Roles", ActionResult = "Permissions", ActiveMenu = false };
            Permission p17 = new Permission { TextLink = "Desagregar Permiso", Controller = "Roles", ActionResult = "DeletePermission", ActiveMenu = false };
            Permission p18 = new Permission { TextLink = "Agregar Permiso", Controller = "Roles", ActionResult = "AddPermission", ActiveMenu = false };
            Permission p19 = new Permission { TextLink = "Procesos", Controller = "Processes", ActionResult = "Index", ActiveMenu = true };
            Permission p20 = new Permission { TextLink = "Crear Proceso", Controller = "Processes", ActionResult = "Index", ActiveMenu = false };
            Permission p21 = new Permission { TextLink = "Editar Procesos", Controller = "Processes", ActionResult = "Index", ActiveMenu = false };
            Permission p22 = new Permission { TextLink = "Eliminar Procesos", Controller = "Processes", ActionResult = "Index", ActiveMenu = false };
            Permission p23 = new Permission { TextLink = "Detalles Proceso", Controller = "Processes", ActionResult = "Index", ActiveMenu = false };
            Permission p24 = new Permission { TextLink = "Operaciones", Controller = "Processes", ActionResult = "Operations", ActiveMenu = false };
            Permission p25 = new Permission { TextLink = "Crear Operaciones", Controller = "Processes", ActionResult = "AddOperation", ActiveMenu = false };
            Permission p26 = new Permission { TextLink = "Editar Operaciones", Controller = "Processes", ActionResult = "EditOperation", ActiveMenu = false };
            Permission p27 = new Permission { TextLink = "Detalles Operaciones", Controller = "Processes", ActionResult = "DeleteOperation", ActiveMenu = false };
            List<Permission> permisos = new List<Permission>();
            permisos.Add(p1); permisos.Add(p2); permisos.Add(p3); permisos.Add(p4); permisos.Add(p5); permisos.Add(p6); permisos.Add(p7); permisos.Add(p8);
            permisos.Add(p9); permisos.Add(p10); permisos.Add(p11); permisos.Add(p12); permisos.Add(p13); permisos.Add(p14); permisos.Add(p15); permisos.Add(p16);
            permisos.Add(p17); permisos.Add(p18); permisos.Add(p19); permisos.Add(p20); permisos.Add(p21); permisos.Add(p22); permisos.Add(p23); permisos.Add(p24);
            permisos.Add(p25); permisos.Add(p26); permisos.Add(p27);
            foreach (Permission p in permisos)
            {
                p.Roles.Add(rol);
                rol.Permisos.Add(p);
                context.Permission.AddOrUpdate(pe => pe.idPermission, p);
            }
            context.Role.AddOrUpdate(r => r.idRole, rol);
            context.User.AddOrUpdate(u => u.idUser,
            new Administrator() { Rut = "1111111-1", Nombre = "Administrador", Apellido = "Root", MailInstitucional = "admin@root.org", Contrasena = "123.pass.321", Rol = context.Role.FirstOrDefault()});
        }
    }
}
