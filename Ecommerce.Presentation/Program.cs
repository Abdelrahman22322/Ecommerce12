using System.Security.Claims;
using System.Text;
using AutoMapper;
using CloudinaryDotNet;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.helper;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.Domain.Validators;
using Ecommerce.Core.DTO;
using Ecommerce.Core.DTO.Map;
using Ecommerce.Core.Services;
using Ecommerce.Core.ServicesContracts;
using Ecommerce.Infrastructure.DbContext;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Services;
using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PdfSharp.Charting;
using CloudinarySettings = Wikandoo.Application.Multimedias.CloudinaryMedias.CloudinarySettings;
using JWT = Ecommerce.Presentation.helper.JWT;

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

// Add Hangfire services
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

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
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<IShipperService, ShipperService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IShippingMethodService, ShippingMethodService>();
builder.Services.AddScoped<IShippingStateService, ShippingStateService>();
builder.Services.AddScoped<IOrderStateService, OrderStateService>();

// Add TokenCleanupService
//builder.Services.AddScoped<ITokenCleanupService, TokenCleanupService>();
builder.Services.AddSingleton<Func<IServiceProvider, ITokenCleanupService>>(sp =>
{
    return scopeServiceProvider =>
    {
        using (var scope = scopeServiceProvider.CreateScope())
        {
            return scope.ServiceProvider.GetRequiredService<ITokenCleanupService>();
        }
    };
});
builder.Services.AddScoped<ITokenCleanupService, TokenCleanupService>();

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

builder.Services.Configure<Ecommerce.Core.Domain.helper.Twilio>(builder.Configuration.GetSection("Twilio"));


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

    // Map JsonPatchDocument to an OpenApiSchema
    options.MapType<JsonPatchDocument>(() => new OpenApiSchema
    {
        Type = "object",
        AdditionalPropertiesAllowed = true
    });
});

// Add CommentService with banned words
var bannedWords = new List<string> { "����", "����", "�����", "����", "���" };
builder.Services.AddScoped<ICommentService>(provider =>
{
    var mapper = provider.GetRequiredService<IMapper>();
    var commentRepository = provider.GetRequiredService<IGenericRepository<Comment>>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    return new CommentService(mapper, commentRepository, bannedWords, httpContextAccessor);
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

//// Schedule the cleanup job to run every hour
//var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
//recurringJobManager.AddOrUpdate(
//    "CleanupExpiredTokens",
//    () => app.Services.GetService<ITokenCleanupService>()!.CleanupExpiredTokensAsync(),
//    Cron.Hourly);


using (var scope = app.Services.CreateScope())
{
    var tokenCleanupFactory = scope.ServiceProvider.GetRequiredService<Func<IServiceProvider, ITokenCleanupService>>();
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate(
        "TokenCleanupJob",
        () => tokenCleanupFactory(app.Services).CleanupExpiredTokensAsync(),
        Cron.Hourly);
}
app.Run();
