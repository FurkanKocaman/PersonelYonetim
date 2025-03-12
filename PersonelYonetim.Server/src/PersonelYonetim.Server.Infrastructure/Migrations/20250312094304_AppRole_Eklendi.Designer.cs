﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonelYonetim.Server.Infrastructure.Context;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250312094304_AppRole_Eklendi")]
    partial class AppRole_Eklendi
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Departmanlar.Departman", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aciklama")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Departmanlar");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.IzinTalepleri.IzinTalep", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aciklama")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("BaslangicTarihi")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("BitisTarihi")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("DepartmanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IzinTipi")
                        .HasColumnType("int");

                    b.Property<int>("OnayDurumu")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("OnaylanmaTarihi")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("OnaylayanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonelId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("IzinTalepleri");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.PersonelDepartmanlar.PersonelDepartman", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DepartmanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PozisyonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DepartmanId");

                    b.HasIndex("PersonelId");

                    b.HasIndex("PozisyonId");

                    b.ToTable("PersonelDepartman");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Personeller.Personel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("Cinsiyet")
                        .HasColumnType("bit");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DogumTarihi")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ProfilResimUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Personeller");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Pozisyonlar.Pozisyon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aciklama")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DepartmanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Pozisyonlar");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Rols.AppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Users.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(MAX)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.PersonelDepartmanlar.PersonelDepartman", b =>
                {
                    b.HasOne("PersonelYonetim.Server.Domain.Departmanlar.Departman", "Departman")
                        .WithMany("PersonelDepartmanlar")
                        .HasForeignKey("DepartmanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersonelYonetim.Server.Domain.Personeller.Personel", "Personel")
                        .WithMany("PersonelDepartmanlar")
                        .HasForeignKey("PersonelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersonelYonetim.Server.Domain.Pozisyonlar.Pozisyon", "Pozisyon")
                        .WithMany("PersonelDepartmanlar")
                        .HasForeignKey("PozisyonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departman");

                    b.Navigation("Personel");

                    b.Navigation("Pozisyon");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Personeller.Personel", b =>
                {
                    b.OwnsOne("PersonelYonetim.Server.Domain.Personeller.Adres", "Adres", b1 =>
                        {
                            b1.Property<Guid>("PersonelId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Ilce")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("İlçe");

                            b1.Property<string>("Sehir")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Şehir");

                            b1.Property<string>("TamAdres")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("TamAdres");

                            b1.Property<string>("Ulke")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Ülke");

                            b1.HasKey("PersonelId");

                            b1.ToTable("Personeller");

                            b1.WithOwner()
                                .HasForeignKey("PersonelId");
                        });

                    b.OwnsOne("PersonelYonetim.Server.Domain.Personeller.Iletisim", "Iletisim", b1 =>
                        {
                            b1.Property<Guid>("PersonelId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Eposta")
                                .IsRequired()
                                .HasColumnType("nvarchar(450)")
                                .HasColumnName("Eposta");

                            b1.Property<string>("Telefon")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Telefon");

                            b1.HasKey("PersonelId");

                            b1.HasIndex("Eposta")
                                .IsUnique();

                            b1.ToTable("Personeller");

                            b1.WithOwner()
                                .HasForeignKey("PersonelId");
                        });

                    b.Navigation("Adres")
                        .IsRequired();

                    b.Navigation("Iletisim")
                        .IsRequired();
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Departmanlar.Departman", b =>
                {
                    b.Navigation("PersonelDepartmanlar");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Personeller.Personel", b =>
                {
                    b.Navigation("PersonelDepartmanlar");
                });

            modelBuilder.Entity("PersonelYonetim.Server.Domain.Pozisyonlar.Pozisyon", b =>
                {
                    b.Navigation("PersonelDepartmanlar");
                });
#pragma warning restore 612, 618
        }
    }
}
