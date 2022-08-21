using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Models.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options)
            : base (options)
        {
        }

        public DbSet<EmailLog> Emails { get; set; }
    }
}