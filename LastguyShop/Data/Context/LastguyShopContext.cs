using System;
using System.Collections.Generic;
using LastguyShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LastguyShop.Data.Context;

public partial class LastguyShopContext : DbContext
{
    public LastguyShopContext()
    {
    }

    public LastguyShopContext(DbContextOptions<LastguyShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FileUpload> FileUploads { get; set; }

    public virtual DbSet<HistoryPrice> HistoryPrices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductFile> ProductFiles { get; set; }

    public virtual DbSet<ProductHistoryPrice> ProductHistoryPrices { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSqlLocalDB; Initial Catalog=LastguyShop; Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileUpload>(entity =>
        {
            entity.HasKey(e => e.FileId);

            entity.ToTable("FileUpload");

            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.FileName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FileNameContent)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FolderPath)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Mimetype)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("MIMEType");
        });

        modelBuilder.Entity<HistoryPrice>(entity =>
        {
            entity.ToTable("HistoryPrice");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(200);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductFile>(entity =>
        {
            entity.ToTable("ProductFile");

            entity.Property(e => e.CreatedDate).HasColumnType("date");
        });

        modelBuilder.Entity<ProductHistoryPrice>(entity =>
        {
            entity.ToTable("ProductHistoryPrice");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.ContactName).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LineId)
                .HasMaxLength(50)
                .HasColumnName("LineID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OfficeHours).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Workday).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
