using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.DataContext;

public partial class DemoTokenContext : DbContext
{
    public DemoTokenContext()
    {
    }

    public DemoTokenContext(DbContextOptions<DemoTokenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RolesMaster> RolesMasters { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UsersMaster> UsersMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JWTDemo;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E390A8BADEC");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshTokenId).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Jwtid)
                .HasColumnType("ntext")
                .HasColumnName("JWTId");
            entity.Property(e => e.RefreshToken1)
                .HasMaxLength(400)
                .IsFixedLength()
                .HasColumnName("RefreshToken");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserID");
        });

        modelBuilder.Entity<RolesMaster>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__RolesMas__8AFACE1A9EB326F9");

            entity.ToTable("RolesMaster");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasColumnName("CreatedOn ");
            entity.Property(e => e.RoleName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.Role).WithMany()
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User");
        });

        modelBuilder.Entity<UsersMaster>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UsersMas__1788CC4CEBDC0B96");

            entity.ToTable("UsersMaster");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
