using EntityFrameworkConsole.Models;
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
            Program.Main();
            Console.ReadKey();
        }

        static async Task Main()
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

            var thanksCards = await test.GetThanksCards();
            Console.WriteLine(ObjectDumper.Dump(thanksCards));
            foreach (var thanksCard in thanksCards)
            {
                Console.WriteLine($"  ThanksCard {thanksCard.Title}");
                foreach (var tag in thanksCard.Tags)
                {
                    Console.WriteLine($"    Tag {tag.Name}");
                }
            }
            /*
            foreach (var thanksCard in thanksCards)
            {
                Console.WriteLine($"  thanksCard {thanksCard.Title}");
                foreach (var thanksCardTag in thanksCard.ThanksCardTags)
                {
                    Console.WriteLine($"    Tag {thanksCardTag.Tag.Name}");
                }
            }
            */

            var data = await test.GetData();
            Console.WriteLine(ObjectDumper.Dump(data));
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

                var tag1 = new Tag { Name="tag1" };
                var tag2 = new Tag { Name="tag2" };
                var tag3 = new Tag { Name="tag3" };
                _context.Tags.Add(tag1);
                _context.Tags.Add(tag2);
                _context.Tags.Add(tag3);

                var thanksCard1 = new ThanksCard { Title="title1", Body="body1", From=admin, To=user1, CreatedDateTime=DateTime.Now };
                var thanksCard2 = new ThanksCard { Title="title2", Body="body2", From=admin, To=user1, CreatedDateTime=DateTime.Now };
                var thanksCard3 = new ThanksCard { Title="title3", Body="body3", From=admin, To=user1, CreatedDateTime=DateTime.Now };
                var thanksCard4 = new ThanksCard { Title="title4", Body="body4", From=admin, To=user2, CreatedDateTime=DateTime.Now };
                var thanksCard5 = new ThanksCard { Title="title5", Body="body5", From=admin, To=user2, CreatedDateTime=DateTime.Now };
                var thanksCard6 = new ThanksCard { Title="title6", Body="body6", From=user1, To=admin, CreatedDateTime=DateTime.Now };
                var thanksCard7 = new ThanksCard { Title="title7", Body="body7", From=user2, To=admin, CreatedDateTime=DateTime.Now };

                _context.ThanksCards.Add(thanksCard1);
                _context.ThanksCards.Add(thanksCard2);
                _context.ThanksCards.Add(thanksCard3);
                _context.ThanksCards.Add(thanksCard4);
                _context.ThanksCards.Add(thanksCard5);
                _context.ThanksCards.Add(thanksCard6);
                _context.ThanksCards.Add(thanksCard7);

                var thanksCardTag1 = new ThanksCardTag { ThanksCard=thanksCard1, Tag=tag1 };
                var thanksCardTag2 = new ThanksCardTag { ThanksCard=thanksCard1, Tag=tag2 };
                var thanksCardTag3 = new ThanksCardTag { ThanksCard=thanksCard2, Tag=tag2 };
                var thanksCardTag4 = new ThanksCardTag { ThanksCard=thanksCard2, Tag=tag3 };
                _context.ThanksCardTags.Add(thanksCardTag1);
                _context.ThanksCardTags.Add(thanksCardTag2);
                _context.ThanksCardTags.Add(thanksCardTag3);
                _context.ThanksCardTags.Add(thanksCardTag4);

                for(int i = 0; i < 10; i++)
                {
                    Data data = new Data { Value = i * 10 + (i * i), DateTime = DateTime.Now };
                    _context.Data.Add(data);
                }
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

        public async Task<IEnumerable<ThanksCard>> GetThanksCards()
        {
            return await _context.ThanksCards
                                  .Include(ThanksCard => ThanksCard.ThanksCardTags) // Eager-loading
                                    .ThenInclude(ThanksCardTag => ThanksCardTag.Tag) // Load for Many-to-Many
                                  .ToListAsync();
        }

        public async Task<IEnumerable<Data>> GetData()
        {
            var data = await _context.Data.OrderBy(d => d.Id).ToListAsync();
            return data.Zip(data.Skip(1), (prev, next) => new Data { Value = next.Value - prev.Value, DateTime = prev.DateTime });
        }
    }
}
