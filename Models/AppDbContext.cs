using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkConsole.Models
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            base.OnConfiguring(opt);
            opt.UseNpgsql("Host=localhost;Database=consoleapp;Username=postgres;Password=postgres");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ThanksCard> ThanksCards { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ThanksCardTag> ThanksCardTags { get; set; }
    }
}
