using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var _context = new AppDbContext();
            if (_context.Users.Count() == 0)
            {
                // Usersテーブルが空なら初期データを作成する。
                var admin = new User { Name="admin", Password="admin", IsAdmin=true };
                var user  = new User { Name="user", Password="user", IsAdmin=false };
                _context.Users.Add(admin);
                _context.Users.Add(user);

                _context.ThanksCards.Add(new ThanksCard { Title="title1", Body="body1", From=admin, To=user,  CreatedDateTime=DateTime.Now});
                _context.ThanksCards.Add(new ThanksCard { Title="title2", Body="body2", From=admin, To=user,  CreatedDateTime=DateTime.Now});
                _context.ThanksCards.Add(new ThanksCard { Title="title3", Body="body3", From=admin, To=user,  CreatedDateTime=DateTime.Now});
                _context.ThanksCards.Add(new ThanksCard { Title="title4", Body="body4", From=user, To=admin,  CreatedDateTime=DateTime.Now});
                _context.SaveChanges();
            }

            var ranks = _context.ThanksCards
                                  .GroupBy(t => t.To)
                                  .Select(t => new Rank { Name = t.Key.Name, Count = t.Count() })
                                  .OrderByDescending(t => t.Count)
                                  .ToList();

            Console.WriteLine(ObjectDumper.Dump(ranks));
        }
    }

    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual Department Department { get; set; }
    }

    public class Department
    {
        public long Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

    public class ThanksCard
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual User From { get; set; }
        public virtual User To { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class Rank
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

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
    }
}
