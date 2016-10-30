# SMGPA
Repositorio que almacenará los fuentes del proyecto "Sistema de Modelamiento y Gestión de procesos académicos"
--------------------------------------------------------------------------------------------------------------
Instrucciones para ejecutar proyecto:
<br />
1 Instalar SQL Managment Studio 2014 Enterprise Edition.
<br />
2 Instalar Visual Studio 2015
<br />
3 Crear Base de  datos con el nombre 'SMGPA_SCHEMA'
<br />
4 Abrir proyecto 'SMGPA' con Visual Studio 2015
<br />
5 Abrir Web config de proyecto e ir a sección "connectionString"
<br />
6 Editar parámetro Server, indicar nombre del servidor donde se desplegará.
<br />
7 Ir a SQL Managment Studio, crear usuario con el nombre sa y la contraseña 123.pass, asignar como dueño 'owner' de BD SMGPA_SCHEMA.
<br />
8 Ir a proyecto (En visual studio) -> ir a Herramientas ('Tools') -> Administrador de paquetes NuGet-> Consola de administrador de paquetes.
<br />
9 En consola de administrador de paquetes ejecutar comando 'updatedatabase, luego de esto la BD se generará de manera automática.
<br />
10 Finalmente ejecutar aplicación.
<br />
11 Credenciales-> -User: admin@root.org
	          -Pass: 123.pass.321

