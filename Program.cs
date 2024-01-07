using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentApi.Application.Services;
using StudentApi.Core.Infrastructure;
using StudentApi.Web.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
ConfigurationManager configuration = builder.Configuration;



// Add services to the container.

services.AddControllers();
services.AddDbContext<StudentContext>(x => x.UseSqlServer());
services.AddScoped<IStudentService, StudentService>();


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(options => options.AddPolicy("AllowWebApp",
                     builder => builder.AllowAnyMethod()
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowAnyOrigin()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowWebApp");
app.UseHttpsRedirection();

app.UseAuthorization();
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseHsts();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();

app.Run();
