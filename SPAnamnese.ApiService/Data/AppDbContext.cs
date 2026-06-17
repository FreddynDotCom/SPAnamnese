using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SPAnamnese.ApiService.Models;

namespace SPAnamnese.ApiService.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<tbanamnese> tbanamneses { get; set; }

    public virtual DbSet<tbanexo> tbanexos { get; set; }

    public virtual DbSet<tbatendimento> tbatendimentos { get; set; }

    public virtual DbSet<tbformulario> tbformularios { get; set; }

    public virtual DbSet<tbpaciente> tbpacientes { get; set; }

    public virtual DbSet<tbperguntum> tbpergunta { get; set; }

    public virtual DbSet<tbprofissional> tbprofissionals { get; set; }

    public virtual DbSet<tbrespostum> tbresposta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<tbanamnese>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.DataAtualizacao)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()");
            entity.Property(e => e.DataCriacao).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(a => a.Paciente)
                  .WithMany()
                  .HasForeignKey(a => a.PacienteId);

            entity.HasOne(a => a.Profissional)
                  .WithMany()
                  .HasForeignKey(a => a.ProfissionalId);

        });

        modelBuilder.Entity<tbanexo>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");
        });

        modelBuilder.Entity<tbatendimento>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");
        });

        modelBuilder.Entity<tbformulario>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");
        });

        modelBuilder.Entity<tbpaciente>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");
        });

        modelBuilder.Entity<tbperguntum>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");
        });

        modelBuilder.Entity<tbprofissional>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");
        });

        modelBuilder.Entity<tbrespostum>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
