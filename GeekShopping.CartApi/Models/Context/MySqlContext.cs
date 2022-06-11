using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartApi.Models.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options)
            : base (options)
        {
        }

    }
}