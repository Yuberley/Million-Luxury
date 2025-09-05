# MillionLuxury - Real Estate Property Management System

🏡 **Sistema de Gestión de Propiedades Inmobiliarias** desarrollado con .NET 8 y Clean Architecture.

## 🚀 **Funcionalidades Principales**

### 🏠 **Gestión Completa de Propiedades**
- ✅ **Crear Propiedades** - Registro completo con validaciones exhaustivas
- ✅ **Actualizar Información** - Modificación de todos los campos
- ✅ **Gestión de Precios** - Cambios de precio con auditoría automática
- ✅ **Gestión de Imágenes** - Subida y gestión de fotos de propiedades
- ✅ **Búsqueda con Filtros** - Filtros múltiples con paginación optimizada

### 🔍 **Sistema de Búsqueda**
- Paginación eficiente para grandes conjuntos de datos
- Índices optimizados para consultas rápidas
- Búsqueda por texto en direcciones

### 🔒 **Seguridad Empresarial**
- Autenticación JWT robusta
- Autorización basada en roles
- Validaciones exhaustivas de entrada
- Protección contra ataques comunes

#### 🎯 **Performance**
- ✅ **Índices de Base de Datos**: Optimización para consultas frecuentes
- ✅ **Paginación**: Resultados limitados y eficientes
- ✅ **Consultas Optimizadas**: Entity Framework con proyecciones
- ✅ **Async/Await**: Operaciones no bloqueantes

## 🏗️ **Arquitectura**

Implementación completa de **Clean Architecture** con:

```
┌──────────────────┐
│   WebApi Layer   │  Controllers REST, Auth, Versioning
├──────────────────┤
│   Application    │  CQRS, Handlers, DTOs, Validations  
├──────────────────┤
│  Infrastructure  │  Database, Repositories, Services
├──────────────────┤
│   Domain Layer   │  Entities, Value Objects, Events
└──────────────────┘
```

### 🎯 **Patrones Implementados**
- **CQRS + Mediator** - Separación de comandos y consultas
- **Repository Pattern** - Abstracción de acceso a datos
- **Domain Events** - Comunicación entre agregados
- **Value Objects** - Tipos seguros para primitivos
- **SOLID Principles** - Código mantenible y extensible

## 🧪 **Testing**

### **Pruebas Implementadas**
- ✅ **Pruebas de Dominio** - Validación de reglas de negocio
- ✅ **Pruebas de Aplicación** - Testing de casos de uso
- ✅ **Pruebas de Validación** - FluentValidation rules
- ✅ **Mocking** - Aislamiento de dependencias

### **Ejecutar Pruebas**
```bash
dotnet test tests/MillionLuxury.UnitTests/
```

---

## 🛠️ **Instalación y Configuración**

### **Prerrequisitos**
- .NET 8 SDK
- SQL Server (local o Docker)
- Visual Studio 2022 / VS Code

### **Pasos de Instalación**

1. **Clonar el repositorio**
```bash
git clone https://github.com/Yuberley/Million-Luxury.git
cd Million-Luxury
```

2. **Configurar Base de Datos**
```bash
# Opcional: Iniciar SQL Server y Minio con Docker
cd infrastructure
docker-compose up -d

# Aplicar migraciones
dotnet ef database update --project src/MillionLuxury.Infrastructure --startup-project src/MillionLuxury.WebApi
```

3. **Ejecutar la Aplicación**
```bash
dotnet run --project src/MillionLuxury.WebApi 
```

4. **Acceder a la API**
- API: `https://localhost:44385`
- Swagger: `https://localhost:44385/swagger`

---

## Credenciales

- Credenciales de [SQL server ↗️](https://github.com/Yuberley/Million-Luxury/blob/master/infrastructure/sqlserver.env)
- Credenciales de [Minio ↗️](https://github.com/Yuberley/Million-Luxury/blob/master/infrastructure/minio.env)

---

**Desarrollado con ❤️ usando .NET 8 y Clean Architecture**

---

## Crear migraciones
```shell
dotnet ef migrations add NameMigration --project .\src\MillionLuxury.Infrastructure\ --startup-project .\src\MillionLuxury.WebApi\ -o Database\Migrations
```

## Aplicar migraciones
```shell
dotnet ef database update --project .\src\MillionLuxury.Infrastructure\ --startup-project .\src\MillionLuxury.WebApi\
```

## Eliminar última migración
```shell
dotnet ef migrations remove --project .\src\MillionLuxury.Infrastructure\ --startup-project .\src\MillionLuxury.WebApi\
```
