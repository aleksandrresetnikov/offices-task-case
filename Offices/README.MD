# Справочник Терминалов (Test Task)

Реализация справочника терминалов с автоматическим фоновым обновлением данных из JSON-источника.

## Технологический стек
- **Runtime**: .NET 9 (C# 13)
- **Framework**: ASP.NET Core
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core 9
- **Logging**: Microsoft Extension Logging (Structured)
- **Architecture**: Service-Provider Pattern, Background Services, Extension-based DI.

## Особенности реализации

### 1. Фоновый импорт (BackgroundService)
Используется `TerminalImportBackgroundService` на базе `PeriodicTimer`. В отличие от `Task.Delay`, обеспечивает точные интервалы запуска с учетом времени выполнения задачи.
- Первый запуск происходит сразу при старте приложения.
- Реализован **Graceful Shutdown** через `CancellationToken`.
- Т.к. под капотом BackgroundService - это Singleton, внутри используется `IServiceScopeFactory` для корректной работы со Scoped-сервисами внутри синглтона.

### 2. Парсинг адресов
Реализован `AddressParser`, использующий **скомпилированные регулярные выражения** (`GeneratedRegex`). Парсер умеет:
- Выделять регион и очищать его от суффиксов (г., обл.).
- Извлекать номер дома, включая корпуса, строения и литеры.
- Парсить номер квартиры/офиса с приведением к `int?`.

### 3. Архитектура и Clean Code
- **Extensions**: Все настройки (CORS, Swagger, DI, Settings) вынесены в отдельные методы расширения для чистоты `Program.cs`.
- **Fluent API**: БД сконфигурирована через `IEntityTypeConfiguration` с четким указанием длин полей, индексов и конвертаций.
- **Middleware**: Реализован `ExceptionHandlingMiddleware` для глобального перехвата ошибок.
- **Complex Types**: Для координат использован `ComplexProperty`.

## Настройка (appsettings.json)

Перед запуском убедитесь, что в `appsettings.json` указаны корректные данные для подключения к вашей БД PostgreSQL:

```json
{
  "ImportSettings": {
    "IntervalHours": 24 
  },
  "DbConnectionSettings": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "offices_task_db",
    "Username": "postgres",
    "Password": "ВАШ_ПАРОЛЬ"
  }
}