﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BMOS.Models.Entities;

public partial class BmosContext : DbContext
{
    public BmosContext()
    {
    }

    public BmosContext(DbContextOptions<BmosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAddress> TblAddresses { get; set; }

    public virtual DbSet<TblBlog> TblBlogs { get; set; }

    public virtual DbSet<TblFavouriteList> TblFavouriteLists { get; set; }

    public virtual DbSet<TblFeedback> TblFeedbacks { get; set; }

    public virtual DbSet<TblImage> TblImages { get; set; }

    public virtual DbSet<TblNotify> TblNotifies { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblOrderDetail> TblOrderDetails { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblProductInRouting> TblProductInRoutings { get; set; }

    public virtual DbSet<TblRefund> TblRefunds { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRouting> TblRoutings { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblVoucherCode> TblVoucherCodes { get; set; }

    public virtual DbSet<TblVoucherUsed> TblVoucherUseds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PHAT\\SQLEXPRESS;Initial Catalog=BMOS;User ID=sa;Password=12345;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Tbl_Addr__CAA247C817634350");

            entity.ToTable("Tbl_Address");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .HasColumnName("address");
            entity.Property(e => e.BlockVillage)
                .HasMaxLength(100)
                .HasColumnName("block_village");
            entity.Property(e => e.CityProvince)
                .HasMaxLength(100)
                .HasColumnName("city_province");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TblAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tbl_Addre__user___5070F446");
        });

        modelBuilder.Entity<TblBlog>(entity =>
        {
            entity.HasKey(e => e.BlogId);

            entity.ToTable("Tbl_Blog");

            entity.Property(e => e.BlogId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("blog_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TblFavouriteList>(entity =>
        {
            entity.HasKey(e => e.FavouriteListId);

            entity.ToTable("Tbl_FavouriteList");

            entity.Property(e => e.FavouriteListId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("favourite_list_id");
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.TblFavouriteLists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Tbl_FavouriteList_Tbl_Product");

            entity.HasOne(d => d.User).WithMany(p => p.TblFavouriteLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tbl_FavouriteList_Tbl_User");
        });

        modelBuilder.Entity<TblFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId);

            entity.ToTable("Tbl_Feedback");

            entity.Property(e => e.FeedbackId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("feedback_id");
            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .HasColumnName("content");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.Star).HasColumnName("star");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.TblFeedbacks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Tbl_Feedback_Tbl_Product");

            entity.HasOne(d => d.User).WithMany(p => p.TblFeedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tbl_Feedback_Tbl_User");
        });

        modelBuilder.Entity<TblImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("Tbl_Image");

            entity.Property(e => e.ImageId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("image_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.RelationId).HasColumnName("relation_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.Url)
                .HasColumnType("text")
                .HasColumnName("url");
        });

        modelBuilder.Entity<TblNotify>(entity =>
        {
            entity.HasKey(e => e.NotifyId);

            entity.ToTable("Tbl_Notify");

            entity.Property(e => e.NotifyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("notify_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Message)
                .HasMaxLength(200)
                .HasColumnName("message");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TblNotifies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tbl_Notify_Tbl_User");
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("Tbl_Order");

            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_id");
            entity.Property(e => e.Address)
                .HasColumnType("ntext")
                .HasColumnName("address");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.IsConfirm).HasColumnName("is_confirm");
            entity.Property(e => e.Note)
                .HasColumnType("text")
                .HasColumnName("note");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_type");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Point).HasColumnName("point");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<TblOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId);

            entity.ToTable("Tbl_OrderDetail");

            entity.Property(e => e.OrderDetailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_detail_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.TblOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Tbl_OrderDetail_Tbl_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.TblOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Tbl_OrderDetail_Tbl_Product");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId);

            entity.ToTable("Tbl_Permission");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("permission_name");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("Tbl_Product");

            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.IsLoved).HasColumnName("is_loved");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SoldQuantity).HasColumnName("sold_quantity");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<TblProductInRouting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Prod__3213E83FC7A5A9EC");

            entity.ToTable("Tbl_ProductInRouting");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.RoutingId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("routing_id");
        });

        modelBuilder.Entity<TblRefund>(entity =>
        {
            entity.HasKey(e => e.RefundId);

            entity.ToTable("Tbl_Refund");

            entity.Property(e => e.RefundId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("refund_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.IsConfirm).HasColumnName("is_confirm");
            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Order).WithMany(p => p.TblRefunds)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Tbl_Refund_Tbl_Order");

            entity.HasOne(d => d.User).WithMany(p => p.TblRefunds)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tbl_Refund_Tbl_User");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(10)
                .HasColumnName("role_name");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

            entity.HasOne(d => d.UserRole).WithMany()
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Role_Tbl_Permission");

            entity.HasOne(d => d.UserRoleNavigation).WithMany()
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Role_Tbl_User");
        });

        modelBuilder.Entity<TblRouting>(entity =>
        {
            entity.HasKey(e => e.RoutingId);

            entity.ToTable("Tbl_Routing");

            entity.Property(e => e.RoutingId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("routing_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_User");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.DateCreate)
                .HasColumnType("datetime")
                .HasColumnName("date_create");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.IsConfirm).HasColumnName("is_confirm");
            entity.Property(e => e.LastActivitty)
                .HasColumnType("datetime")
                .HasColumnName("last_activitty");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Numberphone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numberphone");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Point).HasColumnName("point");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<TblVoucherCode>(entity =>
        {
            entity.HasKey(e => e.VoucherId);

            entity.ToTable("Tbl_VoucherCode");

            entity.Property(e => e.VoucherId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("voucher_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Used).HasColumnName("used");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.VoucherCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("voucher_code");
        });

        modelBuilder.Entity<TblVoucherUsed>(entity =>
        {
            entity.ToTable("Tbl_VoucherUsed");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoucherId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("voucher_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
