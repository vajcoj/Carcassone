﻿// <auto-generated />
using System;
using CarcassoneAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarcassoneAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200520185356_DontRequireBoardId")]
    partial class DontRequireBoardId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarcassoneAPI.Models.Board", b =>
                {
                    b.Property<int>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("BoardId");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("CarcassoneAPI.Models.BoardComponent", b =>
                {
                    b.Property<int>("BoardComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("TerrainType")
                        .HasColumnType("int");

                    b.HasKey("BoardComponentId");

                    b.HasIndex("BoardId");

                    b.ToTable("BoardComponents");
                });

            modelBuilder.Entity("CarcassoneAPI.Models.Tile", b =>
                {
                    b.Property<int>("TileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rotation")
                        .HasColumnType("int");

                    b.Property<int>("TileTypeId")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("TileId");

                    b.HasIndex("TileTypeId");

                    b.HasIndex("BoardId", "X", "Y")
                        .IsUnique();

                    b.ToTable("Tiles");
                });

            modelBuilder.Entity("CarcassoneAPI.Models.TileComponent", b =>
                {
                    b.Property<int>("TileId")
                        .HasColumnType("int");

                    b.Property<int>("TileTypeComponentId")
                        .HasColumnType("int");

                    b.Property<int>("BoardComponentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.HasKey("TileId", "TileTypeComponentId");

                    b.HasIndex("BoardComponentId");

                    b.HasIndex("TileTypeComponentId");

                    b.ToTable("TileComponents");
                });

            modelBuilder.Entity("CarcassoneAPI.Models.TileType", b =>
                {
                    b.Property<int>("TileTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("TileTypeId");

                    b.ToTable("TileTypes");
                });

            modelBuilder.Entity("CarcassoneAPI.Models.TileTypeComponent", b =>
                {
                    b.Property<int>("TileTypeComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TerrainType")
                        .HasColumnType("int");

                    b.Property<int>("TileTypeId")
                        .HasColumnType("int");

                    b.HasKey("TileTypeComponentId");

                    b.HasIndex("TileTypeId");

                    b.ToTable("TileTypeComponents");
                });

            modelBuilder.Entity("CarcassoneAPI.Models.TileTypeTerrain", b =>
                {
                    b.Property<int>("TileTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("TerrainType")
                        .HasColumnType("int");

                    b.Property<int>("TileComponentId")
                        .HasColumnType("int");

                    b.Property<int?>("TileTypeComponentId")
                        .HasColumnType("int");

                    b.HasKey("TileTypeId", "Position");

                    b.HasIndex("TileTypeComponentId");

                    b.ToTable("TileTypeTerrains");
                });

            modelBuilder.Entity("CarcassoneAPI.Models.BoardComponent", b =>
                {
                    b.HasOne("CarcassoneAPI.Models.Board", "Board")
                        .WithMany()
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CarcassoneAPI.Models.Tile", b =>
                {
                    b.HasOne("CarcassoneAPI.Models.Board", "Board")
                        .WithMany("Tiles")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarcassoneAPI.Models.TileType", "TileType")
                        .WithMany()
                        .HasForeignKey("TileTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("CarcassoneAPI.Models.TileComponent", b =>
                {
                    b.HasOne("CarcassoneAPI.Models.BoardComponent", "BoardComponent")
                        .WithMany("Components")
                        .HasForeignKey("BoardComponentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CarcassoneAPI.Models.Tile", "Tile")
                        .WithMany("Components")
                        .HasForeignKey("TileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CarcassoneAPI.Models.TileTypeComponent", "TileTypeComponent")
                        .WithMany()
                        .HasForeignKey("TileTypeComponentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("CarcassoneAPI.Models.TileTypeComponent", b =>
                {
                    b.HasOne("CarcassoneAPI.Models.TileType", "TileType")
                        .WithMany("Components")
                        .HasForeignKey("TileTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CarcassoneAPI.Models.TileTypeTerrain", b =>
                {
                    b.HasOne("CarcassoneAPI.Models.TileTypeComponent", "TileTypeComponent")
                        .WithMany("Terrains")
                        .HasForeignKey("TileTypeComponentId");

                    b.HasOne("CarcassoneAPI.Models.TileType", "TileType")
                        .WithMany("Terrains")
                        .HasForeignKey("TileTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
