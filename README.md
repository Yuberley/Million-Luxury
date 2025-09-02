# MillionLuxury - Real Estate Property Management System

🏡 **Sistema de Gestión de Propiedades Inmobiliarias** desarrollado con .NET 8 y Clean Architecture.

## 🚀 **Funcionalidades Principales**

### 🏠 **Gestión Completa de Propiedades**
- ✅ **Crear Propiedades** - Registro completo con validaciones exhaustivas
- ✅ **Actualizar Información** - Modificación de todos los campos
- ✅ **Gestión de Precios** - Cambios de precio con auditoría automática
- ✅ **Gestión de Imágenes** - Subida y gestión de fotos de propiedades
- ✅ **Búsqueda Avanzada** - Filtros múltiples con paginación optimizada

### 🔍 **Sistema de Búsqueda Potente**
- Filtros por precio, tipo, ubicación, habitaciones, baños, área
- Paginación eficiente para grandes conjuntos de datos
- Índices optimizados para consultas rápidas
- Búsqueda por texto en direcciones

### 🔒 **Seguridad Empresarial**
- Autenticación JWT robusta
- Autorización basada en roles
- Validaciones exhaustivas de entrada
- Protección contra ataques comunes

## 🏗️ **Arquitectura**

Implementación completa de **Clean Architecture** con:

```
┌─────────────────┐
│   WebApi Layer  │  Controllers REST, Auth, Versioning
├─────────────────┤
│ Application     │  CQRS, Handlers, DTOs, Validations  
├─────────────────┤
│ Infrastructure  │  Database, Repositories, Services
├─────────────────┤
│   Domain Layer  │  Entities, Value Objects, Events
└─────────────────┘
```

### 🎯 **Patrones Implementados**
- **CQRS + Mediator** - Separación de comandos y consultas
- **Repository Pattern** - Abstracción de acceso a datos
- **Domain Events** - Comunicación entre agregados
- **Value Objects** - Tipos seguros para primitivos
- **SOLID Principles** - Código mantenible y extensible

## 📊 **Base de Datos**

### **Entidades Principales**

#### 🏠 **Properties**
- Información completa de propiedades
- 10 tipos diferentes (Casa, Apartamento, Villa, etc.)
- 6 estados (Disponible, Vendida, Alquilada, etc.)
- Campos optimizados para búsquedas

#### 📸 **PropertyImages**
- Gestión de imágenes asociadas
- Validación de formatos (JPG, PNG, WEBP)
- Control de tamaño y calidad
- Soft delete para historial

### **Índices de Performance**
```sql
-- Optimización para búsquedas frecuentes
CREATE INDEX ix_properties_price ON Properties(price);
CREATE INDEX ix_properties_property_type ON Properties(property_type);
CREATE INDEX ix_properties_bedrooms ON Properties(bedrooms);
-- + 6 índices adicionales
```

## 🚀 **API Endpoints**

### **Gestión de Propiedades**
```http
POST   /api/v1/properties              # Crear propiedad
GET    /api/v1/properties/{id}         # Obtener por ID
PUT    /api/v1/properties/{id}         # Actualizar
PATCH  /api/v1/properties/{id}/price   # Cambiar precio
POST   /api/v1/properties/{id}/images  # Agregar imagen
GET    /api/v1/properties/search       # Búsqueda avanzada
```

### **Autenticación**
```http
POST   /api/v1/auth/register           # Registro de usuario
POST   /api/v1/auth/login              # Inicio de sesión
GET    /api/v1/auth/whoami             # Información del usuario
```

## 📋 **Ejemplos de Uso**

### **Crear Villa de Lujo**
```json
POST /api/v1/properties
{
  "name": "Villa Sunset Paradise",
  "address": "1234 Ocean Drive, Malibu, CA",
  "price": 2500000.00,
  "internalCode": "VILLA001",
  "year": 2021,
  "propertyType": 5,
  "bedrooms": 6,
  "bathrooms": 8,
  "areaInSquareMeters": 450.75,
  "description": "Exquisite oceanfront villa..."
}
```

### **Búsqueda Avanzada**
```http
GET /api/v1/properties/search?
    minPrice=500000&
    maxPrice=2000000&
    propertyType=5&
    minBedrooms=3&
    status=1&
    page=1&
    pageSize=20
```

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

## 🛠️ **Instalación y Configuración**

### **Prerrequisitos**
- .NET 8 SDK
- SQL Server (local o Docker)
- Visual Studio 2022 / VS Code

### **Pasos de Instalación**

1. **Clonar el repositorio**
```bash
git clone https://github.com/tu-usuario/MillionLuxury.git
cd MillionLuxury
```

2. **Configurar Base de Datos**
```bash
# Opcional: Iniciar SQL Server con Docker
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
- API: `https://localhost:7000`
- Swagger: `https://localhost:7000/swagger`

## 📚 **Documentación**

- 📖 **[Documentación Técnica Completa](PROPERTY_API_DOCUMENTATION.md)**
- 🔧 **[Ejemplos de Uso de la API](API_USAGE_EXAMPLES.md)**
- 📋 **[Resumen de Implementación](IMPLEMENTATION_SUMMARY.md)**

## 🔧 **Configuración**

### **Cadena de Conexión**
```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Server=localhost;Database=MillionLuxuryDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### **JWT Configuration**
```json
{
  "JwtOptions": {
    "Issuer": "MillionLuxury",
    "Audience": "MillionLuxury.WebApi",
    "SecretKey": "your-super-secret-key-here-must-be-very-long"
  }
}
```

## 🏷️ **Tipos de Propiedades**

| ID | Tipo | Descripción |
|----|------|-------------|
| 1 | House | Casa tradicional |
| 2 | Apartment | Apartamento |
| 3 | Condo | Condominio |
| 4 | Townhouse | Casa adosada |
| 5 | Villa | Villa de lujo |
| 6 | Penthouse | Ático |
| 7 | Studio | Estudio |
| 8 | Duplex | Dúplex |
| 9 | Commercial | Comercial |
| 10 | Land | Terreno |

## 📊 **Estados de Propiedades**

| ID | Estado | Descripción |
|----|--------|-------------|
| 1 | Available | Disponible |
| 2 | Sold | Vendida |
| 3 | Rented | Alquilada |
| 4 | Pending | Pendiente |
| 5 | Inactive | Inactiva |
| 6 | UnderConstruction | En construcción |

## 🎯 **Características Destacadas**

### **Performance**
- 🚀 Índices optimizados para búsquedas
- 📄 Paginación eficiente
- 🔄 Operaciones asíncronas
- 💾 Consultas optimizadas con EF Core

### **Seguridad**
- 🔐 JWT Authentication
- 🛡️ Input validation
- 🔒 SQL injection protection
- 📝 Audit trails con Domain Events

### **Mantenibilidad**
- 🏗️ Clean Architecture
- 🧪 Testing comprehensivo
- 📚 Documentación completa
- 🔧 Configuración flexible

## 🤝 **Contribución**

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 **Licencia**

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

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