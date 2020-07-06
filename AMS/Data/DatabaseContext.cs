using Microsoft.EntityFrameworkCore;

namespace AMS.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
    }
}
