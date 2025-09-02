# MillionLuxury API - Ejemplos de Uso

Este archivo contiene ejemplos prácticos de cómo utilizar la API de gestión de propiedades.

## 🔐 Autenticación

Todas las peticiones requieren un token JWT válido. Primero debes autenticarte:

### Registrar Usuario
```http
POST /api/v1/auth/register
Content-Type: application/json

{
  "email": "admin@millionluxury.com",
  "password": "SecurePassword123!",
  "roles": ["Admin"]
}
```

### Iniciar Sesión
```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "email": "admin@millionluxury.com",
  "password": "SecurePassword123!"
}
```

**Respuesta:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": 3600
}
```

Usa el token en todas las peticiones posteriores:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🏠 Gestión de Propiedades

### 1. Crear una Villa de Lujo

```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Villa Sunset Paradise",
  "address": "1234 Ocean Drive, Malibu, CA 90265",
  "price": 2500000.00,
  "internalCode": "MALIBU_VILLA_001",
  "year": 2021,
  "propertyType": 5,
  "bedrooms": 6,
  "bathrooms": 8,
  "areaInSquareMeters": 450.75,
  "description": "Exquisite oceanfront villa with panoramic Pacific Ocean views, infinity pool, private beach access, and state-of-the-art smart home technology."
}
```

**Respuesta:**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

### 2. Crear un Apartamento Moderno

```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Downtown Luxury Loft",
  "address": "567 Broadway Street, New York, NY 10012",
  "price": 850000.00,
  "internalCode": "NYC_LOFT_002",
  "year": 2020,
  "propertyType": 2,
  "bedrooms": 2,
  "bathrooms": 2,
  "areaInSquareMeters": 120.5,
  "description": "Modern luxury loft in the heart of Manhattan featuring floor-to-ceiling windows, exposed brick walls, and premium finishes."
}
```

### 3. Crear una Casa Familiar

```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Family Dream House",
  "address": "890 Maple Street, Austin, TX 78704",
  "price": 675000.00,
  "internalCode": "AUSTIN_HOUSE_003",
  "year": 2019,
  "propertyType": 1,
  "bedrooms": 4,
  "bathrooms": 3,
  "areaInSquareMeters": 280.0,
  "description": "Spacious family home with large backyard, modern kitchen, and excellent school district location."
}
```

### 4. Obtener Detalles de una Propiedad

```http
GET /api/v1/properties/a1b2c3d4-e5f6-7890-abcd-ef1234567890
Authorization: Bearer {your-token}
```

**Respuesta:**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "name": "Villa Sunset Paradise",
  "address": "1234 Ocean Drive, Malibu, CA 90265",
  "price": 2500000.00,
  "internalCode": "MALIBU_VILLA_001",
  "year": 2021,
  "ownerId": "user-id-here",
  "propertyType": 5,
  "status": 1,
  "bedrooms": 6,
  "bathrooms": 8,
  "areaInSquareMeters": 450.75,
  "description": "Exquisite oceanfront villa...",
  "createdAt": "2023-08-31T10:30:00Z",
  "updatedAt": "2023-08-31T10:30:00Z",
  "images": []
}
```

### 5. Agregar Imagen a la Villa

```http
POST /api/v1/properties/a1b2c3d4-e5f6-7890-abcd-ef1234567890/images
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "fileName": "villa-oceanview.jpg",
  "base64Content": "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAMCAgMCAgMDAwMEAwMEBQgFBQQEBQoHBwYIDAoMDAsKCwsNDhIQDQ4RDgsLEBYQERMUFRUVDA8XGBYUGBIUFRT/2wBDAQMEBAUEBQkFBQkUDQsNFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBT/wAARCAEAAQADASIAAhEBAxEB/..."
}
```

### 6. Cambiar Precio de la Villa

```http
PATCH /api/v1/properties/a1b2c3d4-e5f6-7890-abcd-ef1234567890/price
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "price": 2750000.00
}
```

### 7. Actualizar Información de la Propiedad

