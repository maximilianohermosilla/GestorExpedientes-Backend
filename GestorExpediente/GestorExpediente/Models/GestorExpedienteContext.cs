using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestorExpediente.Models;

public partial class GestorExpedienteContext : DbContext
{
    public GestorExpedienteContext()
    {
    }

    public GestorExpedienteContext(DbContextOptions<GestorExpedienteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acto> Acto { get; set; }

    public virtual DbSet<Caratula> Caratula { get; set; }

    public virtual DbSet<Expediente> Expediente { get; set; }

    public virtual DbSet<Perfil> Perfil { get; set; }

    public virtual DbSet<SituacionRevista> SituacionRevista { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost; Database=GestorExpediente; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acto>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Caratula>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.Property(e => e.Documento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Expediente1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Expediente");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaExpediente).HasColumnType("date");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones).IsUnicode(false);

            entity.HasOne(d => d.IdActoNavigation).WithMany(p => p.Expediente)
                .HasForeignKey(d => d.IdActo)
                .HasConstraintName("FK_Expediente_Acto");

            entity.HasOne(d => d.IdCaratulaNavigation).WithMany(p => p.Expediente)
                .HasForeignKey(d => d.IdCaratula)
                .HasConstraintName("FK_Expediente_Caratula");

            entity.HasOne(d => d.IdSituacionRevistaNavigation).WithMany(p => p.Expediente)
                .HasForeignKey(d => d.IdSituacionRevista)
                .HasConstraintName("FK_Expediente_SituacionRevista");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SituacionRevista>(entity =>
        {
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdPerfil)
                .HasConstraintName("FK_Usuario_Perfil");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
