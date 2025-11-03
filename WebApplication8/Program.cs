// Подключение библиотеки для работы с OpenAPI/Swagger моделями
using Microsoft.OpenApi.Models;
// Подключение библиотеки для JWT Bearer аутентификации
using Microsoft.AspNetCore.Authentication.JwtBearer;
// Подключение библиотеки для работы с токенами безопасности
using Microsoft.IdentityModel.Tokens;
// Подключение библиотеки для работы с текстом и кодировками
using System.Text;

// Создание билдера веб-приложения с аргументами командной строки
var builder = WebApplication.CreateBuilder(args);

// JWT setup
// Получение секретного ключа из конфигурации приложения (appsettings.json)
var key = builder.Configuration["jwt:key"]! ; // тот же ключ, что в TokenService
// Добавление сервиса аутентификации в контейнер зависимостей
builder.Services.AddAuthentication(options =>
    {
        // Установка схемы аутентификации по умолчанию - JWT Bearer
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        // Установка схемы проверки прав доступа по умолчанию - JWT Bearer
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // Настройка параметров JWT Bearer аутентификации
    .AddJwtBearer(options =>
    {
        // Установка параметров валидации токена
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Отключение проверки издателя токена (Issuer)
            ValidateIssuer = false,
            // Отключение проверки аудитории токена (Audience)
            ValidateAudience = false,
            // Включение проверки подписи токена
            ValidateIssuerSigningKey = true,
            // Установка ключа для проверки подписи токена (симметричный ключ из конфигурации)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// Добавление сервиса авторизации в контейнер зависимостей
builder.Services.AddAuthorization();
// Добавление поддержки контроллеров в приложение
builder.Services.AddControllers();
// Добавление генератора конечных точек для API Explorer (для Swagger)
builder.Services.AddEndpointsApiExplorer();
// Настройка генератора документации Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Регистрация документа Swagger версии v1 с названием "CarShop API"
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarShop API", Version = "v1" });

    // Добавляем JWT авторизацию в Swagger
    // Добавление определения схемы безопасности Bearer для Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // Описание для пользователя о том, как использовать JWT авторизацию
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        // Имя заголовка для передачи токена
        Name = "Authorization",
        // Указание, что токен передается в заголовке запроса
        In = ParameterLocation.Header,
        // Тип схемы безопасности - ключ API
        Type = SecuritySchemeType.ApiKey,
        // Схема авторизации - Bearer
        Scheme = "Bearer"
    });

    // Добавление требования безопасности для всех операций API
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // Ссылка на схему безопасности Bearer, определенную выше
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            // Пустой массив областей (scopes) - все эндпоинты требуют авторизации
            new string[] {}
        }
    });
});

// Построение приложения из конфигурации билдера
var app = builder.Build();

// Middleware
// Включение middleware для генерации JSON документации Swagger
app.UseSwagger();
// Включение middleware для Swagger UI (веб-интерфейс документации API)
app.UseSwaggerUI();

// Включение middleware для перенаправления HTTP запросов на HTTPS
app.UseHttpsRedirection();
// Включение middleware аутентификации (проверка JWT токенов)
app.UseAuthentication();
// Включение middleware авторизации (проверка прав доступа)
app.UseAuthorization();
// Регистрация маршрутов контроллеров
app.MapControllers();

// Запуск приложения
app.Run();

// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
// dotnet add package Microsoft.OpenApi
// dotnet add package Swashbuckle.AspNetCore
// dotnet add package System.IdentityModel.Tokens.Jwt
