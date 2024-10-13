using LR5.Models;
using LR5;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddFile(builder.Configuration["FileLoggerPath"]);
builder.Services.AddSingleton(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultLogger"));
var app = builder.Build();

app.UseMiddlewareExceptionLogger();

app.MapGet("/", async (HttpResponse response) =>
{
    try
    {
        var html = await File.ReadAllTextAsync("Views/Home/Form.html");
        await response.WriteAsync(html);
    }
    catch (Exception ex)
    {
        await response.WriteAsync($"Error occurred: {ex.Message}");
    }
});

app.MapGet("/cookies", async (HttpRequest request, HttpResponse response) =>
{
    var html = "<html><head><title>Cookie list</title></head><body><h1>Cookies:</h1><ul>";

    foreach (var cookie in request.Cookies)
    {
        if (cookie.Key == "test-cookie")
        {
            html += $"<li><strong>{cookie.Key}</strong>: {cookie.Value}</li>";
        }
    }

    html += "</ul></body></html>";
    await response.WriteAsync(html);
});


app.MapPost("/test", async (HttpRequest request, HttpResponse response) =>
{
    string cookieKey = "test-cookie";
    string cookieValue = request.Form["myValue"];
    string inputDate = request.Form["myDateTime"];

    // Console.WriteLine($"Received value: {cookieValue}");

    if (cookieValue.Equals("789"))
    {
        throw new Exception("Value is 789!");
    }

    if (cookieValue.Equals("000"))
    {
        throw new Exception("Value is 000!");
    }

    if (string.IsNullOrWhiteSpace(cookieValue))
    {
        throw new ArgumentException("Value cannot be empty.");
    }

    DateTime expiresDateTime;
    if (!DateTime.TryParse(inputDate, out expiresDateTime))
    {
        throw new FormatException("Invalid date/time format.");
    }

    DateTime currentDateTime = DateTime.Now;

    if (expiresDateTime < currentDateTime)
    {
        throw new Exception("The date cannot be earlier than the default one!");
    }

    // Якщо дата і час старіння == поточній, то збільшуємо хвилини на 1
    if (expiresDateTime == currentDateTime)
    {
        expiresDateTime = currentDateTime.AddMinutes(1);
    }

    // Встановлення параметрів cookie
    CookieOptions option = new CookieOptions
    {
        Expires = expiresDateTime,
        Path = "/"  // cookies доступні для всього додатку
    };

    string formattedDate = expiresDateTime.ToString($"dd.MM.yyyy HH:mm");
    string combinedValue = $"{formattedDate}&nbsp;&nbsp;&nbsp;{cookieValue}";

    // Console.WriteLine($"Saving cookie: {cookieKey} = {combinedValue}");

    response.Cookies.Append(cookieKey, combinedValue, option);

    await response.WriteAsync("Cookie was saved successfully");
});

app.Run();