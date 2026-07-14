# 📘 Arquitectura de Software - Práctica 5

## 👨‍💻 Información del Estudiante

- **Nombre:** Joaquin Uriona
- **Matrícula:** SW2509057
- **Grupo:** A
- **Cuatrimestre:** Tercer Cuatrimestre
- **Carrera:** TSU en Desarrollo e Innovación de Software
- **Profesor:** Jorge Javier Pedrozo Romero
---



# App de citas médicas y API REST construida con ASP.NET Core (.NET 10)

## Descripción del Proyecto

Este proyecto es un sistema integral de gestión clínica desarrollado en **ASP.NET Core MVC**, estructurado bajo una arquitectura hexagonal (Domain, Application, Infrastructure, Web) que separa las responsabilidades del sistema, diseñado desde una perspectiva arquitectónica limpia que originalmente prescindía de bases de datos relacionales pesadas para implementar en su lugar una persistencia de datos rápida y portable basada en archivos JSON, y que más adelante evolucionó hacia una persistencia relacional real con **PostgreSQL** administrada mediante **Entity Framework Core**, sin perder la flexibilidad de poder alternar entre distintos motores de almacenamiento gracias a que todo el sistema sigue dependiendo únicamente de interfaces. El código destaca por la aplicación de **Principios SOLID** al separar estrictamente la lógica de acceso a datos mediante el **Patrón Repositorio**, por utilizar la **Inyección de Dependencias** nativa del framework para acoplar las interfaces con sus implementaciones y garantizar así un código escalable y fácil de mantener, y por incorporar además una interfaz de usuario moderna, responsiva y orientada a aplicaciones SaaS médicas, junto con una **API REST** para exponer los recursos del sistema, incluyendo utilidades adicionales como una calculadora.

## Entidades

- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente
- **Calculadora API** — endpoint utilitario para realizar operaciones de búsqueda de datos de la app principal, además de realizar operaciones matemáticas a través de la API

## Persistencia

El proyecto nació apoyándose únicamente en archivos planos dentro de `data/` (JSON y CSV, sin motor de base de datos), y esa capa de persistencia basada en archivos se conserva como una de las implementaciones disponibles del patrón Repositorio:

- `data/pacientes.json`
- `data/medicos.json`
- `data/medicos.csv`
- `data/citas.json`

A partir de la incorporación de PostgreSQL, esa persistencia basada en archivos convive con una persistencia relacional real, de modo que tanto los repositorios JSON/CSV como los repositorios PostgreSQL implementan las mismas interfaces del dominio (`IPacienteRepository`, `IMedicoRepository`, `ICitaRepository`) y pueden intercambiarse desde `Program.cs` sin tocar el resto de la aplicación.

### PostgreSQL + Entity Framework Core

Se agregó una nueva base de datos relacional (`citas_app`) administrada por EF Core a través del paquete `Npgsql.EntityFrameworkCore.PostgreSQL`, junto con un `DbContext` (`Citas_App.Infrastructure/db/CitasDbContext.cs`) que mapea las entidades `Paciente`, `Medico` y `Cita` a las tablas `pacientes`, `medicos` y `citas`, y con tres nuevos repositorios (`PostgresPacienteRepository`, `PostgresMedicoRepository`, `PostgresCitaRepository`) que implementan las interfaces existentes del dominio reutilizando ese contexto en lugar de leer y escribir archivos.

El esquema de la base de datos ya no se escribe a mano, sino que se genera y versiona a través de migraciones de EF Core, guardadas en `Citas_App.Infrastructure/db/`. Para recrear el esquema en un entorno nuevo:

```bash
dotnet ef migrations add InicialPostgres --project Citas_App.Infrastructure --startup-project Citas_App.Web -o db
dotnet ef database update --project Citas_App.Infrastructure --startup-project Citas_App.Web
```

La cadena de conexión se lee desde `appsettings.json` / `appsettings.Development.json` de cada proyecto de presentación (`Citas_App.Web` y `Citas_App.api`), bajo la clave `ConnectionStrings:Postgres`.

