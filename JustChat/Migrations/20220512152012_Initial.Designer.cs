﻿// <auto-generated />
using System;
using JustChat.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JustChat.Migrations
{
    [DbContext(typeof(DbMain))]
    [Migration("20220512152012_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("JustChat.Models.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("SendDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("SenderId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.ToTable("ChatMessage");
                });

            modelBuilder.Entity("JustChat.Models.ChatUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailVrfToken")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime?>("EmailVrfTokenExpiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("NewEmail")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordResetToken")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime?>("PasswordResetTokenExpiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Username", "Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("JustChat.Models.ChatMessage", b =>
                {
                    b.HasOne("JustChat.Models.ChatUser", "Sender")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("JustChat.Models.ChatUser", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
