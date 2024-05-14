﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cinema.Models;

#nullable disable

namespace cinema.Migrations
{
    [DbContext(typeof(AppDataContext))]
    partial class AppDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("cinema.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FilmeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FilmeId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("cinema.Models.Filme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Duracao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Filmes");
                });

            modelBuilder.Entity("cinema.Models.Sala", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AssentosOcupados")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuantidadeAssentos")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Sala");
                });

            modelBuilder.Entity("cinema.Models.Secao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Dia")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FilmeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HorarioFinal")
                        .HasColumnType("TEXT");

                    b.Property<string>("HorarioInicio")
                        .HasColumnType("TEXT");

                    b.Property<int>("Mes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SalaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FilmeId")
                        .IsUnique();

                    b.HasIndex("SalaId")
                        .IsUnique();

                    b.ToTable("Secao");
                });

            modelBuilder.Entity("cinema.Models.Categoria", b =>
                {
                    b.HasOne("cinema.Models.Filme", "Filme")
                        .WithMany("Categorias")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filme");
                });

            modelBuilder.Entity("cinema.Models.Secao", b =>
                {
                    b.HasOne("cinema.Models.Filme", "Filme")
                        .WithOne("Secao")
                        .HasForeignKey("cinema.Models.Secao", "FilmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cinema.Models.Sala", "Sala")
                        .WithOne("Secao")
                        .HasForeignKey("cinema.Models.Secao", "SalaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filme");

                    b.Navigation("Sala");
                });

            modelBuilder.Entity("cinema.Models.Filme", b =>
                {
                    b.Navigation("Categorias");

                    b.Navigation("Secao");
                });

            modelBuilder.Entity("cinema.Models.Sala", b =>
                {
                    b.Navigation("Secao");
                });
#pragma warning restore 612, 618
        }
    }
}
