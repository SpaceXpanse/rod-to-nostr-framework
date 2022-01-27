﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Relay.Data;

#nullable disable

namespace Relay.Migrations
{
    [DbContext(typeof(RelayDbContext))]
    [Migration("20220127141832_Balances")]
    partial class Balances
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NNostr.Client.NostrEvent", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<int>("Kind")
                        .HasColumnType("integer");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Signature")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("NNostr.Client.NostrEventTag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<List<string>>("Data")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("EventId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TagIdentifier")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventTags");
                });

            modelBuilder.Entity("Relay.Data.Balance", b =>
                {
                    b.Property<string>("PublicKey")
                        .HasColumnType("text");

                    b.Property<long>("CurrentBalance")
                        .HasColumnType("bigint");

                    b.HasKey("PublicKey");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("Relay.Data.BalanceTopup", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BalanceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId");

                    b.ToTable("BalanceTopups");
                });

            modelBuilder.Entity("Relay.Data.BalanceTransaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BalanceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BalanceTopupId")
                        .HasColumnType("text");

                    b.Property<string>("EventId")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId");

                    b.HasIndex("BalanceTopupId");

                    b.HasIndex("EventId");

                    b.ToTable("BalanceTransactions");
                });

            modelBuilder.Entity("NNostr.Client.NostrEventTag", b =>
                {
                    b.HasOne("NNostr.Client.NostrEvent", "Event")
                        .WithMany("Tags")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Relay.Data.BalanceTopup", b =>
                {
                    b.HasOne("Relay.Data.Balance", "Balance")
                        .WithMany()
                        .HasForeignKey("BalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Balance");
                });

            modelBuilder.Entity("Relay.Data.BalanceTransaction", b =>
                {
                    b.HasOne("Relay.Data.Balance", "Balance")
                        .WithMany("BalanceTransactions")
                        .HasForeignKey("BalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Relay.Data.BalanceTopup", "Topup")
                        .WithMany("BalanceTransactions")
                        .HasForeignKey("BalanceTopupId");

                    b.HasOne("NNostr.Client.NostrEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.Navigation("Balance");

                    b.Navigation("Event");

                    b.Navigation("Topup");
                });

            modelBuilder.Entity("NNostr.Client.NostrEvent", b =>
                {
                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Relay.Data.Balance", b =>
                {
                    b.Navigation("BalanceTransactions");
                });

            modelBuilder.Entity("Relay.Data.BalanceTopup", b =>
                {
                    b.Navigation("BalanceTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
