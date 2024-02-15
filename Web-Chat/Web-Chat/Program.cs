using Web_Chat.Abstractions;
using Web_Chat.Controllers;
using Web_Chat.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var sectionName = nameof(RealTimeConnectionsController).SkipLast(10);
    if (context.Request.Path.ToString().Contains($"http://localhost{context.Request.Host}/api/{sectionName}") && !context.Request.HttpContext.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync(sectionName + "endpoints must define web sockets functionality.");
        return;
    }

    await next(context);
});

app.Run();