```http
PUT /api/v1/properties/a1b2c3d4-e5f6-7890-abcd-ef1234567890
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Villa Sunset Paradise - Updated",
  "address": "1234 Ocean Drive, Malibu, CA 90265",
  "year": 2021,
  "propertyType": 5,
  "status": 1,
  "bedrooms": 7,
  "bathrooms": 9,
  "areaInSquareMeters": 500.0,
  "description": "Newly renovated exquisite oceanfront villa with additional guest suite and expanded outdoor entertainment area."
}
```

## 🔍 Búsquedas Avanzadas

### 1. Buscar Villas de Lujo (> $1M)

```http
GET /api/v1/properties/search?propertyType=5&minPrice=1000000&pageSize=10&page=1
Authorization: Bearer {your-token}
```

### 2. Buscar Apartamentos en NYC con 2+ Habitaciones

```http
GET /api/v1/properties/search?propertyType=2&minBedrooms=2&address=New%20York&pageSize=20
Authorization: Bearer {your-token}
```

### 3. Buscar Propiedades por Rango de Precios

```http
GET /api/v1/properties/search?minPrice=500000&maxPrice=1000000&page=1&pageSize=15
Authorization: Bearer {your-token}
```

### 4. Buscar Casas Familiares (3-5 habitaciones)

```http
GET /api/v1/properties/search?propertyType=1&minBedrooms=3&maxBedrooms=5&minBathrooms=2
Authorization: Bearer {your-token}
```

### 5. Buscar por Área y Estado

```http
GET /api/v1/properties/search?minArea=200&maxArea=400&status=1&page=1&pageSize=25
Authorization: Bearer {your-token}
```

**Respuesta de Búsqueda:**
```json
{
  "properties": [
    {
      "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
      "name": "Villa Sunset Paradise",
      "address": "1234 Ocean Drive, Malibu, CA 90265",
      "price": 2750000.00,
      "internalCode": "MALIBU_VILLA_001",
      "year": 2021,
      "ownerId": "user-id",
      "propertyType": 5,
      "status": 1,
      "bedrooms": 7,
      "bathrooms": 9,
      "areaInSquareMeters": 500.0,
      "description": "Newly renovated exquisite oceanfront villa...",
      "createdAt": "2023-08-31T10:30:00Z",
      "updatedAt": "2023-08-31T14:20:00Z",
      "images": [
        {
          "id": "img-id-1",
          "fileName": "villa-oceanview.jpg",
          "filePath": "/images/properties/a1b2c3d4.../img-id-1.jpg",
          "isEnabled": true,
          "createdAt": "2023-08-31T11:15:00Z"
        }
      ]
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 25,
  "totalPages": 1
}
```

## 🏢 Casos de Uso por Tipo de Propiedad

### Propiedades Comerciales
```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Downtown Office Building",
  "address": "100 Business Plaza, Chicago, IL 60601",
  "price": 1200000.00,
  "internalCode": "CHI_OFFICE_004",
  "year": 2018,
  "propertyType": 9,
  "bedrooms": 0,
  "bathrooms": 6,
  "areaInSquareMeters": 800.0,
  "description": "Modern office building with 20 private offices, conference rooms, and prime downtown location."
}
```

### Estudios para Inversión
```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Cozy Studio Apartment",
  "address": "45 University Ave, Boston, MA 02215",
  "price": 280000.00,
  "internalCode": "BOS_STUDIO_005",
  "year": 2017,
  "propertyType": 7,
  "bedrooms": 0,
  "bathrooms": 1,
  "areaInSquareMeters": 35.0,
  "description": "Perfect studio for students or young professionals near public transportation and universities."
}
```

### Terrenos para Desarrollo
```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Prime Development Land",
  "address": "500 Acre Road, Phoenix, AZ 85001",
  "price": 450000.00,
  "internalCode": "PHX_LAND_006",
  "year": 2023,
  "propertyType": 10,
  "bedrooms": 0,
  "bathrooms": 0,
  "areaInSquareMeters": 4047.0,
  "description": "1-acre lot zoned for residential development with utilities available and mountain views."
}
```

## 📊 Búsquedas por Categoría de Inversión

### Propiedades de Lujo (> $2M)
```http
GET /api/v1/properties/search?minPrice=2000000&propertyType=5&status=1
Authorization: Bearer {your-token}
```

