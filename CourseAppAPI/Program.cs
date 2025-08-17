using CourseAppAPI.DAL;
using CourseAppAPI.Helper;
using CourseAppAPI.Models;
using CourseAppAPI.Repository;
using CourseAppAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);

//Configure authorization using JWT Token
var configkey = builder.Configuration.GetValue<string>("Jwt:Key");
if(string.IsNullOrEmpty(configkey))
{
    throw new ArgumentException("JWT Key is not configured properly in appsettings.json");
}
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configkey))
            };
        });
builder.Services.AddSingleton<JWTTokenService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CourseAppAPI", Version = "v1" });

    var bearerscheme = new OpenApiSecurityScheme
    {
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", bearerscheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            bearerscheme, Array.Empty<string>()
        }
    });
});
builder.Services.AddMetrics();
if (builder.Configuration.GetValue<bool>("UseUnMemoryDb"))
{
    builder.Services.AddDbContext<CourseDetailDBContext>(options => options.UseInMemoryDatabase("CourseDetailDB"));}
else
{
    builder.Services.AddDbContext<CourseDetailDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDetailDB")));
}
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CourseDetailDBContext>();

    if (dbContext.Database.IsInMemory() && app.Environment.IsDevelopment())
    {
        // Seed the in-memory database with initial data if needed
        if (dbContext.Database.EnsureCreated())
        {
            dbContext.AddRange(
                new CourseDetail { CourseId = 1, CourseNumber = "X01", CourseName = ".Net Development", CourseDescription = "Detailed tuitorial on .Net Development", CourseCost = 99.99, CourseDuration = 8, CourseTutor = "Mr. X",CourseStatus="Active",CreatedAt= DateTime.Parse("2025-08-14") },
                new CourseDetail { CourseId = 2, CourseNumber = "X02", CourseName = "C# Programming", CourseDescription = "C# Programming tuitorial for beginners", CourseCost = 89.99, CourseDuration = 10, CourseTutor = "Mr. Y", CourseStatus = "Inactive" },
                new CourseDetail { CourseId = 3, CourseNumber = "X03", CourseName = "Entity Framework", CourseDescription = "Hands-on project using entity framework", CourseCost = 99.99, CourseDuration = 8, CourseTutor = "Mr. X", CourseStatus = "Active" }
            );
            dbContext.SaveChanges();
        }

    }
}
    

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Map("/",() => Results.Redirect("/swagger/"));
}

app.UseCors(options =>
options.WithOrigins("http://localhost:4200")
.AllowAnyMethod()
.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.MapMetrics(); // Add this line to expose the metrics endpoint

app.MapControllers();

app.Run();