## Arquitectura

La solución está dividida en cinco proyectos independientes:

```
Citas_App/
├── Citas_App.Domain/              # Contratos e entidades del negocio
│   ├── Interfaces/
│   │   ├── ICitaRepository.cs
│   │   ├── IMedicoRepository.cs
│   │   ├── IPacienteRepository.cs
│   │   └── ICitaObserver.cs       # Contrato del patrón Observer
│   └── Models/
│       ├── Cita.cs
│       ├── CitaJson.cs            # DTO de serialización JSON
│       ├── Medico.cs
│       ├── Paciente.cs
│       └── AgendarViewModel.cs
│
├── Citas_.Application/            # Casos de uso y lógica de aplicación
│   └── Services/
│       ├── CitaService.cs         # Subject del patrón Observer
│       ├── MedicoService.cs
│       └── PacienteService.cs
│
├── Citas_App.api/                 # Capa de presentación API RESTful
│   ├── Controllers/
│   │   ├── CalculadoraController.cs # Endpoint utilitario matemático
│   │   ├── CitaController.cs
│   │   ├── MedicoController.cs
│   │   └── PacienteController.cs
│   └── data/
│
├── Citas_App.Infrastructure/      # Implementaciones de repositorios
│   ├── db/                        # Persistencia relacional (PostgreSQL + EF Core)
│   │   ├── CitasDbContext.cs
│   │   └── <migraciones generadas por EF Core>
│   ├── Repositories/
│   │   ├── JsonCitaRepository.cs
│   │   ├── JsonMedicoRepository.cs
│   │   ├── JsonPacienteRepository.cs
│   │   ├── CsvCitaRepository.cs
│   │   ├── CsvMedicoRepository.cs
│   │   ├── CsvPacienteRepository.cs
│   │   ├── SqliteCitaRepository.cs
│   │   ├── SqliteMedicoRepository.cs
│   │   ├── SqlitePacienteRepository.cs
│   │   ├── PostgresCitaRepository.cs      # Repositorio sobre PostgreSQL / EF Core
│   │   ├── PostgresMedicoRepository.cs    # Repositorio sobre PostgreSQL / EF Core
│   │   ├── PostgresPacienteRepository.cs  # Repositorio sobre PostgreSQL / EF Core
│   │   ├── MemoriaPacienteRepository.cs
│   │   ├── LoggingPacienteRepository.cs   # Decorator sobre IPacienteRepository
│   │   └── RepositoryFactory.cs   # Factory de repositorios
│   └── Observers/
│       ├── EmailObserver.cs       # Observer concreto
│       └── SmsObserver.cs         # Observer concreto
│
└── Citas_App.Web/                 # Capa de presentación (MVC Web)
    ├── Controllers/
    │   ├── AgendarController.cs
    │   ├── CitaController.cs
    │   ├── MedicoController.cs
    │   └── PacienteController.cs
    ├── Views/
    ├── wwwroot/
    ├── data/
    └── Program.cs
```

## Diagramas C4

Los diagramas de arquitectura del sistema están en:

[`docs/c4-arquitectura.md`](docs/c4-arquitectura.md)

## Navegación

- `/Pacientes` — lista de pacientes
- `/Medicoc` — lista de médicos
- `/Citas` — lista de citas
- `/Panel central` — panel central de administración
- `/Privacidad` — privacidad de la página

## Endpoints Principales (API)

- `/api/Paciente` — Operaciones CRUD para pacientes
- `/api/Medico` — Operaciones CRUD para médicos
- `/api/Cita` — Gestión de la agenda médica
- `/api/Calculadora` — Operaciones matemáticas expuestas como servicio

## 🛠️ Stack Tecnológico

**Backend & Framework**

- **C# / .NET 10:** Lenguaje y entorno de ejecución principal.
- **ASP.NET Core MVC:** Patrón arquitectónico para la separación estructurada de responsabilidades (Model-View-Controller).
- **ASP.NET Core Web API:** Creación de servicios HTTP REST.

