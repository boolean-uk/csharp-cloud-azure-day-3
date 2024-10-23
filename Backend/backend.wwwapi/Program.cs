using backend.wwwapi.EndPoints;
using backend.wwwapi.Repository;
using backend.wwwapi.EndPoints;
using backend.wwwapi.Models;
using backend.wwwapi.Repository;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add CORS service with policy that allows any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDatabaseRepository<Dog>, DatabaseRepository<Dog>>();

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureTodoAPI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();