# SMGPA
Repositorio que almacenará los fuentes del proyecto "Sistema de Modelamiento y Gestión de procesos académicos"
--------------------------------------------------------------------------------------------------------------
Instrucciones para ejecutar proyecto:
1 <br />Instalar SQL Managment Studio 2014 Enterprise Edition.
2 <br />Instalar Visual Studio 2015
3 <br />Crear Base de  datos con el nombre 'SMGPA_SCHEMA'
4 <br />Abrir proyecto 'SMGPA' con Visual Studio 2015
5 <br />Abrir Web config de proyecto e ir a sección "connectionString"
6 <br />Editar parámetro Server, indicar nombre del servidor donde se desplegará.
7 <br />Ir a SQL Managment Studio, crear usuario con el nombre sa y la contraseña 123.pass, asignar como dueño 'owner' de BD SMGPA_SCHEMA.
8 <br />Ir a proyecto (En visual studio) -> ir a Herramientas ('Tools') -> Administrador de paquetes NuGet-> Consola de administrador de paquetes.
9 <br />En consola de administrador de paquetes ejecutar comando 'updatedatabase, luego de esto la BD se generará de manera automática.
10 <br />Finalmente ejecutar aplicación.
11 <br />Credenciales-> -User: admin@root.org
	          -Pass: 123.pass.321

