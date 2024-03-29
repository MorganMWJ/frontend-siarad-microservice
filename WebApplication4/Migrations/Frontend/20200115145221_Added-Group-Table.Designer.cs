﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication4.Data;

namespace WebApplication4.Migrations.Frontend
{
    [DbContext(typeof(FrontendContext))]
    [Migration("20200115145221_Added-Group-Table")]
    partial class AddedGroupTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WebApplication4.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<bool>("IsPrivate")
                        .HasColumnName("is_private");

                    b.Property<string>("ModuleCode")
                        .IsRequired()
                        .HasColumnName("module_code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<string>("Uid1")
                        .HasColumnName("uid_1");

                    b.Property<string>("Uid2")
                        .HasColumnName("uid_2");

                    b.HasKey("Id");

                    b.ToTable("group");
                });
#pragma warning restore 612, 618
        }
    }
}