**Persistencia**

- **PostgreSQL:** Motor de base de datos relacional para la persistencia principal del sistema.
- **Entity Framework Core (Npgsql):** ORM utilizado para mapear las entidades del dominio a PostgreSQL, generar y versionar el esquema mediante migraciones, y ejecutar las operaciones de lectura/escritura desde los repositorios.
- **JSON / CSV / SQLite:** Implementaciones alternativas de persistencia conservadas como parte del patrón Repositorio, intercambiables sin afectar al resto de las capas.

**Patrones de Diseño**

- **Principios SOLID:** Código modular, altamente desacoplado y preparado para escalar.
- **Hexagonal:** Domain, Application, Infrastructure, Web.

**Frontend & UI**

- **Razor Views (`.cshtml`):** Motor de plantillas para la generación de vistas dinámicas enlazadas a los modelos de C#.
- **CSS3 / UI Personalizada:** Diseño de interfaz desde cero orientado a la experiencia de usuario (UX) de un SaaS corporativo.
- **Bootstrap 5:** Utilizado para la estructura del layout base y la barra de navegación responsiva.

---

## Patrones GoF

+ **Factory** — `RepositoryFactory` (`Citas_App.Infrastructure/Repositories/RepositoryFactory.cs`) <br>
Centraliza la creación de los repositorios concretos según el entorno de ejecución (Production vs. desarrollo), devolviendo siempre la abstracción (`IPacienteRepository`, `IMedicoRepository`, `ICitaRepository`). Esto evita instanciar repositorios concretos con `new` desde `Program.cs` y permite cambiar la implementación de persistencia (JSON, SQLite, PostgreSQL, memoria, CSV) sin tocar el resto de la aplicación.

+ **Decorator** — `LoggingPacienteRepository` (`Citas_App.Infrastructure/Repositories/LoggingPacienteRepository.cs`) <br>
Envuelve cualquier `IPacienteRepository` (ya sea el que devuelve la Factory o, tras la migración a PostgreSQL, el `PostgresPacienteRepository`) y le añade un comportamiento de logging (timestamps, conteo de registros, resultado de la operación) sin modificar la implementación original ni el contrato. Se compone así: `new LoggingPacienteRepository(repositorioConcreto)`, demostrando cómo Factory y Decorator se combinan en el mismo registro de dependencias (`Program.cs`), incluso cuando el repositorio concreto cambió de una fuente basada en archivos a una basada en PostgreSQL.

+ **Observer** — `ICitaObserver` (`Citas_App.Domain/Interfaces/ICitaObserver.cs`) + `EmailObserver` / `SmsObserver` (`Citas_App.Infrastructure/Observers/`) <br>
`CitaService` actúa como Subject: mantiene una lista de observadores (`AgregarObserver`) y, cuando una cita cambia a estado "Confirmada" (`ConfirmarCita`), notifica a todos los observadores registrados llamando a `OnCitaConfirmada(cita)`. `EmailObserver` y `SmsObserver` son observadores concretos que reaccionan a esa notificación simulando el envío de una confirmación, lo cual desacopla la lógica de negocio de las notificaciones y permite agregar nuevos canales (push, webhook, etc.) implementando `ICitaObserver` sin modificar `CitaService`.

---

## Refactorización y deuda técnica (Unidad III)

Como parte de la revisión de calidad del código se identificaron y corrigieron dos code smells concretos del proyecto, documentados aquí junto con la técnica de refactorización aplicada en cada caso.

### 1. Long Method y código duplicado — `JsonCitaRepository.Agregar`

El método `Agregar` de `JsonCitaRepository` reconstruía por su cuenta toda la lógica de lectura y escritura de JSON que ya existía en `ObtenerTodos`, pero de forma inconsistente, ya que usaba una ruta hardcodeada en lugar del campo `_path` calculado a partir de `IWebHostEnvironment`, creaba sus propias `JsonSerializerOptions` en vez de reutilizar las ya definidas, y deserializaba directo a `Cita` en lugar de pasar por el DTO `CitaJson` que sí usaba el resto de la clase. Esto significaba que cualquier cambio futuro en el formato de almacenamiento (por ejemplo, el formato de fecha) debía replicarse en dos lugares distintos del mismo archivo, con el riesgo constante de que se actualizara uno y se olvidara el otro.

