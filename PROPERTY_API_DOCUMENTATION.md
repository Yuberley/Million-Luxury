# MillionLuxury - Real Estate Property Management API

Esta es una API REST desarrollada con .NET 8 siguiendo los principios de Clean Architecture, CQRS y el patrón Mediator para gestionar propiedades inmobiliarias.

## 🏗️ Arquitectura

El proyecto sigue Clean Architecture con las siguientes capas:

- **Domain Layer**: Entidades de dominio, objetos de valor, eventos de dominio y contratos de repositorio
- **Application Layer**: Casos de uso (Commands/Queries), DTOs, validaciones y lógica de aplicación
- **Infrastructure Layer**: Implementación de repositorios, base de datos, servicios externos
- **WebApi Layer**: Controladores REST, configuración de API

## 🚀 Funcionalidades Implementadas

### Gestión de Propiedades

#### 1. **Crear Propiedad**
- **Endpoint**: `POST /api/v1/properties`
- **Descripción**: Crea una nueva propiedad inmobiliaria
- **Validaciones**:
  - Nombre requerido (máx. 200 caracteres)
  - Dirección requerida (máx. 500 caracteres)
  - Precio mayor que cero
  - Código interno único (máx. 50 caracteres)
  - Año válido (1900-2100)
  - Tipo de propiedad válido
  - Número de habitaciones ≥ 0
  - Número de baños ≥ 0
  - Área mayor que cero
  - Descripción requerida (máx. 2000 caracteres)

```json
{
  "name": "Beautiful Villa",
  "address": "123 Ocean Drive, Miami, FL",
  "price": 750000.00,
  "internalCode": "VILLA001",
  "year": 2020,
  "propertyType": 1,
  "bedrooms": 4,
  "bathrooms": 3,
  "areaInSquareMeters": 250.5,
  "description": "Stunning villa with ocean view"
}
```

#### 2. **Obtener Propiedad**
- **Endpoint**: `GET /api/v1/properties/{id}`
- **Descripción**: Obtiene los detalles de una propiedad específica
- **Incluye**: Información completa de la propiedad e imágenes asociadas

#### 3. **Actualizar Propiedad**
- **Endpoint**: `PUT /api/v1/properties/{id}`
- **Descripción**: Actualiza los datos de una propiedad existente
- **Validaciones**: Similares a la creación (excepto precio y código interno)

```json
{
  "name": "Updated Villa Name",
  "address": "123 Updated Address",
  "year": 2021,
  "propertyType": 1,
  "status": 1,
  "bedrooms": 5,
  "bathrooms": 4,
  "areaInSquareMeters": 300.0,
  "description": "Updated description"
}
```

#### 4. **Cambiar Precio**
- **Endpoint**: `PATCH /api/v1/properties/{id}/price`
- **Descripción**: Actualiza únicamente el precio de la propiedad
- **Genera**: Evento de dominio para auditoría de cambios de precio

```json
{
  "price": 850000.00
}
```

#### 5. **Agregar Imagen**
- **Endpoint**: `POST /api/v1/properties/{id}/images`
- **Descripción**: Agrega una imagen a la propiedad
- **Formatos soportados**: JPG, JPEG, PNG, WEBP
- **Validaciones**:
  - Tamaño máximo: 5MB
  - Formato de imagen válido
  - Contenido en Base64

```json
{
  "fileName": "villa-front.jpg",
  "base64Content": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQ..."
}
```

#### 6. **Buscar Propiedades con Filtros**
- **Endpoint**: `GET /api/v1/properties/search`
- **Descripción**: Búsqueda avanzada con múltiples filtros
- **Parámetros de búsqueda**:
  - `page`: Número de página (default: 1)
  - `pageSize`: Elementos por página (default: 10, máx: 100)
  - `minPrice` / `maxPrice`: Rango de precios
  - `propertyType`: Tipo de propiedad
  - `status`: Estado de la propiedad
  - `minBedrooms` / `maxBedrooms`: Rango de habitaciones
  - `minBathrooms` / `maxBathrooms`: Rango de baños
  - `minArea` / `maxArea`: Rango de área en m²
  - `address`: Búsqueda por dirección (contiene texto)

