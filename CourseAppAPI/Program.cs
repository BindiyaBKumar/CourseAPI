using CourseAppAPI.DAL;
using CourseAppAPI.Repository;
using CourseAppAPI.Services;
using Microsoft.EntityFrameworkCore;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMetrics();
builder.Services.AddDbContext<CourseDetailDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDetailDB")));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
options.WithOrigins("http://localhost:4200")
.AllowAnyMethod()
.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseAuthorization();

app.MapMetrics(); // Add this line to expose the metrics endpoint

app.MapControllers();

app.Run();
