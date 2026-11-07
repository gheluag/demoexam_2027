using System;
using System.Collections.Generic;
using ChudoObuv.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ChudoObuv.DbContexts;

public partial class ChudoobuvContext : DbContext
{
    public ChudoobuvContext()
    {
    }

    public ChudoobuvContext(DbContextOptions<ChudoobuvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacrure> Manufacrures { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Ordersitem> Ordersitems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Stockitem> Stockitems { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=123456789;database=chudoobuv", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.46-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCat).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.IdCat).HasColumnName("idCat");
            entity.Property(e => e.CatName)
                .HasColumnType("text")
                .HasColumnName("catName");
        });

        modelBuilder.Entity<Manufacrure>(entity =>
        {
            entity.HasKey(e => e.IdManuf).HasName("PRIMARY");

            entity.ToTable("manufacrures");

            entity.Property(e => e.IdManuf).HasColumnName("idManuf");
            entity.Property(e => e.ManufName).HasColumnType("text");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.UserId, "fk_UserIdOrd_idx");

            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_UserIdOrd");
        });

        modelBuilder.Entity<Ordersitem>(entity =>
        {
            entity.HasKey(e => e.IdOrderItem).HasName("PRIMARY");

            entity.ToTable("ordersitem");

            entity.HasIndex(e => e.OrderId, "fk_OrderId_idx");

            entity.HasIndex(e => e.ProdId, "fk_ProdIdOrd_idx");

            entity.Property(e => e.IdOrderItem).HasColumnName("idOrderItem");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProdId).HasColumnName("prodId");
            entity.Property(e => e.SizeId).HasColumnName("sizeId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasPrecision(10, 2).HasColumnName("unitPrice");
            entity.HasOne(d => d.Size).WithMany().HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_OrderItemSize");

            entity.HasOne(d => d.Order).WithMany(p => p.Ordersitems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_OrderId");

            entity.HasOne(d => d.Prod).WithMany(p => p.Ordersitems)
                .HasForeignKey(d => d.ProdId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_ProdIdOrd");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProd).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.CatId, "fk_CatId_idx");

            entity.HasIndex(e => e.ManufId, "fk_ManufId_idx");

            entity.HasIndex(e => e.SubCatId, "fk_SubCatId_idx");

            entity.Property(e => e.IdProd).HasColumnName("idProd");
            entity.Property(e => e.Descrip)
                .HasColumnType("text")
                .HasColumnName("descrip");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.ManufId).HasColumnName("manufId");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProdName)
                .HasColumnType("text")
                .HasColumnName("prodName");
            entity.Property(e => e.Struct)
                .HasColumnType("text")
                .HasColumnName("struct");
            entity.Property(e => e.SubCatId).HasColumnName("subCatId");

            entity.HasOne(d => d.Cat).WithMany(p => p.Products)
                .HasForeignKey(d => d.CatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_CatId");

            entity.HasOne(d => d.Manuf).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_ManufId");

            entity.HasOne(d => d.SubCat).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubCatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_SubCatId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.RoleName).HasColumnType("text");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.IdSize).HasName("PRIMARY");

            entity.ToTable("sizes");

            entity.Property(e => e.IdSize).HasColumnName("idSize");
            entity.Property(e => e.Size1)
                .HasColumnType("text")
                .HasColumnName("Size");
        });

        modelBuilder.Entity<Stockitem>(entity =>
        {
            entity.HasKey(e => e.IdStockItem).HasName("PRIMARY");

            entity.ToTable("stockitems");

            entity.HasIndex(e => e.ProdId, "fk_ProdStockId_idx");

            entity.HasIndex(e => e.SizeId, "fk_SizeId_idx");

            entity.Property(e => e.IdStockItem).HasColumnName("idStockItem");
            entity.Property(e => e.ProdId).HasColumnName("prodId");
            // EF проверяет, что остаток не изменился после загрузки.
            entity.Property(e => e.Quantity).HasColumnName("quantity").IsConcurrencyToken();

            entity.HasOne(d => d.Prod).WithMany(p => p.Stockitems)
                .HasForeignKey(d => d.ProdId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_ProdStockId");

            entity.HasOne(d => d.Size).WithMany(p => p.Stockitems)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_SizeId");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.IdSubCat).HasName("PRIMARY");

            entity.ToTable("subcategory");

            entity.Property(e => e.IdSubCat).HasColumnName("idSubCat");
            entity.Property(e => e.SubCatName)
                .HasColumnType("text")
                .HasColumnName("subCatName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "fk_roleId_idx");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.FirstName)
                .HasColumnType("text")
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasColumnType("text")
                .HasColumnName("lastName");
            entity.Property(e => e.Login)
                .HasColumnType("text")
                .HasColumnName("login");
            entity.Property(e => e.MiddleName)
                .HasColumnType("text")
                .HasColumnName("middleName");
            entity.Property(e => e.RoleId).HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_roleId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
