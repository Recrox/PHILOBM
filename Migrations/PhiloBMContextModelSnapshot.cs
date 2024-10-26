﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PHILOBM.Database;

#nullable disable

namespace PHILOBM.Migrations
{
    [DbContext(typeof(PhiloBMContext))]
    partial class PhiloBMContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("PHILOBM.Models.Base.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FactureId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Prix")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FactureId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("PHILOBM.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Adresse")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom")
                        .HasColumnType("TEXT");

                    b.Property<string>("Prenom")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("PHILOBM.Models.Facture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("VoitureId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("VoitureId");

                    b.ToTable("Factures");
                });

            modelBuilder.Entity("PHILOBM.Models.Voiture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Kilometrage")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroChassis")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroPlaque")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Voitures");
                });

            modelBuilder.Entity("PHILOBM.Models.Base.Service", b =>
                {
                    b.HasOne("PHILOBM.Models.Facture", null)
                        .WithMany("Services")
                        .HasForeignKey("FactureId");
                });

            modelBuilder.Entity("PHILOBM.Models.Facture", b =>
                {
                    b.HasOne("PHILOBM.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PHILOBM.Models.Voiture", "Voiture")
                        .WithMany()
                        .HasForeignKey("VoitureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Voiture");
                });

            modelBuilder.Entity("PHILOBM.Models.Voiture", b =>
                {
                    b.HasOne("PHILOBM.Models.Client", "Proprietaire")
                        .WithMany("Voitures")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proprietaire");
                });

            modelBuilder.Entity("PHILOBM.Models.Client", b =>
                {
                    b.Navigation("Voitures");
                });

            modelBuilder.Entity("PHILOBM.Models.Facture", b =>
                {
                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}