Se aplicó **Extract Method** para consolidar esa lógica en dos métodos privados, `LeerCitas()` y `GuardarCitas()`, de modo que tanto `ObtenerTodos` como `Agregar` comparten ahora la misma ruta, las mismas opciones de serialización y el mismo mapeo entre `CitaJson` y `Cita`, sin que el comportamiento observable del sistema haya cambiado.

### 2. Tight Coupling por registro duplicado de dependencias — `Program.cs`

En `Citas_App.Web/Program.cs`, la interfaz `IPacienteRepository` se registraba dos veces en el contenedor de inyección de dependencias: una primera vez a través de `RepositoryFactory` envuelto en el decorador `LoggingPacienteRepository`, y una segunda vez apuntando directo a `JsonPacienteRepository`. Como en ASP.NET Core el último registro de una misma interfaz es el que prevalece, el decorador con logging y la lógica de la Factory quedaban silenciosamente anulados en tiempo de ejecución, de forma que el sistema aparentaba estar usando ambos patrones cuando en realidad ninguno de los dos se ejecutaba.

La corrección consistió en eliminar el registro duplicado y dejar una única fuente de verdad para `IPacienteRepository`, de manera que el Controller siempre recibe la instancia esperada (Factory + Decorator) y el acoplamiento oculto hacia una implementación concreta desaparece.

---
## 📸 Capturas de Pantalla

<div align="center">
  <img src="Citas_App.Web/Img/Home.png" alt="Panel de Inicio" width="800">
  <p><em>Panel de control principal (Home)</em></p>
</div>

<div align="center">
  <img src="Citas_App.Web/Img/Agendar.png" alt="Vista del Panel Central" width="800">
  <p><em>Vista unificada del sistema (Agendar)</em></p>
</div>

<div align="center">
  <img src="Citas_App.Web/Img/Citas.png" alt="Vista de Agenda" width="800">
  <p><em>Gestor centralizado de citas y horarios</em></p>
</div>

<div align="center">
  <img src="Citas_App.Web/Img/Pacientes.png" alt="Directorio de Pacientes" width="800">
  <p><em>Directorio humanizado y fichas clínicas</em></p>
</div>

<div align="center">
  <img src="Citas_App.Web/Img/Privacidad.png" alt="Políticas de Privacidad" width="800">
  <p><em>Documento de privacidad y seguridad de datos médicas</em></p>
</div>

---

## 🤝 Agradecimientos

- **Profesor Jorge Javier Pedrozo Romero** por la estructura del curso y la práctica
- **Tecnológico de Software** por la formación integral

---

## 📧 Contacto

- **Email Institucional:** joaquin.uriona@tecdesoftware.edu.mx
- **GitHub:** [Joako601](https://github.com/TU-USUARIO)

---

## 📄 Licencia

Este proyecto fue desarrollado por **Joaquin Uriona** como parte de las prácticas académicas para el **Tecnológico de Software**.

Distribuido bajo la Licencia MIT. Siéntete libre de utilizar la arquitectura del código y el diseño de la interfaz para fines educativos o proyectos personales, siempre y cuando se mantenga el reconocimiento al autor original.

Consulta el archivo `LICENSE` para más detalles.

---

## 🤖 Declaración de Uso de IA

Este proyecto integra asistencia de Inteligencia Artificial exclusivamente para la corrección de la indentación y el apartado frontend, así como para la refactorización de code smells identificados en la Unidad III y la integración de la persistencia con PostgreSQL mediante Entity Framework Core.

---

<div align="center">

**⭐ Si te gustó este proyecto, dale una estrella ⭐**

Hecho con 💙 por Joaquin Uriona - 2026

</div>
