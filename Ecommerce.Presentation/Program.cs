using System.Security.Claims;
using System.Text;
using CloudinaryDotNet;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.Domain.Validators;
using Ecommerce.Core.DTO;
using Ecommerce.Core.DTO.Map;
using Ecommerce.Core.Services;
using Ecommerce.Core.ServicesContracts;
using Ecommerce.Infrastructure.DbContext;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Presentation.helper;
using Ecommerce.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PdfSharp.Charting;
using CloudinarySettings = Wikandoo.Application.Multimedias.CloudinaryMedias.CloudinarySettings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Add console logging


// Add Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add specific repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add services
builder.Services.AddScoped<IProductService, ProductServices>();
builder.Services.AddScoped<IBrandServices, BrandServices>();
builder.Services.AddScoped<ICategoriesServices, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IProductAttributeServices, ProductAttributeServices>();
builder.Services.AddScoped<IProductAttributeValueServices, ProductAttributeValueServices>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IProductTagService, ProductTagService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IUserManagerRepository, UserManagerRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IWishlistItemService, WishlistItemService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add Product Validator
builder.Services.AddScoped<IValidator<AddProductDto>, ProductValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, ProductValidator1>();

// Add Cloudinary configuration
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetService<IOptions<CloudinarySettings>>()?.Value;
    if (config == null)
    {
        throw new InvalidOperationException("Cloudinary settings are not configured properly.");
    }
    return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
       // RoleClaimType = "role"
    };
});

builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "user"));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Register the Swagger generator, defining 1 or more Swagger documents
// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce API", Version = "v1" });

    // Add JWT Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Set the requirement for JWT Bearer token in every endpoint
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
