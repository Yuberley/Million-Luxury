# Million Luxury

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