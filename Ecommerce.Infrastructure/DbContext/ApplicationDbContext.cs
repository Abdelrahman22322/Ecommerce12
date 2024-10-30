using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Infrastructure.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{

    public ApplicationDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Tracking> Trackings { get; set; }
    public DbSet<Shipping> Shippings { get; set; }
    public DbSet<CloudinaryResult> CloudinaryResults { get; set; }

    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Category> Categories { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Customer> Customers { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Order> Orders { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.OrderDetail> OrderDetails { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Product> Products { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Supplier> Suppliers { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.User> Users { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Role> Roles { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.UserRole> UserRoles { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.UserProfile> UserProfiles { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Comment> Comments { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Rating> Ratings { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Brand> Brands { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.ProductAttribute> ProductAttributes { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.ProductAttributeValue> ProductAttributeValues { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Cart> Carts { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.CartItem> CartItems { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.OrderStatus> OrderStatuses { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Shipper> Shippers { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Wishlist> Wishlists { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.WishlistItem> WishlistItems { get; set; }
    public DbSet<ProductImage?> ProductImages { get; set; }
    public DbSet<ShippingMethod?> ShippingCost  { get; set; }
    public DbSet<ShippingState?> ShippingState { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.ProductCategory> ProductCategories { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.ProductTag> ProductTags { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Ecommerce.Core.Domain.Entities.Tag> Tags { get; set; }

    
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    
        optionsBuilder.EnableSensitiveDataLogging();
    
     }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        //modelBuilder.Entity<CartItem>()
        //    .HasKey(ci => new { ci.CartId, ci.ProductId });

        //modelBuilder.Entity<OrderDetail>()
        //    .HasKey(od => new { od.OrderId, od.ProductId });

        //modelBuilder.Entity<ProductCategory>()
        //    .HasKey(pc => new { pc.ProductId, pc.CategoryId });

        //modelBuilder.Entity<ProductTag>()
        //    .HasKey(pt => new { pt.ProductId, pt.TagId });


        //modelBuilder.Entity<OrderStatus>()
        //    .HasMany(os => os.Orders)
        //    .WithOne(o => o.OrderStatus)
        //    .HasForeignKey(o => o.OrderStatusId);

        //modelBuilder.Entity<Payment>()
        //    .HasMany(pm => pm.Orders)
        //    .WithOne(o => o.Payment)
        //    .HasForeignKey(o => o.PaymentId);

        //modelBuilder.Entity<Shipper>()
        //    .HasMany(s => s.ShippingMethods)
        //    .WithOne(sm => sm.Shipper)
        //    .HasForeignKey(sm => sm.ShipperId);

        //modelBuilder.Entity<Shipping>()
        //    .HasMany(sm => sm.Orders)
        //    .WithOne(o => o.ShippingMethod)
        //    .HasForeignKey(o => o.ShippingMethodId);

        //modelBuilder.Entity<User>()
        //    .HasOne(u => u.Cart)
        //    .WithOne(c => c.User);


        //modelBuilder.Entity<Cart>()
        //    .HasMany(c => c.CartItems)
        //    .WithOne(ci => ci.Cart)
        //    .HasForeignKey(ci => ci.CartId);

        //modelBuilder.Entity<User>()
        //    .HasOne(u => u.Wishlist)
        //    .WithOne(w => w.User);


        //modelBuilder.Entity<Wishlist>()
        //    .HasMany(w => w.WishlistItems)
        //    .WithOne(wi => wi.Wishlist)
        //    .HasForeignKey(wi => wi.WishlistId);

        //modelBuilder.Entity<User>()
        //    .HasMany(u => u.Ratings)
        //    .WithOne(r => r.User)
        //    .HasForeignKey(r => r.UserId);

        //modelBuilder.Entity<User>()
        //    .HasMany(u => u.Comments)
        //    .WithOne(r => r.User)
        //    .HasForeignKey(r => r.UserId);

        //modelBuilder.Entity<Product>()
        //    .HasMany(p => p.ProductAttributeValues)
        //    .WithOne(pav => pav.Product)
        //    .HasForeignKey(pav => pav.ProductId);


        //modelBuilder.Entity<ProductAttribute>()
        //    .HasMany(pa => pa.ProductAttributeValues)
        //    .WithOne(pav => pav.ProductAttribute)
        //    .HasForeignKey(pav => pav.ProductAttributeId);

        //modelBuilder.Entity<UserRole>()
        //    .HasKey(ur => new { ur.UserId, ur.RoleId });

        //modelBuilder.Entity<UserRole>()
        //    .HasOne(ur => ur.User)
        //    .WithMany(u => u.UserRoles)
        //    .HasForeignKey(ur => ur.UserId);

        //modelBuilder.Entity<UserRole>()
        //    .HasOne(ur => ur.Role)
        //    .WithMany(r => r.UserRoles)
        //    .HasForeignKey(ur => ur.RoleId);

        //modelBuilder.Entity<User>()
        //    .HasOne(u => u.UserProfile)
        //    .WithOne(up => up.User)
        //    .HasForeignKey<UserProfile>(up => up.UserId);

        modelBuilder.Entity<CartItem>()
     .HasKey(ci => new { ci.CartId, ci.ProductId });

        //modelBuilder.Entity<OrderDetail>()
        //    .HasKey(od => new { od.OrderId, od.ProductId });

        modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });

        modelBuilder.Entity<ProductTag>()
            .HasKey(pt => new { pt.ProductId, pt.TagId });

       

        modelBuilder.Entity<OrderStatus>()
            .HasMany(os => os.Orders)
            .WithOne(o => o.OrderStatus)
            .HasForeignKey(o => o.OrderStatusId);

        modelBuilder.Entity<Payment>()
            .HasMany(pm => pm.Orders)
            .WithOne(o => o.Payment)
            .HasForeignKey(o => o.PaymentId);

        //modelBuilder.Entity<Shipper>()
        //    .HasMany(s => s.ShippingMethods)
        //    .WithOne(sm => sm.Shipper)
        //    .HasForeignKey(sm => sm.ShipperId);

        modelBuilder.Entity<Shipping>()
            .HasMany(sm => sm.Orders)
            .WithOne(o => o.Shipping)
            .HasForeignKey(o => o.ShippingId);
        //.WithOne(o => o.ShippingMethod)
        //.HasForeignKey(o => o.ShippingMethodId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId); 

        modelBuilder.Entity<Cart>()
            .HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Wishlist)
            .WithOne(w => w.User)
            .HasForeignKey<Wishlist>(w => w.UserId); 

 
        modelBuilder.Entity<Wishlist>()
            .HasMany(w => w.WishlistItems)
            .WithOne(wi => wi.Wishlist)
            .HasForeignKey(wi => wi.WishlistId);

        
        modelBuilder.Entity<WishlistItem>()
            .HasKey(wi => new { wi.WishlistId, wi.ProductId });

        modelBuilder.Entity<User>()
            .HasMany(u => u.Ratings)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductAttributeValues)
            .WithOne(pav => pav.Product)
            .HasForeignKey(pav => pav.ProductId);

        modelBuilder.Entity<ProductAttribute>()
            .HasMany(pa => pa.ProductAttributeValues)
            .WithOne(pav => pav.ProductAttribute)
            .HasForeignKey(pav => pav.ProductAttributeId);

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(up => up.User)
            .HasForeignKey<UserProfile>(up => up.UserId);

   
        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Discount)
            .WithMany(d => d.Products)
            .HasForeignKey(p => p.DiscountId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(x=> x.User)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    
        modelBuilder.Entity<Shipping>()
            .HasOne(s => s.ShippingMethod)
            .WithMany(sc => sc.Shippings)
            .HasForeignKey(s => s.ShippingMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<Shipping>()
            .HasOne(s => s.ShippingState)
            .WithMany(ss => ss.Shippings)
            .HasForeignKey(s => s.ShippingStateId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}
