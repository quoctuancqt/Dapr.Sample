using Microsoft.EntityFrameworkCore;
using SharedKernel.Intefaces;

namespace OrderService.Infrastructure
{
    public class DatabaseFactory
    {
        private readonly IApplicationContext _context;

        public DatabaseFactory(IApplicationContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }
    }
}
