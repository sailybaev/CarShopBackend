// Определение пространства имен для сервисов приложения
namespace WebApplication8.Services;

// Подключение библиотеки для работы с JWT токенами
using System.IdentityModel.Tokens.Jwt;
// Подключение библиотеки для работы с утверждениями (claims) безопасности
using System.Security.Claims;
// Подключение библиотеки для работы с текстом и кодировками
using System.Text;
// Подключение библиотеки для работы с токенами безопасности Microsoft
using Microsoft.IdentityModel.Tokens;
// Подключение моделей данных приложения
using WebApplication8.Models;



// Объявление статического класса для работы с JWT токенами
public static class TokenService
{
    //AppSettings 
    // Секретный ключ для подписи JWT токенов (должен храниться в конфигурации на продакшене)
    private static string key = "ThisIsASecretKeyForJwtTokenGenerationkfoejf2onfk23poj342tewfwe";
    // секретный ключ, на проде хранить безопасно

    // Метод для генерации JWT токена на основе данных пользователя
    public static string GenerateToken(User user)
    {
        // Создание обработчика JWT токенов
        var tokenHandler = new JwtSecurityTokenHandler();
        // Преобразование секретного ключа из строки в массив байтов с кодировкой UTF8
        var tokenKey = Encoding.UTF8.GetBytes(key);
        // Создание дескриптора токена с параметрами безопасности
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Установка субъекта токена (идентификационные данные пользователя)
            Subject = new ClaimsIdentity(new Claim[]
            {
                // Добавление утверждения с именем пользователя
                new Claim(ClaimTypes.Name, user.Username),
                // Добавление утверждения с ролью пользователя
                new Claim(ClaimTypes.Role, user.Role)
            }),
            // Установка времени истечения токена (1 час с текущего момента в UTC)
            Expires = DateTime.UtcNow.AddYears(1),
            // Настройка учетных данных для подписи токена
            SigningCredentials = new SigningCredentials(
                // Создание симметричного ключа безопасности из массива байтов
                new SymmetricSecurityKey(tokenKey), 
                // Указание алгоритма подписи HMAC-SHA256
                SecurityAlgorithms.HmacSha256Signature)
        };
        // Создание токена на основе дескриптора
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Преобразование токена в строковый формат и возврат результата
        return tokenHandler.WriteToken(token);
    }
}
