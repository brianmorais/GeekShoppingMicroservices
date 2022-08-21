using GeekShopping.Email.Messages;
using GeekShopping.Email.Models;
using GeekShopping.Email.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<MySqlContext> _context;

        public EmailRepository(DbContextOptions<MySqlContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            var email = new EmailLog
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} - has been created successfully"
            };

            await using var db = new MySqlContext(_context);
            db.Emails.Add(email);
            await db.SaveChangesAsync();
        }
    }
}