**Ejemplo de búsqueda:**
```
GET /api/v1/properties/search?minPrice=100000&maxPrice=500000&propertyType=1&minBedrooms=2&page=1&pageSize=20
```

**Respuesta:**
```json
{
  "properties": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "Beautiful Villa",
      "address": "123 Ocean Drive, Miami, FL",
      "price": 750000.00,
      "internalCode": "VILLA001",
      "year": 2020,
      "ownerId": "987fcdeb-51a2-43d1-b789-123456789abc",
      "propertyType": 1,
      "status": 1,
      "bedrooms": 4,
      "bathrooms": 3,
      "areaInSquareMeters": 250.5,
      "description": "Stunning villa with ocean view",
      "createdAt": "2023-08-31T10:30:00Z",
      "updatedAt": "2023-08-31T15:45:00Z",
      "images": [
        {
          "id": "456e7890-e12b-34d5-a678-426614174001",
          "fileName": "villa-front.jpg",
          "filePath": "/images/properties/123e4567-e89b-12d3-a456-426614174000/456e7890-e12b-34d5-a678-426614174001.jpg",
          "isEnabled": true,
          "createdAt": "2023-08-31T11:00:00Z"
        }
      ]
    }
  ],
  "totalCount": 15,
  "page": 1,
  "pageSize": 20,
  "totalPages": 1
}
```

## 📊 Tipos de Datos

### PropertyType (Tipo de Propiedad)
- `1`: House (Casa)
- `2`: Apartment (Apartamento)
- `3`: Condo (Condominio)
- `4`: Townhouse (Casa adosada)
- `5`: Villa (Villa)
- `6`: Penthouse (Ático)
- `7`: Studio (Estudio)
- `8`: Duplex (Dúplex)
- `9`: Commercial (Comercial)
- `10`: Land (Terreno)

### PropertyStatus (Estado de Propiedad)
- `1`: Available (Disponible)
- `2`: Sold (Vendida)
- `3`: Rented (Alquilada)
- `4`: Pending (Pendiente)
- `5`: Inactive (Inactiva)
- `6`: UnderConstruction (En construcción)

## 🗄️ Base de Datos

### Tabla Properties
- `id` (uniqueidentifier, PK)
- `name` (nvarchar(200), NOT NULL)
- `address` (nvarchar(500), NOT NULL)
- `price` (decimal(18,2), NOT NULL)
- `internal_code` (nvarchar(50), NOT NULL, UNIQUE)
- `year` (int, NOT NULL)
- `owner_id` (uniqueidentifier, NOT NULL)
- `property_type` (int, NOT NULL)
- `status` (int, NOT NULL)
- `bedrooms` (int, NOT NULL)
- `bathrooms` (int, NOT NULL)
- `area_in_square_meters` (decimal(10,2), NOT NULL)
- `description` (nvarchar(2000), NOT NULL)
- `created_at` (datetime2, NOT NULL)
- `updated_at` (datetime2, NOT NULL)

### Tabla PropertyImages
- `id` (uniqueidentifier, PK)
- `property_id` (uniqueidentifier, FK, NOT NULL)
- `file_name` (nvarchar(255), NOT NULL)
- `file_path` (nvarchar(500), NOT NULL)
- `is_enabled` (bit, NOT NULL)
- `created_at` (datetime2, NOT NULL)

### Índices para Performance
- `ix_properties_price`: Optimiza búsquedas por rango de precios
- `ix_properties_property_type`: Optimiza filtros por tipo
- `ix_properties_status`: Optimiza filtros por estado
- `ix_properties_bedrooms`: Optimiza filtros por habitaciones
- `ix_properties_bathrooms`: Optimiza filtros por baños
- `ix_properties_area_in_square_meters`: Optimiza filtros por área
- `ix_properties_created_at`: Optimiza ordenación temporal
- `ix_properties_internal_code`: UNIQUE, validación de duplicados
- `ix_property_images_property_id`: Optimiza carga de imágenes
- `ix_property_images_is_enabled`: Optimiza filtros de imágenes activas

## 🔐 Seguridad

