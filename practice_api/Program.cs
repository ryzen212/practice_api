
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using practice_api.Contracts;
using practice_api.Converters;
using practice_api.Data;
using practice_api.Repositories;
using practice_api.Services;
using practice_api.Validations;
using Scalar.AspNetCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TrimmingStringJsonConverter());
        //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddOpenApi("v2");
builder.Services.AddDbContext<AppDbContext>(option=>
option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddRateLimiter(rateLimetterOptions =>
{
    rateLimetterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 3;
        options.QueueLimit = 3;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    rateLimetterOptions.AddFixedWindowLimiter("login", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 3;
        options.QueueLimit = 0;
    });
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("FrontEnd", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
     .AllowCredentials()
     .AllowAnyMethod()
     .AllowAnyHeader();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)
),

    };
});
builder.Services.AddAuthorization();
builder.Services.AddIdentityCore<AppIdentityUser>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<AppIdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();



//Binding
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IAuthServices), typeof(AuthServices));
builder.Services.AddScoped(typeof(IUserServices), typeof(UserServices));
builder.Services.AddScoped<UserValidation>();
builder.Services.AddScoped(typeof(ITokenServices), typeof(TokenServices));
builder.Services.AddScoped(typeof(IValidationService), typeof(ValidationService));
builder.Services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));

builder.Services.AddScoped<FileService>();


//Validation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

//Api  versioning 

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
}).AddMvc(options =>
{
    options.Conventions.Add(new VersionByNamespaceConvention());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
builder.Logging.AddConsole();   // logs to console

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    //app.MapScalarApiReference();

    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/openapi/{description.GroupName}.json", description.GroupName.ToUpperInvariant());
        }
    });
}

//app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRateLimiter();
app.UseCors("FrontEnd");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
