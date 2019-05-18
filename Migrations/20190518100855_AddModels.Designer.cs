﻿// <auto-generated />
using System;
using EntityFrameworkConsole;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EntityFrameworkConsole.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190518100855_AddModels")]
    partial class AddModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("EntityFrameworkConsole.Department", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("EntityFrameworkConsole.ThanksCard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<long?>("FromId");

                    b.Property<string>("Title");

                    b.Property<long?>("ToId");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("ThanksCards");
                });

            modelBuilder.Entity("EntityFrameworkConsole.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("DepartmentId");

                    b.Property<bool>("IsAdmin");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EntityFrameworkConsole.ThanksCard", b =>
                {
                    b.HasOne("EntityFrameworkConsole.User", "From")
                        .WithMany()
                        .HasForeignKey("FromId");

                    b.HasOne("EntityFrameworkConsole.User", "To")
                        .WithMany()
                        .HasForeignKey("ToId");
                });

            modelBuilder.Entity("EntityFrameworkConsole.User", b =>
                {
                    b.HasOne("EntityFrameworkConsole.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId");
                });
#pragma warning restore 612, 618
        }
    }
}