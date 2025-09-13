using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DAL.UserModels;
namespace DAL;

public partial class ShippingContext : IdentityDbContext<ApplicationUser>
{
    public ShippingContext()
    {

    }

    public ShippingContext(DbContextOptions<ShippingContext> options)
        : base(options)
    {

    }

    public virtual DbSet<TbCarrier> TbCarriers { get; set; }

    public virtual DbSet<TbCity> TbCities { get; set; }

    public virtual DbSet<TbCountry> TbCountries { get; set; }

    public virtual DbSet<TbPaymentMethod> TbPaymentMethods { get; set; }

    public virtual DbSet<TbSetting> TbSettings { get; set; }

    public virtual DbSet<TbShippingType> TbShippingTypes { get; set; }

    public virtual DbSet<TbShippment> TbShippments { get; set; }

    public virtual DbSet<TbShippmentStatus> TbShippmentStatuses { get; set; }

    public virtual DbSet<TbSubscriptionPackage> TbSubscriptionPackages { get; set; }

    public virtual DbSet<TbUserReceiver> TbUserReceivers { get; set; }

    public virtual DbSet<TbUserSender> TbUserSenders { get; set; }

    public virtual DbSet<TbUserSubscription> TbUserSubscriptions { get; set; }

    public virtual DbSet<VwCities> VwCities { get; set; }

    public virtual DbSet<TbRefreshToken> TbRefreshTokens { get; set; }
    public virtual DbSet<TbShippingPackage> TbShippingPackages { get; set; }

    public virtual DbSet<ShipmentFlat> ShipmentFlats { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ShipmentFlat>().HasNoKey().ToView(null);


        base.OnModelCreating(modelBuilder);



        modelBuilder.Entity<VwCities>().ToView("VwCities");


        modelBuilder.Entity<TbCarrier>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CarrierName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbCity>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CityAname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CityAName");
            entity.Property(e => e.CityEname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CityEName");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Country).WithMany(p => p.TbCities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbCities_TbCountries");
        });

        modelBuilder.Entity<TbCountry>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CountryAname)
                .HasMaxLength(200)
                .HasColumnName("CountryAName");
            entity.Property(e => e.CountryEname)
                .HasMaxLength(200)
                .HasColumnName("CountryEName");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbPaymentMethod>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MethdAname)
                .HasMaxLength(200)
                .HasColumnName("MethdAName");
            entity.Property(e => e.MethodEname)
                .HasMaxLength(200)
                .HasColumnName("MethodEName");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbSetting>(entity =>
        {
            entity.ToTable("TbSetting");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TbShippingType>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ShippingTypeAname)
                .HasMaxLength(200)
                .HasColumnName("ShippingTypeAName");
            entity.Property(e => e.ShippingTypeEname)
                .HasMaxLength(200)
                .HasColumnName("ShippingTypeEName");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbShippment>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PackageValue).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.ShippingDate).HasColumnType("datetime");
            entity.Property(e => e.ShippingRate).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_TbShippments_TbPaymentMethods");

            entity.HasOne(d => d.Receiver).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbShippments_TbUserReceivers");

            entity.HasOne(d => d.Sender).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbShippments_TbUserSenders");

            entity.HasOne(d => d.ShippingType).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.ShippingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbShippments_TbShippingTypes");



              entity.HasOne(d => d.Carrier).WithMany(p => p.TbShippments)
               .HasForeignKey(d => d.CarrierId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_TbShippments_TbCarriers");

               entity.HasOne(s => s.ShippingPackging)
              .WithMany(p => p.TbShippments)
              .HasForeignKey(s => s.ShippingPackgingId);

        });

        modelBuilder.Entity<TbShippmentStatus>(entity =>
        {
            entity.ToTable("TbShippmentStatus");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

           

            entity.HasOne(d => d.Shippment).WithMany(p => p.ShippmentStatus)
                .HasForeignKey(d => d.ShippmentId)
                .HasConstraintName("FK_TbShippmentStatus_TbShippments");
        });

        modelBuilder.Entity<TbSubscriptionPackage>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PackageName).HasMaxLength(200);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbUserReceiver>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(200);
            entity.Property(e => e.ReceiverName).HasMaxLength(200);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.TbUserReceivers)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbUserReceivers_TbCities");
        });

        modelBuilder.Entity<TbUserSender>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(200);
            entity.Property(e => e.SenderName).HasMaxLength(200);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.TbUserSenders)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbUserSenders_TbCities");
        });

        modelBuilder.Entity<TbUserSubscription>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Package).WithMany(p => p.TbUserSubscriptions)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbUserSubscriptions_TbSubscriptionPackages");
        });

        modelBuilder.Entity<TbRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

           //  0 = Active, 1 = Revoked
            entity.Property(e => e.CurrentState)
                .HasDefaultValue(0) 
                .IsRequired();

            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("GETDATE()");
        });




        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);




}
