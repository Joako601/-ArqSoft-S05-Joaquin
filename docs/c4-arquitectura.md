# Arquitectura C4 — CitasApp
---

## Nivel 1 — Contexto

**Para quién es:** cualquier persona (cliente, equipo no técnico, nuevo integrante). <br>
**Qué responde:** ¿qué es CitasApp y quién lo usa?

```mermaid
graph TD
    Paciente["Paciente"] -->|agenda y consulta citas| CitasApp["CitasApp
Sistema de gestión de citas médicas"]
    Medico["Médico"] -->|revisa su agenda| CitasApp
    Cliente["Cliente API
(Postman / integraciones externas)"] -->|consume la API REST| CitasApp
```


---

## Nivel 2 — Contenedores

**Para quién es:** el equipo técnico (desarrolladores, DevOps). <br>
**Qué responde:** ¿de qué piezas grandes y desplegables se compone CitasApp?

```mermaid
graph TD
    Navegador["Navegador
(usuario final)"] -->|HTTP| Web["Citas_App.Web
ASP.NET Core MVC"]
    ClienteAPI["Cliente API
(Postman / Frontend externo)"] -->|HTTP / JSON| Api["Citas_App.api
ASP.NET Core Web API"]

    Web -->|usa| Nucleo["Citas_.Application + Citas_App.Domain
Servicios y contratos del negocio"]
    Api -->|usa| Nucleo

    Nucleo -->|persiste a través de| Infra["Citas_App.Infrastructure
Repositorios (Factory + Decorator)"]

    Infra -->|lee/escribe| Datos["Persistencia
Archivos JSON / CSV / SQLite en data/"]
```

Los contenedores reales del repositorio:

| Contenedor | Proyecto | Responsabilidad |
|---|---|---|
| Web (MVC) | `Citas_App.Web` | Interfaz web para pacientes/médicos/admin |
| API REST | `Citas_App.api` | Expone `/api/Paciente`, `/api/Medico`, `/api/Cita`, `/api/Calculadora` |
| Núcleo de negocio | `Citas_.Application`, `Citas_App.Domain` | Servicios y contratos (interfaces) |
| Infraestructura | `Citas_App.Infrastructure` | Repositorios concretos, Factory, Decorator, Observers |
| Persistencia | `data/*.json`, `*.csv` | Sin base de datos relacional |

---

## Nivel 3 — Componentes

**Para quién es:** quien va a modificar o extender el backend. <br>
**Qué responde:** ¿qué hay dentro de la API/Web y cómo se aplican los patrones GoF?

```mermaid
graph TD
    subgraph API["Citas_App.api / Citas_App.Web"]
        Controllers["Controllers
CitaController, PacienteController, MedicoController"]
    end

    subgraph APP["Citas_.Application"]
        CitaService["CitaService
(Subject del Observer)"]
        PacienteService["PacienteService"]
        MedicoService["MedicoService"]
    end

    subgraph INFRA["Citas_App.Infrastructure"]
        Factory["RepositoryFactory
(Factory)"]
        Decorator["LoggingPacienteRepository
(Decorator sobre IPacienteRepository)"]
        Repos["Repositorios concretos
Json / Csv / Sqlite / Memoria"]
        ObsEmail["EmailObserver"]
        ObsSms["SmsObserver"]
    end

    subgraph DOMAIN["Citas_App.Domain"]
        Interfaces["Interfaces
ICitaRepository, IPacienteRepository,
IMedicoRepository, ICitaObserver"]
    end

    Controllers --> CitaService
    Controllers --> PacienteService
    Controllers --> MedicoService

    PacienteService --> Decorator
    Decorator --> Factory
    Factory --> Repos
    Repos -.implementa.-> Interfaces

    CitaService -->|ConfirmarCita notifica a| ObsEmail
    CitaService -->|ConfirmarCita notifica a| ObsSms
    ObsEmail -.implementa.-> Interfaces
    ObsSms -.implementa.-> Interfaces
```

### Patrones GoF representados

- **Factory** — `RepositoryFactory` (`Citas_App.Infrastructure/Repositories/RepositoryFactory.cs`) decide qué repositorio concreto instanciar (JSON, SQLite, memoria) según el entorno, devolviendo siempre la interfaz del `Domain`.
- **Decorator** — `LoggingPacienteRepository` envuelve a `IPacienteRepository` (normalmente el que devuelve la Factory) y añade logging sin tocar la implementación original.
- **Observer** — `CitaService` actúa como *Subject*: cuando una cita se confirma (`ConfirmarCita`), notifica a los observadores registrados (`EmailObserver`, `SmsObserver`) que implementan `ICitaObserver`.
