﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iOSClub.Data;

#nullable disable

namespace iOSClub.Data.Migrations
{
    [DbContext(typeof(iOSContext))]
    [Migration("20250518075347_AddDepartmentKey")]
    partial class AddDepartmentKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("ProjectModelStaffModel", b =>
                {
                    b.Property<string>("ProjectsId")
                        .HasColumnType("varchar(33)");

                    b.Property<string>("StaffsUserId")
                        .HasColumnType("varchar(10)");

                    b.HasKey("ProjectsId", "StaffsUserId");

                    b.HasIndex("StaffsUserId");

                    b.ToTable("ProjectModelStaffModel");
                });

            modelBuilder.Entity("StaffModelTaskModel", b =>
                {
                    b.Property<string>("TasksId")
                        .HasColumnType("varchar(33)");

                    b.Property<string>("UsersUserId")
                        .HasColumnType("varchar(10)");

                    b.HasKey("TasksId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("StaffModelTaskModel");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.ArticleModel", b =>
                {
                    b.Property<string>("Path")
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastWriteTime")
                        .HasColumnType("DATE");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.HasKey("Path");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.DepartmentModel", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.HasKey("Name");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.ProjectModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(33)");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(512)");

                    b.Property<string>("EndTime")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("StartTime")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentName");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.ResourceModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(33)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Tag")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.StaffModel", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId");

                    b.HasIndex("DepartmentName");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.StudentModel", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Academy")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("varchar(2)");

                    b.Property<DateTime>("JoinTime")
                        .HasColumnType("DATE");

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasColumnType("varchar(11)");

                    b.Property<string>("PoliticalLandscape")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.TaskModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(33)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("EndTime")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasColumnType("varchar(33)");

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.TodoModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(33)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("EndTime")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("StartTime")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("ProjectModelStaffModel", b =>
                {
                    b.HasOne("iOSClub.Data.DataModels.ProjectModel", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("iOSClub.Data.DataModels.StaffModel", null)
                        .WithMany()
                        .HasForeignKey("StaffsUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StaffModelTaskModel", b =>
                {
                    b.HasOne("iOSClub.Data.DataModels.TaskModel", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("iOSClub.Data.DataModels.StaffModel", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.ProjectModel", b =>
                {
                    b.HasOne("iOSClub.Data.DataModels.DepartmentModel", "Department")
                        .WithMany("Projects")
                        .HasForeignKey("DepartmentName");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.StaffModel", b =>
                {
                    b.HasOne("iOSClub.Data.DataModels.DepartmentModel", "Department")
                        .WithMany("Staffs")
                        .HasForeignKey("DepartmentName");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.TaskModel", b =>
                {
                    b.HasOne("iOSClub.Data.DataModels.ProjectModel", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.TodoModel", b =>
                {
                    b.HasOne("iOSClub.Data.DataModels.StudentModel", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.DepartmentModel", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("Staffs");
                });

            modelBuilder.Entity("iOSClub.Data.DataModels.ProjectModel", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
