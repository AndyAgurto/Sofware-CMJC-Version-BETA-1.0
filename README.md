# CMJC Version 1.0

Software de escritorio para la gestión de un Consultorio Médico, desarrollado en C# y WPF (.NET 7.0). Permite administrar pacientes, historias clínicas, consultas, recetas y usuarios, facilitando la operación diaria de un consultorio.

## Características principales

- **Gestión de Pacientes:** Alta, edición, listado y búsqueda de pacientes.
- **Historias Clínicas:** Creación y edición de historias clínicas y antecedentes médicos.
- **Consultas Médicas:** Registro, visualización y gestión de consultas por paciente.
- **Recetas Médicas:** Generación de recetas estándar y personalizadas, impresión y almacenamiento.
- **Gestión de Usuarios:** Registro, inicio de sesión y administración de usuarios del sistema.
- **Perfiles y Roles:** Control de acceso según el perfil del usuario.
- **Interfaz amigable:** Ventanas modernas y navegación intuitiva.
- **Soporte para migraciones y base de datos local (Entity Framework Core).**

## Estructura del proyecto

- `/Modelo/` — Clases de dominio: Paciente, Usuario, Consulta, Diagnóstico, Receta, Historia Clínica, etc.
- `/VistaModelo/` — ViewModels para la lógica de presentación y enlace de datos (MVVM).
- `/Vistas/` — Ventanas y controles visuales (WPF XAML): Login, SignUp, Pacientes, Consultas, Recetas, etc.
- `/Contexto_CMJC/` — Contexto de Entity Framework para acceso a datos.
- `/Migrations/` — Migraciones de base de datos generadas por EF Core.
- `/Recursos/` — Imágenes, íconos y estilos visuales.

## Dependencias principales

- **.NET 7.0 (WPF Desktop)**
- **Entity Framework Core 7.0** (SQL Server, InMemory)
- **MailKit** (envío de correos)
- **DocX** y **Spire.Doc** (generación y manipulación de documentos Word)

## Instalación y ejecución local

1. Clona el repositorio:
   ```
   git clone https://github.com/tuusuario/CMJC.git
   ```
2. Restaura los paquetes NuGet:
   ```
   dotnet restore
   ```
3. Aplica las migraciones de base de datos (opcional, si usas SQL Server):
   ```
   dotnet ef database update
   ```
4. Compila y ejecuta la aplicación:
   ```
   dotnet run
   ```

## Primer uso
- Si no existen usuarios en la base de datos, se mostrará la ventana de registro (SignUp).
- Si ya hay usuarios, se mostrará la ventana de inicio de sesión (Login).

## Autor y Licencia

- **Autor:** Andy Agurto Urcia
- **Empresa:** DEVELOPERS AAU // devopsolutionsa@gmail.com
- **Licencia:** Propietario, uso interno.

---

¡Contribuciones y sugerencias son bienvenidas!
