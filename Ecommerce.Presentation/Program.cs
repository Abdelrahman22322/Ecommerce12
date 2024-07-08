using CloudinaryDotNet;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.Domain.Validators;
using Ecommerce.Core.Services;
using Ecommerce.Core.ServicesContracts;
using Ecommerce.Infrastructure.DbContext;
using Ecommerce.Infrastructure.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wikandoo.Application.Multimedias.CloudinaryMedias;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Add Generic Repository
builder.Services.AddScoped<IProductService, ProductServices>(); // Add ProductService
builder.Services.AddScoped<IBrandServices, BrandServices>(); // Add BrandService
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Add ProductRepository
builder.Services.AddScoped<IValidator<Product>, ProductValidator>(); // Add ProductValidator

builder.Services.AddSingleton(provider =>
{
    var config = provider.GetService<IOptions<CloudinarySettings>>().Value;
    return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
});


// add cloudinary configuration
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Register the Swagger generator, defining 1 or more Swagger documents) 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