### Autenticación y Autorización
- **JWT Bearer Token**: Requerido para todos los endpoints
- **Validación de Usuario**: El sistema identifica automáticamente al usuario propietario
- **Control de Acceso**: Solo usuarios autenticados pueden crear/modificar propiedades

### Validaciones de Seguridad
- **Validación de Entrada**: FluentValidation en todos los endpoints
- **Sanitización**: Prevención de inyección de código
- **Validación de Imágenes**: Verificación de formato y tamaño de archivos
- **Códigos Internos Únicos**: Prevención de duplicados

## ⚡ Performance

### Optimizaciones Implementadas
1. **Índices de Base de Datos**: Para consultas frecuentes
2. **Paginación**: Resultados limitados para búsquedas
3. **Proyecciones**: Solo se cargan los datos necesarios
4. **Async/Await**: Operaciones no bloqueantes
5. **Lazy Loading**: Imágenes cargadas bajo demanda

### Búsqueda Optimizada
- Índices compuestos para filtros múltiples
- Consultas SQL optimizadas con Entity Framework
- Paginación eficiente con `Skip` y `Take`

## 🧪 Testing

### Pruebas Unitarias Implementadas
- **Entidades de Dominio**: Validación de reglas de negocio
- **Casos de Uso**: Testing de handlers con mocks
- **Validaciones**: Testing de FluentValidation rules

### Cobertura de Testing
- ✅ Creación de propiedades
- ✅ Cambio de precios
- ✅ Validaciones de entrada
- ✅ Manejo de errores
- ✅ Reglas de dominio

## 🚀 Cómo Ejecutar

### Prerrequisitos
- .NET 8 SDK
- SQL Server (local o Docker)
- Visual Studio 2022 o VS Code

### Configuración
1. Clona el repositorio
2. Configura la cadena de conexión en `appsettings.json`
3. Ejecuta las migraciones:
   ```bash
   dotnet ef database update --project src/MillionLuxury.Infrastructure --startup-project src/MillionLuxury.WebApi
   ```
4. Ejecuta la aplicación:
   ```bash
   dotnet run --project src/MillionLuxury.WebApi
   ```

### Docker Support
```bash
# Iniciar SQL Server
cd infrastructure
docker-compose up -d

# Configurar la aplicación
dotnet ef database update --project src/MillionLuxury.Infrastructure --startup-project src/MillionLuxury.WebApi

# Ejecutar la aplicación
dotnet run --project src/MillionLuxury.WebApi
```

## 📚 Patrones y Principios Aplicados

### Clean Architecture
- ✅ Separación de responsabilidades en capas
- ✅ Inversión de dependencias
- ✅ Independencia de frameworks

### CQRS + Mediator
- ✅ Separación de comandos y consultas
- ✅ Handlers dedicados para cada operación
- ✅ Pipelines de validación

### Domain-Driven Design
- ✅ Entidades ricas en comportamiento
- ✅ Objetos de valor
- ✅ Eventos de dominio
- ✅ Repositorios específicos

### SOLID Principles
- ✅ Single Responsibility
- ✅ Open/Closed
- ✅ Liskov Substitution
- ✅ Interface Segregation
- ✅ Dependency Inversion

## 🔄 Eventos de Dominio

### Eventos Implementados
- `PropertyCreatedDomainEvent`: Se dispara al crear una propiedad
- `PropertyPriceChangedDomainEvent`: Se dispara al cambiar el precio
- `PropertyUpdatedDomainEvent`: Se dispara al actualizar una propiedad
- `PropertyImageAddedDomainEvent`: Se dispara al agregar una imagen
- `PropertyImageRemovedDomainEvent`: Se dispara al remover una imagen

## 📈 Métricas y Monitoreo

### Logs Estructurados
- Entity Framework logs para queries SQL
- Application logs para operaciones de negocio
- Error logs para excepciones

### Health Checks
- Database connectivity
- External services status

---

## 🎯 Próximas Mejoras

- [ ] Implementar caché distribuido (Redis)
- [ ] Agregar búsqueda geoespacial
- [ ] Implementar notificaciones en tiempo real
- [ ] Agregar métricas de performance
- [ ] Implementar rate limiting
- [ ] Agregar integración con servicios de almacenamiento en la nube para imágenes
