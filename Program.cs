using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new AppDbContext();

            var test = new LINQTest(context);

            var departments = await test.GetDepartmentsAsync();
            foreach(var department in departments)
            {
                Console.WriteLine(ObjectDumper.Dump(department));
            }

            var userRanking = await test.ThanksCardUserRankingAsync();
            Console.WriteLine(ObjectDumper.Dump(userRanking));

            var departmentRanking = await test.ThanksCardDepartmentRankingAsync();
            Console.WriteLine(ObjectDumper.Dump(departmentRanking));
        }
    }

    public class LINQTest
    {
        private readonly AppDbContext _context;

        public LINQTest(AppDbContext context)
        {
            this._context = context;

            // Usersテーブルが空なら初期データを作成する。
            if (_context.Users.Count() == 0)
            {
                var dept1 = new Department { Code=1, Name="dept1", Parent=null  };
                var dept2 = new Department { Code=2, Name="dept2", Parent=dept1 };
                _context.Departments.Add(dept1);
                _context.Departments.Add(dept2);

                var admin  = new User { Name="admin",  Password="admin",  IsAdmin=true,  Department=dept1 };
                var user1  = new User { Name="user1",  Password="user1",  IsAdmin=false, Department=dept1 };
                var user2  = new User { Name="user2",  Password="user2",  IsAdmin=false, Department=dept2 };
                _context.Users.Add(admin);
                _context.Users.Add(user1);
                _context.Users.Add(user2);

                _context.ThanksCards.Add(new ThanksCard { Title="title1", Body="body1", From=admin, To=user1, CreatedDateTime=DateTime.Now });
                _context.ThanksCards.Add(new ThanksCard { Title="title2", Body="body2", From=admin, To=user1, CreatedDateTime=DateTime.Now });
                _context.ThanksCards.Add(new ThanksCard { Title="title3", Body="body3", From=admin, To=user1, CreatedDateTime=DateTime.Now });
                _context.ThanksCards.Add(new ThanksCard { Title="title4", Body="body4", From=admin, To=user2, CreatedDateTime=DateTime.Now });
                _context.ThanksCards.Add(new ThanksCard { Title="title5", Body="body5", From=admin, To=user2, CreatedDateTime=DateTime.Now });
                _context.ThanksCards.Add(new ThanksCard { Title="title6", Body="body6", From=user1, To=admin, CreatedDateTime=DateTime.Now });
                _context.ThanksCards.Add(new ThanksCard { Title="title7", Body="body7", From=user2, To=admin, CreatedDateTime=DateTime.Now });
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Rank>> ThanksCardUserRankingAsync()
        {
            return await _context.ThanksCards
                                  .GroupBy(t => t.To)
                                  .Select(t => new Rank { Name = t.Key.Name, Count = t.Count() })
                                  .OrderByDescending(t => t.Count)
                                  .ToListAsync();
        } 

        public async Task<IEnumerable<Rank>> ThanksCardDepartmentRankingAsync()
        {
            return await _context.ThanksCards
                                  .GroupBy(t => t.To.Department)
                                  .Select(t => new Rank { Name = t.Key.Name, Count = t.Count() })
                                  .OrderByDescending(t => t.Count)
                                  .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments
                                  .Include(Department => Department.Parent) // Eager-loading
                                  .ToListAsync();
        }
    }

    #region Entities
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
        public virtual Department Parent { get; set; }
        public virtual ICollection<Department> Children { get; set; }
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
    #endregion

    #region DbContext
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
    #endregion
}
