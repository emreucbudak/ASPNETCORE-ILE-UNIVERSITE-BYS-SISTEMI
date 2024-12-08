﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication8.Data;

#nullable disable

namespace WebApplication8.Migrations
{
    [DbContext(typeof(RepositoryDbContext))]
    [Migration("20241207222054_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication8.Models.Advisors", b =>
                {
                    b.Property<int>("AdvisorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdvisorID"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdvisorID");

                    b.ToTable("Advisors");
                });

            modelBuilder.Entity("WebApplication8.Models.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseID"));

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Credit")
                        .HasColumnType("int");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("bit");

                    b.HasKey("CourseID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("WebApplication8.Models.CourseQuota", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("Quota")
                        .HasColumnType("int");

                    b.Property<int>("RemainingQuota")
                        .HasColumnType("int");

                    b.HasKey("CourseId");

                    b.ToTable("CourseQuotas");
                });

            modelBuilder.Entity("WebApplication8.Models.CourseSelectionHistory", b =>
                {
                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.Property<DateTime>("SelectionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("StudentID");

                    b.ToTable("CourseSelectionHistory");
                });

            modelBuilder.Entity("WebApplication8.Models.NonConfirmedSelections", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SelectedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("NonConfirmedSelections");
                });

            modelBuilder.Entity("WebApplication8.Models.Student", b =>
                {
                    b.Property<int>("StudentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentID"));

                    b.Property<int?>("AdvisorID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentID");

                    b.HasIndex("AdvisorID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("WebApplication8.Models.StudentCourseSelections", b =>
                {
                    b.Property<int>("SelectionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SelectionID"));

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<DateTime>("SelectionDate")
                        .HasColumnType("date");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("SelectionID");

                    b.HasIndex("CourseID");

                    b.HasIndex("StudentID");

                    b.ToTable("StudentCourseSelections");
                });

            modelBuilder.Entity("WebApplication8.Models.Transcripts", b =>
                {
                    b.Property<int>("TranscriptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TranscriptID"));

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<string>("Grade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Semester")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("TranscriptID");

                    b.ToTable("Transcripts");
                });

            modelBuilder.Entity("WebApplication8.Models.Users", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RelatedID")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApplication8.Models.CourseQuota", b =>
                {
                    b.HasOne("WebApplication8.Models.Course", "Course")
                        .WithOne()
                        .HasForeignKey("WebApplication8.Models.CourseQuota", "CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("WebApplication8.Models.CourseSelectionHistory", b =>
                {
                    b.HasOne("WebApplication8.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("WebApplication8.Models.NonConfirmedSelections", b =>
                {
                    b.HasOne("WebApplication8.Models.Course", "Course")
                        .WithMany("NonConfirmedSelections")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApplication8.Models.Student", "Student")
                        .WithMany("NonConfirmedSelections")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("WebApplication8.Models.Student", b =>
                {
                    b.HasOne("WebApplication8.Models.Advisors", "Advisors")
                        .WithMany("Students")
                        .HasForeignKey("AdvisorID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Advisors");
                });

            modelBuilder.Entity("WebApplication8.Models.StudentCourseSelections", b =>
                {
                    b.HasOne("WebApplication8.Models.Course", "Course")
                        .WithMany("StudentCourseSelections")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication8.Models.Student", "Student")
                        .WithMany("StudentCourseSelections")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("WebApplication8.Models.Advisors", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("WebApplication8.Models.Course", b =>
                {
                    b.Navigation("NonConfirmedSelections");

                    b.Navigation("StudentCourseSelections");
                });

            modelBuilder.Entity("WebApplication8.Models.Student", b =>
                {
                    b.Navigation("NonConfirmedSelections");

                    b.Navigation("StudentCourseSelections");
                });
#pragma warning restore 612, 618
        }
    }
}
