﻿// <auto-generated />
using CourseSignUp;
using CourseSignUp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CourseSignUp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20180707192256_Enrollment")]
    partial class Enrollment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CourseSignUp.Domain.Model.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<int>("MaxStudentCount");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("CourseSignUp.Domain.Model.Enrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<int?>("CourseId");

                    b.Property<int?>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("CourseSignUp.Domain.Model.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CourseSignUp.Domain.Model.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("CourseSignUp.Domain.Model.Enrollment", b =>
                {
                    b.HasOne("CourseSignUp.Domain.Model.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId");

                    b.HasOne("CourseSignUp.Domain.Model.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");
                });
#pragma warning restore 612, 618
        }
    }
}
