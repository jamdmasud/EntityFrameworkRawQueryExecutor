
using Microsoft.EntityFrameworkCore;

namespace RawQueryApp
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=DESKTOP-0UTFTAA\MASUD;database=TodoDb;user id=sa; password=141");
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<TodoDetail> TodoDetail { set; get; }
        public DbSet<TodoItem> TodoItem { get; set; }
    }
}
