using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<MyTask> Tasks { get; set; }
        public DbSet<MyNote> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyTask>().HasData(GetTestTasks());
            modelBuilder.Entity<MyNote>().HasData(GetTestNotes());
            base.OnModelCreating(modelBuilder);
        }

        private MyTask[] GetTestTasks()
        {
            return new MyTask[]
            {
            new MyTask { Id = 1, Content = "Wyrzucić śmieci", Deadline = DateTime.Now, Priority = 1, IsCompleted = false},
            new MyTask { Id = 2, Content = "Poodkurzać", Deadline = DateTime.Now, Priority = 3, IsCompleted = false},
            new MyTask { Id = 3, Content = "Pozmywać", Deadline = DateTime.Now, Priority = 2, IsCompleted = false},
            new MyTask { Id = 4, Content = "Zrobić pranie", Deadline = DateTime.Now, Priority = 2, IsCompleted = false},
            new MyTask { Id = 5, Content = "Dokończyć projekt", Deadline = DateTime.Now, Priority = 1, IsCompleted = false}
            };
        }
        private MyNote[] GetTestNotes()
        {
            return new MyNote[]
            {
            new MyNote { Id = 1, Content = "Wyrzucić śmieci", Created = DateTime.Now},
            new MyNote { Id = 2, Content = "Poodkurzać", Created = DateTime.Now},
            new MyNote { Id = 3, Content = "Pozmywać", Created = DateTime.Now},
            new MyNote { Id = 4, Content = "Zrobić pranie", Created = DateTime.Now},
            new MyNote { Id = 5, Content = "Dokończyć projekt", Created = DateTime.Now}
            };
        }
    }
}
