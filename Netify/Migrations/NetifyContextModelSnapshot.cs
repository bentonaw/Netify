﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetifyAPI.Data;

#nullable disable

namespace NetifyAPI.Migrations
{
    [DbContext(typeof(NetifyContext))]
    partial class NetifyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ArtistTrack", b =>
                {
                    b.Property<int>("ArtistsArtistId")
                        .HasColumnType("int");

                    b.Property<int>("TracksTrackId")
                        .HasColumnType("int");

                    b.HasKey("ArtistsArtistId", "TracksTrackId");

                    b.HasIndex("TracksTrackId");

                    b.ToTable("ArtistTrack");
                });

            modelBuilder.Entity("NetifyAPI.Models.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtistId"), 1L, 1);

                    b.Property<string>("ArtistName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GenreId")
                        .HasColumnType("int");

                    b.Property<string>("SpotifyArtistId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ArtistId");

                    b.HasIndex("GenreId");

                    b.HasIndex("UserId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("NetifyAPI.Models.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreId"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GenreId");

                    b.HasIndex("UserId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("NetifyAPI.Models.Track", b =>
                {
                    b.Property<int>("TrackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrackId"), 1L, 1);

                    b.Property<string>("SpotifySongId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TrackId");

                    b.HasIndex("UserId");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("NetifyAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ArtistTrack", b =>
                {
                    b.HasOne("NetifyAPI.Models.Artist", null)
                        .WithMany()
                        .HasForeignKey("ArtistsArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetifyAPI.Models.Track", null)
                        .WithMany()
                        .HasForeignKey("TracksTrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetifyAPI.Models.Artist", b =>
                {
                    b.HasOne("NetifyAPI.Models.Genre", null)
                        .WithMany("Artists")
                        .HasForeignKey("GenreId");

                    b.HasOne("NetifyAPI.Models.User", null)
                        .WithMany("Artists")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NetifyAPI.Models.Genre", b =>
                {
                    b.HasOne("NetifyAPI.Models.User", null)
                        .WithMany("Genres")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NetifyAPI.Models.Track", b =>
                {
                    b.HasOne("NetifyAPI.Models.User", null)
                        .WithMany("Tracks")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NetifyAPI.Models.Genre", b =>
                {
                    b.Navigation("Artists");
                });

            modelBuilder.Entity("NetifyAPI.Models.User", b =>
                {
                    b.Navigation("Artists");

                    b.Navigation("Genres");

                    b.Navigation("Tracks");
                });
#pragma warning restore 612, 618
        }
    }
}
