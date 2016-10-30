# SMGPA
Repositorio que almacenará los fuentes del proyecto "Sistema de Modelamiento y Gestión de procesos académicos"
--------------------------------------------------------------------------------------------------------------
Instrucciones para ejecutar proyecto:
>Instalar SQL Managment Studio 2014 Enterprise Edition.
>Instalar Visual Studio 2015
>Crear Base de  datos con el nombre 'SMGPA_SCHEMA'
>Abrir proyecto 'SMGPA' con Visual Studio 2015
>Abrir Web config de proyecto e ir a sección 
  <connectionStrings>
    <add name="SMGPAConnection" connectionString="Server=NOMBRESERVIDOR;Database=SMGPA_SCHEMA;User=sa;Password=123.pass;" providerName="System.Data.SqlClient" />
  </connectionStrings>
>Editar parámetro Server, indicar nombre del servidor donde se desplegará.
>Ir a SQL Managment Studio, crear usuario con el nombre sa y la contraseña 123.pass, asignar como dueño 'owner' de BD SMGPA_SCHEMA.
>Ir a proyecto (En visual studio) -> ir a Herramientas ('Tools') -> Administrador de paquetes NuGet-> Consola de administrador de paquetes.
>En consola de administrador de paquetes ejecutar comando 'updatedatabase, luego de esto la BD se generará de manera automática.
>Finalmente ejecutar aplicación.
>Cuenta: User: admin@root.org
	 Pass: 123.pass.321