### Oportunidades de Inversión (< $500K)
```http
GET /api/v1/properties/search?maxPrice=500000&status=1&pageSize=50
Authorization: Bearer {your-token}
```

### Propiedades para Alquiler (2-3 habitaciones)
```http
GET /api/v1/properties/search?minBedrooms=2&maxBedrooms=3&status=1&minArea=80&maxArea=150
Authorization: Bearer {your-token}
```

## 🔄 Gestión de Estados

### Marcar Propiedad como Vendida
```http
PUT /api/v1/properties/a1b2c3d4-e5f6-7890-abcd-ef1234567890
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "Villa Sunset Paradise",
  "address": "1234 Ocean Drive, Malibu, CA 90265",
  "year": 2021,
  "propertyType": 5,
  "status": 2,
  "bedrooms": 7,
  "bathrooms": 9,
  "areaInSquareMeters": 500.0,
  "description": "SOLD - Exquisite oceanfront villa..."
}
```

### Marcar Propiedad como Alquilada
```http
PUT /api/v1/properties/property-id
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "status": 3,
  // ... otros campos
}
```

## ⚠️ Manejo de Errores

### Error de Validación
```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "name": "",
  "price": -100
}
```

**Respuesta (400 Bad Request):**
```json
{
  "code": "ValidationError",
  "message": "Validation failed",
  "errors": [
    {
      "propertyName": "Name",
      "errorMessage": "Name is required"
    },
    {
      "propertyName": "Price",
      "errorMessage": "Price must be greater than or equal to 1"
    }
  ]
}
```

### Error de Propiedad No Encontrada
```http
GET /api/v1/properties/invalid-id
Authorization: Bearer {your-token}
```

**Respuesta (404 Not Found):**
```json
{
  "code": "PropertyNotFound",
  "message": "Property not found"
}
```

### Error de Código Interno Duplicado
```http
POST /api/v1/properties
Authorization: Bearer {your-token}
Content-Type: application/json

{
  "internalCode": "EXISTING_CODE"
  // ... otros campos
}
```

**Respuesta (400 Bad Request):**
```json
{
  "code": "PropertyInternalCodeAlreadyExists",
  "message": "Property with internal code 'EXISTING_CODE' already exists"
}
```

## 🔧 Herramientas de Testing

### Usando cURL

```bash
# Crear propiedad
curl -X POST "https://localhost:7000/api/v1/properties" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Property",
    "address": "123 Test St",
    "price": 300000,
    "internalCode": "TEST001",
    "year": 2020,
    "propertyType": 1,
    "bedrooms": 3,
    "bathrooms": 2,
    "areaInSquareMeters": 150,
    "description": "Test property"
  }'

# Buscar propiedades
curl -X GET "https://localhost:7000/api/v1/properties/search?minPrice=200000&maxPrice=400000" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Usando PowerShell

```powershell
# Variables
$baseUrl = "https://localhost:7000/api/v1"
$token = "YOUR_JWT_TOKEN"
$headers = @{ "Authorization" = "Bearer $token"; "Content-Type" = "application/json" }

# Crear propiedad
$propertyData = @{
    name = "PowerShell Test Property"
    address = "456 PowerShell Ave"
    price = 425000
    internalCode = "PS001"
    year = 2021
    propertyType = 2
    bedrooms = 2
    bathrooms = 2
    areaInSquareMeters = 95.5
    description = "Created via PowerShell"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$baseUrl/properties" -Method POST -Headers $headers -Body $propertyData

# Buscar propiedades
Invoke-RestMethod -Uri "$baseUrl/properties/search?propertyType=2" -Method GET -Headers $headers
```

---

## 📋 Checklist de Testing

- [ ] ✅ Crear diferentes tipos de propiedades
- [ ] ✅ Actualizar información de propiedades
- [ ] ✅ Cambiar precios
- [ ] ✅ Agregar imágenes (formato válido)
- [ ] ✅ Buscar con diferentes filtros
- [ ] ✅ Paginación de resultados
- [ ] ✅ Validaciones de entrada
- [ ] ✅ Manejo de errores
- [ ] ✅ Autenticación requerida
- [ ] ✅ Estados de propiedades

Este conjunto de ejemplos cubre todos los aspectos funcionales de la API de gestión de propiedades inmobiliarias.
