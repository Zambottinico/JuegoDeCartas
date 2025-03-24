using System;
using System.Collections.Generic;
using Juego_Sin_Nombre.Models;
using Microsoft.EntityFrameworkCore;

namespace Juego_Sin_Nombre.Data;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
    public DbSet<DiamondOfert> DiamondOfert { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<Card> Cards { get; set; }
    public virtual DbSet<CardOfert> CardOferts { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<Decision> Decisions { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Models.Type> Types { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Invoice>(entity =>
    {
        entity.Property(i => i.CreatedAt)
              .HasColumnType("timestamp with time zone")
              .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        entity.Property(i => i.PaidAt)
       .HasColumnType("timestamp with time zone")
       .HasConversion(
           v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null, // Aplica ToUniversalTime solo si no es nulo
           v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null); // Igual para la conversión inversa



        entity.HasOne(i => i.DiamondOfert)
              .WithMany(d => d.Invoices)
              .HasForeignKey(i => i.DiamondOfferId)
              .OnDelete(DeleteBehavior.Cascade); // Opcional, define el comportamiento de eliminación
    });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("card_pkey");

            entity.ToTable("card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CharacterId).HasColumnName("character_id");
            entity.Property(e => e.Decision1).HasColumnName("decision1");
            entity.Property(e => e.Decision2).HasColumnName("decision2");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsPlayable).HasColumnName("is_playable");
            entity.Property(e => e.Typeid).HasColumnName("typeid");

            entity.HasOne(d => d.Character).WithMany(p => p.Cards)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("fk_cards_characters");

            entity.HasOne(d => d.Decision1Navigation).WithMany(p => p.CardDecision1Navigations)
                .HasForeignKey(d => d.Decision1)
                .HasConstraintName("card_decision1_fkey");

            entity.HasOne(d => d.Decision2Navigation).WithMany(p => p.CardDecision2Navigations)
                .HasForeignKey(d => d.Decision2)
                .HasConstraintName("card_decision2_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Cards)
                .HasForeignKey(d => d.Typeid)
                .HasConstraintName("card_typeid_fkey");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("characters_pkey");

            entity.ToTable("characters");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Lore)
                .HasColumnName("lore");
        });

        modelBuilder.Entity<Decision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("decision_pkey");

            entity.ToTable("decision");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Army).HasColumnName("army");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Economy).HasColumnName("economy");
            entity.Property(e => e.Magic).HasColumnName("magic");
            entity.Property(e => e.Population).HasColumnName("population");
            entity.Property(e => e.UnlockableCharacter).HasColumnName("unlockable_character");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games_pkey");

            entity.ToTable("games");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Armystate).HasColumnName("armystate");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Economystate).HasColumnName("economystate");
            entity.Property(e => e.Gamestate)
                .HasMaxLength(255)
                .HasColumnName("gamestate");
            entity.Property(e => e.Lastcardid).HasColumnName("lastcardid");
            entity.Property(e => e.Magicstate).HasColumnName("magicstate");
            entity.Property(e => e.Populationstate).HasColumnName("populationstate");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Lastcard).WithMany(p => p.Games)
                .HasForeignKey(d => d.Lastcardid)
                .HasConstraintName("games_lastcardid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Games)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("games_userid_fkey");
        });

        modelBuilder.Entity<Models.Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("types_pkey");

            entity.ToTable("types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type1)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(250)
                .HasColumnName("clave");
            entity.Property(e => e.Maxdays).HasColumnName("maxdays");
            entity.Property(e => e.Gold).HasColumnName("gold");
            entity.Property(e => e.Diamonds).HasColumnName("diamonds");
            entity.Property(e => e.MaxLives).HasColumnName("maxlives");
            entity.Property(e => e.LastLifeRecharge)
              .HasColumnName("last_life_recharge") // Nombre en la base de datos
              .HasColumnType("timestamp with time zone") // Define el tipo TIMESTAMP WITH TIME ZONE en PostgreSQL
              .IsRequired(false);

            entity.Property(e => e.Lives).HasColumnName("lives");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("Email");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("Username");

            entity.HasMany(d => d.Characters).WithMany(p => p.Players)
                .UsingEntity<Dictionary<string, object>>(
                    "PlayerCharacter",
                    r => r.HasOne<Character>().WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_characters_character_id_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_characters_player_id_fkey"),
                    j =>
                    {
                        j.HasKey("PlayerId", "CharacterId").HasName("player_characters_pkey");
                        j.ToTable("player_characters");
                        j.IndexerProperty<int>("PlayerId").HasColumnName("player_id");
                        j.IndexerProperty<int>("CharacterId").HasColumnName("character_id");
                    });
        });
        modelBuilder.HasSequence("games_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
