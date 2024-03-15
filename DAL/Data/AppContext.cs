using System;
using System.Collections.Generic;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public partial class AppContext : DbContext
{
    public AppContext()
    {
    }

    public AppContext(DbContextOptions<AppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<StdExam> StdExams { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("branch");

            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("name");

            entity.HasMany(d => d.Depts).WithMany(p => p.Branches)
                .UsingEntity<Dictionary<string, object>>(
                    "BranchDepartment",
                    r => r.HasOne<Department>().WithMany()
                        .HasForeignKey("DeptId")
                        .HasConstraintName("FK_branch_department_department"),
                    l => l.HasOne<Branch>().WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_branch_department_branch"),
                    j =>
                    {
                        j.HasKey("BranchId", "DeptId");
                        j.ToTable("branch_department");
                        j.IndexerProperty<int>("BranchId").HasColumnName("branch_id");
                        j.IndexerProperty<int>("DeptId").HasColumnName("dept_id");
                    });
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.ChId);

            entity.ToTable("choice");

            entity.Property(e => e.ChId).HasColumnName("ch_id");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("text");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CrsId);

            entity.ToTable("course");

            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.CrsName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("crs_name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId);

            entity.ToTable("department");

            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExId);

            entity.ToTable("exam");

            entity.Property(e => e.ExId).HasColumnName("ex_id");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Duration)
                .HasPrecision(0)
                .HasColumnName("duration");
            entity.Property(e => e.TotalScore).HasColumnName("total_score");

            entity.HasOne(d => d.Crs).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CrsId)
                .HasConstraintName("FK_exam_course");

            entity.HasMany(d => d.QIds).WithMany(p => p.Exes)
                .UsingEntity<Dictionary<string, object>>(
                    "ExQuestion",
                    r => r.HasOne<Question>().WithMany()
                        .HasForeignKey("QId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ex_question_question"),
                    l => l.HasOne<Exam>().WithMany()
                        .HasForeignKey("ExId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ex_question_exam"),
                    j =>
                    {
                        j.HasKey("ExId", "QId");
                        j.ToTable("ex_question");
                        j.IndexerProperty<int>("ExId").HasColumnName("ex_id");
                        j.IndexerProperty<int>("QId").HasColumnName("q_id");
                    });
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InsId);

            entity.ToTable("instructor");

            entity.Property(e => e.InsId)
                .ValueGeneratedNever()
                .HasColumnName("ins_id");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");

            entity.HasOne(d => d.Dept).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_instructor_department1");

            entity.HasOne(d => d.Ins).WithOne(p => p.Instructor)
                .HasForeignKey<Instructor>(d => d.InsId)
                .HasConstraintName("FK_instructor_user");

            entity.HasMany(d => d.Crs).WithMany(p => p.Ins)
                .UsingEntity<Dictionary<string, object>>(
                    "InstructorCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CrsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_instructor_course_course"),
                    l => l.HasOne<Instructor>().WithMany()
                        .HasForeignKey("InsId")
                        .HasConstraintName("FK_instructor_course_instructor"),
                    j =>
                    {
                        j.HasKey("InsId", "CrsId");
                        j.ToTable("instructor_course");
                        j.IndexerProperty<int>("InsId").HasColumnName("ins_id");
                        j.IndexerProperty<int>("CrsId").HasColumnName("crs_id");
                    });
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QId);

            entity.ToTable("question");

            entity.Property(e => e.QId).HasColumnName("q_id");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.QText)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("q_text");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("type");

            entity.HasOne(d => d.Crs).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CrsId)
                .HasConstraintName("FK_question_course");

            entity.HasMany(d => d.Ches).WithMany(p => p.QIds)
                .UsingEntity<Dictionary<string, object>>(
                    "QuestionsChoice",
                    r => r.HasOne<Choice>().WithMany()
                        .HasForeignKey("ChId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_questions_choices_choice"),
                    l => l.HasOne<Question>().WithMany()
                        .HasForeignKey("QId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_questions_choices_question"),
                    j =>
                    {
                        j.HasKey("QId", "ChId");
                        j.ToTable("questions_choices");
                        j.IndexerProperty<int>("QId").HasColumnName("q_id");
                        j.IndexerProperty<int>("ChId").HasColumnName("ch_id");
                    });
        });

        modelBuilder.Entity<StdExam>(entity =>
        {
            entity.HasKey(e => new { e.StdId, e.ExId, e.QId });

            entity.ToTable("std_exam");

            entity.Property(e => e.StdId).HasColumnName("std_id");
            entity.Property(e => e.ExId).HasColumnName("ex_id");
            entity.Property(e => e.QId).HasColumnName("q_id");
            entity.Property(e => e.StdAnswer).HasColumnName("std_answer");

            entity.HasOne(d => d.Ex).WithMany(p => p.StdExams)
                .HasForeignKey(d => d.ExId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_std_takes_exam_exam");

            entity.HasOne(d => d.QIdNavigation).WithMany(p => p.StdExams)
                .HasForeignKey(d => d.QId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_std_exam_question");

            entity.HasOne(d => d.Std).WithMany(p => p.StdExams)
                .HasForeignKey(d => d.StdId)
                .HasConstraintName("FK_std_exam_student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StdId);

            entity.ToTable("student");

            entity.Property(e => e.StdId)
                .ValueGeneratedNever()
                .HasColumnName("std_id");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");

            entity.HasOne(d => d.Dept).WithMany(p => p.Students)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_student_department");

            entity.HasOne(d => d.Std).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.StdId)
                .HasConstraintName("FK_student_user");
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => new { e.SId, e.CrsId }).HasName("PK_student course");

            entity.ToTable("student_course");

            entity.Property(e => e.SId).HasColumnName("s_id");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.Grade).HasColumnName("grade");

            entity.HasOne(d => d.Crs).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_student_course_course");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK_student_course_student");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("topic");

            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.TopicName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("topic_name");

            entity.HasOne(d => d.Crs).WithMany()
                .HasForeignKey(d => d.CrsId)
                .HasConstraintName("FK_topic_course");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UId);

            entity.ToTable("user");

            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("fname");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("lname");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
