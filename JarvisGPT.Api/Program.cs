using JarvisGPT.Application;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ChatGptOptions>(builder.Configuration.GetSection(nameof(ChatGptOptions)));
builder.Services.AddSingleton<IChatAIService, ChatGPTService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("/api/chat", async ([FromBody] ChatRequest request, [FromServices] IChatAIService ai) => {
    return Results.Ok(new {
        Result = await ai.AskCli(request.Content)
    });
});

app.MapPost("/api/pseudo", async ([FromBody] ChatRequest request, [FromServices] IChatAIService ai) =>
{
    return Results.Ok(new
    {
        Result = await ai.AskPseudo(request.Content)
    });
});

app.MapPost("/api/pseudo/reset", async ([FromServices] IChatAIService ai) =>
{
    await ai.ResetPseudo();
    return Results.NoContent();
});

app.Run();
