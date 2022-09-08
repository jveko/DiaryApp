using System.Net.Mime;
using DiaryApp.Contexts;
using DiaryApp.Filters;
using DiaryApp.Interfaces;
using DiaryApp.Middlewares;
using DiaryApp.Responses;
using DiaryApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "DiaryApp", Version = "v1"});
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization", Type = SecuritySchemeType.ApiKey, Scheme = "Bearer", BearerFormat = "JWT",
            In = ParameterLocation.Header, Description = "JWT Authorization header using the Bearer scheme."
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.Audience = "api1";
    x.Authority = "http://localhost:5000";
    x.RequireHttpsMetadata = false;
});
builder.Services.AddDbContext<DiaryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DiaryContext")));

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INoteService, NoteService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddControllers(conf =>
{
    conf.Filters.Add<ExceptionFilter>();
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var result = new ValidationErrorResult(context.ModelState);
        result.ContentTypes.Add(MediaTypeNames.Application.Json);
        return result;
    };
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseWhen(context => context.Request.Path.StartsWithSegments("/notes"), conf =>
{
    conf.UseMiddleware<ValidateTokenMiddleware>();
});
app.MapControllers();


app.Run();